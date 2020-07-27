// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ValueNodeConverter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Threading.Tasks;
using DotLogix.Core.Extensions;
using DotLogix.Core.Nodes.Processor;
#endregion

namespace DotLogix.Core.Nodes.Converters {
    /// <summary>
    /// An implementation of the <see cref="IAsyncNodeConverter"/> interface to convert primitives
    /// </summary>
    public class ValueNodeConverter : NodeConverter {
        /// <summary>
        /// Creates a new instance of <see cref="ValueNodeConverter"/>
        /// </summary>
        public ValueNodeConverter(TypeSettings typeSettings) : base(typeSettings) { }

        /// <inheritdoc />
        public override ValueTask WriteAsync(object instance, string name, IAsyncNodeWriter writer, IReadOnlyConverterSettings settings) {
            var scopedSettings = settings.GetScoped(TypeSettings);
            if (scopedSettings.ShouldEmitValue(instance) == false)
                return default;

            return writer.WriteValueAsync(name, JsonPrimitive.FromObject(instance, settings));
        }

        /// <inheritdoc />
        public override object ConvertToObject(Node node, IReadOnlyConverterSettings settings) {
            if (node.Type == NodeTypes.Empty)
                return Type.GetDefaultValue();

            if (node is NodeValue nodeValue && nodeValue.Value is JsonPrimitive primitive)
                return primitive.ToObject(Type, settings);
            throw new ArgumentException("Node is not a NodeValue");
        }
    }
}
