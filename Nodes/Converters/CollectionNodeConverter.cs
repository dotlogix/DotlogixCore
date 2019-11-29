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
using System.Reflection;
using System.Threading.Tasks;
using DotLogix.Core.Nodes.Processor;
using DotLogix.Core.Reflection.Dynamics;
using DotLogix.Core.Types;
#endregion

namespace DotLogix.Core.Nodes.Converters {
    /// <summary>
    /// An implementation of the <see cref="IAsyncNodeConverter"/> interface to convert arrays
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
        public override async ValueTask WriteAsync(object instance, string name, IAsyncNodeWriter writer, ConverterSettings settings) {
            if (!(instance is IEnumerable<T> values))
                throw new ArgumentException("Instance is not type of IEnumerable<T>");
            var task = writer.BeginListAsync(name);
            if(task.IsCompletedSuccessfully == false)
                await task;

            if(settings.Resolver.TryResolve(typeof(T), out var valueTypeSettings) == false)
                throw new NotSupportedException($"Can not resolve a converter for element type {typeof(T).Name}");

            foreach(var value in values) {
                if (valueTypeSettings.ShouldEmitValue(instance, settings) == false)
                    return;

                task = valueTypeSettings.Converter.WriteAsync(value, null, writer, settings);
                if(task.IsCompletedSuccessfully == false)
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
            for(var i = 0; i < childCount; i++)
                array[i] = (T)valueTypeSettings.Converter.ConvertToObject(children[i], settings);

            if(_ctor.IsDefault == false)
                return _ctor.Invoke((IEnumerable<T>)array);

            var collection = (ICollection<T>)_ctor.Invoke();
            foreach(var value in array)
                collection.Add(value);
            return collection;
        }
    }
}
