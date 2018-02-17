// ==================================================
// Copyright 2018(C) , DotLogix
// File:  JsonNodesFormatter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Core.Nodes.Io {
    public class JsonNodesFormatter {
        private static JsonNodesFormatter _defaultFormatter = new JsonNodesFormatter();
        public string EnumFormat { get; set; }
        public string GuidFormat { get; set; }
        public string NumberFormat { get; set; }
        public IFormatProvider NumberFormatProvider { get; set; }
        public string DateTimeFormat { get; set; }
        public IFormatProvider DateTimeFormatProvider { get; set; }
        public bool Ident { get; set; }
        public int IdentSize { get; set; }

        public JsonNodesFormatter() {
            IdentSize = 4;
        }

        public JsonNodesFormatter(JsonNodesFormatter basedOn) {
            EnumFormat = basedOn.EnumFormat;
            GuidFormat = basedOn.GuidFormat;
            NumberFormat = basedOn.NumberFormat;
            NumberFormatProvider = basedOn.NumberFormatProvider;
            DateTimeFormat = basedOn.DateTimeFormat;
            DateTimeFormatProvider = basedOn.DateTimeFormatProvider;
            Ident = basedOn.Ident;
            IdentSize = basedOn.IdentSize;
        }

        public static JsonNodesFormatter CreateNewDefault() {
            return new JsonNodesFormatter(_defaultFormatter);
        }

        public static void ReplaceDefault(JsonNodesFormatter defaultFormatter) {
            _defaultFormatter = defaultFormatter;
        }
    }

    //public static class JsonNodes
    //{

    //    #region Serialize
    //    public static string Serialize(object instance, JsonNodesFormatter formatter = null) {
    //        var node = instance.ConvertToNode();
    //        return Serialize(node, formatter);
    //    }
    //    public static void Serialize(object instance, StringBuilder builder, JsonNodesFormatter formatter = null)
    //    {
    //        var node = instance.ConvertToNode();
    //        Serialize(node, builder, formatter);
    //    }

    //    public static string Serialize(Node node, JsonNodesFormatter formatter = null)
    //    {
    //        var sb = new StringBuilder(256);
    //        Serialize(node, sb, formatter);
    //        return sb.ToString();
    //    }
    //    public static void Serialize(Node node, StringBuilder builder, JsonNodesFormatter formatter = null) {
    //        if (formatter == null)
    //            formatter = JsonNodesFormatter.CreateNewDefault();

    //        SerializeRecursive("root", node, builder, formatter, 0, false, false);
    //    }

    //    private static void SerializeRecursive(string fallbackName, Node node, StringBuilder builder, JsonNodesFormatter formatter, int ident, bool appendName, bool hasNext) {
    //        var nodeName = node.Name ?? fallbackName;
    //        if(formatter.Ident)
    //            builder.Append(' ', ident);

    //        if (appendName) {
    //            var nameTag = $"\"{nodeName}\": ";
    //            builder.Append(nameTag);
    //        }

    //        switch (node.NodeType) {
    //            case NodeTypes.Empty:
    //            case NodeTypes.Value:
    //                SerializeValue((NodeValue)node, builder, formatter);
    //                break;
    //            case NodeTypes.List:
    //                SerializeCollection(nodeName, (NodeCollection)node, builder, formatter, ident, false, '[', ']');
    //                break;
    //            case NodeTypes.Map:
    //                SerializeCollection(nodeName, (NodeCollection)node, builder, formatter, ident, true, '{', '}');
    //                break;
    //            default:
    //                throw new ArgumentOutOfRangeException();
    //        }
    //        if (hasNext == false)
    //            return;
    //        builder.Append(",");
    //        if (formatter.Ident)
    //            builder.AppendLine();
    //    }

    //    private static void SerializeCollection(string fallbackName, NodeCollection node, StringBuilder builder, JsonNodesFormatter formatter, int ident, bool appendName, char openChar, char closeChar) {
    //        if(node.ChildCount == 0) {
    //            builder.Append(openChar, closeChar);
    //            return;
    //        }
    //        builder.Append(openChar);
    //        if (formatter.Ident)
    //            builder.AppendLine();
    //        using (var enumerator = node.Children().GetEnumerator()) {
    //            enumerator.MoveNext();
    //            bool hasNext;
    //            do {
    //                var current = enumerator.Current;
    //                hasNext = enumerator.MoveNext();
    //                SerializeRecursive(fallbackName, current, builder, formatter, ident + formatter.IdentSize, appendName, hasNext);
    //            } while (hasNext);
    //        }
    //        if(formatter.Ident) {
    //            builder.AppendLine();
    //            builder.Append(' ', ident);
    //        }

    //        builder.Append(closeChar);
    //    }

    //    private static void SerializeValue(NodeValue node, StringBuilder builder, JsonNodesFormatter formatter) {
    //       AppendValueString(builder,node.Value, formatter);
    //    }

    //    private static void AppendValueString(StringBuilder sb, object nodeValue, JsonNodesFormatter formatter) {
    //        if(nodeValue == null) {
    //            sb.Append("null");
    //            return;
    //        }

    //        var dataType = nodeValue.ToDataType();
    //        var flags = dataType.Flags & DataTypeFlags.PrimitiveMask;
    //        switch (flags)
    //        {
    //            case DataTypeFlags.Guid:
    //                sb.Append(((Guid)nodeValue).ToString(formatter.GuidFormat));
    //                break;
    //            case DataTypeFlags.Bool:
    //                sb.Append((bool)nodeValue ? "true" : "false");
    //                break;
    //            case DataTypeFlags.Enum:
    //                sb.Append(((Enum)nodeValue).ToString(formatter.EnumFormat));
    //                break;
    //            case var _ when (flags & DataTypeFlags.NumericMask) != 0:
    //                sb.Append(((IFormattable)nodeValue).ToString(formatter.NumberFormat, formatter.NumberFormatProvider));
    //                break;
    //            case var _ when (flags & DataTypeFlags.TextMask) != 0:
    //                AppendJsonString(sb, (string)nodeValue);
    //                break;
    //            case var _ when (flags & DataTypeFlags.TimeMask) != 0:
    //                sb.Append(((IFormattable)nodeValue).ToString(formatter.DateTimeFormat, formatter.DateTimeFormatProvider));
    //                break;
    //            default:
    //                throw new ArgumentOutOfRangeException();
    //        }
    //    }


    //    public static string UnescapeJsonString(string value, bool removeQuotes = true)
    //    {
    //        var sb = new StringBuilder();
    //        var safeCharactersStart = -1;
    //        var safeCharactersCount = 0;
    //        var startIndex = removeQuotes ? 1 : 0;
    //        var length = removeQuotes ? value.Length : value.Length - 2;
    //        for (var i = startIndex; i < length; i++)
    //        {
    //            var current = value[i];
    //            if (current == '\\')
    //            {
    //                var nextChr = value[i + 1];
    //                switch (nextChr)
    //                {
    //                    case '"':
    //                    case '\\':
    //                    case '/':
    //                        current = nextChr;
    //                        break;
    //                    case 'b':
    //                        current = '\b';
    //                        break;
    //                    case 'f':
    //                        current = '\f';
    //                        break;
    //                    case 'n':
    //                        current = '\n';
    //                        break;
    //                    case 'r':
    //                        current = '\r';
    //                        break;
    //                    case 't':
    //                        current = '\t';
    //                        break;
    //                    case 'u':
    //                        current = FromCharAsUnicode(value, i + 2);
    //                        i += 4;
    //                        break;
    //                    default:
    //                        throw new ArgumentException($"Invalid char at {i}", nameof(value));
    //                }
    //                i++;
    //            }
    //            else
    //            {
    //                if (safeCharactersStart == -1)
    //                    safeCharactersStart = i;
    //                safeCharactersCount++;
    //                continue;
    //            }

    //            if (safeCharactersCount > 0)
    //            {
    //                sb.Append(value, safeCharactersStart, safeCharactersCount);
    //                safeCharactersStart = -1;
    //                safeCharactersCount = 0;
    //            }
    //            sb.Append(current);
    //        }

    //        if (safeCharactersCount == value.Length)
    //            return removeQuotes ? value.Substring(1, length) : value;

    //        return safeCharactersCount == 0
    //                   ? sb.ToString()
    //                   : sb.Append(value, safeCharactersStart, safeCharactersCount).ToString();
    //    }

    //    public static string EscapeJsonString(string value, bool addQuotes = true)
    //    {
    //        var sb = new StringBuilder();
    //        AppendJsonString(sb, value, addQuotes);
    //        return sb.ToString();
    //    }

    //    public static void AppendJsonString(StringBuilder sb, string value, bool addQuotes = true)
    //    {
    //        if (addQuotes)
    //            sb.Append("\"");
    //        var unicodeBuffer = new char[6];
    //        var safeCharactersCount = 0;
    //        for (var i = 0; i < value.Length; i++)
    //        {
    //            var current = value[i];
    //            switch (current)
    //            {
    //                case '"':
    //                case '\\':
    //                case '/':
    //                    break;
    //                case '\b':
    //                    current = 'b';
    //                    break;
    //                case '\f':
    //                    current = 'f';
    //                    break;
    //                case '\n':
    //                    current = 'n';
    //                    break;
    //                case '\r':
    //                    current = 'r';
    //                    break;
    //                case '\t':
    //                    current = 't';
    //                    break;
    //                default:
    //                    if (current < ' ')
    //                    {
    //                        ToCharAsUnicode(current, unicodeBuffer);
    //                        sb.Append(unicodeBuffer);
    //                    }
    //                    else
    //                    {
    //                        safeCharactersCount++;
    //                    }
    //                    continue;
    //            }

    //            if (safeCharactersCount > 0)
    //            {
    //                sb.Append(value, i - safeCharactersCount, safeCharactersCount);
    //                safeCharactersCount = 0;
    //            }
    //            sb.Append('\\');
    //            sb.Append(current);
    //        }


    //        if (safeCharactersCount > 0)
    //        {
    //            if (safeCharactersCount == value.Length)
    //                sb.Append(value);
    //            else
    //                sb.Append(value, value.Length - safeCharactersCount, safeCharactersCount);
    //        }
    //        if (addQuotes)
    //            sb.Append("\"");
    //    }

    //    public static void ToCharAsUnicode(int chr, char[] buffer)
    //    {
    //        buffer[0] = '\\';
    //        buffer[1] = 'u';

    //        for (var i = 5; i > 1; i--)
    //        {
    //            buffer[i] = IntToHex(chr & 15);
    //            chr >>= 4;
    //        }
    //    }

    //    public static char FromCharAsUnicode(string value, int startIndex)
    //    {
    //        var chr = HexToInt(value[startIndex]);
    //        for (var i = 1; i < 4; i++)
    //        {
    //            chr = (chr << 4) + HexToInt(value[startIndex + i]);
    //        }
    //        return (char)chr;
    //    }

    //    public static char IntToHex(int number)
    //    {
    //        if (number <= 9)
    //            return (char)(number + 48); // + '0'
    //        return (char)(number + 87); // - 10 + 'a'
    //    }

    //    public static int HexToInt(int hex)
    //    {
    //        if (hex <= 57) // <= '9'
    //            return hex - 48; // - '0'
    //        return hex - 87; // - 10 + 'a'
    //    }
    //    #endregion

    //    //#region Deserialize
    //    //public Node Deserialize(string json)
    //    //{
    //    //    return Deserialize(json.ToCharArray());
    //    //}

    //    //public Node Deserialize(char[] json)
    //    //{

    //    //} 
    //    //#endregion
    //}
}
