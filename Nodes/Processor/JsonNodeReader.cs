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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotLogix.Core.Extensions;
#endregion

namespace DotLogix.Core.Nodes.Processor {
    public class JsonNodeReader : NodeReaderBase {
        [Flags]
        public enum JsonCharacter {
            None = 0,
            End = 1 << 0,
            OpenObject = 1 << 1,
            CloseObject = 1 << 2,
            OpenList = 1 << 3,
            CloseList = 1 << 4,
            String = 1 << 5,
            ValueAssignment = 1 << 6,
            ValueDelimiter = 1 << 7,
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

        public JsonNodeReader(string json) {
            _jsonChars = json.Select(c => {
                                         ProcessChar(c);
                                         return new ValueTask<char>(c);
                                     }).GetEnumerator();
        }

        public JsonNodeReader(TextReader reader) {
            _jsonChars = reader.AsAsyncSequence(1024, ProcessChar).GetEnumerator();
        }

        protected override void Dispose(bool disposing) {
            _jsonChars?.Dispose();
        }

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
                    var nextCharacter = charTask.IsCompleted ? charTask.Result : await charTask;

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
                            if(vTask.IsCompleted == false)
                                await vTask;
                            name = null;

                            allowedCharacters = JsonCharacter.String | JsonCharacter.CloseObject;
                            stateStack.Push(NodeContainerType.Map);

                            break;
                        case JsonCharacter.CloseObject:
                        case JsonCharacter.CloseList:
                            vTask = stateStack.Pop() == NodeContainerType.Map ? nodeWriter.EndMapAsync() : nodeWriter.EndListAsync();
                            if(vTask.IsCompleted == false)
                                await vTask;

                            if(stateStack.Count == 0) {
                                allowedCharacters = JsonCharacter.End;
                                break;
                            }

                            allowedCharacters = GetAllowedCharacters(stateStack, JsonCharacter.ValueDelimiter | JsonCharacter.CloseObject, JsonCharacter.ValueDelimiter | JsonCharacter.CloseList);
                            break;
                        case JsonCharacter.OpenList:
                            vTask = nodeWriter.BeginListAsync(name);
                            if(vTask.IsCompleted == false)
                                await vTask;
                            name = null;

                            allowedCharacters = JsonCharacter.OpenObject | JsonCharacter.OpenList | JsonCharacter.String | JsonCharacter.Other | JsonCharacter.CloseList;
                            stateStack.Push(NodeContainerType.List);
                            break;
                        case JsonCharacter.String:
                            var strTask = NextJsonStringAsync(enumerator);
                            var str = strTask.IsCompleted ? strTask.Result : await strTask;
                            if((stateStack.Count > 0) && (stateStack.Peek() == NodeContainerType.Map) && (name == null)) {
                                name = str;
                                allowedCharacters = JsonCharacter.ValueAssignment;
                                break;
                            }

                            vTask = nodeWriter.WriteValueAsync(name, str);
                            if(vTask.IsCompleted == false)
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
                            var value = valueTask.IsCompleted ? valueTask.Result : await valueTask;
                            vTask = nodeWriter.WriteValueAsync(name, value);
                            if(vTask.IsCompleted == false)
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

        private static bool TryGetValueFromString(string valueStr, out object value) {
            switch(valueStr) {
                case "null":
                    value = null;
                    return true;
                case "true":
                    value = true;
                    return true;
                case "false":
                    value = false;
                    return true;
                default:
                    if(double.TryParse(valueStr, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign | NumberStyles.AllowExponent, NumberFormatInfo.InvariantInfo, out var number) == false) {
                        value = null;
                        return false;
                    }

                    // calculate if number is an integer
                    if((number <= int.MaxValue) && (number >= int.MinValue)) {
                        var truncated = Math.Truncate(number);
                        if(Math.Abs(number - truncated) <= Epsilon) {
                            value = (int)number;
                            return true;
                        }
                    }

                    value = number;
                    return true;
            }
        }

        private async ValueTask<string> NextJsonStringAsync(IEnumerator<ValueTask<char>> enumerator) {
            var builder = new StringBuilder();
            while(enumerator.MoveNext()) {
                var task = enumerator.Current;
                var current = task.IsCompleted ? task.Result : await task;
                switch(current) {
                    case '\"':
                        return builder.ToString();
                    case '\\':

                        if(enumerator.MoveNext() == false)
                            throw new JsonParsingException("The escape sequence '\\' requires at least one following character to be valid.", _position, _line, _position - _lineStart, Near);
                        task = enumerator.Current;
                        current = task.IsCompleted ? task.Result : await task;

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
                                    current = task.IsCompleted ? task.Result : await task;

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
                var current = task.IsCompleted ? task.Result : await task;
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
                var current = task.IsCompleted ? task.Result : await task;
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
