namespace DotLogix.Core.Nodes.Processor {
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
}