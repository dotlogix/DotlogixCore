using System;

namespace DotLogix.Core.Nodes.Processor {
    /// <summary>
    /// An interface to represent node readers
    /// </summary>
    public interface INodeReader : IDisposable {
        /// <summary>
        /// Reads a map start operation with an optional name argument
        /// </summary>
       string ReadName();
        /// <summary>
        /// Reads a map start operation with an optional name argument
        /// </summary>
        void ReadBeginMap();
        /// <summary>
        /// Reads a map end operation
        /// </summary>
        void ReadEndMap();

        /// <summary>
        /// Reads a list start operation with an optional name argument
        /// </summary>
        void ReadBeginList();
        /// <summary>
        /// Reads a list end operation
        /// </summary>
        void ReadEndList();

        /// <summary>
        /// Reads a primitive value token with a name argument
        /// </summary>
       object ReadValue();

        /// <summary>
        /// Reads an operation token
        /// </summary>
       NodeOperation ReadOperation();

        /// <summary>
        /// Reads an operation token without consuming it
        /// </summary>
       NodeOperation? PeekOperation();
        
        /// <summary>
        /// Copies all remaining operation tokens to a node writer
        /// </summary>
        void CopyTo(INodeWriter writer);

    }
}