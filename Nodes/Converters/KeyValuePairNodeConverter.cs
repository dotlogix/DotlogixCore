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
using System.Threading.Tasks;
using DotLogix.Core.Extensions;
using DotLogix.Core.Nodes.Processor;
using DotLogix.Core.Reflection.Dynamics;
using DotLogix.Core.Utils.Naming;

#endregion

namespace DotLogix.Core.Nodes.Converters {
    /// <summary>
    /// An implementation of the <see cref="IAsyncNodeConverter"/> interface to convert key value pairs
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
        public override async Task WriteAsync(object instance, IAsyncNodeWriter writer, IReadOnlyConverterSettings settings) {
            var keyFieldValue = _keySettings.Accessor.GetValue(instance);
            var valueFieldValue = _valueSettings.Accessor.GetValue(instance);

            var (keyName, valueName) = EnsureMemberNames(settings.NamingStrategy);

            await writer.WriteBeginMapAsync().ConfigureAwait(false);

            await writer.WriteNameAsync(keyName).ConfigureAwait(false);
            await _keySettings.Converter.WriteAsync(keyFieldValue, writer, settings).ConfigureAwait(false);

            await writer.WriteNameAsync(valueName).ConfigureAwait(false);
            await _valueSettings.Converter.WriteAsync(valueFieldValue, writer, settings).ConfigureAwait(false);

            await writer.WriteEndMapAsync().ConfigureAwait(false);
        }

        /// <inheritdoc />
        public override async Task<object> ReadAsync(IAsyncNodeReader reader, IReadOnlyConverterSettings settings) {
            var next = await reader.PeekOperationAsync().ConfigureAwait(false);
            if (next.HasValue == false || (next.Value.Type == NodeOperationTypes.Value && next.Value.Value == null))
                return _defaultCtor.Invoke();

            Optional<object> key = default;
            Optional<object> value = default;
            var (keyName, valueName) = EnsureMemberNames(settings.NamingStrategy);

            await reader.ReadBeginMapAsync().ConfigureAwait(false);
            for(var i = 0; i < 2; i++) {
                var name = await reader.ReadNameAsync().ConfigureAwait(false);
                if(key.IsUndefined && name == keyName) {
                    key = await _keySettings.Converter.ReadAsync(reader, settings).ConfigureAwait(false);
                } else if(value.IsUndefined && name == valueName) {
                    value = await _valueSettings.Converter.ReadAsync(reader, settings).ConfigureAwait(false);
                }
            }
            await reader.ReadEndMapAsync().ConfigureAwait(false);

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
