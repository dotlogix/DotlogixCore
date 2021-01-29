// ==================================================
// Copyright 2018(C) , DotLogix
// File:  OptionalNodeConverter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  16.07.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using DotLogix.Core.Nodes.Formats;
using DotLogix.Core.Nodes.Formats.Nodes;
using DotLogix.Core.Nodes.Schema;

#endregion

namespace DotLogix.Core.Nodes.Converters {
    /// <summary>
    /// An implementation of the <see cref="INodeConverter"/> interface to optional values
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class OptionalNodeConverter<TValue> : NodeConverter {
        /// <summary>
        /// Creates a new instance of <see cref="OptionalNodeConverter{TValue}"/>
        /// </summary>
        public OptionalNodeConverter(TypeSettings typeSettings) : base(typeSettings) { }

        /// <inheritdoc />
        public override void Write(object instance, INodeWriter writer, IReadOnlyConverterSettings settings) {
            if(!(instance is Optional<TValue> opt))
                return;

            if (opt.IsDefined == false)
                return;

            var scopedSettings = settings.GetScoped(TypeSettings.ChildSettings);
            var childConverter = TypeSettings.ChildSettings.Converter;

            if (scopedSettings.ShouldEmitValue(opt.Value) == false)
                return;

            childConverter.Write(opt.Value, writer, scopedSettings);
        }

        /// <inheritdoc />
        public override object Read(INodeReader reader, IReadOnlyConverterSettings settings) {
            var next = reader.PeekOperation();
            if (next.HasValue == false || (next.Value.Type == NodeOperationTypes.Value && next.Value.Value == null)) {
                reader.ReadOperation();
                return new Optional<TValue>(default);
            }

            var scopedSettings = settings.GetScoped(TypeSettings.ChildSettings);
            var childConverter = TypeSettings.ChildSettings.Converter;

            return childConverter.Read(reader, scopedSettings);
        }

        /// <inheritdoc />
        public override object ConvertToObject(Node node, IReadOnlyConverterSettings settings) {
            if(node.Type == NodeTypes.Empty)
                return new Optional<TValue>(default);


            var scopedSettings = settings.GetScoped(TypeSettings.ChildSettings);
            var childConverter = TypeSettings.ChildSettings.Converter;

            var optionalValue = childConverter.ConvertToObject(node, scopedSettings);
            return new Optional<TValue>((TValue)optionalValue);

        }
    }
}
