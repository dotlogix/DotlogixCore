// ==================================================
// Copyright 2016(C) , DotLogix
// File:  NodeConverter.cs
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
