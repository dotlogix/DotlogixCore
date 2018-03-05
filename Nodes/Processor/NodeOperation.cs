namespace DotLogix.Core.Nodes.Processor {
    public struct NodeOperation {
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