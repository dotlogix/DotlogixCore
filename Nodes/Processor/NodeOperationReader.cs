// ==================================================
// Copyright 2018(C) , DotLogix
// File:  NodeOperationReader.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  23.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Collections.Generic;
#endregion

namespace DotLogix.Core.Nodes.Processor {
    public class NodeOperationReader : INodeReader {
        public IEnumerable<NodeOperation> Operations { get; }

        public NodeOperationReader(IEnumerable<NodeOperation> operations) {
            Operations = operations;
        }


        public void CopyTo(INodeWriter writer) {
            foreach(var nodeOperation in Operations)
                writer.Execute(nodeOperation);
        }

        public IEnumerable<NodeOperation> Read() {
            return Operations;
        }
    }
}
