// ==================================================
// Copyright 2018(C) , DotLogix
// File:  NodeOperationReader.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  23.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Collections.Generic;
using System.Threading.Tasks;
#endregion

namespace DotLogix.Core.Nodes.Processor {
    public class NodeOperationReader : NodeReaderBase {
        public IEnumerator<NodeOperation> Operations { get; }

        public NodeOperationReader(IEnumerable<NodeOperation> operations) {
            Operations = operations.GetEnumerator();
        }

        protected override Task<NodeOperation?> ReadNextAsync() {
            return Operations.MoveNext()
                       ? Task.FromResult<NodeOperation?>(Operations.Current)
                       : Task.FromResult<NodeOperation?>(null);
        }

        protected override void Dispose(bool disposing)
        {
            Operations.Dispose();
        }
    }
}
