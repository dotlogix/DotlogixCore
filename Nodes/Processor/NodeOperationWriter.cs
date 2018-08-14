// ==================================================
// Copyright 2018(C) , DotLogix
// File:  NodeOperationWriter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  22.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Collections.Generic;
#endregion

namespace DotLogix.Core.Nodes.Processor {
    public class NodeOperationWriter : INodeWriter {
        private readonly List<NodeOperation> _operations = new List<NodeOperation>();

        public IEnumerable<NodeOperation> Operations => _operations;

        public void BeginMap() {
            _operations.Add(new NodeOperation(NodeOperationTypes.BeginMap));
        }

        public void BeginMap(string name) {
            _operations.Add(new NodeOperation(NodeOperationTypes.BeginMap, name));
        }

        public void EndMap() {
            _operations.Add(new NodeOperation(NodeOperationTypes.EndMap));
        }

        public void BeginList() {
            _operations.Add(new NodeOperation(NodeOperationTypes.BeginList));
        }

        public void BeginList(string name) {
            _operations.Add(new NodeOperation(NodeOperationTypes.BeginList, name));
        }

        public void EndList() {
            _operations.Add(new NodeOperation(NodeOperationTypes.EndList));
        }

        public void WriteValue(string name, object value) {
            _operations.Add(new NodeOperation(NodeOperationTypes.WriteValue, name, value));
        }

        public void WriteValue(object value) {
            _operations.Add(new NodeOperation(NodeOperationTypes.WriteValue, value: value));
        }

        public void AutoComplete() {
            _operations.Add(new NodeOperation(NodeOperationTypes.AutoComplete));
        }

        public void Execute(NodeOperation operation) {
            _operations.Add(operation);
        }
    }
}
