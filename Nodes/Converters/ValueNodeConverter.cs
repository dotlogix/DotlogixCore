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
using DotLogix.Core.Types;
#endregion

namespace DotLogix.Core.Nodes.Converters {
    /// <summary>
    /// An implementation of the <see cref="IAsyncNodeConverter"/> interface to convert primitives
    /// </summary>
    public class ValueNodeConverter2 : NodeConverter {
        /// <summary>
        /// Creates a new instance of <see cref="ValueNodeConverter"/>
        /// </summary>
        public ValueNodeConverter2(TypeSettings typeSettings) : base(typeSettings) { }

        /// <inheritdoc />
        public override ValueTask WriteAsync(object instance, string name, IAsyncNodeWriter writer, IConverterSettings settings) {
            var scopedSettings = settings.GetScoped(TypeSettings);
            if (scopedSettings.ShouldEmitValue(instance) == false)
                return default;

            return writer.WriteValueAsync(name, instance);
        }

        /// <inheritdoc />
        public override object ConvertToObject(Node node, IConverterSettings settings) {
            if (node.Type == NodeTypes.Empty)
                return Type.GetDefaultValue();

            if (node is NodeValue nodeValue)
                return nodeValue.GetValue(Type);
            throw new ArgumentException($"Expected node of type \"NodeValue\" got \"{node.Type}\"");
        }
    }


    /// <summary>
    /// An implementation of the <see cref="IAsyncNodeConverter"/> interface to convert primitives
    /// </summary>
    public class ValueNodeConverter : NodeConverter {
        /// <summary>
        /// Creates a new instance of <see cref="ValueNodeConverter"/>
        /// </summary>
        public ValueNodeConverter(TypeSettings typeSettings) : base(typeSettings) { }

        /// <inheritdoc />
        public override ValueTask WriteAsync(object instance, string name, IAsyncNodeWriter writer, IConverterSettings settings) {
            var scopedSettings = settings.GetScoped(TypeSettings);
            if (scopedSettings.ShouldEmitValue(instance) == false)
                return default;

            return writer.WriteValueAsync(name, JsonPrimitive.FromObject(instance, settings));
        }

        /// <inheritdoc />
        public override object ConvertToObject(Node node, IConverterSettings settings) {
            if (node.Type == NodeTypes.Empty)
                return Type.GetDefaultValue();

            if (node is NodeValue nodeValue && nodeValue.Value is JsonPrimitive primitive)
                return primitive.ToObject(Type, settings);
            throw new ArgumentException("Node is not a NodeValue");
        }
    }
}
