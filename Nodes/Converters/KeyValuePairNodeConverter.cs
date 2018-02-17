// ==================================================
// Copyright 2018(C) , DotLogix
// File:  KeyValuePairNodeConverter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
using System.Reflection;
using DotLogix.Core.Nodes.Io;
using DotLogix.Core.Reflection.Dynamics;
using DotLogix.Core.Types;
#endregion

namespace DotLogix.Core.Nodes.Converters {
    public class KeyValuePairNodeConverter : NodeConverter {
        private const string KeyNodeName = "Key";
        private const string ValueNodeName = "Value";

        private readonly DynamicCtor _defaultCtor;
        private readonly DynamicField _keyField;
        private readonly DynamicField _valueField;

        public KeyValuePairNodeConverter(DataType type) : base(type) {
            var dynamicType = Type.CreateDynamicType(MemberTypes.Field | MemberTypes.Constructor);
            _defaultCtor = dynamicType.GetDefaultConstructor();
            _keyField = dynamicType.GetField("key");
            _valueField = dynamicType.GetField("value");
        }

        public override void Write(object instance, string rootName, INodeWriter writer) {
            var keyFieldValue = _keyField.GetValue(instance);
            var valueFieldValue = _valueField.GetValue(instance);

            writer.BeginMap(rootName);
            Nodes.WriteToInternal(KeyNodeName, keyFieldValue, _keyField.ValueType, writer);
            Nodes.WriteToInternal(ValueNodeName, valueFieldValue, _valueField.ValueType, writer);
            writer.EndMap();
        }

        public override object ConvertToObject(Node node) {
            if(!(node is NodeMap nodeMap))
                throw new ArgumentException("Node is not a NodeMap");

            var keyNode = nodeMap.GetChild(KeyNodeName);
            if(keyNode == null)
                throw new ArgumentException("KeyNode is not defined");

            var valueNode = nodeMap.GetChild(ValueNodeName);
            if(valueNode == null)
                throw new ArgumentException("ValueNode is not defined");

            var keyFieldValue = Nodes.ToObject(keyNode, _keyField.ValueType);
            var valueFieldValue = Nodes.ToObject(valueNode, _valueField.ValueType);

            var instance = _defaultCtor.Invoke();
            _keyField.SetValue(instance, keyFieldValue);
            _valueField.SetValue(instance, valueFieldValue);
            return instance;
        }
    }
}
