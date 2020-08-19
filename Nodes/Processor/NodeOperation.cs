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

        public bool Equals(NodeOperation other) {
            return Type == other.Type && Name == other.Name && Equals(Value, other.Value);
        }

        /// <inheritdoc />
        public override bool Equals(object obj) {
            return obj is NodeOperation other && Equals(other);
        }

        /// <inheritdoc />
        public override int GetHashCode() {
            unchecked {
                var hashCode = (int)Type;
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Value != null ? Value.GetHashCode() : 0);
                return hashCode;
            }
        }

        /// <summary>Returns the fully qualified type name of this instance.</summary>
        /// <returns>The fully qualified type name.</returns>
        public override string ToString() {
            return $"{Type} {{Name: {Name ?? "null"}, Value: {Value?.ToString() ?? "null"}}}";
        }
    }
}
