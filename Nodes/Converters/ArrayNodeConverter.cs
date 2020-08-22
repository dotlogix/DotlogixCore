// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ArrayNodeConverter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Linq;
using DotLogix.Core.Nodes.Processor;
using DotLogix.Core.Nodes.Schema;

#endregion

namespace DotLogix.Core.Nodes.Converters {
    /// <summary>
    /// An implementation of the <see cref="INodeConverter"/> interface to convert arrays
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ArrayNodeConverter<T> : NodeConverter {
        /// <summary>
        /// Creates a new instance of <see cref="ArrayNodeConverter{T}"/>
        /// </summary>
        public ArrayNodeConverter(TypeSettings typeSettings) : base(typeSettings) { }

        /// <inheritdoc />
        public override void Write(object instance, INodeWriter writer, IReadOnlyConverterSettings settings) {
            var scopedSettings = settings.GetScoped(TypeSettings);
            var childConverter = TypeSettings.ChildSettings.Converter;
            

            if (scopedSettings.ShouldEmitValue(instance) == false)
                return;

            scopedSettings = settings.GetScoped(TypeSettings.ChildSettings);

            if (instance == null) {
                writer.WriteValue(null);
                return;
            }

            if (!(instance is IEnumerable<T> values))
                throw new ArgumentException($"Expected instance of type \"IEnumerable<T>\" got \"{instance.GetType()}\"", nameof(instance));

            writer.WriteBeginList();

            foreach (var value in values) {
                childConverter.Write(value, writer, scopedSettings);
            }

            writer.WriteEndList();
        }

        /// <inheritdoc />
        public override object Read(INodeReader reader, IReadOnlyConverterSettings settings) {
            var next = reader.PeekOperation();
            if (next.HasValue == false || (next.Value.Type == NodeOperationTypes.Value && next.Value.Value == null))
                return default;

            reader.ReadBeginList();

            var scopedSettings = settings.GetScoped(TypeSettings.ChildSettings);
            var childConverter = TypeSettings.ChildSettings.Converter;
            var collection = new List<T>();
            while (true) {
                next = reader.PeekOperation();
                if(next.HasValue == false || next.Value.Type == NodeOperationTypes.EndList)
                    break;

                var child = (T)childConverter.Read(reader, scopedSettings);
                collection.Add(child);
            }
            reader.ReadEndList();
            return collection.ToArray();
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
            for (var i = 0; i < childCount; i++)
                array[i] = (T)childConverter.ConvertToObject(children[i], scopedSettings);

            return array;
        }
    }
}
