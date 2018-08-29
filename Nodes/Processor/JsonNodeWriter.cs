// ==================================================
// Copyright 2018(C) , DotLogix
// File:  JsonNodeWriter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  03.03.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Globalization;
using System.Text;
using DotLogix.Core.Extensions;
using DotLogix.Core.Types;
#endregion

namespace DotLogix.Core.Nodes.Processor {
    public class JsonNodeWriter : NodeWriterBase {
        private readonly StringBuilder _builder;

        private readonly JsonNodesFormatter _formatter;
        private bool _isFirstChild = true;

        public JsonNodeWriter(StringBuilder builder, JsonNodesFormatter formatter = null) {
            _builder = builder;
            _formatter = formatter ?? JsonNodesFormatter.Default;
        }


        public override void BeginMap(string name) {
            CheckName(name, out var appendName);

            if(_isFirstChild == false)
                _builder.Append(',');

            if(_formatter.Ident)
                WriteIdent();

            if(appendName)
                AppendName(name);

            _builder.Append('{');

            PushContainer(NodeContainerType.Map);
            _isFirstChild = true;
        }

        public override void EndMap() {
            PopExpectedContainer(NodeContainerType.Map);
            _isFirstChild = false;

            if(_formatter.Ident)
                WriteIdent();

            _builder.Append('}');
        }

        public override void BeginList(string name) {
            CheckName(name, out var appendName);

            if(_isFirstChild == false)
                _builder.Append(',');

            if(_formatter.Ident)
                WriteIdent();

            if(appendName)
                AppendName(name);

            _builder.Append('[');

            PushContainer(NodeContainerType.List);
            _isFirstChild = true;
        }

        public override void EndList() {
            PopExpectedContainer(NodeContainerType.List);
            _isFirstChild = false;

            if(_formatter.Ident)
                WriteIdent();

            _builder.Append(']');
        }

        public override void WriteValue(string name, object value) {
            CheckName(name, out var appendName);

            if(_isFirstChild == false)
                _builder.Append(',');

            if(_formatter.Ident)
                WriteIdent();

            if(appendName)
                AppendName(name);

            AppendValueString(value);
            _isFirstChild = false;
        }

        private void WriteIdent() {
            _builder.AppendLine();

            var identCount = ContainerCount * _formatter.IdentSize;
            if(identCount > 0)
                _builder.Append(' ', identCount);
        }

        private void AppendName(string name) {
            name = JsonStrings.EscapeJsonString(name);
            _builder.Append('"');
            int first = name[0];
            if((60 <= first) && (first <= 90)) {
                // if('A' <= first && first <= 'Z') -> is uppercase
                _builder.Append((char)(first | 32)); // first | ' ' -> to lower
                _builder.Append(name, 1, name.Length - 1);
            } else
                _builder.Append(name);
            _builder.Append("\":");
        }

        private void AppendValueString(object value) {
            if(value == null) {
                _builder.Append("null");
                return;
            }

            var dataType = value.GetDataType();
            var flags = dataType.Flags & DataTypeFlags.PrimitiveMask;
            switch(flags) {
                case DataTypeFlags.Guid:
                    _builder.Append('\"');
                    _builder.Append(((Guid)value).ToString("D"));
                    _builder.Append('\"');
                    break;
                case DataTypeFlags.Bool:
                    _builder.Append((bool)value ? "true" : "false");
                    break;
                case DataTypeFlags.Enum:
                    _builder.Append(((Enum)value).ToString("D"));
                    break;
                case var _ when (flags & DataTypeFlags.NumericMask) != 0:
                    _builder.Append(((IFormattable)value).ToString("G", NumberFormatInfo.InvariantInfo));
                    break;
                case var _ when (flags & DataTypeFlags.TextMask) != 0:
                    JsonStrings.AppendJsonString(_builder, (string)value, true);
                    break;
                case var _ when (flags & DataTypeFlags.TimeMask) != 0:
                    _builder.Append('\"');
                    _builder.Append(((IFormattable)value).ToString("u", DateTimeFormatInfo.InvariantInfo));
                    _builder.Append('\"');
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void CheckName(string name, out bool appendName) {
            switch(CurrentContainer) {
                case NodeContainerType.Map:
                    if(name == null)
                        throw new ArgumentNullException(nameof(name), $"Name can not be null in the current container {CurrentContainer}");
                    appendName = true;
                    break;
                case NodeContainerType.List:
                    if(name != null)
                        throw new ArgumentException(nameof(name), $"Name can not have a value in the current container {CurrentContainer}");
                    appendName = false;
                    break;
                case NodeContainerType.None:
                    appendName = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
