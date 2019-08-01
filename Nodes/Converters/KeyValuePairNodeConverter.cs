// ==================================================
// Copyright 2018(C) , DotLogix
// File:  KeyValuePairNodeConverter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Reflection;
using System.Threading.Tasks;
using DotLogix.Core.Nodes.Processor;
using DotLogix.Core.Reflection.Dynamics;
using DotLogix.Core.Types;
#endregion

namespace DotLogix.Core.Nodes.Converters {
    /// <summary>
    /// An implementation of the <see cref="IAsyncNodeConverter"/> interface to convert key value pairs
    /// </summary>
    public class KeyValuePairNodeConverter : NodeConverter {
        private const string KeyNodeName = "Key";
        private const string ValueNodeName = "Value";

        private readonly DynamicCtor _defaultCtor;
        private readonly DynamicField _keyField;
        private readonly DynamicField _valueField;
        /// <summary>
        /// Creates a new instance of <see cref="KeyValuePairNodeConverter"/>
        /// </summary>
        public KeyValuePairNodeConverter(DataType type) : base(type) {
            var dynamicType = Type.CreateDynamicType(MemberTypes.Field | MemberTypes.Constructor);
            _defaultCtor = dynamicType.GetDefaultConstructor();
            _keyField = dynamicType.GetField("key");
            _valueField = dynamicType.GetField("value");
        }

        /// <inheritdoc />
        public override async ValueTask WriteAsync(object instance, string rootName, IAsyncNodeWriter writer, ConverterSettings settings) {
            var keyFieldValue = _keyField.GetValue(instance);
            var valueFieldValue = _valueField.GetValue(instance);

            var task = writer.BeginMapAsync(rootName);
            if(task.IsCompletedSuccessfully == false)
                await task;

            task = Nodes.WriteToAsync(KeyNodeName, keyFieldValue, _keyField.ValueType, writer, settings);
            if(task.IsCompletedSuccessfully == false)
                await task;

            task = Nodes.WriteToAsync(ValueNodeName, valueFieldValue, _valueField.ValueType, writer, settings);
            if(task.IsCompletedSuccessfully == false)
                await task;

            task = writer.EndMapAsync();
            if(task.IsCompletedSuccessfully == false)
                await task;
        }

        /// <inheritdoc />
        public override object ConvertToObject(Node node, ConverterSettings settings) {
            if(!(node is NodeMap nodeMap))
                throw new ArgumentException("Node is not a NodeMap");

            var keyNode = nodeMap.GetChild(settings.NamingStrategy?.TransformName(KeyNodeName) ?? KeyNodeName);
            if(keyNode == null)
                throw new ArgumentException("KeyNode is not defined");

            var valueNode = nodeMap.GetChild(settings.NamingStrategy?.TransformName(ValueNodeName) ?? ValueNodeName);
            if(valueNode == null)
                throw new ArgumentException("ValueNode is not defined");

            var keyFieldValue = Nodes.ToObject(keyNode, _keyField.ValueType, settings);
            var valueFieldValue = Nodes.ToObject(valueNode, _valueField.ValueType, settings);

            var instance = _defaultCtor.Invoke();
            _keyField.SetValue(instance, keyFieldValue);
            _valueField.SetValue(instance, valueFieldValue);
            return instance;
        }
    }
}
