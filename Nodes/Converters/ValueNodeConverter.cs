// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ValueNodeConverter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using DotLogix.Core.Extensions;
using DotLogix.Core.Nodes.Formats.Nodes;
using DotLogix.Core.Nodes.Schema;

#endregion

namespace DotLogix.Core.Nodes.Converters {
    /// <summary>
    ///     An implementation of the <see cref="INodeConverter" /> interface to convert primitives
    /// </summary>
    public class ValueNodeConverter : NodeConverter {
        /// <summary>
        ///     Creates a new instance of <see cref="ValueNodeConverter" />
        /// </summary>
        public ValueNodeConverter(TypeSettings typeSettings) : base(typeSettings) { }

        /// <inheritdoc />
        public override void Write(object instance, INodeWriter writer, IReadOnlyConverterSettings settings) {
            var scopedSettings = settings.GetScoped(TypeSettings);
            if(scopedSettings.ShouldEmitValue(instance) == false)
                return;

            writer.WriteValue(instance);
        }

        public override object Read(INodeReader reader, IReadOnlyConverterSettings settings) {
            var value = reader.ReadValue();
            return value.TryConvertTo(Type, out value) ? value : default;
        }

        /// <inheritdoc />
        public override object ConvertToObject(Node node, IReadOnlyConverterSettings settings) {
            if(node.Type == NodeTypes.Empty)
                return Type.GetDefaultValue();

            if(node is NodeValue nodeValue)
                return nodeValue.Value.TryConvertTo(Type, out var value) ? value : Type.GetDefaultValue();
            throw new ArgumentException("Node is not a NodeValue");
        }
    }
}
