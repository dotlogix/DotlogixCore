// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ArrayNodeConverter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Linq;
using DotLogix.Core.Nodes.Io;
using DotLogix.Core.Types;
#endregion

namespace DotLogix.Core.Nodes.Converters {
    public class ArrayNodeConverter<T> : NodeConverter {
        private readonly Type _elementType = typeof(T);
        public ArrayNodeConverter(DataType type) : base(type) { }

        public override void Write(object instance, string rootName, INodeWriter writer) {
            var values = instance as IEnumerable<T>;
            if(values == null)
                throw new ArgumentException("Instance is not type of IEnumerable<T>");

            writer.BeginList(rootName);
            foreach(var value in values)
                Nodes.WriteToInternal(null, value, _elementType, writer);
            writer.EndList();
        }

        public override object ConvertToObject(Node node) {
            var nodeList = node as NodeList;
            if(nodeList == null)
                throw new ArgumentException("Node is not a NodeList");

            var children = nodeList.Children().ToArray();
            var childCount = children.Length;
            var array = new T[childCount];
            for(var i = 0; i < childCount; i++)
                array[i] = (T)Nodes.ToObject(children[i], _elementType);

            return array;
        }
    }
}
