// ==================================================
// Copyright 2018(C) , DotLogix
// File:  OptionalNodeConverter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  16.07.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Threading.Tasks;
using DotLogix.Core.Nodes.Processor;
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
        public override ValueTask WriteAsync(object instance, string name, IAsyncNodeWriter writer, IReadOnlyConverterSettings settings) {
            if(!(instance is Optional<TValue> opt))
                return default;

            if (opt.IsDefined == false)
                return default;

            var scopedSettings = settings.GetScoped(TypeSettings.ChildSettings);
            var childConverter = TypeSettings.ChildSettings.Converter;

            if (scopedSettings.ShouldEmitValue(opt.Value) == false)
                return default;

            return childConverter.WriteAsync(opt.Value, name, writer, scopedSettings);
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
