// ==================================================
// Copyright 2018(C) , DotLogix
// File:  JsonNodeWriter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  03.03.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using DotLogix.Core.Extensions;
using DotLogix.Core.Types;
#endregion

namespace DotLogix.Core.Nodes.Processor {
    /// <summary>
    /// An implementation of the <see cref="IAsyncNodeWriter"/> interface to write json text
    /// </summary>
    public class JsonNodeWriter : NodeWriterBase {
        private readonly StringBuilder _builder;
        private readonly TextWriter _writer;
        private readonly int _bufferSize;
        private readonly JsonFormatterSettings _formatterSettings;
        private bool _isFirstChild = true;

        /// <summary>
        /// Creates a new instance of <see cref="JsonNodeReader"/>
        /// </summary>
        public JsonNodeWriter(TextWriter writer, JsonFormatterSettings formatterSettings = null, int bufferSize = 100) : base(formatterSettings ?? JsonFormatterSettings.Default) {
            _writer = writer;
            _bufferSize = bufferSize;
            _builder = new StringBuilder(bufferSize);
            _formatterSettings = formatterSettings ?? (JsonFormatterSettings)ConverterSettings;
        }


        /// <inheritdoc />
        public override async ValueTask BeginMapAsync(string name) {
            CheckName(name, out var appendName);

            if(_isFirstChild == false)
                _builder.Append(',');

            if(_formatterSettings.Ident)
                WriteIdentAsync();

            if(appendName)
                AppendName(name);

            _builder.Append('{');

            PushContainer(NodeContainerType.Map);
            _isFirstChild = true;

            if(_builder.Length >= _bufferSize) {
                var task = _writer.WriteAsync(_builder.ToString());
                if(task.IsCompleted == false || task.IsFaulted)
                    await task;
                _builder.Clear();
            }
        }

        /// <inheritdoc />
        public override async ValueTask EndMapAsync() {
            PopExpectedContainer(NodeContainerType.Map);
            _isFirstChild = false;

            if(_formatterSettings.Ident)
                WriteIdentAsync();
            _builder.Append('}');
            
            if(_builder.Length >= _bufferSize || ContainerCount == 0) {
                var task = _writer.WriteAsync(_builder.ToString());
                if(task.IsCompleted == false || task.IsFaulted)
                    await task;
                _builder.Clear();
            }
        }

        /// <inheritdoc />
        public override async ValueTask BeginListAsync(string name) {
            CheckName(name, out var appendName);

            if(_isFirstChild == false)
                _builder.Append(',');

            if(_formatterSettings.Ident)
                WriteIdentAsync();

            if(appendName)
                AppendName(name);

            _builder.Append('[');

            PushContainer(NodeContainerType.List);
            _isFirstChild = true;

            if(_builder.Length >= _bufferSize) {
                var task = _writer.WriteAsync(_builder.ToString());
                if(task.IsCompleted == false || task.IsFaulted)
                    await task;
                _builder.Clear();
            }
        }

        /// <inheritdoc />
        public override async ValueTask EndListAsync() {
            PopExpectedContainer(NodeContainerType.List);
            _isFirstChild = false;

            if(_formatterSettings.Ident)
                WriteIdentAsync();
            _builder.Append(']');
            
            if(_builder.Length >= _bufferSize || ContainerCount == 0) {
                var task = _writer.WriteAsync(_builder.ToString());
                if(task.IsCompleted == false || task.IsFaulted)
                    await task;
                _builder.Clear();
            }
        }

        /// <inheritdoc />
        public override async ValueTask WriteValueAsync(string name, object value) {
            CheckName(name, out var appendName);

            if(_isFirstChild == false)
                _builder.Append(',');

            if(_formatterSettings.Ident)
                WriteIdentAsync();

            if(appendName)
                AppendName(name);

            AppendValueString(value);
            _isFirstChild = false;

            if(_builder.Length >= _bufferSize || ContainerCount == 0) {
                var task = _writer.WriteAsync(_builder.ToString());
                if(task.IsCompleted == false || task.IsFaulted)
                    await task;
                _builder.Clear();
            }
        }

        private void WriteIdentAsync() {
            if(ContainerCount == 0)
                return;

            _builder.AppendLine();

            var identCount = ContainerCount * _formatterSettings.IdentSize;
            if(identCount > 0)
                _builder.Append(' ', identCount);
        }

        private void AppendName(string name) {
            _builder.Append('"');

            JsonStrings.AppendJsonString(_builder, name);

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
                case DataTypeFlags.Bool:
                    _builder.Append((bool)value ? "true" : "false");
                    break;
                case DataTypeFlags.Guid:
                    _builder.Append('\"');
                    _builder.Append(((IFormattable)value).ToString(_formatterSettings.GuidFormat, _formatterSettings.FormatProvider));
                    _builder.Append('\"');
                    break;
                case DataTypeFlags.Enum:
                    _builder.Append('\"');
                    _builder.Append(((IFormattable)value).ToString(_formatterSettings.EnumFormat, _formatterSettings.FormatProvider));
                    _builder.Append('\"');
                    break;
                case DataTypeFlags.Char:
                    value = new string((char)value, 1);
                    goto case DataTypeFlags.String;
                case DataTypeFlags.SByte:
                case DataTypeFlags.Byte:
                case DataTypeFlags.Short:
                case DataTypeFlags.UShort:
                case DataTypeFlags.Int:
                case DataTypeFlags.UInt:
                case DataTypeFlags.Long:
                case DataTypeFlags.ULong:
                case DataTypeFlags.Float:
                case DataTypeFlags.Double:
                case DataTypeFlags.Decimal:
                    _builder.Append(((IFormattable)value).ToString(_formatterSettings.NumberFormat, _formatterSettings.FormatProvider));
                    break;
                case DataTypeFlags.DateTime:
                case DataTypeFlags.DateTimeOffset:
                case DataTypeFlags.TimeSpan:
                    _builder.Append('\"');
                    _builder.Append(((IFormattable)value).ToString(_formatterSettings.TimeFormat, _formatterSettings.FormatProvider));
                    _builder.Append('\"');
                    break;
                case DataTypeFlags.String:
                    JsonStrings.AppendJsonString(_builder, (string)value, true);
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
