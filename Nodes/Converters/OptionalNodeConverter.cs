// ==================================================
// Copyright 2018(C) , DotLogix
// File:  OptionalNodeConverter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  16.07.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using DotLogix.Core.Nodes.Processor;
using DotLogix.Core.Types;
#endregion

namespace DotLogix.Core.Nodes.Converters {
    public class OptionalNodeConverter<TValue> : NodeConverter {
        public OptionalNodeConverter(DataType dataType) : base(dataType) { }

        public override void Write(object instance, string rootName, INodeWriter writer) {
            var opt = (Optional<TValue>)instance;
            if(opt.IsDefined)
                Nodes.WriteTo(rootName, opt.Value, typeof(TValue), writer);
        }

        public override object ConvertToObject(Node node) {
            if(node.Type == NodeTypes.Empty)
                return new Optional<TValue>(default);

            var value = Nodes.ToObject<TValue>(node);
            return new Optional<TValue>(value);
        }
    }
}
