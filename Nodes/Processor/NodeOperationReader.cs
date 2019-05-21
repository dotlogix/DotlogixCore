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
    public class NodeOperationReader : IAsyncNodeReader {
        public IEnumerable<NodeOperation> Operations { get; }

        public NodeOperationReader(IEnumerable<NodeOperation> operations) {
            Operations = operations;
        }


        public async ValueTask CopyToAsync(IAsyncNodeWriter writer) {
            foreach(var nodeOperation in Operations) {
                var task = writer.ExecuteAsync(nodeOperation);
                if(task.IsCompleted)
                    await task;
            }
                
        }

        public ValueTask<IEnumerable<NodeOperation>> ReadAsync() {
            return new ValueTask<IEnumerable<NodeOperation>>(Operations);
        }

        public void Dispose() { }
    }
}
