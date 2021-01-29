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
using DotLogix.Core.Nodes.Formats.Nodes;
using DotLogix.Core.Nodes.Utils;
#endregion

namespace DotLogix.Core.Nodes.Formats.Json {
    /// <summary>
    ///     An implementation of the <see cref="INodeWriter" /> interface to write json text
    /// </summary>
    public class JsonNodeWriter : NodeWriterBase {
        private readonly int _bufferSize;
        private readonly CharBuffer _builder;
        private readonly JsonConverterSettings _converterSettings;
        private readonly TextWriter _writer;
        private bool _isFirstChild = true;

        /// <summary>
        ///     Creates a new instance of <see cref="JsonNodeReader" />
        /// </summary>
        public JsonNodeWriter(TextWriter writer, JsonConverterSettings converterSettings = null, int bufferSize = 100) : base(converterSettings ?? JsonConverterSettings.Default) {
            _writer = writer;
            _bufferSize = bufferSize;
            _builder = new CharBuffer(bufferSize);
            _converterSettings = (JsonConverterSettings)ConverterSettings;
        }


        /// <inheritdoc />
        public override void WriteBeginMap() {
            var appendName = CheckName(CurrentName);

            if(_isFirstChild == false)
                _builder.Append(',');

            if(_converterSettings.Ident)
                AppendIdent();

            if(appendName) {
                AppendName(CurrentName);
                CurrentName = null;
            }

            _builder.Append('{');

            ContainerStack.Push(NodeContainerType.Map);
            _isFirstChild = true;

            FlushBuffer();
        }

        /// <inheritdoc />
        public override void WriteEndMap() {
            ContainerStack.PopExpected(NodeContainerType.Map);
            _isFirstChild = false;

            if(_converterSettings.Ident)
                AppendIdent();
            _builder.Append('}');

            FlushBuffer();
        }

        /// <inheritdoc />
        public override void WriteBeginList() {
            var appendName = CheckName(CurrentName);

            if(_isFirstChild == false)
                _builder.Append(',');

            if(_converterSettings.Ident)
                AppendIdent();

            if(appendName) {
                AppendName(CurrentName);
                CurrentName = null;
            }

            _builder.Append('[');

            ContainerStack.Push(NodeContainerType.List);
            _isFirstChild = true;

            FlushBuffer();
        }

        /// <inheritdoc />
        public override void WriteEndList() {
            ContainerStack.PopExpected(NodeContainerType.List);
            _isFirstChild = false;

            if(_converterSettings.Ident)
                AppendIdent();
            _builder.Append(']');

            FlushBuffer();
        }

        /// <inheritdoc />
        public override void WriteValue(object value) {
            var appendName = CheckName(CurrentName);

            if(_isFirstChild == false)
                _builder.Append(',');

            if(_converterSettings.Ident)
                AppendIdent();

            if(appendName) {
                AppendName(CurrentName);
                CurrentName = null;
            }

            switch(value) {
                case null:
                    _builder.Append(JsonStrings.JsonNull);
                    break;
                case IJsonPrimitive primitive:
                    primitive.AppendJson(_builder);
                    break;
                default:
                    JsonPrimitives.FromObject(value, _converterSettings).AppendJson(_builder);
                    break;
            }

            _isFirstChild = false;
            FlushBuffer();
        }

        private bool CheckName(string name) {
            switch(ContainerStack.Current) {
                case NodeContainerType.Map:
                    if(name == null)
                        throw new ArgumentNullException(nameof(name), $"Name can not be null in the current container {ContainerStack.Current}");
                    return true;
                case NodeContainerType.List:
                    if(name != null)
                        throw new ArgumentException(nameof(name), $"Name can not have a value in the current container {ContainerStack.Current}");
                    return false;
                case NodeContainerType.None:
                    return false;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void AppendIdent() {
            _builder.Append('\n');
            if(ContainerStack.Count == 0)
                return;

            var identCount = ContainerStack.Count * _converterSettings.IdentSize;
            if(identCount > 0)
                _builder.Append(' ', identCount);
        }

        private void AppendName(string name) {
            _builder.Append('"');
            JsonStrings.AppendJsonString(_builder, name);

            _builder.Append('"').Append(':');
        }

        private void FlushBuffer() {
            if((_builder.Count < _bufferSize) && (ContainerStack.Count != 0))
                return;

            var value = _builder.ToString();
            _builder.Clear();
            _writer.Write(value);
        }
    }
}
