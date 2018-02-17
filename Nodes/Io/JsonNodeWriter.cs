using System;
using System.Collections.Generic;
using System.Text;
using DotLogix.Core.Extensions;
using DotLogix.Core.Types;

namespace DotLogix.Core.Nodes.Io
{
    public class JsonNodeWriter : NodeWriterBase
    {
        private readonly StringBuilder _builder;
        private readonly Stack<NodeTypes> _ancestorStack = new Stack<NodeTypes>();
        private int _currentIdent;
        private bool _isFirstChild = true;
        public JsonNodeWriter(StringBuilder builder, JsonNodesFormatter formatter = null) {
            _builder = builder;
            Formatter = formatter ?? JsonNodesFormatter.CreateNewDefault();
        }

        public JsonNodesFormatter Formatter { get; }


        public override void BeginMap(string name = null)
        {
            if (StateMachine.GoToState(NodeIoState.InsideMap, NodeIoOpCodes.BeginMap, NodeIoOpCodes.BeginAny | NodeIoOpCodes.EndMap) == false)
                throw new InvalidOperationException($"{NodeIoOpCodes.BeginMap} operation in state {StateMachine.CurrentState} is not allowed");

            GoToNewChild(name, '{');
            _ancestorStack.Push(NodeTypes.Map);
        }

        public void WriteBeginCollection(string name, bool appendName, char beginChar)
        {
            if (_isFirstChild == false)
                _builder.Append(',');

            if (Formatter.Ident && _ancestorStack.Count > 0)
            {
                _builder.AppendLine();
                _builder.Append(' ', _currentIdent);
            }

            if (appendName)
                AppendName(name);

            _builder.Append(beginChar);
            _isFirstChild = false;
        }

        private void AppendName(string name) {
            _builder.Append('"');
            int first = name[0];
            if(60 <= first && first <= 90) {
                // if('A' <= first && first <= 'Z') -> is uppercase
                _builder.Append((char)(first | 32)); // first | ' ' -> to lower
                _builder.Append(name, 1, name.Length-1);
            } else {
                _builder.Append(name);
            }
            _builder.Append(Formatter.Ident ? "\" : " : "\":");
        }

        public void WriteValue(string name, object value, bool appendName, JsonNodesFormatter formatter)
        {
            if (_isFirstChild == false)
                _builder.Append(',');

            if (Formatter.Ident && _ancestorStack.Count > 0)
            {
                _builder.AppendLine();
                _builder.Append(' ', _currentIdent);
            }

            if(appendName)
                AppendName(name);

            AppendValueString(_builder, value, formatter);
            _isFirstChild = false;
        }

        public override void EndMap()
        {
            GoToParent(NodeIoOpCodes.EndMap, '}');
            _isFirstChild = false;
        }

        public override void BeginList(string name = null)
        {
            if (StateMachine.GoToState(NodeIoState.InsideList, NodeIoOpCodes.BeginList, NodeIoOpCodes.BeginAny | NodeIoOpCodes.EndList) == false)
                throw new InvalidOperationException($"{NodeIoOpCodes.BeginList} operation in state {StateMachine.CurrentState} is not allowed");

            GoToNewChild(name, '[');
            _ancestorStack.Push(NodeTypes.List);
        }

