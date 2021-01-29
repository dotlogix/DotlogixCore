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
#endregion

namespace DotLogix.Core.Nodes.Formats.Nodes {
    public class NodeOperationWriter : INodeWriter {
        private readonly List<NodeOperation> _operations = new List<NodeOperation>();

        public IEnumerable<NodeOperation> Operations => _operations;

        #region 

        protected string CurrentName { get; set; }
        
        public void WriteName(string name) {
            if (CurrentName != null)
                throw new InvalidOperationException($"Unexpected operation, property name is already set to {CurrentName}");
            CurrentName = name ?? throw new ArgumentNullException(nameof(name));
        }

        public void WriteBeginMap() {
            _operations.Add(new NodeOperation(NodeOperationTypes.BeginMap, CurrentName));
            CurrentName = null;
        }

        public void WriteEndMap() {
            _operations.Add(new NodeOperation(NodeOperationTypes.EndMap));
        }

        public void WriteBeginList() {
            _operations.Add(new NodeOperation(NodeOperationTypes.BeginList, CurrentName));
            CurrentName = null;
        }

        public void WriteEndList() {
            _operations.Add(new NodeOperation(NodeOperationTypes.EndList));
        }

        public void WriteValue(string name, object value) {
            _operations.Add(new NodeOperation(NodeOperationTypes.Value, name, value));
        }

        public void WriteValue(object value) {
            _operations.Add(new NodeOperation(NodeOperationTypes.Value, CurrentName, value));
            CurrentName = null;
        }

        public void WriteOperation(NodeOperation operation) {
            _operations.Add(operation);
            CurrentName = null;
        }

        #endregion

        void IDisposable.Dispose()
        {
        }
    }
}
