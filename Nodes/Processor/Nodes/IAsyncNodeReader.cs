using System.Threading.Tasks;

namespace DotLogix.Core.Nodes.Processor {
    /// <summary>
    /// An interface to represent node readers
    /// </summary>
    public interface IAsyncNodeReader {
        /// <summary>
        /// Reads a map start operation with an optional name argument
        /// </summary>
        ValueTask<string> ReadNameAsync();
        /// <summary>
        /// Reads a map start operation with an optional name argument
        /// </summary>
        ValueTask ReadBeginMapAsync();
        /// <summary>
        /// Reads a map end operation
        /// </summary>
        ValueTask ReadEndMapAsync();

        /// <summary>
        /// Reads a list start operation with an optional name argument
        /// </summary>
        ValueTask ReadBeginListAsync();
        /// <summary>
        /// Reads a list end operation
        /// </summary>
        ValueTask ReadEndListAsync();

        /// <summary>
        /// Reads a primitive value token with a name argument
        /// </summary>
        ValueTask<object> ReadValueAsync();

        /// <summary>
        /// Reads an operation token
        /// </summary>
        ValueTask<NodeOperation> ReadOperationAsync();

        /// <summary>
        /// Reads an operation token without consuming it
        /// </summary>
        ValueTask<NodeOperation?> PeekOperationAsync();
        
        /// <summary>
        /// Copies all remaining operation tokens to a node writer
        /// </summary>
        ValueTask CopyToAsync(IAsyncNodeWriter writer);

    }
}