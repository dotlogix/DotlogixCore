// ==================================================
// Copyright 2018(C) , DotLogix
// File:  INodeConverterFactory.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using DotLogix.Core.Nodes.Converters;
using DotLogix.Core.Types;
#endregion

namespace DotLogix.Core.Nodes.Factories {
    /// <summary>
    /// An interface representing a factory to create node converters
    /// </summary>
    public interface INodeConverterFactory {
        /// <summary>
        /// Create a new node converter
        /// </summary>
        IAsyncNodeConverter CreateConverter(NodeTypes nodeType, DataType dataType);
        /// <summary>
        /// Try to create a new node converter
        /// </summary>
        bool TryCreateConverter(NodeTypes nodeType, DataType dataType, out IAsyncNodeConverter converter);
    }
}
