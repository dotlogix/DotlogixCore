using System.Collections.Generic;

namespace DotLogix.Core.Nodes.Io {
    public abstract class NodeReaderBase : INodeReader
    {
        public abstract void CopyTo(INodeWriter nodeWriter);
    }
}