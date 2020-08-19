// ==================================================
// Copyright 2018(C) , DotLogix
// File:  INodeReader.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  03.03.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
#endregion

namespace DotLogix.Core.Nodes.Processor {
    /// <summary>
    /// An interface to represent node readers
    /// </summary>
    public interface IAsyncNodeReader2 : IDisposable {
        /// <summary>
        /// Copies all nodes to a node writer
        /// </summary>
        Task CopyToAsync(IAsyncNodeWriter writer);
        /// <summary>
        /// Read the nodes as node operations
        /// </summary>
        Task<IEnumerable<NodeOperation>> ReadAsync();
    }
}
