// ==================================================
// Copyright 2018(C) , DotLogix
// File:  KeyValuePairNodeConverter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using DotLogix.Core.Extensions;
using DotLogix.Core.Nodes.Processor;
using DotLogix.Core.Nodes.Schema;
using DotLogix.Core.Reflection.Dynamics;
using DotLogix.Core.Utils.Naming;

#endregion

namespace DotLogix.Core.Nodes.Converters {
    /// <summary>
    /// An implementation of the <see cref="INodeConverter"/> interface to convert key value pairs
    /// </summary>
    public class KeyValuePairNodeConverter : NodeConverter {
        private readonly MemberSettings _keySettings;
        private readonly MemberSettings _valueSettings;
        private const string KeyNodeName = "Key";
        private const string ValueNodeName = "Value";

        private readonly DynamicCtor _defaultCtor;
        private Dictionary<INamingStrategy, (string keyName, string valueName)> MemberNameCache { get; } = new Dictionary<INamingStrategy, (string keyName, string valueName)>();


        /// <summary>
        /// Creates a new instance of <see cref="KeyValuePairNodeConverter"/>
        /// </summary>
        public KeyValuePairNodeConverter(TypeSettings typeSettings, MemberSettings keySettings, MemberSettings valueSettings) : base(typeSettings) {
            _defaultCtor = typeSettings.DynamicType.DefaultConstructor;
            _keySettings = keySettings;
            _valueSettings = valueSettings;
        }

        /// <inheritdoc />
        public override void Write(object instance, INodeWriter writer, IReadOnlyConverterSettings settings) {
            var keyFieldValue = _keySettings.Accessor.GetValue(instance);
            var valueFieldValue = _valueSettings.Accessor.GetValue(instance);

            var (keyName, valueName) = EnsureMemberNames(settings.NamingStrategy);

            writer.WriteBeginMap();

            writer.WriteName(keyName);
            _keySettings.Converter.Write(keyFieldValue, writer, settings);

            writer.WriteName(valueName);
            _valueSettings.Converter.Write(valueFieldValue, writer, settings);

            writer.WriteEndMap();
        }

        /// <inheritdoc />
        public override object Read(INodeReader reader, IReadOnlyConverterSettings settings) {
            var next = reader.PeekOperation();
            if (next.HasValue == false || (next.Value.Type == NodeOperationTypes.Value && next.Value.Value == null))
                return _defaultCtor.Invoke();

            Optional<object> key = default;
            Optional<object> value = default;
            var (keyName, valueName) = EnsureMemberNames(settings.NamingStrategy);

            reader.ReadBeginMap();
            for(var i = 0; i < 2; i++) {
                var name = reader.ReadName();
                if(key.IsUndefined && name == keyName) {
                    key = _keySettings.Converter.Read(reader, settings);
                } else if(value.IsUndefined && name == valueName) {
                    value = _valueSettings.Converter.Read(reader, settings);
                }
            }
            reader.ReadEndMap();

            if (key.IsUndefined)
                throw new ArgumentException("KeyNode is not defined");
            if (value.IsUndefined)
                throw new ArgumentException("ValueNode is not defined");

            var instance = _defaultCtor.Invoke();
            _keySettings.Accessor.SetValue(instance, key.Value);
            _valueSettings.Accessor.SetValue(instance, value.Value);
            return instance;
        }

        /// <inheritdoc />
        public override object ConvertToObject(Node node, IReadOnlyConverterSettings settings) {
            if(!(node is NodeMap nodeMap))
                throw new ArgumentException($"Expected node of type \"NodeMap\" got \"{node.Type}\"");

            var (keyName, valueName) = EnsureMemberNames(settings.NamingStrategy);

            var keyNode = nodeMap.GetChild(keyName);
            if(keyNode == null)
                throw new ArgumentException("KeyNode is not defined");

            var valueNode = nodeMap.GetChild(valueName);
            if(valueNode == null)
                throw new ArgumentException("ValueNode is not defined");

            var keyFieldValue = _keySettings.Converter.ConvertToObject(keyNode, settings);
            var valueFieldValue = _valueSettings.Converter.ConvertToObject(valueNode, settings);

            var instance = _defaultCtor.Invoke();
            _keySettings.Accessor.SetValue(instance, keyFieldValue);
            _valueSettings.Accessor.SetValue(instance, valueFieldValue);
            return instance;
        }

        private (string keyName, string valueName) EnsureMemberNames(INamingStrategy namingStrategy) {
            (string keyName, string valueName) CreateMemberNames(INamingStrategy s) {
                return (GetMemberName(_keySettings, s), GetMemberName(_valueSettings, s));
            }

            return MemberNameCache.GetOrAdd(namingStrategy, CreateMemberNames);
        }
    }
}
