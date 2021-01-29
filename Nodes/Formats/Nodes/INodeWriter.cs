using System;

namespace DotLogix.Core.Nodes.Formats.Nodes {
    /// <summary>
    /// An interface to represent node writers
    /// </summary>
    public interface INodeWriter : IDisposable {
        /// <summary>
        /// Writes a name token
        /// </summary> 
        void WriteName(string name);

        /// <summary>
        /// Writes a map start token
        /// </summary>
        void WriteBeginMap();

        /// <summary>
        /// Writes a map end operation
        /// </summary>
        void WriteEndMap();

        /// <summary>
        /// Writes a list start token
        /// </summary>
        void WriteBeginList();

        /// <summary>
        /// Writes a list end operation
        /// </summary>
        void WriteEndList();

        /// <summary>
        /// Writes a primitive value token
        /// </summary>
        void WriteValue(object value);

        /// <summary>
        /// Writes an operation token
        /// </summary>
        void WriteOperation(NodeOperation operation);
    }
}