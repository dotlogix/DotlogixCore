// ==================================================
// Copyright 2018(C) , DotLogix
// File:  NodeConverter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
using DotLogix.Core.Nodes.Processor;
using DotLogix.Core.Types;
#endregion

namespace DotLogix.Core.Nodes.Converters {
    public abstract class NodeConverter : INodeConverter {
        protected NodeConverter(DataType dataType) {
            DataType = dataType;
            Type = dataType.Type;
        }

        public Type Type { get; }
        public DataType DataType { get; }

        public abstract void Write(object instance, string rootName, INodeWriter writer);

        public abstract object ConvertToObject(Node node);
    }
}
