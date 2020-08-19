// ==================================================
// Copyright 2019(C) , DotLogix
// File:  JsonNodeReader.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  16.02.2019
// LastEdited:  19.02.2019
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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

        private readonly JsonReaderOptions _options;
        private readonly TextReader _reader;
        private readonly bool _canReadFurther;
        private readonly Func<char[], int, int, Task<int>> _readBlockFunc;

        private int _line;
        private int _lineStart;

        private int _position;

        private readonly char[] _buffer;
        private int _remaining;
        private int _index;

        private readonly StringBuilder _stringBuilder;
        private bool IsTolerantMode => (_options & JsonReaderOptions.Tolerant) != 0;
        public string Near => new string(_buffer);


        private Stack<NodeContainerType> _stateStack = new Stack<NodeContainerType>();
        private string _name = null;
        private JsonCharacter _allowedCharacters = JsonCharacter.BeginMap | JsonCharacter.BeginList | JsonCharacter.String | JsonCharacter.Other | JsonCharacter.End;
        
        private JsonCharacter _previousCharacter = JsonCharacter.None;
        private JsonCharacter _currentCharacter = JsonCharacter.None;

        /// <summary>
        /// Creates a new instance of <see cref="JsonNodeReader"/>
        /// </summary>
        public JsonNodeReader(string json, JsonReaderOptions options = JsonReaderOptions.None)
        {
            _options = options;
            _buffer = json.ToCharArray();
            _stringBuilder = new StringBuilder(Math.Min(50, json.Length));
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
        protected override async Task<NodeOperation?> ReadNextAsync() {
            while(true) {
                if (_remaining <= 0)
                {
                    if (_canReadFurther == false)
                        break;

                    var succeed = await EnsureNextCharsAsync().ConfigureAwait(false);
                    if (succeed == false)
                        break;
                }


                var currentCharacter = GetJsonCharacter();
                if (currentCharacter == JsonCharacter.None)
                    continue;
                
                _previousCharacter = _currentCharacter;
                _currentCharacter = currentCharacter;
                
                if((_allowedCharacters & _currentCharacter) == 0) {
                    var errorBehaviour = HandleParsingError();
                    switch(errorBehaviour) {
                        case ErrorBehaviour.Unhandled:
                            throw new JsonParsingException($"Character {_currentCharacter} is not allowed in the current state {{ current: {_previousCharacter} allowed: {_allowedCharacters} }}", _position, _line, _position - _lineStart, Near);
                        case ErrorBehaviour.SkipCharacter:
                            continue;
                        case ErrorBehaviour.SkipToEnd:
                            return null;
                        case ErrorBehaviour.Accept:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                switch(_currentCharacter) {
                    case JsonCharacter.End:
                        _allowedCharacters = JsonCharacter.None;
                        break;
                    case JsonCharacter.BeginMap:
                        return HandleBeginMap();
                    case JsonCharacter.EndMap:
                        return HandleEndMap();
                    case JsonCharacter.EndList:
                        return HandleEndList();
                    case JsonCharacter.BeginList:
                        return HandleBeginList();
                    case JsonCharacter.String:
                        var str = await NextJsonStringAsync().ConfigureAwait(false);
                        var op = HandleString(str);
                        if (op.HasValue == false)
                            continue;
                        return op.Value;
                    case JsonCharacter.ValueAssignment:
                        _allowedCharacters = JsonCharacter.BeginMap | JsonCharacter.BeginList | JsonCharacter.String | JsonCharacter.Other;
                        break;
                    case JsonCharacter.ValueDelimiter:
                        _allowedCharacters = GetAllowedCharacters(JsonCharacter.String, JsonCharacter.BeginMap | JsonCharacter.BeginList | JsonCharacter.String | JsonCharacter.Other);
                        break;
                    case JsonCharacter.Other:
                        var value = await NextJsonValueAsync().ConfigureAwait(false);
                        return HandleValue(value);
                    default:
                        throw new JsonParsingException("The current state is invalid", _position, _line, _position - _lineStart, Near);
                }
            }

            if(_stateStack.Count > 0)
                throw new JsonParsingException("Wrong end of json, make sure you are closing all opened arrays and objects", _position, _line, _position - _lineStart, Near);

            return null;
        }

        private async Task<string> NextJsonStringAsync() {
            while(true) {
                if (_remaining <= 0) {
                    if (_canReadFurther == false)
                        break;

                    var succeed = await EnsureNextCharsAsync().ConfigureAwait(false);
                    if (succeed == false)
                        break;
                }

                var current = NextChar();
                switch(current) {
                    case '\"':
                        var json = _stringBuilder.ToString();
                        _stringBuilder.Clear();
                        return json;
                    case '\\':
                        if(_remaining < 1 && (_canReadFurther && await EnsureNextCharsAsync().ConfigureAwait(false)) == false)
                        {
                            _stringBuilder.Clear();
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
                                    _stringBuilder.Clear();
                                    throw new JsonParsingException("The escape sequence '\\u' requires 4 following hex digits to be valid.", _position, _line, _position - _lineStart, Near);
                                }

                                for(int i = 0; i < 4; i++)
                                {
                                    current = NextChar();

                                    if(JsonStrings.IsHex(current) == false)
                                    {
                                        _stringBuilder.Clear();
                                        throw new JsonParsingException($"The character '{current}' is not a valid hex character.", _position, _line, _position - _lineStart, Near);
                                    }

                                    _unicodeBuffer[i] = current;
                                }

                                current = JsonStrings.FromCharAsUnicode(_unicodeBuffer, 0);
                                break;
                            default:
                                _stringBuilder.Clear();
                                throw new JsonParsingException($"The character '{current}' can not be escaped.", _position, _line, _position - _lineStart, Near);
                        }

                        break;
                    default:
                        int currentInt = current;
                        if((currentInt < 0x20) || ((currentInt >= 0x7F) && (currentInt <= 0x9F))) {
                            if(IsTolerantMode) {
                                continue;
                            }

                            _stringBuilder.Clear(); 
                            throw new JsonParsingException($"The character '{JsonStrings.ToCharAsUnicode(current)}' is a control character and requires to be escaped to be valid.", _position, _line, _position - _lineStart, Near);
                        }
                        break;
                }

                _stringBuilder.Append(current);
            }

            _stringBuilder.Clear();
            throw new JsonParsingException("The string never ends", _position, _line, _position - _lineStart, Near);
        }

        private async Task<object> NextJsonValueAsync()
        {
            object GetValueFromString() {
                var result = TryGetValueFromString(_stringBuilder, out var obj);
                _stringBuilder.Clear();
                if (result == false)
                    throw new JsonParsingException($"Value can not be parsed make sure \"{_stringBuilder}\" is a valid json value", _position, _line, _position - _lineStart, Near);
                return obj;
            }

            while(true) {
                if (_remaining <= 0) {
                    if (_canReadFurther == false)
                        break;

                    var succeed = await EnsureNextCharsAsync().ConfigureAwait(false);
                    if (succeed == false)
                        break;
                }

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
                        _stringBuilder.Append(current);
                        break;
                }
            }

            return GetValueFromString();
        }

        private NodeOperation HandleValue(object value) {

            var op = new NodeOperation(NodeOperationTypes.Value, _name, value);
            _name = null;
            _allowedCharacters = GetAllowedCharacters(JsonCharacter.ValueDelimiter | JsonCharacter.EndMap, JsonCharacter.ValueDelimiter | JsonCharacter.EndList);
            return op;
        }

        private NodeOperation? HandleString(string str) {
            if (_stateStack.Count > 0 && _stateStack.Peek() == NodeContainerType.Map && _name == null) {
                _name = str;
                _allowedCharacters = JsonCharacter.ValueAssignment;
                return null;
            }

            var value = new JsonString(str);
            var op = new NodeOperation(NodeOperationTypes.Value, _name, value);
            _name = null;
            _allowedCharacters = GetAllowedCharacters(JsonCharacter.ValueDelimiter | JsonCharacter.EndMap, JsonCharacter.ValueDelimiter | JsonCharacter.EndList);
            return op;
        }

        private NodeOperation HandleEndMap() {
            return HandleEndAny(NodeOperationTypes.EndMap);
        }

        private NodeOperation HandleEndAny(NodeOperationTypes operationType) {
            _stateStack.Pop();
            var op = new NodeOperation(operationType, _name);
            if(_stateStack.Count == 0) {
                _allowedCharacters = JsonCharacter.End;
            }

            _allowedCharacters = GetAllowedCharacters(JsonCharacter.ValueDelimiter | JsonCharacter.EndMap, JsonCharacter.ValueDelimiter | JsonCharacter.EndList);
            return op;
        }

        private NodeOperation HandleEndList() {
            return HandleEndAny(NodeOperationTypes.EndList);
        }

        private NodeOperation HandleBeginMap() {
            var op = new NodeOperation(NodeOperationTypes.BeginMap, _name);
            _name = null;
            _allowedCharacters = JsonCharacter.String | JsonCharacter.EndMap;
            _stateStack.Push(NodeContainerType.Map);
            return op;
        }

        private NodeOperation HandleBeginList() {
            var op = new NodeOperation(NodeOperationTypes.BeginList, _name);
            _name = null;
            _allowedCharacters = JsonCharacter.BeginMap | JsonCharacter.BeginList | JsonCharacter.String | JsonCharacter.Other | JsonCharacter.EndList;
            _stateStack.Push(NodeContainerType.List);
            return op;
        }

        private ErrorBehaviour HandleParsingError() {
            if(_allowedCharacters == JsonCharacter.None || _allowedCharacters == JsonCharacter.End)
                return ErrorBehaviour.SkipToEnd;

            if(IsTolerantMode) {
                if(_currentCharacter == JsonCharacter.ValueDelimiter)
                    return ErrorBehaviour.SkipCharacter;
                if(_previousCharacter == JsonCharacter.ValueDelimiter && (JsonCharacter.CloseAny & _currentCharacter) != 0)
                    return ErrorBehaviour.Accept;
            }

            return ErrorBehaviour.Unhandled;
        }

        private static bool TryGetValueFromString(StringBuilder stringBuilder, out IJsonPrimitive value)
        {
            switch (stringBuilder.Length)
            {
                case 0:
                    value = default;
                    return false;
                case 4:
                    switch (stringBuilder[0])
                    {
                        case 'n' when stringBuilder[1] == 'u' && stringBuilder[2] == 'l' && stringBuilder[3] == 'l':
                            value = JsonPrimitives.Null;
                            return true;
                        case 't' when stringBuilder[1] == 'r' && stringBuilder[2] == 'u' && stringBuilder[3] == 'e':
                            value = JsonPrimitives.True;
                            return true;
                    }

                    break;
                case 5 when stringBuilder[0] == 'f' && stringBuilder[1] == 'a' && stringBuilder[2] == 'l' && stringBuilder[3] == 's' && stringBuilder[4] == 'e':
                    value = JsonPrimitives.False;
                    return true;
            }

            var json = stringBuilder.ToString();
            value = new JsonNumber(json);
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
                _index = 0;
                _remaining += await _readBlockFunc.Invoke(_buffer, _remaining, _buffer.Length - _remaining).ConfigureAwait(false);
            }

            return _remaining >= minCharsRemaining;
        }

        private void ReverseChar()
        {
            _index--;
            _remaining++;
        }

        private JsonCharacter GetJsonCharacter() {
            var current = NextChar();
            switch(current) {
                case ' ':
                case '\t':
                case '\r':
                case '\n':
                    return JsonCharacter.None;
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

        private JsonCharacter GetAllowedCharacters(JsonCharacter forMap, JsonCharacter forList) {
            if(_stateStack.Count == 0)
                return JsonCharacter.End;
            return _stateStack.Peek() == NodeContainerType.Map ? forMap : forList;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private char NextChar()
        {
            var chr = _buffer[_index];
            _remaining--;
            _index++;
            _position++;

            if(chr != '\n')
                return chr;

            _line++;
            _lineStart = _position;
            return chr;
        }


    }
}
