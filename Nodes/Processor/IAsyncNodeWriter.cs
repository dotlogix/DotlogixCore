// ==================================================
// Copyright 2018(C) , DotLogix
// File:  INodeWriter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  03.03.2018
// LastEdited:  01.08.2018
// ==================================================

using System.Threading.Tasks;

namespace DotLogix.Core.Nodes.Processor {
    /// <summary>
    /// An interface to represent node writers
    /// </summary>
    public interface IAsyncNodeWriter {
        /// <summary>
        /// Starts a new map node
        /// </summary>
        ValueTask BeginMapAsync();
        /// <summary>
        /// Starts a new map node with a name
        /// </summary>
        ValueTask BeginMapAsync(string name);
        /// <summary>
        /// End the current map node
        /// </summary>
        ValueTask EndMapAsync();

        /// <summary>
        /// Starts a new list node
        /// </summary>
        ValueTask BeginListAsync();
        /// <summary>
        /// Starts a new list node with a name
        /// </summary>
        ValueTask BeginListAsync(string name);
        /// <summary>
        /// End the current list node
        /// </summary>
        ValueTask EndListAsync();

        /// <summary>
        /// Writes a primitive value with a name
        /// </summary>
        ValueTask WriteValueAsync(string name, object value);
        /// <summary>
        /// Writes a primitive value
        /// </summary>
        ValueTask WriteValueAsync(object value);
        /// <summary>
        /// Auto close all open collections and finish the writing process
        /// </summary>
        ValueTask AutoCompleteAsync();
        /// <summary>
        /// Execute a node operation
        /// </summary>
        ValueTask ExecuteAsync(NodeOperation operation);
    }
}
