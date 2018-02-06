using System.Collections.Generic;

namespace DotLogix.Core.Nodes.Io {
    public abstract class NodeReaderBase : INodeReader
    {
        IEnumerable<NodeIoOp> INodeReader.EnumerateOps() {
            return EnumerateOps();
        }

        protected abstract IEnumerable<NodeIoOp> EnumerateOps();
    }
}