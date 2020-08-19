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
        private NodeOperation? _next;
        public NodeOperation Current { get; protected set; }

        #region Async
        /// <inheritdoc />
        public async Task<string> ReadNameAsync() {
            var next = await PeekOperationAsync().ConfigureAwait(false);
            return next?.Name;
        }

        public Task ReadBeginMapAsync()
        {
            return ReadBeginAnyAsync(NodeOperationTypes.BeginMap);
        }

        public Task ReadEndMapAsync() {
            return ReadEndAnyAsync(NodeOperationTypes.EndMap);
        }

        public Task ReadBeginListAsync() {
            return ReadBeginAnyAsync(NodeOperationTypes.BeginList);
        }

        public Task ReadEndListAsync() {
            return ReadEndAnyAsync(NodeOperationTypes.EndList);
        }

        public async Task<object> ReadValueAsync() {
            if (await MoveNextAsync().ConfigureAwait(false) && Current.Type == NodeOperationTypes.Value)
                return Current.Value;
            throw new InvalidOperationException($"Expected operation {nameof(NodeOperationTypes.Value)} but got {Current.Type}");
        }

        public async Task<NodeOperation> ReadOperationAsync() {
            if (await MoveNextAsync().ConfigureAwait(false) && Current.Type == NodeOperationTypes.Value)
                return Current;
            throw new InvalidOperationException("Expected operation but got none");
        }

        public async Task<NodeOperation?> PeekOperationAsync() {
            if (_next.HasValue) {
                return _next.Value;
            }

            _next = await ReadNextAsync().ConfigureAwait(false);
            return _next;
        }

        public async Task CopyToAsync(IAsyncNodeWriter writer)
        {
            while (await MoveNextAsync().ConfigureAwait(false))
                await writer.WriteOperationAsync(Current).ConfigureAwait(false);
        }

        private async Task ReadBeginAnyAsync(NodeOperationTypes expectedTypes)
        {
            if (await MoveNextAsync().ConfigureAwait(false) && (Current.Type & expectedTypes) != 0)
                return;
            throw new InvalidOperationException($"Expected operation {expectedTypes} but got {Current.Type}");
        }
        private async Task ReadEndAnyAsync(NodeOperationTypes expectedTypes)
        {
            if (await MoveNextAsync().ConfigureAwait(false) && (Current.Type & expectedTypes) != 0)
                return;
            throw new InvalidOperationException($"Expected operation {expectedTypes} but got {Current.Type}");
        }

        public async Task<bool> MoveNextAsync() {
            if (_next.HasValue) {
                Current = _next.Value;
                _next = null;
                return true;
            }

            var next = await ReadNextAsync().ConfigureAwait(false);
            if(next.HasValue == false)
                return false;

            Current = next.Value;
            _next = null;
            return true;
        }

        protected abstract Task<NodeOperation?> ReadNextAsync();

        #endregion

        protected virtual void Dispose(bool disposing) { }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
