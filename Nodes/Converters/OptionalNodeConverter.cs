// ==================================================
// Copyright 2018(C) , DotLogix
// File:  OptionalNodeConverter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  16.07.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Threading.Tasks;
using DotLogix.Core.Nodes.Processor;
using DotLogix.Core.Types;
#endregion

namespace DotLogix.Core.Nodes.Converters {
    /// <summary>
    /// An implementation of the <see cref="IAsyncNodeConverter"/> interface to optional values
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class OptionalNodeConverter<TValue> : NodeConverter {
        /// <summary>
        /// Creates a new instance of <see cref="OptionalNodeConverter{TValue}"/>
        /// </summary>
        public OptionalNodeConverter(TypeSettings typeSettings) : base(typeSettings) { }

        /// <inheritdoc />
        public override ValueTask WriteAsync(object instance, string name, IAsyncNodeWriter writer, ConverterSettings settings) {
            var opt = (Optional<TValue>)instance;
            if(opt.IsDefined == false || TypeSettings.ShouldEmitValue(opt.Value, settings) == false)
                return default;
            if(settings.Resolver.TryResolve(typeof(TValue), out var optionalTypeSettings)) {
                optionalTypeSettings.Converter.WriteAsync(opt.Value, name, writer, settings);
            } else {
                throw new NotSupportedException($"Can not resolve a converter for optional value type {typeof(TValue).Name}");
            }

            return default;
        }

        /// <inheritdoc />
        public override object ConvertToObject(Node node, ConverterSettings settings) {
            if(node.Type == NodeTypes.Empty)
                return new Optional<TValue>(default);

            if(settings.Resolver.TryResolve(typeof(TValue), out var optionalTypeSettings) == false)
                throw new NotSupportedException($"Can not resolve a converter for optional value type {typeof(TValue).Name}");


            var optionalValue = optionalTypeSettings.Converter.ConvertToObject(node, settings);
            return new Optional<TValue>((TValue)optionalValue);

        }
    }
    
    /// <summary>
    /// An implementation of the <see cref="IAsyncNodeConverter"/> interface to node values
    /// </summary>
    public class NodeToNodeConverter : NodeConverter {
        /// <summary>
        /// Creates a new instance of <see cref="OptionalNodeConverter{TValue}"/>
        /// </summary>
        public NodeToNodeConverter(TypeSettings typeSettings, bool dynamic = false) : base(typeSettings) { }

        /// <inheritdoc />
        public override ValueTask WriteAsync(object instance, string name, IAsyncNodeWriter writer, ConverterSettings settings) {
            var reader = new NodeReader((Node)instance);
            return reader.CopyToAsync(writer);
        }

        /// <inheritdoc />
        public override object ConvertToObject(Node node, ConverterSettings settings) {
            return node;

        }
    }
}
