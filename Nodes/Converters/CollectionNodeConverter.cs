// ==================================================
// Copyright 2018(C) , DotLogix
// File:  CollectionNodeConverter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Linq;
using DotLogix.Core.Nodes.Formats;
using DotLogix.Core.Nodes.Formats.Nodes;
using DotLogix.Core.Nodes.Schema;
using DotLogix.Core.Reflection.Dynamics;
#endregion

namespace DotLogix.Core.Nodes.Converters {
    /// <summary>
    /// An implementation of the <see cref="INodeConverter"/> interface to convert arrays
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CollectionNodeConverter<T> : NodeConverter {
        private readonly DynamicCtor _ctor;

        /// <summary>
        /// Creates a new instance of <see cref="CollectionNodeConverter{T}"/>
        /// </summary>
        public CollectionNodeConverter(TypeSettings typeSettings) : base(typeSettings) {
            var enumerableType = typeof(IEnumerable<T>);
            var dynamicType = typeSettings.DynamicType;
            _ctor = dynamicType.GetConstructor(enumerableType);
            if(_ctor != null)
                return;

            _ctor = dynamicType.DefaultConstructor;
            if(_ctor == null)
                throw new InvalidOperationException($"Collection type has to define an empty constructor or one with single argument {enumerableType.FullName}");

        }

        /// <inheritdoc />
        public override void Write(object instance, INodeWriter writer, IReadOnlyConverterSettings settings) {
            var scopedSettings = settings.GetScoped(TypeSettings);
            var childConverter = TypeSettings.ChildSettings.Converter;

            if (scopedSettings.ShouldEmitValue(instance) == false)
                return;

            if (instance == null) {
                writer.WriteValue(null);
                return;
            }

            scopedSettings = settings.GetScoped(TypeSettings.ChildSettings);

            if (!(instance is IEnumerable<T> values))
                throw new ArgumentException($"Expected instance of type \"IEnumerable<T>\" got \"{instance.GetType()}\"");

            writer.WriteBeginList();

            foreach (var value in values) {
                childConverter.Write(value, writer, scopedSettings);
            }
            
            writer.WriteEndList();
        }

        /// <inheritdoc />
        public override object Read(INodeReader reader, IReadOnlyConverterSettings settings) {
            var next = reader.PeekOperation();
            if (next.HasValue == false || (next.Value.Type == NodeOperationTypes.Value && next.Value.Value == null)) {
                reader.ReadOperation();
                return default;
            }

            reader.ReadBeginList();

            var scopedSettings = settings.GetScoped(TypeSettings.ChildSettings);
            var childConverter = TypeSettings.ChildSettings.Converter;

            var collection = _ctor.IsDefault ? (ICollection<T>)_ctor.Invoke() : new List<T>();
            while (true) {
                next = reader.PeekOperation();
                if (next.HasValue == false || next.Value.Type == NodeOperationTypes.EndList)
                    break;

                var child = (T)childConverter.Read(reader, scopedSettings);
                collection.Add(child);
            }

            reader.ReadEndList();

            return Type.IsInstanceOfType(collection)
                       ? collection
                       : _ctor.Invoke(collection);
        }

        /// <inheritdoc />
        public override object ConvertToObject(Node node, IReadOnlyConverterSettings settings) {
            if (node.Type == NodeTypes.Empty)
                return default;

            if (!(node is NodeList nodeList))
                throw new ArgumentException($"Expected node of type \"NodeList\" got \"{node.Type}\"");

            var scopedSettings = settings.GetScoped(TypeSettings.ChildSettings);
            var childConverter = TypeSettings.ChildSettings.Converter;

            var children = nodeList.Children().ToArray();
            var childCount = children.Length;
            var array = new T[childCount];
            for(var i = 0; i < childCount; i++)
                array[i] = (T)childConverter.ConvertToObject(children[i], scopedSettings);

            if(_ctor.IsDefault == false)
                return _ctor.Invoke((IEnumerable<T>)array);

            var collection = (ICollection<T>)_ctor.Invoke();
            foreach(var value in array)
                collection.Add(value);
            return collection;
        }
    }
}
