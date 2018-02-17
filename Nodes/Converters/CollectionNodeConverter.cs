// ==================================================
// Copyright 2018(C) , DotLogix
// File:  CollectionNodeConverter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DotLogix.Core.Nodes.Io;
using DotLogix.Core.Reflection.Dynamics;
using DotLogix.Core.Types;
#endregion

namespace DotLogix.Core.Nodes.Converters {
    public class CollectionNodeConverter<T> : NodeConverter {
        private readonly DynamicCtor _ctor;
        private readonly Type _elementType = typeof(T);
        private readonly bool _isDefaultCtor;

        public CollectionNodeConverter(DataType dataType) : base(dataType) {
            var enumerableType = typeof(IEnumerable<T>);
            var dynamicType = Type.CreateDynamicType(MemberTypes.Constructor);
            _ctor = dynamicType.GetConstructor(enumerableType);
            if(_ctor != null)
                return;

            _ctor = dynamicType.GetDefaultConstructor();
            if(_ctor != null) {
                _isDefaultCtor = true;
                return;
            }

            throw new InvalidOperationException($"Collection type has to define an empty constructor or one with single argument {enumerableType.FullName}");
        }

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

            if(_isDefaultCtor == false)
                return _ctor.Invoke((IEnumerable<T>)array);

            var collection = (ICollection<T>)_ctor.Invoke();
            foreach(var value in array)
                collection.Add(value);
            return collection;
        }
    }
}