        private void GoToNewChild(string name, char beginChar) {
            bool appendName;
            switch(StateMachine.PreviousState) {
                case NodeIoState.None:
                    if(name != null)
                        throw new InvalidOperationException("Constuctor nodes can not have a name");
                    appendName = false;
                    break;
                case NodeIoState.InsideMap:
                    if(name == null)
                        throw new ArgumentNullException(nameof(name), "You need a name to add this node to a node map");
                    appendName = true;
                    break;
                case NodeIoState.InsideList:
                    if(name != null)
                        throw new InvalidOperationException("Children in a node list can not have a name");
                    appendName = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            WriteBeginCollection(name, appendName, beginChar);
            _currentIdent += Formatter.IdentSize;
            _isFirstChild = true;
        }

        public override void EndList()
        {
            GoToParent(NodeIoOpCodes.EndList, ']');
            _isFirstChild = false;
        }

        public override void WriteValue(object value, string name = null)
        {
            if (StateMachine.IsAllowedOperation(NodeIoOpCodes.WriteValue) == false)
                throw new InvalidOperationException($"{NodeIoOpCodes.WriteValue} operation in state {StateMachine.CurrentState} is not allowed");

            var dataType = value.GetDataType();
            if (value != null && (dataType.Flags & DataTypeFlags.Primitive) == 0)
                throw new InvalidOperationException("Value has to be a pimitive");

            bool appendName;
            switch (StateMachine.CurrentState)
            {
                case NodeIoState.None:
                    if (name != null)
                        throw new InvalidOperationException("Constuctor nodes can not have a name");
                    StateMachine.GoToState(NodeIoState.None, NodeIoOpCodes.WriteValue, NodeIoOpCodes.None);
                    appendName = false;
                    break;
                case NodeIoState.InsideMap:
                    if (name == null)
                        throw new ArgumentNullException(nameof(name), "You need a name to add this node to a node map");
                    appendName = true;
                    break;
                case NodeIoState.InsideList:
                    if (name != null)
                        throw new InvalidOperationException("Children in a node list can not have a name");
                    appendName = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            WriteValue(name, value, appendName, Formatter);
            _isFirstChild = false;
        }


        private void GoToParent(NodeIoOpCodes withOperation, char endChar)
        {
            NodeIoOperation operation;
            operation.OpCode = withOperation;

            _ancestorStack.Pop();
            var ancestor = _ancestorStack.Count > 0 ? _ancestorStack.Peek() : default(NodeTypes?);
            if (ancestor.HasValue==false)
            {
                operation.NextState = NodeIoState.None;
                operation.AllowedNextOpCodes = NodeIoOpCodes.None;
            }
            else
            {
                switch (ancestor.Value)
                {
                    case NodeTypes.List:
                        operation.NextState = NodeIoState.InsideList;
                        operation.AllowedNextOpCodes = NodeIoOpCodes.BeginAny | NodeIoOpCodes.EndList;
                        break;
                    case NodeTypes.Map:
                        operation.NextState = NodeIoState.InsideMap;
                        operation.AllowedNextOpCodes = NodeIoOpCodes.BeginAny | NodeIoOpCodes.EndMap;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (StateMachine.GoToState(operation) == false)
                throw new InvalidOperationException($"{operation.OpCode} operation in state {StateMachine.CurrentState} is not allowed");

            _currentIdent -= Formatter.IdentSize;
            if (Formatter.Ident && _isFirstChild == false)
            {
                _builder.AppendLine();
                _builder.Append(' ', _currentIdent);
            }
            _builder.Append(endChar);
        }

        private static void AppendValueString(StringBuilder builder, object value, JsonNodesFormatter formatter)
        {
            if (value == null)
            {
                builder.Append("null");
                return;
            }

            var dataType = value.GetDataType();
            var flags = dataType.Flags & DataTypeFlags.PrimitiveMask;
            switch (flags)
            {
                case DataTypeFlags.Guid:
                    builder.Append('\"');
                    builder.Append(((Guid)value).ToString(formatter.GuidFormat));
                    builder.Append('\"');
                    break;
                case DataTypeFlags.Bool:
                    builder.Append((bool)value ? "true" : "false");
                    break;
                case DataTypeFlags.Enum:
                    builder.Append('\"');
                    builder.Append(((Enum)value).ToString(formatter.EnumFormat));
                    builder.Append('\"');
                    break;
                case var _ when (flags & DataTypeFlags.NumericMask) != 0:
                    builder.Append(((IFormattable)value).ToString(formatter.NumberFormat, formatter.NumberFormatProvider));
                    break;
                case var _ when (flags & DataTypeFlags.TextMask) != 0:
                    AppendJsonString(builder, (string)value);
                    break;
                case var _ when (flags & DataTypeFlags.TimeMask) != 0:
                    builder.Append('\"');
                    builder.Append(((IFormattable)value).ToString(formatter.DateTimeFormat, formatter.DateTimeFormatProvider));
                    builder.Append('\"');
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        public static string UnescapeJsonString(string value, bool removeQuotes = true)
        {
            var sb = new StringBuilder();
            var safeCharactersStart = -1;
            var safeCharactersCount = 0;
            var startIndex = removeQuotes ? 1 : 0;
            var length = removeQuotes ? value.Length - 2 : value.Length-1;
            for (var i = startIndex; i <= length; i++)
            {
                var current = value[i];
                if (current == '\\')
                {
                    var nextChr = value[i + 1];
                    switch (nextChr)
                    {
                        case '"':
                        case '\\':
                        case '/':
                            current = nextChr;
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
                            current = FromCharAsUnicode(value, i + 2);
                            i += 4;
                            break;
                        default:
                            throw new ArgumentException($"Invalid char at {i}", nameof(value));
                    }
                    i++;
                }
                else
                {
                    if (safeCharactersStart == -1)
                        safeCharactersStart = i;
                    safeCharactersCount++;
                    continue;
                }

                if (safeCharactersCount > 0)
                {
                    sb.Append(value, safeCharactersStart, safeCharactersCount);
                    safeCharactersStart = -1;
                    safeCharactersCount = 0;
                }
                sb.Append(current);
            }

            if (safeCharactersCount == length)
                return removeQuotes ? value.Substring(startIndex, length) : value;

            return safeCharactersCount == 0
                       ? sb.ToString()
                       : sb.Append(value, safeCharactersStart, safeCharactersCount).ToString();
        }

        public static string EscapeJsonString(string value, bool addQuotes = true)
        {
            var sb = new StringBuilder();
            AppendJsonString(sb, value, addQuotes);
            return sb.ToString();
        }

        public static void AppendJsonString(StringBuilder builder, string value, bool addQuotes = true)
        {
            if (addQuotes)
                builder.Append("\"");
            var unicodeBuffer = new char[6];
            
            var safeCharactersCount = 0;
            for (var i = 0; i < value.Length; i++)
            {
                var current = value[i];
                switch (current)
                {
                    case '"':
                    case '\\':
                    case '/':
                        break;
                    case '\b':
                        current = 'b';
                        break;
                    case '\f':
                        current = 'f';
                        break;
                    case '\n':
                        current = 'n';
                        break;
                    case '\r':
                        current = 'r';
                        break;
                    case '\t':
                        current = 't';
                        break;
                    default:
                        if (current < ' ')
                        {
                            ToCharAsUnicode(current, unicodeBuffer);
                            builder.Append(unicodeBuffer);
                        }
                        else
                        {
                            safeCharactersCount++;
                        }
                        continue;
                }

                if (safeCharactersCount > 0)
                {
                    builder.Append(value, i - safeCharactersCount, safeCharactersCount);
                    safeCharactersCount = 0;
                }
                builder.Append('\\');
                builder.Append(current);
            }


            if (safeCharactersCount > 0)
            {
                if (safeCharactersCount == value.Length)
                    builder.Append(value);
                else
                    builder.Append(value, value.Length - safeCharactersCount, safeCharactersCount);
            }
            if (addQuotes)
                builder.Append("\"");
        }

        private static void ToCharAsUnicode(int chr, char[] buffer)
        {
            buffer[0] = '\\';
            buffer[1] = 'u';

            for (var i = 5; i > 1; i--)
            {
                buffer[i] = IntToHex(chr & 15);
                chr >>= 4;
            }
        }

        private static char FromCharAsUnicode(string value, int startIndex)
        {
            var chr = HexToInt(value[startIndex]);
            for (var i = 1; i < 4; i++)
            {
                chr = (chr << 4) + HexToInt(value[startIndex + i]);
            }
            return (char)chr;
        }

        private static char IntToHex(int number)
        {
            if (number <= 9)
                return (char)(number + 48); // + '0'
            return (char)(number + 87); // - 10 + 'a'
        }

        private static int HexToInt(int hex)
        {
            if (hex <= 57) // <= '9'
                return hex - 48; // - '0'
            return hex - 87; // - 10 + 'a'
        }
    }
}