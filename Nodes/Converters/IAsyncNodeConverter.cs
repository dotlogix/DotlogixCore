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
    /// <summary>
    /// An interface to represent a node converter
    /// </summary>
    public interface IAsyncNodeConverter {
        /// <summary>
        /// The type
        /// </summary>
        Type Type { get; }
        /// <summary>
        /// The data type
        /// </summary>
        DataType DataType { get; }
        /// <summary>
        /// The type settings
        /// </summary>
        TypeSettings TypeSettings { get; }

        /// <summary>
        /// Write the value to the node writer
        /// </summary>
        ValueTask WriteAsync(object instance, string name, IAsyncNodeWriter writer, IConverterSettings settings);

        /// <summary>
        /// Convert the node to an object of the target type
        /// </summary>
        object ConvertToObject(Node node, IConverterSettings settings);
    }
}
