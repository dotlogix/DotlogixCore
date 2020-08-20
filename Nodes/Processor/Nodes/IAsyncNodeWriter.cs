// ==================================================
// Copyright 2018(C) , DotLogix
// File:  INodeWriter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  03.03.2018
// LastEdited:  01.08.2018
// ==================================================

using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotLogix.Core.Nodes.Processor {
    /// <summary>
    /// An interface to represent node writers
    /// </summary>
    public interface IAsyncNodeWriter {
        /// <summary>
        /// Writes a name token
        /// </summary>
        ValueTask WriteNameAsync(string name);

        /// <summary>
        /// Writes a map start token
        /// </summary>
        ValueTask WriteBeginMapAsync();
        
        /// <summary>
        /// Writes a map end operation
        /// </summary>
        ValueTask WriteEndMapAsync();

        /// <summary>
        /// Writes a list start token
        /// </summary>
        ValueTask WriteBeginListAsync();
        
        /// <summary>
        /// Writes a list end operation
        /// </summary>
        ValueTask WriteEndListAsync();
        
        /// <summary>
        /// Writes a primitive value token
        /// </summary>
        ValueTask WriteValueAsync(object value);

        /// <summary>
        /// Writes an operation token
        /// </summary>
        ValueTask WriteOperationAsync(NodeOperation operation);
    }
}
