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
using System.Threading.Tasks;
using DotLogix.Core.Nodes.Processor;
using DotLogix.Core.Types;
#endregion

namespace DotLogix.Core.Nodes.Converters {
    /// <summary>
    /// An implementation of the <see cref="IAsyncNodeConverter"/> interface to convert arrays
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ArrayNodeConverter<T> : NodeConverter {
        /// <summary>
        /// Creates a new instance of <see cref="ArrayNodeConverter{T}"/>
        /// </summary>
        public ArrayNodeConverter(TypeSettings typeSettings) : base(typeSettings) { }

        /// <inheritdoc />
        public override async ValueTask WriteAsync(object instance, string name, IAsyncNodeWriter writer, ConverterSettings settings) {
            if(TypeSettings.ShouldEmitValue(instance, settings) == false)
                return;

            if(!(instance is IEnumerable<T> values))
                throw new ArgumentException("Instance is not type of IEnumerable<T>");

            var task = writer.BeginListAsync(name);
            if(task.IsCompletedSuccessfully == false)
                await task;


            var elementType = typeof(T);
            settings.Resolver.TryResolve(elementType, out var valueTypeSettings);

            foreach (var value in values) {
                var type = value?.GetType() ?? elementType;

                if(value?.GetType() == elementType) {
                    task = valueTypeSettings.Converter.WriteAsync(value, null, writer, settings);
                } else {

                }

                if (task.IsCompletedSuccessfully == false)
                    await task;
            }

            task = writer.EndListAsync();
            if(task.IsCompletedSuccessfully == false)
                await task;
        }

        /// <inheritdoc />
        public override object ConvertToObject(Node node, ConverterSettings settings) {
            if (node.Type == NodeTypes.Empty)
                return default;

            if (!(node is NodeList nodeList))
                throw new ArgumentException("Node is not a NodeList");

            if (settings.Resolver.TryResolve(typeof(T), out var valueTypeSettings) == false)
                throw new NotSupportedException($"Can not resolve a converter for element type {typeof(T).Name}");


            var children = nodeList.Children().ToArray();
            var childCount = children.Length;
            var array = new T[childCount];
            for (var i = 0; i < childCount; i++)
                array[i] = (T)valueTypeSettings.Converter.ConvertToObject(children[i], settings);

            return array;
        }
    }
}
