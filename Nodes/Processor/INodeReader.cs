using System.Collections.Generic;

namespace DotLogix.Core.Nodes.Processor {
    public interface INodeReader {
        void CopyTo(INodeWriter writer);
        IEnumerable<NodeOperation> Read();
    }
}