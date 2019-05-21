// ==================================================
// Copyright 2018(C) , DotLogix
// File:  NodeConverter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Threading.Tasks;
using DotLogix.Core.Nodes.Processor;
using DotLogix.Core.Types;
#endregion

namespace DotLogix.Core.Nodes.Converters {
    public abstract class NodeConverter : IAsyncNodeConverter {
        protected NodeConverter(DataType dataType) {
            DataType = dataType;
            Type = dataType.Type;
        }

        public Type Type { get; }
        public DataType DataType { get; }

        public abstract ValueTask WriteAsync(object instance, string rootName, IAsyncNodeWriter writer);

        public abstract object ConvertToObject(Node node, ConverterSettings settings);
    }
}
