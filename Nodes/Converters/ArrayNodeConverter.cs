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
        public override async ValueTask WriteAsync(object instance, string name, IAsyncNodeWriter writer, IConverterSettings settings) {
            var scopedSettings = settings.GetScoped(TypeSettings);
            var childConverter = TypeSettings.ChildSettings.Converter;
            

            if (scopedSettings.ShouldEmitValue(instance) == false)
                return;

            ValueTask task;
            if (instance == null) {
                task = writer.WriteValueAsync(name, null);
                if (task.IsCompletedSuccessfully == false)
                    await task;
                return;
            }

            if (!(instance is IEnumerable<T> values))
                throw new ArgumentException($"Expected instance of type \"IEnumerable<T>\" got \"{instance.GetType()}\"");

            task = writer.BeginListAsync(name);
            if(task.IsCompletedSuccessfully == false)
                await task;

            foreach (var value in values) {
                task = childConverter.WriteAsync(value, null, writer, scopedSettings.ChildSettings);

                if (task.IsCompletedSuccessfully == false)
                    await task;
            }

            task = writer.EndListAsync();
            if(task.IsCompletedSuccessfully == false)
                await task;
        }

        /// <inheritdoc />
        public override object ConvertToObject(Node node, IConverterSettings settings) {
            if (node.Type == NodeTypes.Empty)
                return default;

            if (!(node is NodeList nodeList))
                throw new ArgumentException($"Expected node of type \"NodeList\" got \"{node.Type}\"");


            var scopedSettings = settings.GetScoped(TypeSettings);
            var childConverter = TypeSettings.ChildSettings.Converter;

            var children = nodeList.Children().ToArray();
            var childCount = children.Length;
            var array = new T[childCount];
            for (var i = 0; i < childCount; i++)
                array[i] = (T)childConverter.ConvertToObject(children[i], scopedSettings.ChildSettings);

            return array;
        }
    }
}
