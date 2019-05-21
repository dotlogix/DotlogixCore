// ==================================================
// Copyright 2018(C) , DotLogix
// File:  INodeConverter.cs
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
    public interface IAsyncNodeConverter {
        Type Type { get; }
        DataType DataType { get; }
        ValueTask WriteAsync(object instance, string rootName, IAsyncNodeWriter writer);
        object ConvertToObject(Node node, ConverterSettings settings);
    }
}
