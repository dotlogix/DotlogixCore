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

#endregion

namespace DotLogix.Core.Nodes.Processor {
    /// <summary>
    /// An implementation of the <see cref="INodeWriter"/> interface to write json text
    /// </summary>
    public class JsonNodeWriter : NodeWriterBase {
        private readonly CharBuffer _builder;
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
            _builder = new CharBuffer(bufferSize);
            _formatterSettings = (JsonFormatterSettings)ConverterSettings;
        }


        #region 

        /// <inheritdoc />
        public override void WriteBeginMap()
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

            FlushBuffer();
        }

        private void FlushBuffer() {
            if (_builder.Count < _bufferSize && ContainerStack.Count != 0)
                return;

            var value = _builder.ToString();
            _builder.Clear();
            _writer.Write(value);
        }

        /// <inheritdoc />
        public override void WriteEndMap()
        {
            ContainerStack.PopExpected(NodeContainerType.Map);
            _isFirstChild = false;

            if (_formatterSettings.Ident)
                WriteIdent();
            _builder.Append('}');

            FlushBuffer();
        }

        /// <inheritdoc />
        public override void WriteBeginList()
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

            FlushBuffer();
        }

        /// <inheritdoc />
        public override void WriteEndList()
        {
            ContainerStack.PopExpected(NodeContainerType.List);
            _isFirstChild = false;

            if (_formatterSettings.Ident)
                WriteIdent();
            _builder.Append(']');

            FlushBuffer();
        }

        /// <inheritdoc />
        public override void WriteValue(object value)
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

            FlushBuffer();
        }

        #endregion

        private void WriteIdent() {
            _builder.Append('\n');
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
                    _builder.Append(JsonNull.JsonChars);
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
