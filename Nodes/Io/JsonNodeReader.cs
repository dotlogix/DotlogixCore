using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DotLogix.Core.Nodes.Io
{
    public class JsonNodeReader : NodeReaderBase {
        private readonly string _json;

        public JsonNodeReader(string json) {
            _json = json;
        }

        protected override IEnumerable<NodeIoOp> EnumerateOps()
        {
            var stateStack = new Stack<NodeIoState>();
            var setName = false;
            var startIndex = 0;
            for (var i = 0; i < _json.Length; i++)
            {
                var current = _json[i];
                string valueStr;
                switch (current)
                {
                    case '"':
                        var endIndex = FindEndOfString(_json, i + 1);
                        var str = _json.Substring(i+1, endIndex - i - 1);
                        i = endIndex;
                        startIndex = i + 1;
                        if (setName && (stateStack.Count > 0) && (stateStack.Peek() == NodeIoState.InsideMap)) {
                            yield return new NodeIoOp(NodeIoOpCodes.SetName, str);
                            setName = false;
                            continue;
                        }
                        yield return new NodeIoOp(NodeIoOpCodes.WriteValue, JsonNodeWriter.UnescapeJsonString(str, false));
                        break;
                    case '{':
                        yield return new NodeIoOp(NodeIoOpCodes.BeginMap);
                        stateStack.Push(NodeIoState.InsideMap);
                        startIndex = i + 1;
                        break;
                    case '[':
                        yield return new NodeIoOp(NodeIoOpCodes.BeginList);
                        stateStack.Push(NodeIoState.InsideList);
                        startIndex = i + 1;
                        break;
                    case ':':
                    case ' ':
                        startIndex = i + 1;
                        continue;
                    case ',':
                        valueStr = TrimSubstring(_json, startIndex, i);
                        if (valueStr != null)
                        {
                            var value = GetValueFromString(valueStr);
                            yield return new NodeIoOp(NodeIoOpCodes.WriteValue, value);
                        }
                        startIndex = i + 1;
                        break;
                    case ']':
                        valueStr = TrimSubstring(_json, startIndex, i);
                        if (valueStr != null) {
                            var value = GetValueFromString(valueStr);
                            yield return new NodeIoOp(NodeIoOpCodes.WriteValue, value);
                        }
                        yield return new NodeIoOp(NodeIoOpCodes.EndList);
                        stateStack.Pop();
                        startIndex = i + 1;
                        break;
                    case '}':
                        valueStr = TrimSubstring(_json, startIndex, i);
                        if(valueStr != null) {
                            var value = GetValueFromString(valueStr);
                            yield return new NodeIoOp(NodeIoOpCodes.WriteValue, value);
                        }
                        yield return new NodeIoOp(NodeIoOpCodes.EndMap);
                        stateStack.Pop();
                        startIndex = i + 1;
                        break;
                }
                setName = true;
            }
        }

        private static object GetValueFromString(string valueStr) {
            switch(valueStr) {
                case "null":
                    return null;
                case "true":
                    return true;
                case "false":
                    return false;
                default:
                    if(valueStr.Contains('.'))
                        return double.Parse(valueStr);
                    else
                        return int.Parse(valueStr);
            }
        }

        private static string TrimSubstring(string json, int startIndex, int endIndex)
        {
            if ((endIndex - startIndex) <= 0)
                return null;
            bool readFurther = true;
            for(int i = startIndex; readFurther && i < endIndex; i++) {
                switch(json[i]) {
                    case ' ':
                    case '\n':
                    case '\r':
                    case '\t':
                        startIndex++;
                        break;
                    default:
                        readFurther = false;
                        break;
                }
            }

            for (int i = endIndex; readFurther && i > startIndex; i--)
            {
                switch (json[i])
                {
                    case ' ':
                    case '\n':
                    case '\r':
                    case '\t':
                        endIndex--;
                        break;
                    default:
                        readFurther = false;
                        break;
                }
            }

            var split = json.Substring(startIndex, endIndex - startIndex);
            return split.Length > 0 ? split : null;
        }

        private static int FindEndOfString(string json, int startIndex)
        {
            var isEscaped = false;
            for (var i = startIndex; i < json.Length; i++)
            {
                if (isEscaped)
                {
                    isEscaped = false;
                    continue;
                }

                switch (json[i])
                {
                    case '\"':
                        return i;
                    case '\\':
                        isEscaped = true;
                        break;
                }
            }
            throw new InvalidDataException($"String starting at {startIndex} never ends");
        }
    }
}
