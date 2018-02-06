using System;
using DotLogix.Core.Extensions;
using DotLogix.Core.Types;

namespace DotLogix.Core.Nodes {
    public class NodeValue : Node {
        private DataType _dataType;

        public DataType DataType => _dataType ?? (_dataType = Value.GetDataType());

        private object _value;

        public object Value {
            get => _value;
            set {
                NodeType = value == null ? NodeTypes.Empty : NodeTypes.Value;
                _value = value;
                _dataType = null;
            }
        }

        
        internal NodeValue(string name, object value, DataType dataType) : this(name, value) {
            _dataType = dataType;
        }

        public NodeValue(string name, object value) : this(value) {
            Name = name;
            Value = value;
        }
        public NodeValue(object value) : this() {
            Value = value;
        }

        public NodeValue() : base(NodeTypes.Empty) { }

        public object ConvertValue(Type type)
        {
            return Value.ConvertTo(type);
        }

        public TValue ConvertValue<TValue>()
        {
            return Value.ConvertTo<TValue>();
        }

        public bool TryConvertValue(Type type, out object value)
        {
            return Value.TryConvertTo(type, out value);
        }

        public bool TryConvertValue<TValue>(out TValue value)
        {
            return Value.TryConvertTo(out value);
        }
    }
}