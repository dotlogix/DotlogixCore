using System.Collections.Generic;

namespace DotLogix.Core.Nodes.Processor {
    public abstract class NodeReaderBase : INodeReader {
        public abstract void CopyTo(INodeWriter writer);

        public IEnumerable<NodeOperation> Read() {
            var writer = new NodeOperationWriter();
            CopyTo(writer);
            return writer.Operations;
        }
    }
}