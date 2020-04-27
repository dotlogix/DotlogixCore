// ==================================================
// Copyright 2018(C) , DotLogix
// File:  NodeOperation.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  03.03.2018
// LastEdited:  01.08.2018
// ==================================================

namespace DotLogix.Core.Nodes.Processor {
    public readonly struct NodeOperation {
        public readonly NodeOperationTypes Type;
        public readonly string Name;
        public readonly object Value;

        public NodeOperation(NodeOperationTypes type, string name = null, object value = null) {
            Type = type;
            Name = name;
            Value = value;
        }

        /// <summary>Returns the fully qualified type name of this instance.</summary>
        /// <returns>The fully qualified type name.</returns>
        public override string ToString() {
            return $"{Type} {{Name: {Name ?? "null"}, Value: {Value?.ToString() ?? "null"}}}";
        }
    }
}
