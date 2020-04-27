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
        private readonly MemberSettings _keySettings;
        private readonly MemberSettings _valueSettings;
        private const string KeyNodeName = "Key";
        private const string ValueNodeName = "Value";

        private readonly DynamicCtor _defaultCtor;
        /// <summary>
        /// Creates a new instance of <see cref="KeyValuePairNodeConverter"/>
        /// </summary>
        public KeyValuePairNodeConverter(TypeSettings typeSettings, MemberSettings keySettings, MemberSettings valueSettings) : base(typeSettings) {
            _defaultCtor = typeSettings.DynamicType.DefaultConstructor;
            _keySettings = keySettings;
            _valueSettings = valueSettings;
        }

        /// <inheritdoc />
        public override async ValueTask WriteAsync(object instance, string name, IAsyncNodeWriter writer, IReadOnlyConverterSettings settings) {
            var keyFieldValue = _keySettings.Accessor.GetValue(instance);
            var valueFieldValue = _valueSettings.Accessor.GetValue(instance);

            await writer.BeginMapAsync(name).ConfigureAwait(false);
            await _keySettings.Converter.WriteAsync(keyFieldValue, GetMemberName(_keySettings, settings), writer, settings).ConfigureAwait(false);
            await _valueSettings.Converter.WriteAsync(valueFieldValue, GetMemberName(_valueSettings, settings), writer, settings).ConfigureAwait(false);
            await writer.EndMapAsync().ConfigureAwait(false);
        }

        /// <inheritdoc />
        public override object ConvertToObject(Node node, IReadOnlyConverterSettings settings) {
            if(!(node is NodeMap nodeMap))
                throw new ArgumentException($"Expected node of type \"NodeMap\" got \"{node.Type}\"");

            var keyNode = nodeMap.GetChild(GetMemberName(_keySettings, settings));
            if(keyNode == null)
                throw new ArgumentException("KeyNode is not defined");

            var valueNode = nodeMap.GetChild(GetMemberName(_valueSettings, settings));
            if(valueNode == null)
                throw new ArgumentException("ValueNode is not defined");

            var keyFieldValue = _keySettings.Converter.ConvertToObject(keyNode, settings);
            var valueFieldValue = _valueSettings.Converter.ConvertToObject(valueNode, settings);

            var instance = _defaultCtor.Invoke();
            _keySettings.Accessor.SetValue(instance, keyFieldValue);
            _valueSettings.Accessor.SetValue(instance, valueFieldValue);
            return instance;
        }
    }
}
