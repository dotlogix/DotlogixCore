// ==================================================
// Copyright 2019(C) , DotLogix
// File:  JsonNodeReader.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  16.02.2019
// LastEdited:  19.02.2019
// ==================================================

#region
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using DotLogix.Core.Extensions;
using DotLogix.Core.Types;

#endregion

namespace DotLogix.Core.Nodes.Processor {
    /// <summary>
    /// An implementation of the <see cref="IAsyncNodeReader"/> interface to read json text
    /// </summary>
    public class JsonNodeReader : NodeReaderBase {
        /// <summary>
        /// A json instruction character
        /// </summary>
        [Flags]
        public enum JsonCharacter {
            /// <summary>
            /// None
            /// </summary>
            None = 0,
            /// <summary>
            /// The end of the character stream
            /// </summary>
            End = 1,// << 0,
            /// <summary>
            /// The open object character {
            /// </summary>
            BeginMap = 1 << 1,
            /// <summary>
            /// The close object character }
            /// </summary>
            EndMap = 1 << 2,
            /// <summary>
            /// The open list character [
            /// </summary>
            BeginList = 1 << 3,
            /// <summary>
            /// The close list character ]
            /// </summary>
            EndList = 1 << 4,
            /// <summary>
            /// The begin of a string "
            /// </summary>
            String = 1 << 5,
            /// <summary>
            /// The value assignment character :
            /// </summary>
            ValueAssignment = 1 << 6,
            /// <summary>
            /// The value delimiter character ,
            /// </summary>
            ValueDelimiter = 1 << 7,
            /// <summary>
            /// Another unknown character
            /// </summary>
            Other = 1 << 8,


            OpenAny = BeginList | BeginMap,
            CloseAny = EndList | EndMap,
        }


        private readonly char[] _unicodeBuffer = new char[4];
        private const int NearQueueSize = 50;

        private readonly Queue<char> _nearQueue = new Queue<char>(NearQueueSize);
        private readonly JsonReaderOptions _options;

        private int _line;
        private int _lineStart;

        private int _position;

        private readonly char[] _buffer;
        private int _remaining;
        private int _index;

        private readonly Func<char[], int, int, Task<int>> _readBlockFunc;
        private readonly TextReader _reader;
        private readonly bool _canReadFurther;
        private bool IsTolerantMode => (_options & JsonReaderOptions.Tolerant) != 0;
        private string Near => new string(_nearQueue.ToArray());

        private readonly List<char> _valueBuffer;

        /// <summary>
        /// Creates a new instance of <see cref="JsonNodeReader"/>
        /// </summary>
        public JsonNodeReader(string json, JsonReaderOptions options = JsonReaderOptions.None)
        {
            _options = options;
            _buffer = json.ToCharArray();
            _valueBuffer = new List<char>(Math.Min(50, json.Length));
            _remaining = _buffer.Length;
        }
        /// <summary>
        /// Creates a new instance of <see cref="JsonNodeReader"/>
        /// </summary>
        public JsonNodeReader(TextReader reader, JsonReaderOptions options = JsonReaderOptions.None, int bufferSize = 1024) {
            _options = options;
            _readBlockFunc = reader.ReadBlockAsync;
            _canReadFurther = true;
            _reader = reader;
            _buffer = new char[bufferSize];
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing)
                _reader?.Dispose();
            base.Dispose(disposing);
        }

