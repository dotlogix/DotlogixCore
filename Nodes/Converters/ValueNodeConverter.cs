// ==================================================
// Copyright 2016(C) , DotLogix
// File:  ValueNodeConverter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  30.08.2017
// LastEdited:  06.09.2017
// ==================================================

#region
using System;
using DotLogix.Core.Nodes.Io;
using DotLogix.Core.Types;
#endregion

namespace DotLogix.Core.Nodes.Converters {
    public class ValueNodeConverter : NodeConverter {
        public ValueNodeConverter(DataType dynamicType) : base(dynamicType) { }

        public override void Write(object instance, string rootName, INodeWriter writer) {
            writer.WriteValue(instance, rootName);
        }

        public override object ConvertToObject(Node node) {
            var nodeValue = node as NodeValue;
            if(nodeValue != null)
                return nodeValue.ConvertValue(Type);
            throw new ArgumentException("Node is not a NodeValue");
        }
    }
}
