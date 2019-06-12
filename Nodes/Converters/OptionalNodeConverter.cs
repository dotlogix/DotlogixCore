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
        public OptionalNodeConverter(DataType dataType) : base(dataType) { }

        /// <inheritdoc />
        public override ValueTask WriteAsync(object instance, string rootName, IAsyncNodeWriter writer) {
            var opt = (Optional<TValue>)instance;
            return opt.IsDefined ? Nodes.WriteToAsync(rootName, opt.Value, typeof(TValue), writer) : default;
        }

        /// <inheritdoc />
        public override object ConvertToObject(Node node, ConverterSettings settings) {
            if(node.Type == NodeTypes.Empty)
                return new Optional<TValue>(default);

            var value = Nodes.ToObject<TValue>(node);
            return new Optional<TValue>(value);
        }
    }
}
