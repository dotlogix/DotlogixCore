// ==================================================
// Copyright 2018(C) , DotLogix
// File:  NodeOperationWriter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  22.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Collections.Generic;
using System.Threading.Tasks;
#endregion

namespace DotLogix.Core.Nodes.Processor {
    public class NodeOperationWriter : IAsyncNodeWriter {
        private readonly List<NodeOperation> _operations = new List<NodeOperation>();

        public IEnumerable<NodeOperation> Operations => _operations;

        public ValueTask BeginMapAsync() {
            _operations.Add(new NodeOperation(NodeOperationTypes.BeginMap));
            return default;
        }

        public ValueTask BeginMapAsync(string name) {
            _operations.Add(new NodeOperation(NodeOperationTypes.BeginMap, name));
            return default;
        }

        public ValueTask EndMapAsync() {
            _operations.Add(new NodeOperation(NodeOperationTypes.EndMap));
            return default;
        }

        public ValueTask BeginListAsync() {
            _operations.Add(new NodeOperation(NodeOperationTypes.BeginList));
            return default;
        }

        public ValueTask BeginListAsync(string name) {
            _operations.Add(new NodeOperation(NodeOperationTypes.BeginList, name));
            return default;
        }

        public ValueTask EndListAsync() {
            _operations.Add(new NodeOperation(NodeOperationTypes.EndList));
            return default;
        }

        public ValueTask WriteValueAsync(string name, object value) {
            _operations.Add(new NodeOperation(NodeOperationTypes.WriteValue, name, value));
            return default;
        }

        public ValueTask WriteValueAsync(object value) {
            _operations.Add(new NodeOperation(NodeOperationTypes.WriteValue, value: value));
            return default;
        }

        public ValueTask AutoCompleteAsync() {
            _operations.Add(new NodeOperation(NodeOperationTypes.AutoComplete));
            return default;
        }

        public ValueTask ExecuteAsync(NodeOperation operation) {
            _operations.Add(operation);
            return default;
        }
    }
}
