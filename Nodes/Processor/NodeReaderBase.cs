// ==================================================
// Copyright 2018(C) , DotLogix
// File:  NodeReaderBase.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  03.03.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
#endregion

namespace DotLogix.Core.Nodes.Processor {
    public abstract class NodeReaderBase : IAsyncNodeReader {
        public abstract ValueTask CopyToAsync(IAsyncNodeWriter writer);

        public async ValueTask<IEnumerable<NodeOperation>> ReadAsync() {
            var writer = new NodeOperationWriter();
            var task = CopyToAsync(writer);
            if(task.IsCompletedSuccessfully == false)
                await task;
            return writer.Operations;
        }

        protected virtual void Dispose(bool disposing) { }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
