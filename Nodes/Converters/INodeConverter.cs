// ==================================================
// Copyright 2018(C) , DotLogix
// File:  INodeConverter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using DotLogix.Core.Nodes.Formats.Nodes;
using DotLogix.Core.Nodes.Schema;
using DotLogix.Core.Types;
#endregion

namespace DotLogix.Core.Nodes.Converters {
    /// <summary>
    /// An interface to represent a node converter
    /// </summary>
    public interface INodeConverter {
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
        /// Writes an object value to a node writer
        /// </summary>
        void Write(object instance, INodeWriter writer, IReadOnlyConverterSettings settings);

        /// <summary>
        /// Reads an object value from a node reader
        /// </summary>
       object Read(INodeReader reader, IReadOnlyConverterSettings settings);

        /// <summary>
        /// Convert the node to an object of the target type
        /// </summary>
        object ConvertToObject(Node node, IReadOnlyConverterSettings settings);
    }
}
