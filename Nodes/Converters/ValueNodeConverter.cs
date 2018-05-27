// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ValueNodeConverter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
using DotLogix.Core.Extensions;
using DotLogix.Core.Nodes.Processor;
using DotLogix.Core.Types;
#endregion

namespace DotLogix.Core.Nodes.Converters {
    public class ValueNodeConverter : NodeConverter {
        public ValueNodeConverter(DataType dynamicType) : base(dynamicType) { }

        public override void Write(object instance, string rootName, INodeWriter writer) {
            writer.WriteValue(rootName, instance);
        }

        public override object ConvertToObject(Node node) {
            if (node.Type == NodeTypes.Empty)
                return Type.GetDefaultValue();

            if (node is NodeValue nodeValue)
                return nodeValue.ConvertValue(Type);
            throw new ArgumentException("Node is not a NodeValue");
        }
    }
}
