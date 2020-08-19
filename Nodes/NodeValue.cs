// ==================================================
// Copyright 2018(C) , DotLogix
// File:  NodeValue.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  16.07.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using DotLogix.Core.Extensions;
using DotLogix.Core.Nodes.Processor;
using DotLogix.Core.Types;
#endregion

namespace DotLogix.Core.Nodes {
    public class NodeValue : Node {
        private DataType _dataType;

        private object _value;

        public override NodeTypes Type => Value == null ? NodeTypes.Empty : NodeTypes.Value;

        public DataType DataType => _dataType ??= Value.GetDataType();

        public object Value {
            get => _value;
            set {
                _value = value;
                _dataType = null;
            }
        }

        public NodeValue(object value, DataType dataType = null) {
            _value = value;
            _dataType = dataType;
        }

        #region Helper
        protected override ICollection<string> GetFormattingMembers() {
            var c = base.GetFormattingMembers();
            c.Add($"Value: {Value?.ToString() ?? "null"}");
            return c;
        }
        #endregion

        #region GetValue
        public object GetValue(Type targetType) {
            return GetValue(targetType, default);
        }

        public object GetValue(Type targetType, object defaultValue) {
            return Value.TryConvertTo(targetType, out var value)
                   ? value
                   : defaultValue;
        }

        public TValue GetValue<TValue>() {
            return GetValue(default(TValue));
        }

        public TValue GetValue<TValue>(TValue defaultValue) {
            if(TryGetValue(out TValue value))
                return value;
            return defaultValue;
        }

        public bool TryGetValue<TValue>(out TValue value) {
            if(TryGetValue(typeof(TValue), out var obj)) {
                value = (TValue)obj;
                return true;
            }

            value = default;
            return false;
        }

        public bool TryGetValue(Type targetType, out object value) {
            return Value.TryConvertTo(targetType, out value);
        }
        #endregion

        #region ConvertValue
        public object ConvertValue(Type targetType) {
            return ConvertValue(targetType, default);
        }

        public object ConvertValue(Type targetType, object defaultValue) {
            return Value = GetValue(targetType, defaultValue);
        }

        public TValue ConvertValue<TValue>() {
            return ConvertValue(default(TValue));
        }

        public TValue ConvertValue<TValue>(TValue defaultValue) {
            var value = GetValue(defaultValue);
            Value = value;
            return value;
        }

        public bool TryConvertValue(Type targetType, out object value) {
            if(TryGetValue(targetType, out value) == false)
                return false;
            Value = value;
            return true;
        }

        public bool TryConvertValue<TValue>(out TValue value) {
            if(TryGetValue(out value) == false)
                return false;
            Value = value;
            return true;
        }
        #endregion
    }
}
