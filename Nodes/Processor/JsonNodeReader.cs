// ==================================================
// Copyright 2018(C) , DotLogix
// File:  JsonNodeReader.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  03.03.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Globalization;
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

        private readonly char[] _json;

        public JsonNodeReader(string json) {
            _json = json.ToCharArray();
        }

        public override void CopyTo(INodeWriter nodeWriter) {
            var stateStack = new Stack<NodeContainerType>();
            var json = _json;
            string name = null;
            var allowedCharacters = JsonCharacter.OpenObject | JsonCharacter.OpenList | JsonCharacter.String | JsonCharacter.Other | JsonCharacter.End;

            for(var i = 0; i < json.Length; i++) {
                var nextCharacter = NextJsonCharacter(json, ref i);
                if((allowedCharacters & nextCharacter) == 0)
                    throw new JsonParsingException(i, json, $"Character {json[i]} is not allowed in the current state, allowed characters are {allowedCharacters:F}");

                switch(nextCharacter) {
                    case JsonCharacter.End:
                        allowedCharacters = JsonCharacter.None;
                        continue;
                    case JsonCharacter.OpenObject:
                        nodeWriter.BeginMap(name);
                        name = null;

                        allowedCharacters = JsonCharacter.String | JsonCharacter.CloseObject;
                        stateStack.Push(NodeContainerType.Map);

                        continue;
                    case JsonCharacter.CloseObject:
                    case JsonCharacter.CloseList:
                        if(stateStack.Pop() == NodeContainerType.Map)
                            nodeWriter.EndMap();
                        else
                            nodeWriter.EndList();

                        if(stateStack.Count == 0) {
                            allowedCharacters = JsonCharacter.End;
                            continue;
                        }
                        allowedCharacters = GetAllowedCharacters(stateStack, JsonCharacter.ValueDelimiter | JsonCharacter.CloseObject, JsonCharacter.ValueDelimiter | JsonCharacter.CloseList);
                        continue;
                    case JsonCharacter.OpenList:
                        nodeWriter.BeginList(name);
                        name = null;

                        allowedCharacters = JsonCharacter.OpenObject | JsonCharacter.OpenList | JsonCharacter.String | JsonCharacter.Other | JsonCharacter.CloseList;
                        stateStack.Push(NodeContainerType.List);
                        continue;
                    case JsonCharacter.String:
                        i++;
                        var str = NextJsonString(json, ref i);
                        if((stateStack.Count > 0) && (stateStack.Peek() == NodeContainerType.Map) && (name == null)) {
                            name = str;
                            allowedCharacters = JsonCharacter.ValueAssignment;
                            continue;
                        }
                        nodeWriter.WriteValue(name, str);
                        name = null;
                        allowedCharacters = GetAllowedCharacters(stateStack, JsonCharacter.ValueDelimiter | JsonCharacter.CloseObject, JsonCharacter.ValueDelimiter | JsonCharacter.CloseList);
                        continue;
                    case JsonCharacter.ValueAssignment:
                        allowedCharacters = JsonCharacter.OpenObject | JsonCharacter.OpenList | JsonCharacter.String | JsonCharacter.Other;
                        continue;
                    case JsonCharacter.ValueDelimiter:
                        allowedCharacters = GetAllowedCharacters(stateStack, JsonCharacter.String, JsonCharacter.OpenObject | JsonCharacter.OpenList | JsonCharacter.String | JsonCharacter.Other);
                        continue;
                    case JsonCharacter.Other:
                        var value = NextJsonValue(json, ref i);
                        nodeWriter.WriteValue(name, value);
                        name = null;

                        i--;

                        allowedCharacters = GetAllowedCharacters(stateStack, JsonCharacter.ValueDelimiter | JsonCharacter.CloseObject, JsonCharacter.ValueDelimiter | JsonCharacter.CloseList);
                        continue;
                    default:
                        throw new JsonParsingException(i, json, "The current state is invalid");
                }
            }

            if(stateStack.Count > 0)
                throw new JsonParsingException(json.Length - 1, json, "Wrong end of json, make sure you are closing all opened ararys and objects");
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
                    if(double.TryParse(valueStr, NumberStyles.AllowDecimalPoint|NumberStyles.AllowLeadingSign|NumberStyles.AllowExponent, NumberFormatInfo.InvariantInfo, out var number) == false) {
                        value = null;
                        return false;
                    }

                    // calculate if number is an integer
                    if((number <= int.MaxValue) && (number >= int.MinValue)) {
                        var truncated = Math.Truncate(number);
                        if(Math.Abs(number - truncated) <= (double.Epsilon * 100)) {
                            value = (int)number;
                            return true;
                        }
                    }
                    value = number;
                    return true;
            }
        }

        private static string NextJsonString(char[] json, ref int pos) {
            for(var i = pos; i < json.Length; i++) {
                var escaped = false;

                var current = json[i];
                if(current == '\\') {
                    if(json.Length <= ++i)
                        throw new JsonParsingException(i, json, "The escape sequence '\\' requires at least one following character to be valid.");
                    current = json[i];
                    escaped = true;
                }


                if(escaped) {
                    switch(current) {
                        //must
                        case '\\':
                        case '\"':
                        //can
                        case '/':
                        case 'b':
                        case 'f':
                        case 'n':
                        case 'r':
                        case 't':
                            continue;
                        case 'u':
                            var to = ++i + 4;
                            if(json.Length <= to)
                                throw new JsonParsingException(i, json, "The escape sequence '\\u' requires 4 following hex digits to be valid.");

                            for(; i < to; i++) {
                                current = json[i];
                                if(JsonStrings.IsHex(current))
                                    continue;
                                throw new JsonParsingException(i, json, $"The character '{current}' is not a valid hex character.");
                            }
                            continue;
                        default:
                                throw new JsonParsingException(i, json, $"The character '{current}' can not be escaped.");
                    }
                }

                switch (current)
                {
                    case '\"':
                        var str = JsonStrings.UnescapeJsonString(json, pos, i - 1);
                        pos = i;
                        return str;
                    case '\\':
                        throw new JsonParsingException(i, json, $"The character '{current}' must be escaped to be valid.");
                    default:
                        int currentInt = current;
                        if (currentInt < 0x20 || (currentInt >= 0x7F && currentInt <= 0x9F))
                            throw new JsonParsingException(i, json, $"The character '{JsonStrings.ToCharAsUnicode(current)}' is a control character and requires to be escaped to be valid.");
                        continue;
                }
            }
            throw new JsonParsingException(pos, json, "The string never ends");
        }

        private static object NextJsonValue(char[] json, ref int position) {
            for(var i = position; i < json.Length; i++) {
                switch(json[i]) {
                    case ' ':
                    case '\n':
                    case '\r':
                    case '\t':
                    case ',':
                    case ']':
                    case '}':
                        var valueStr = new string(json, position, i - position);
                        if(TryGetValueFromString(valueStr, out var obj) == false)
                            throw new JsonParsingException(position, json, $"Value can not be parsed make sure {valueStr} is a valid json value");
                        position = i;
                        return obj;
                }
            }
            throw new JsonParsingException(position, json, "Can not find end of value string");
        }

        public JsonCharacter NextJsonCharacter(char[] json, ref int position) {
            for(; position < json.Length; position++) {
                var current = json[position];
                switch(current) {
                    case ' ':
                    case '\t':
                    case '\n':
                    case '\r':
                        continue;
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
                    default:
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
    }
}
