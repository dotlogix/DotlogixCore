// ==================================================
// Copyright 2018(C) , DotLogix
// File:  NodeReaderBase.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  03.03.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using DotLogix.Core.Nodes.Formats.Nodes;
#endregion

namespace DotLogix.Core.Nodes.Formats {
    public abstract class NodeReaderBase : INodeReader {
        private NodeOperation? _next;
        public NodeOperation Current { get; protected set; }

        #region 
        /// <inheritdoc />
        public string ReadName() {
            var next = PeekOperation();
            return next?.Name;
        }

        public void ReadBeginMap()
        {
            ReadBeginAny(NodeOperationTypes.BeginMap);
        }

        public void ReadEndMap() {
            ReadEndAny(NodeOperationTypes.EndMap);
        }

        public void ReadBeginList() {
            ReadBeginAny(NodeOperationTypes.BeginList);
        }

        public void ReadEndList() {
            ReadEndAny(NodeOperationTypes.EndList);
        }

        public object ReadValue() {
            if (MoveNext() && Current.Type == NodeOperationTypes.Value)
                return Current.Value;
            throw new InvalidOperationException($"Expected operation {nameof(NodeOperationTypes.Value)} but got {Current.Type}");
        }

        public NodeOperation ReadOperation() {
            if (MoveNext() && Current.Type == NodeOperationTypes.Value)
                return Current;
            throw new InvalidOperationException("Expected operation but got none");
        }

        public NodeOperation? PeekOperation() {
            if (_next.HasValue) {
                return _next.Value;
            }

            _next = ReadNext();
            return _next;
        }

        public void CopyTo(INodeWriter writer)
        {
            while (MoveNext())
                writer.WriteOperation(Current);
        }

        private void ReadBeginAny(NodeOperationTypes expectedTypes)
        {
            if (MoveNext() && (Current.Type & expectedTypes) != 0)
                return;
            throw new InvalidOperationException($"Expected operation {expectedTypes} but got {Current.Type}");
        }
        private void ReadEndAny(NodeOperationTypes expectedTypes)
        {
            if (MoveNext() && (Current.Type & expectedTypes) != 0)
                return;
            throw new InvalidOperationException($"Expected operation {expectedTypes} but got {Current.Type}");
        }

        public bool MoveNext() {
            if (_next.HasValue) {
                Current = _next.Value;
                _next = null;
                return true;
            }

            var next = ReadNext();
            if(next.HasValue == false)
                return false;

            Current = next.Value;
            _next = null;
            return true;
        }

        protected abstract NodeOperation? ReadNext();

        #endregion

        protected virtual void Dispose(bool disposing) { }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
