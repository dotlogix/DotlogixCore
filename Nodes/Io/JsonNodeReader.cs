﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DotLogix.Core.Nodes.Io
{
    public class JsonNodeReader : NodeReaderBase {
        [Flags]
        public enum JsonCharacter
        {
            None = 0,
            End = 1 << 0,
            OpenObject = 1 << 1,
            CloseObject = 1 << 2,
            OpenList = 1 << 3,
            CloseList = 1 << 4,
            String = 1 << 5,
            ValueAssignment = 1 << 6,
            ValueDelimiter = 1 << 7,
            Other = 256
        }

        private readonly char[] _json;

        public JsonNodeReader(string json)
        {
            _json = json.ToCharArray();
        }

        public override void CopyTo(INodeWriter nodeWriter)
        {
            var stateStack = new Stack<NodeIoState>();
            var json = _json;
            string name = null;
            var allowedCharacters = JsonCharacter.OpenObject | JsonCharacter.OpenList | JsonCharacter.String | JsonCharacter.Other | JsonCharacter.End;

            for (var i = 0; i < json.Length; i++)
            {
                var nextCharacter = NextJsonCharacter(json, ref i);
                if ((allowedCharacters & nextCharacter) == 0)
                    throw new JsonParsingException(i, json, $"Character {json[i]} is not allowed in the current state, allowed characters are {allowedCharacters:F}");

                switch (nextCharacter)
                {
                    case JsonCharacter.End:
                        allowedCharacters = JsonCharacter.None;
                        continue;
                    case JsonCharacter.OpenObject:
                        nodeWriter.BeginMap(name);
                        name = null;

                        allowedCharacters = JsonCharacter.String | JsonCharacter.CloseObject;
                        stateStack.Push(NodeIoState.InsideMap);

                        continue;
                    case JsonCharacter.CloseObject:
                    case JsonCharacter.CloseList:
                        if (stateStack.Pop() == NodeIoState.InsideMap)
                            nodeWriter.EndMap();
                        else
                            nodeWriter.EndList();

                        if (stateStack.Count == 0)
                        {
                            allowedCharacters = JsonCharacter.End;
                            continue;
                        }

                        allowedCharacters = stateStack.Peek() == NodeIoState.InsideMap
                                                ? JsonCharacter.ValueDelimiter | JsonCharacter.CloseObject
                                                : JsonCharacter.ValueDelimiter | JsonCharacter.CloseList;
                        continue;
                    case JsonCharacter.OpenList:
                        nodeWriter.BeginList(name);
                        name = null;

                        allowedCharacters = JsonCharacter.OpenObject | JsonCharacter.OpenList | JsonCharacter.String | JsonCharacter.Other | JsonCharacter.CloseList;
                        stateStack.Push(NodeIoState.InsideList);
                        continue;
                    case JsonCharacter.String:
                        i++;
                        var str = NextJsonString(json, ref i);
                        if ((stateStack.Peek() == NodeIoState.InsideMap) && (name == null))
                        {
                            name = str;
                            allowedCharacters = JsonCharacter.ValueAssignment;
                            continue;
                        }
                        nodeWriter.WriteValue(str, name);
                        name = null;
                        allowedCharacters = stateStack.Peek() == NodeIoState.InsideMap
                                                ? JsonCharacter.ValueDelimiter | JsonCharacter.CloseObject
                                                : JsonCharacter.ValueDelimiter | JsonCharacter.CloseList;
                        continue;
                    case JsonCharacter.ValueAssignment:
                        allowedCharacters = JsonCharacter.OpenObject | JsonCharacter.OpenList | JsonCharacter.String | JsonCharacter.Other;
                        continue;
                    case JsonCharacter.ValueDelimiter:
                        allowedCharacters = stateStack.Peek() == NodeIoState.InsideMap
                                                ? JsonCharacter.String
                                                : JsonCharacter.OpenObject | JsonCharacter.OpenList | JsonCharacter.String | JsonCharacter.Other;
                        continue;
                    case JsonCharacter.Other:
                        var value = NextJsonValue(json, ref i);
                        nodeWriter.WriteValue(value, name);
                        name = null;
                        allowedCharacters = stateStack.Peek() == NodeIoState.InsideMap
                                                ? JsonCharacter.ValueDelimiter | JsonCharacter.CloseObject
                                                : JsonCharacter.ValueDelimiter | JsonCharacter.CloseList;
                        i--;
                        continue;
                    default:
                        throw new JsonParsingException(i, json, "The current state is invalid");
                }
            }
        }

        private static bool TryGetValueFromString(string valueStr, out object value)
        {
            switch (valueStr)
            {
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
                    if (double.TryParse(valueStr, out var number) == false)
                    {
                        value = null;
                        return false;
                    }

                    // calculate if number is an integer
                    if ((number <= int.MaxValue) && (number >= int.MinValue))
                    {
                        var truncated = Math.Truncate(number);
                        if (Math.Abs(number - truncated) <= (double.Epsilon * 100))
                        {
                            value = (int)number;
                            return true;
                        }
                    }
                    value = number;
                    return true;
            }
        }

        private static string NextJsonString(char[] json, ref int pos)
        {
            var escaped = false;
            for (var i = pos; i < json.Length; i++)
            {
                var current = json[i];
                if (escaped && (current >= ' '))
                {
                    escaped = false;
                    continue;
                }

                switch (current)
                {
                    case '/':
                    case '\b':
                    case '\f':
                    case '\n':
                    case '\r':
                    case '\t':
                        throw new JsonParsingException(pos, json, $"The character {current} is not allowed in the current state. Maybe your string is not escaped correctly");
                    default:
                        if (current < ' ')
                            throw new JsonParsingException(pos, json, $"The character {current} is not allowed in the current state. Maybe your string is not escaped correctly");
                        continue;
                    case '\"':
                        var value = new string(json, pos, i - pos);
                        pos = i;
                        return value;
                    case '\\':
                        escaped = true;
                        break;
                }
            }
            throw new JsonParsingException(pos, json, "The string never ends");
        }

        private static object NextJsonValue(char[] json, ref int position)
        {
            for (var i = position; i < json.Length; i++)
            {
                switch (json[i])
                {
                    case ' ':
                    case '\n':
                    case '\r':
                    case '\t':
                    case ',':
                    case ']':
                    case '}':
                        var valueStr = new string(json, position, i - position);
                        if (TryGetValueFromString(valueStr, out var obj) == false)
                            throw new JsonParsingException(position, json, $"Value can not be parsed make sure {valueStr} is a valid json value");
                        position = i;
                        return obj;
                }
            }
            throw new JsonParsingException(position, json, "Can not find end of value string");
        }

        public JsonCharacter NextJsonCharacter(char[] json, ref int position)
        {
            for (; position < json.Length; position++)
            {
                var current = json[position];
                switch (current)
                {
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
    }
}
