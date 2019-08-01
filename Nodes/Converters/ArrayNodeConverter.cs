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
        private readonly Type _elementType = typeof(T);
        /// <summary>
        /// Creates a new instance of <see cref="ArrayNodeConverter{T}"/>
        /// </summary>
        public ArrayNodeConverter(DataType type) : base(type) { }

        /// <inheritdoc />
        public override async ValueTask WriteAsync(object instance, string rootName, IAsyncNodeWriter writer, ConverterSettings settings) {
            if(!(instance is IEnumerable<T> values))
                throw new ArgumentException("Instance is not type of IEnumerable<T>");

            var task = writer.BeginListAsync(rootName);
            if(task.IsCompletedSuccessfully == false)
                await task;
            foreach(var value in values) {
                task = Nodes.WriteToAsync(null, value, _elementType, writer, settings);
                if(task.IsCompletedSuccessfully == false)
                    await task;
            }

            task = writer.EndListAsync();
            if(task.IsCompletedSuccessfully == false)
                await task;
        }

        /// <inheritdoc />
        public override object ConvertToObject(Node node, ConverterSettings settings) {
            if(!(node is NodeList nodeList))
                throw new ArgumentException("Node is not a NodeList");

            var children = nodeList.Children().ToArray();
            var childCount = children.Length;
            var array = new T[childCount];
            for(var i = 0; i < childCount; i++)
                array[i] = (T)Nodes.ToObject(children[i], _elementType, settings);

            return array;
        }
    }
}