        /// <inheritdoc />
        public override async ValueTask CopyToAsync(IAsyncNodeWriter nodeWriter)
        {
            var stateStack = new Stack<NodeContainerType>();
            string name = null;
            var allowedCharacters = JsonCharacter.BeginMap | JsonCharacter.BeginList | JsonCharacter.String | JsonCharacter.Other | JsonCharacter.End;

            var previousCharacter = JsonCharacter.None;
            var currentCharacter = JsonCharacter.None;

            while (_remaining > 0 || (_canReadFurther && await EnsureNextCharsAsync().ConfigureAwait(false)))
            {
                previousCharacter = currentCharacter;
                currentCharacter = await NextJsonCharacterAsync().ConfigureAwait(false);

                if ((allowedCharacters & currentCharacter) == 0) {
                    var errorBehaviour = HandleParsingError(currentCharacter, previousCharacter, allowedCharacters);
                    switch (errorBehaviour) {
                        case ErrorBehaviour.Unhandled:
                            throw new JsonParsingException($"Character {currentCharacter} is not allowed in the current state {{ current: {previousCharacter} allowed: {allowedCharacters} }}", _position, _line, _position - _lineStart, Near);
                        case ErrorBehaviour.SkipCharacter:
                            continue;
                        case ErrorBehaviour.SkipToEnd:
                            return;
                        case ErrorBehaviour.Accept:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                switch (currentCharacter) {
                    case JsonCharacter.End:
                        allowedCharacters = JsonCharacter.None;
                        break;
                    case JsonCharacter.BeginMap:
                        await nodeWriter.BeginMapAsync(name).ConfigureAwait(false);
                        name = null;
                        allowedCharacters = JsonCharacter.String | JsonCharacter.EndMap;
                        stateStack.Push(NodeContainerType.Map);

                        break;
                    case JsonCharacter.EndMap:
                    case JsonCharacter.EndList:
                        if (stateStack.Pop() == NodeContainerType.Map)
                            await nodeWriter.EndMapAsync().ConfigureAwait(false);
                        else
                            await nodeWriter.EndListAsync().ConfigureAwait(false);

                        if (stateStack.Count == 0) {
                            allowedCharacters = JsonCharacter.End;
                            break;
                        }

                        allowedCharacters = GetAllowedCharacters(stateStack, JsonCharacter.ValueDelimiter | JsonCharacter.EndMap, JsonCharacter.ValueDelimiter | JsonCharacter.EndList);
                        break;
                    case JsonCharacter.BeginList:
                        await nodeWriter.BeginListAsync(name).ConfigureAwait(false);
                        name = null;

                        allowedCharacters = JsonCharacter.BeginMap | JsonCharacter.BeginList | JsonCharacter.String | JsonCharacter.Other | JsonCharacter.EndList;
                        stateStack.Push(NodeContainerType.List);
                        break;
                    case JsonCharacter.String:
                        var str = await NextJsonStringAsync().ConfigureAwait(false);
                        if (stateStack.Count > 0 && stateStack.Peek() == NodeContainerType.Map && name == null) {
                            name = str;
                            allowedCharacters = JsonCharacter.ValueAssignment;
                            break;
                        }

                        await nodeWriter.WriteValueAsync(name, new JsonPrimitive(JsonPrimitiveType.String, str)).ConfigureAwait(false);
                        name = null;
                        allowedCharacters = GetAllowedCharacters(stateStack, JsonCharacter.ValueDelimiter | JsonCharacter.EndMap, JsonCharacter.ValueDelimiter | JsonCharacter.EndList);
                        break;
                    case JsonCharacter.ValueAssignment:
                        allowedCharacters = JsonCharacter.BeginMap | JsonCharacter.BeginList | JsonCharacter.String | JsonCharacter.Other;
                        break;
                    case JsonCharacter.ValueDelimiter:
                        allowedCharacters = GetAllowedCharacters(stateStack, JsonCharacter.String, JsonCharacter.BeginMap | JsonCharacter.BeginList | JsonCharacter.String | JsonCharacter.Other);
                        break;
                    case JsonCharacter.Other:
                        var value = await NextJsonValueAsync().ConfigureAwait(false);
                        await nodeWriter.WriteValueAsync(name, value).ConfigureAwait(false);
                        
                        name = null;
                        allowedCharacters = GetAllowedCharacters(stateStack, JsonCharacter.ValueDelimiter | JsonCharacter.EndMap, JsonCharacter.ValueDelimiter | JsonCharacter.EndList);
                        continue;
                    default:
                        throw new JsonParsingException("The current state is invalid", _position, _line, _position - _lineStart, Near);
                }
            }

            if (stateStack.Count > 0)
                throw new JsonParsingException("Wrong end of json, make sure you are closing all opened arrays and objects", _position, _line, _position - _lineStart, Near);

        }

        private ErrorBehaviour HandleParsingError(JsonCharacter current, JsonCharacter previous, JsonCharacter allowed) {
            if(allowed == JsonCharacter.None || allowed == JsonCharacter.End)
                return ErrorBehaviour.SkipToEnd;

            if(IsTolerantMode) {
                if(current == JsonCharacter.ValueDelimiter)
                    return ErrorBehaviour.SkipCharacter;
                if(previous == JsonCharacter.ValueDelimiter && (JsonCharacter.CloseAny & current) != 0)
                    return ErrorBehaviour.Accept;
            }

            return ErrorBehaviour.Unhandled;
        }

        private static bool TryGetValueFromString(List<char> valueStr, out JsonPrimitive value)
        {
            if (valueStr.Count < 1)
            {
                value = default;
                return false;
            }

            if (EqualsString(valueStr, "null")) {
                value = JsonPrimitive.Null;

            } else if (EqualsString(valueStr, "true")) {
                value = JsonPrimitive.True;

            } else if (EqualsString(valueStr, "false")) {
                value = JsonPrimitive.False;
            }
            else
            {
                value = new JsonPrimitive(JsonPrimitiveType.Number, new string(valueStr.ToArray()));
            }
            return true;
        }

        private static bool EqualsString(IReadOnlyList<char> valueStr, string other)
        {
            if (valueStr.Count != other.Length)
                return false;

            for (var i = 0; i < valueStr.Count; i++)
            {
                if (valueStr[i] != other[i])
                    return false;
            }
            return true;
        }

        private async ValueTask<bool> EnsureNextCharsAsync(int minCharsRemaining = 1)
        {
            for (var i = 0; i < _remaining; i++)
            {
                _buffer[i] = _buffer[_index + i];
            }

            if (_canReadFurther)
            {
                _remaining += await _readBlockFunc.Invoke(_buffer, _remaining, _buffer.Length - _remaining).ConfigureAwait(false);
            }

            return _remaining >= minCharsRemaining;
        }



        private async ValueTask<string> NextJsonStringAsync() {
            while(_remaining > 0 || (_canReadFurther && await EnsureNextCharsAsync().ConfigureAwait(false))) {
                var current = NextChar();
                switch(current) {
                    case '\"':
                        var json = new string(_valueBuffer.ToArray());
                        _valueBuffer.Clear();
                        return json;
                    case '\\':
                        if(_remaining < 1 && (_canReadFurther && await EnsureNextCharsAsync().ConfigureAwait(false)) == false)
                        {
                            _valueBuffer.Clear();
                            throw new JsonParsingException("The escape sequence '\\' requires at least one following character to be valid.", _position, _line, _position - _lineStart, Near);
                        }
                        current = NextChar();

                        switch(current) {
                            case '\\':
                            case '\"':
                            case '/':
                                // no need to change anything
                                break;
                            case 'b':
                                current = '\b';
                                break;
                            case 'f':
                                current = '\f';
                                break;
                            case 'n':
                                current = '\n';
                                break;
                            case 'r':
                                current = '\r';
                                break;
                            case 't':
                                current = '\t';
                                break;
                            case 'u':
                                if (_remaining < 4 && (_canReadFurther && await EnsureNextCharsAsync(4).ConfigureAwait(false)) == false)
                                {
                                    _valueBuffer.Clear();
                                    throw new JsonParsingException("The escape sequence '\\u' requires 4 following hex digits to be valid.", _position, _line, _position - _lineStart, Near);
                                }

                                for(int i = 0; i < 4; i++)
                                {
                                    current = NextChar();

                                    if(JsonStrings.IsHex(current) == false)
                                    {
                                        _valueBuffer.Clear();
                                        throw new JsonParsingException($"The character '{current}' is not a valid hex character.", _position, _line, _position - _lineStart, Near);
                                    }

                                    _unicodeBuffer[i] = current;
                                }

                                current = JsonStrings.FromCharAsUnicode(_unicodeBuffer, 0);
                                break;
                            default:
                                _valueBuffer.Clear();
                                throw new JsonParsingException($"The character '{current}' can not be escaped.", _position, _line, _position - _lineStart, Near);
                        }

                        break;
                    default:
                        int currentInt = current;
                        if((currentInt < 0x20) || ((currentInt >= 0x7F) && (currentInt <= 0x9F))) {
                            if(IsTolerantMode) {
                                continue;
                            }

                            _valueBuffer.Clear(); 
                            throw new JsonParsingException($"The character '{JsonStrings.ToCharAsUnicode(current)}' is a control character and requires to be escaped to be valid.", _position, _line, _position - _lineStart, Near);
                        }
                        break;
                }

                _valueBuffer.Add(current);
            }

            _valueBuffer.Clear();
            throw new JsonParsingException("The string never ends", _position, _line, _position - _lineStart, Near);
        }

        private async ValueTask<object> NextJsonValueAsync()
        {
            object GetValueFromString() {
                var result = TryGetValueFromString(_valueBuffer, out var obj);
                _valueBuffer.Clear();
                if (result == false)
                    throw new JsonParsingException($"Value can not be parsed make sure \"{new string(_valueBuffer.ToArray())}\" is a valid json value", _position, _line, _position - _lineStart, Near);
                return obj;
            }

            while(_remaining > 0 || (_canReadFurther && await EnsureNextCharsAsync().ConfigureAwait(false))) {
                var current = NextChar();
                switch(current) {
                    case ' ':
                    case '\t':
                    case '\r':
                    case '\n':
                    case ',':
                    case ']':
                    case '}':
                    case '\0':
                        ReverseChar();
                        return GetValueFromString();
                    default:
                        _valueBuffer.Add(current);
                        break;
                }
            }

            return GetValueFromString();
        }

        private void ReverseChar()
        {
            _index--;
            _remaining++;
        }

        private async ValueTask<JsonCharacter> NextJsonCharacterAsync() {
            while(_remaining > 0 || (_canReadFurther && await EnsureNextCharsAsync().ConfigureAwait(false)))
            {
                var current = NextChar();
                switch (current)
                {
                    case ' ':
                    case '\t':
                    case '\r':
                    case '\n':
                        break;
                    case '{':
                        return JsonCharacter.BeginMap;
                    case '}':
                        return JsonCharacter.EndMap;
                    case '[':
                        return JsonCharacter.BeginList;
                    case ']':
                        return JsonCharacter.EndList;
                    case '"':
                        return JsonCharacter.String;
                    case ':':
                        return JsonCharacter.ValueAssignment;
                    case ',':
                        return JsonCharacter.ValueDelimiter;
                    case '\0':
                        return JsonCharacter.End;
                    default:
                        ReverseChar();
                        return JsonCharacter.Other;
                } 
            }
            return JsonCharacter.End;
        }

        private static JsonCharacter GetAllowedCharacters(Stack<NodeContainerType> stateStack, JsonCharacter forMap, JsonCharacter forList) {
            if(stateStack.Count == 0)
                return JsonCharacter.End;
            return stateStack.Peek() == NodeContainerType.Map ? forMap : forList;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private char NextChar()
        {
            var chr = _buffer[_index];
            _remaining--;
            _index++;
            _position++;
            //if (_nearQueue.Count > NearQueueSize)
            //    _nearQueue.Dequeue();
            //_nearQueue.Enqueue(chr);

            if(chr != '\n')
                return chr;

            _line++;
            _lineStart = _position;
            return chr;
        }


    }
}
