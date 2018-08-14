// ==================================================
// Copyright 2018(C) , DotLogix
// File:  NodeReaderBase.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  03.03.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Collections.Generic;
#endregion

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
