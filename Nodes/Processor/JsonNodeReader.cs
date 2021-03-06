﻿// ==================================================
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotLogix.Core.Extensions;
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
            End = 1 << 0,
            /// <summary>
            /// The open object character {
            /// </summary>
            OpenObject = 1 << 1,
            /// <summary>
            /// The close object character }
            /// </summary>
            CloseObject = 1 << 2,
            /// <summary>
            /// The open list character [
            /// </summary>
            OpenList = 1 << 3,
            /// <summary>
            /// The close list character ]
            /// </summary>
            CloseList = 1 << 4,
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
            Other = 1 << 8
        }
        private readonly char[] _unicodeBuffer = new char[4];

        private const int NearQueueSize = 50;
        private const double Epsilon = double.Epsilon * 100;
        private readonly IEnumerator<ValueTask<char>> _jsonChars;
        private readonly Queue<char> _nearQueue = new Queue<char>(NearQueueSize);
        private int _line;
        private int _lineStart;

        private int _position;
        private string Near => new string(_nearQueue.ToArray());

        /// <summary>
        /// Creates a new instance of <see cref="JsonNodeReader"/>
        /// </summary>
        public JsonNodeReader(string json) {
            _jsonChars = json.Select(c => {
                                         ProcessChar(c);
                                         return new ValueTask<char>(c);
                                     }).GetEnumerator();
        }
        /// <summary>
        /// Creates a new instance of <see cref="JsonNodeReader"/>
        /// </summary>
        public JsonNodeReader(TextReader reader) {
            _jsonChars = reader.AsAsyncSequence(1024, ProcessChar).GetEnumerator();
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing) {
            _jsonChars?.Dispose();
        }

        /// <inheritdoc />
        public override async ValueTask CopyToAsync(IAsyncNodeWriter nodeWriter) {
            var enumerator = _jsonChars;
            using(enumerator) {
                var stateStack = new Stack<NodeContainerType>();
                string name = null;
                var allowedCharacters = JsonCharacter.OpenObject | JsonCharacter.OpenList | JsonCharacter.String | JsonCharacter.Other | JsonCharacter.End;

                enumerator.MoveNext();

                ValueTask vTask;
                do {
                    var charTask = NextJsonCharacterAsync(enumerator);
                    var nextCharacter = charTask.IsCompletedSuccessfully ? charTask.Result : await charTask;

                    if((allowedCharacters & nextCharacter) == 0) {
                        if(allowedCharacters == JsonCharacter.None || allowedCharacters == JsonCharacter.End)
                            return;
                        throw new JsonParsingException($"Character {enumerator.Current} is not allowed in the current state", _position, _line, _position - _lineStart, Near);
                    }

                    switch(nextCharacter) {
                        case JsonCharacter.End:
                            allowedCharacters = JsonCharacter.None;
                            break;
                        case JsonCharacter.OpenObject:
                            vTask = nodeWriter.BeginMapAsync(name);
                            if(vTask.IsCompletedSuccessfully == false)
                                await vTask;
                            name = null;

                            allowedCharacters = JsonCharacter.String | JsonCharacter.CloseObject;
                            stateStack.Push(NodeContainerType.Map);

                            break;
                        case JsonCharacter.CloseObject:
                        case JsonCharacter.CloseList:
                            vTask = stateStack.Pop() == NodeContainerType.Map ? nodeWriter.EndMapAsync() : nodeWriter.EndListAsync();
                            if(vTask.IsCompletedSuccessfully == false)
                                await vTask;

                            if(stateStack.Count == 0) {
                                allowedCharacters = JsonCharacter.End;
                                break;
                            }

                            allowedCharacters = GetAllowedCharacters(stateStack, JsonCharacter.ValueDelimiter | JsonCharacter.CloseObject, JsonCharacter.ValueDelimiter | JsonCharacter.CloseList);
                            break;
                        case JsonCharacter.OpenList:
                            vTask = nodeWriter.BeginListAsync(name);
                            if(vTask.IsCompletedSuccessfully == false)
                                await vTask;
                            name = null;

                            allowedCharacters = JsonCharacter.OpenObject | JsonCharacter.OpenList | JsonCharacter.String | JsonCharacter.Other | JsonCharacter.CloseList;
                            stateStack.Push(NodeContainerType.List);
                            break;
                        case JsonCharacter.String:
                            var strTask = NextJsonStringAsync(enumerator);
                            var str = strTask.IsCompletedSuccessfully ? strTask.Result : await strTask;
                            if((stateStack.Count > 0) && (stateStack.Peek() == NodeContainerType.Map) && (name == null)) {
                                name = str;
                                allowedCharacters = JsonCharacter.ValueAssignment;
                                break;
                            }

                            vTask = nodeWriter.WriteValueAsync(name, new JsonPrimitive(JsonPrimitiveType.String, str));
                            if(vTask.IsCompletedSuccessfully == false)
                                await vTask;
                            name = null;
                            allowedCharacters = GetAllowedCharacters(stateStack, JsonCharacter.ValueDelimiter | JsonCharacter.CloseObject, JsonCharacter.ValueDelimiter | JsonCharacter.CloseList);
                            break;
                        case JsonCharacter.ValueAssignment:
                            allowedCharacters = JsonCharacter.OpenObject | JsonCharacter.OpenList | JsonCharacter.String | JsonCharacter.Other;
                            break;
                        case JsonCharacter.ValueDelimiter:
                            allowedCharacters = GetAllowedCharacters(stateStack, JsonCharacter.String, JsonCharacter.OpenObject | JsonCharacter.OpenList | JsonCharacter.String | JsonCharacter.Other);
                            break;
                        case JsonCharacter.Other:
                            var valueTask = NextJsonValueAsync(enumerator);
                            var value = valueTask.IsCompletedSuccessfully ? valueTask.Result : await valueTask;
                            vTask = nodeWriter.WriteValueAsync(name, value);
                            if(vTask.IsCompletedSuccessfully == false)
                                await vTask;
                            name = null;
                            allowedCharacters = GetAllowedCharacters(stateStack, JsonCharacter.ValueDelimiter | JsonCharacter.CloseObject, JsonCharacter.ValueDelimiter | JsonCharacter.CloseList);
                            continue;
                        default:
                            throw new JsonParsingException("The current state is invalid", _position, _line, _position - _lineStart, Near);
                    }

                    if(enumerator.MoveNext() == false)
                        break;
                } while(true);

                if(stateStack.Count > 0)
                    throw new JsonParsingException("Wrong end of json, make sure you are closing all opened ararys and objects", _position, _line, _position - _lineStart, Near);
            }
        }

        private static bool TryGetValueFromString(string valueStr, out JsonPrimitive value) {
            switch(valueStr) {
                case "null":
                    value = JsonPrimitive.Null;
                    return true;
                case "true":
                    value = JsonPrimitive.True;
                    return true;
                case "false":
                    value = JsonPrimitive.False;
                    return true;
                default:
                    value = new JsonPrimitive(JsonPrimitiveType.Number, valueStr);
                    return true;
            }
        }

        private async ValueTask<string> NextJsonStringAsync(IEnumerator<ValueTask<char>> enumerator) {
            var builder = new StringBuilder();
            while(enumerator.MoveNext()) {
                var task = enumerator.Current;
                var current = task.IsCompletedSuccessfully ? task.Result : await task;
                switch(current) {
                    case '\"':
                        return builder.ToString();
                    case '\\':

                        if(enumerator.MoveNext() == false)
                            throw new JsonParsingException("The escape sequence '\\' requires at least one following character to be valid.", _position, _line, _position - _lineStart, Near);
                        task = enumerator.Current;
                        current = task.IsCompletedSuccessfully ? task.Result : await task;

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
                                for(int i = 0; i < 4; i++) {
                                    if(enumerator.MoveNext() == false)
                                        throw new JsonParsingException("The escape sequence '\\u' requires 4 following hex digits to be valid.", _position, _line, _position - _lineStart, Near);
                                    task = enumerator.Current;
                                    current = task.IsCompletedSuccessfully ? task.Result : await task;

                                    if(JsonStrings.IsHex(current) == false)
                                        throw new JsonParsingException($"The character '{current}' is not a valid hex character.", _position, _line, _position - _lineStart, Near);

                                    _unicodeBuffer[i] = current;
                                }

                                current = JsonStrings.FromCharAsUnicode(_unicodeBuffer, 0);
                                break;
                            default:
                                throw new JsonParsingException($"The character '{current}' can not be escaped.", _position, _line, _position - _lineStart, Near);
                        }

                        break;
                    default:
                        int currentInt = current;
                        if((currentInt < 0x20) || ((currentInt >= 0x7F) && (currentInt <= 0x9F)))
                            throw new JsonParsingException($"The character '{JsonStrings.ToCharAsUnicode(current)}' is a control character and requires to be escaped to be valid.", _position, _line, _position - _lineStart, Near);
                        break;
                }

                builder.Append(current);
            }

            throw new JsonParsingException("The string never ends", _position, _line, _position - _lineStart, Near);
        }

        private async ValueTask<object> NextJsonValueAsync(IEnumerator<ValueTask<char>> enumerator) {
            var builder = new StringBuilder();

            object GetValueFromString() {
                var valueStr = builder.ToString();
                if(TryGetValueFromString(valueStr, out var obj) == false)
                    throw new JsonParsingException($"Value can not be parsed make sure {valueStr} is a valid json value", _position, _line, _position - _lineStart, Near);
                return obj;
            }

            do {
                var task = enumerator.Current;
                var current = task.IsCompletedSuccessfully ? task.Result : await task;
                switch(current) {
                    case ' ':
                    case '\t':
                    case '\r':
                    case '\n':
                    case ',':
                    case ']':
                    case '}':
                    case '\0':
                        return GetValueFromString();
                    default:
                        builder.Append(current);
                        break;
                }
            } while(enumerator.MoveNext());
            return GetValueFromString();
        }

        private async ValueTask<JsonCharacter> NextJsonCharacterAsync(IEnumerator<ValueTask<char>> enumerator) {
            do
            {
                var task = enumerator.Current;
                var current = task.IsCompletedSuccessfully ? task.Result : await task;
                switch (current)
                {
                    case ' ':
                    case '\t':
                    case '\r':
                    case '\n':
                        break;
                    case '{':
                        return JsonCharacter.OpenObject;
                    case '}':
                        return JsonCharacter.CloseObject;
                    case '[':
                        return JsonCharacter.OpenList;
                    case ']':
                        return JsonCharacter.CloseList;
                    case '"':
                        return JsonCharacter.String;
                    case ':':
                        return JsonCharacter.ValueAssignment;
                    case ',':
                        return JsonCharacter.ValueDelimiter;
                    case '\0':
                        return JsonCharacter.End;
                    default:
                        return JsonCharacter.Other;
                } 
            } while (enumerator.MoveNext());
            return JsonCharacter.End;
        }

        private static JsonCharacter GetAllowedCharacters(Stack<NodeContainerType> stateStack, JsonCharacter forMap, JsonCharacter forList) {
            if(stateStack.Count == 0)
                return JsonCharacter.End;
            return stateStack.Peek() == NodeContainerType.Map ? forMap : forList;
        }

        private void ProcessChar(char chr) {
            _position++;
            if (_nearQueue.Count > NearQueueSize)
                _nearQueue.Dequeue();
            _nearQueue.Enqueue(chr);

            if(chr != '\n')
                return;

            _line++;
            _lineStart = _position;
        }


    }
}
