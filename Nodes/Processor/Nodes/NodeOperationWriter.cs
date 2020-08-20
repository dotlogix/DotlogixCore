// ==================================================
// Copyright 2018(C) , DotLogix
// File:  NodeOperationWriter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  22.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
#endregion

namespace DotLogix.Core.Nodes.Processor {
    public class NodeOperationWriter : IAsyncNodeWriter {
        private readonly List<NodeOperation> _operations = new List<NodeOperation>();

        public IEnumerable<NodeOperation> Operations => _operations;

        #region Async

        protected string CurrentName { get; set; }
        
        public ValueTask WriteNameAsync(string name) {
            if (CurrentName != null)
                throw new InvalidOperationException($"Unexpected operation, property name is already set to {CurrentName}");
            CurrentName = name ?? throw new ArgumentNullException(nameof(name));
            return default;
        }

        public ValueTask WriteBeginMapAsync() {
            _operations.Add(new NodeOperation(NodeOperationTypes.BeginMap, CurrentName));
            CurrentName = null;
            return default;
        }

        public ValueTask WriteEndMapAsync() {
            _operations.Add(new NodeOperation(NodeOperationTypes.EndMap));
            return default;
        }

        public ValueTask WriteBeginListAsync() {
            _operations.Add(new NodeOperation(NodeOperationTypes.BeginList, CurrentName));
            CurrentName = null;
            return default;
        }

        public ValueTask WriteEndListAsync() {
            _operations.Add(new NodeOperation(NodeOperationTypes.EndList));
            return default;
        }

        public ValueTask WriteValueAsync(string name, object value) {
            _operations.Add(new NodeOperation(NodeOperationTypes.Value, name, value));
            return default;
        }

        public ValueTask WriteValueAsync(object value) {
            _operations.Add(new NodeOperation(NodeOperationTypes.Value, CurrentName, value));
            CurrentName = null;
            return default;
        }

        public ValueTask WriteOperationAsync(NodeOperation operation) {
            _operations.Add(operation);
            CurrentName = null;
            return default;
        }

        #endregion
    }
}
