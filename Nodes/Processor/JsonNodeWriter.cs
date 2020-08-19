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


        #region Async

        /// <inheritdoc />
        public override async Task WriteBeginMapAsync()
        {
            CheckName(CurrentName, out var appendName);

            if (_isFirstChild == false)
                _builder.Append(',');

            if (_formatterSettings.Ident)
                WriteIdent();

            if (appendName) {
                AppendName(CurrentName);
                CurrentName = null;
            }

            _builder.Append('{');

            ContainerStack.Push(NodeContainerType.Map);
            _isFirstChild = true;

            if(_builder.Length >= _bufferSize) {
                await _writer.WriteAsync(_builder.ToString()).ConfigureAwait(false);
                _builder.Clear();
            }
        }

        /// <inheritdoc />
        public override async Task WriteEndMapAsync()
        {
            ContainerStack.PopExpected(NodeContainerType.Map);
            _isFirstChild = false;

            if (_formatterSettings.Ident)
                WriteIdent();
            _builder.Append('}');

            if(_builder.Length >= _bufferSize || ContainerStack.Count == 0) {
                await _writer.WriteAsync(_builder.ToString()).ConfigureAwait(false);
                _builder.Clear();
            }
        }

        /// <inheritdoc />
        public override async Task WriteBeginListAsync()
        {
            CheckName(CurrentName, out var appendName);

            if (_isFirstChild == false)
                _builder.Append(',');

            if (_formatterSettings.Ident)
                WriteIdent();

            if (appendName) {
                AppendName(CurrentName);
                CurrentName = null;
            }

            _builder.Append('[');

            ContainerStack.Push(NodeContainerType.List);
            _isFirstChild = true;

            if(_builder.Length >= _bufferSize) {
                await _writer.WriteAsync(_builder.ToString()).ConfigureAwait(false);
                _builder.Clear();
            }
        }

        /// <inheritdoc />
        public override async Task WriteEndListAsync()
        {
            ContainerStack.PopExpected(NodeContainerType.List);
            _isFirstChild = false;

            if (_formatterSettings.Ident)
                WriteIdent();
            _builder.Append(']');

            if(_builder.Length >= _bufferSize || ContainerStack.Count == 0) {
                await _writer.WriteAsync(_builder.ToString()).ConfigureAwait(false);
                _builder.Clear();
            }
        }

        /// <inheritdoc />
        public override async Task WriteValueAsync(object value)
        {
            CheckName(CurrentName, out var appendName);

            if (_isFirstChild == false)
                _builder.Append(',');

            if (_formatterSettings.Ident)
                WriteIdent();

            if (appendName) {
                AppendName(CurrentName);
                CurrentName = null;
            }

            AppendValueString(value);
            _isFirstChild = false;

            if(_builder.Length >= _bufferSize || ContainerStack.Count == 0) {
                await _writer.WriteAsync(_builder.ToString()).ConfigureAwait(false);
                _builder.Clear();
            }
        }

        #endregion

        private void WriteIdent() {
            _builder.AppendLine();
            if (ContainerStack.Count == 0)
                return;

            var identCount = ContainerStack.Count * _formatterSettings.IdentSize;
            if(identCount > 0)
                _builder.Append(' ', identCount);
        }

        private void AppendName(string name) {
            _builder.Append('"');

            JsonStrings.AppendJsonString(_builder, name);

            _builder.Append('"').Append(':');
        }

        private void AppendValueString(object value)
        {
            switch (value)
            {
                case null:
                    _builder.Append("null");
                    return;
                case IJsonPrimitive primitive:
                    primitive.AppendJson(_builder);
                    break;
                default:
                    JsonStrings.AppendJsonString(_builder, value.ToString(), true);
                    break;
            }
        }

        private void CheckName(string name, out bool appendName) {
            switch(ContainerStack.Current) {
                case NodeContainerType.Map:
                    if(name == null)
                        throw new ArgumentNullException(nameof(name), $"Name can not be null in the current container {ContainerStack.Current}");
                    appendName = true;
                    break;
                case NodeContainerType.List:
                    if(name != null)
                        throw new ArgumentException(nameof(name), $"Name can not have a value in the current container {ContainerStack.Current}");
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
