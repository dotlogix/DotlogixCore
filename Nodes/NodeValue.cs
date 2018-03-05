// ==================================================
// Copyright 2018(C) , DotLogix
// File:  NodeValue.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
using DotLogix.Core.Extensions;
using DotLogix.Core.Types;
#endregion

namespace DotLogix.Core.Nodes {
    public class NodeValue : Node {
        private DataType _dataType;

        private object _value;

        public DataType DataType => _dataType ?? (_dataType = Value.GetDataType());

        public object Value {
            get => _value;
            set {
                Type = value == null ? NodeTypes.Empty : NodeTypes.Value;
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

        public object ConvertValue(Type type) {
            return Value.ConvertTo(type);
        }

        public TValue ConvertValue<TValue>() {
            return Value.ConvertTo<TValue>();
        }

        public bool TryConvertValue(Type type, out object value) {
            return Value.TryConvertTo(type, out value);
        }

        public bool TryConvertValue<TValue>(out TValue value) {
            return Value.TryConvertTo(out value);
        }
    }
}
