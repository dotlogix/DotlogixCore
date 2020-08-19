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
    public interface IAsyncNodeWriter2 {
        /// <summary>
        /// Starts a new map node
        /// </summary>
        Task BeginMapAsync();
        /// <summary>
        /// Starts a new map node with a name
        /// </summary>
        Task BeginMapAsync(string name);
        /// <summary>
        /// End the current map node
        /// </summary>
        Task EndMapAsync();

        /// <summary>
        /// Starts a new list node
        /// </summary>
        Task BeginListAsync();
        /// <summary>
        /// Starts a new list node with a name
        /// </summary>
        Task BeginListAsync(string name);
        /// <summary>
        /// End the current list node
        /// </summary>
        Task EndListAsync();

        /// <summary>
        /// Writes a primitive value with a name
        /// </summary>
        Task WriteValueAsync(string name, object value);
        /// <summary>
        /// Writes a primitive value
        /// </summary>
        Task WriteValueAsync(object value);
        /// <summary>
        /// Auto close all open collections and finish the writing process
        /// </summary>
        Task AutoCompleteAsync();
        /// <summary>
        /// WriteOperation a node operation
        /// </summary>
        Task ExecuteAsync(NodeOperation operation);
    }


    /// <summary>
    /// An interface to represent node writers
    /// </summary>
    public interface IAsyncNodeWriter {
        /// <summary>
        /// Writes a name token
        /// </summary>
        Task WriteNameAsync(string name);

        /// <summary>
        /// Writes a map start token
        /// </summary>
        Task WriteBeginMapAsync();
        
        /// <summary>
        /// Writes a map end operation
        /// </summary>
        Task WriteEndMapAsync();

        /// <summary>
        /// Writes a list start token
        /// </summary>
        Task WriteBeginListAsync();
        
        /// <summary>
        /// Writes a list end operation
        /// </summary>
        Task WriteEndListAsync();
        
        /// <summary>
        /// Writes a primitive value token
        /// </summary>
        Task WriteValueAsync(object value);

        /// <summary>
        /// Writes an operation token
        /// </summary>
        Task WriteOperationAsync(NodeOperation operation);
    }


    /// <summary>
    /// An interface to represent node writers
    /// </summary>
    public interface INodeWriter {
        /// <summary>
        /// Writes a map start token
        /// </summary>
        void WriteBeginMap();

        /// <summary>
        /// Writes a map start operation with a name argument
        /// </summary>
        void WriteBeginMap(string name);

        /// <summary>
        /// Writes a map end operation
        /// </summary>
        void WriteEndMap();

        /// <summary>
        /// Writes a list start token
        /// </summary>
        void WriteBeginList();

        /// <summary>
        /// Writes a list start token with a name argument
        /// </summary>
        void WriteBeginList(string name);

        /// <summary>
        /// Writes a list end operation
        /// </summary>
        void WriteEndList();

        /// <summary>
        /// Writes a primitive value token with a name argument
        /// </summary>
        void WriteValue(string name, object value);

        /// <summary>
        /// Writes a primitive value token
        /// </summary>
        void WriteValue(object value);

        /// <summary>
        /// Writes an operation token
        /// </summary>
        void WriteOperation(NodeOperation operation);
    }

    /// <summary>
    /// An interface to represent node readers
    /// </summary>
    public interface IAsyncNodeReader {
        /// <summary>
        /// Reads a map start operation with an optional name argument
        /// </summary>
        Task<string> ReadNameAsync();
        /// <summary>
        /// Reads a map start operation with an optional name argument
        /// </summary>
        Task ReadBeginMapAsync();
        /// <summary>
        /// Reads a map end operation
        /// </summary>
        Task ReadEndMapAsync();

        /// <summary>
        /// Reads a list start operation with an optional name argument
        /// </summary>
        Task ReadBeginListAsync();
        /// <summary>
        /// Reads a list end operation
        /// </summary>
        Task ReadEndListAsync();

        /// <summary>
        /// Reads a primitive value token with a name argument
        /// </summary>
        Task<object> ReadValueAsync();

        /// <summary>
        /// Reads an operation token
        /// </summary>
        Task<NodeOperation> ReadOperationAsync();

        /// <summary>
        /// Reads an operation token without consuming it
        /// </summary>
        Task<NodeOperation?> PeekOperationAsync();
        
        /// <summary>
        /// Copies all remaining operation tokens to a node writer
        /// </summary>
        Task CopyToAsync(IAsyncNodeWriter writer);

    }
}
