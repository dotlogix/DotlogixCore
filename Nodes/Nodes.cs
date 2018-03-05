// ==================================================
// Copyright 2018(C) , DotLogix
// File:  Nodes.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using DotLogix.Core.Extensions;
using DotLogix.Core.Nodes.Converters;
using DotLogix.Core.Nodes.Factories;
using DotLogix.Core.Nodes.Processor;
using DotLogix.Core.Types;
#endregion

namespace DotLogix.Core.Nodes {
    public static class Nodes {
        private static readonly ConcurrentDictionary<Type, INodeConverter> CachedNodeConverters = new ConcurrentDictionary<Type, INodeConverter>();

        private static readonly List<INodeConverterFactory> NodeConverterFactories = new List<INodeConverterFactory>();

        static Nodes() {
            NodeConverterFactories.Add(new ObjectNodeConverterFactory());
            NodeConverterFactories.Add(new ListNodeConverterFactory());
            NodeConverterFactories.Add(new KeyValuePairNodeConverterFactory());
            NodeConverterFactories.Add(new ValueNodeConverterFactory());
        }

        #region NodeTypes
        public static NodeTypes GetNodeType(DataType dataType) {
            if(dataType == null)
                throw new ArgumentNullException(nameof(dataType));

            NodeTypes nodeType;
            switch(dataType.Flags & DataTypeFlags.CategoryMask) {
                case DataTypeFlags.Primitive:
                    nodeType = NodeTypes.Value;
                    break;
                case DataTypeFlags.Complex:
                    nodeType = NodeTypes.Map;
                    break;
                case DataTypeFlags.Collection:
                    nodeType = NodeTypes.List;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return nodeType;
        }
        #endregion


        #region WriteTo
        public static void WriteTo<TInstance>(TInstance instance, INodeWriter writer) {
            WriteTo(null, instance, typeof(TInstance), writer);
        }

        public static void WriteTo(object instance, INodeWriter writer) {
            WriteTo(null, instance, instance?.GetType(), writer);
        }

        public static void WriteTo(object instance, Type instanceType, INodeWriter writer) {
            WriteTo(null, instance, instanceType, writer);
        }

        public static void WriteTo<TInstance>(string name, TInstance instance, INodeWriter writer)
        {
            WriteTo(name, instance, typeof(TInstance), writer);
        }

        public static void WriteTo(string name, object instance, INodeWriter writer)
        {
            WriteTo(name, instance, instance?.GetType(), writer);
        }

        public static void WriteTo(string name, object instance, Type instanceType, INodeWriter writer) {
            if(instance == null) {
                writer.WriteValue(name, null);
                return;
            }

            if(instanceType == null)
                throw new ArgumentNullException(nameof(instanceType));

            if(TryCreateConverter(instanceType, out var converter))
                converter.Write(instance, name, writer);
            else
                writer.WriteValue(name, null);
        }
        #endregion

        #region ToNode
        public static Node ToNode<TInstance>(TInstance instance) {
            return ToNode(null, instance, typeof(TInstance));
        }


        public static Node ToNode(object instance) {
            return ToNode(null, instance, instance?.GetType());
        }

        public static Node ToNode(object instance, Type instanceType) {
            return ToNode(null, instance, instanceType);
        }

        public static Node ToNode<TInstance>(string name, TInstance instance)
        {
            return ToNode(name, instance, typeof(TInstance));
        }


        public static Node ToNode(string name, object instance)
        {
            return ToNode(name, instance, instance?.GetType());
        }

        public static Node ToNode(string name, object instance, Type instanceType)
        {
            var nodeWriter = new NodeWriter();
            WriteTo(name, instance, instanceType, nodeWriter);
            return nodeWriter.Root;
        }
        #endregion

        #region ToObject
        public static T ToObject<T>(Node node) {
            if((node == null) || (node.Type == NodeTypes.Empty))
                return default(T);

            return (T)ToObject(node, typeof(T));
        }

        public static object ToObject(Node node, Type type) {
            if((node == null) || (node.Type == NodeTypes.Empty))
                return type.GetDefaultValue();
            return TryCreateConverter(type, out var converter)
                ? converter.ConvertToObject(node)
                : type.GetDefaultValue();
        }
        #endregion

        #region NodeConverter
        public static void RegisterFactory<TFactory>() where TFactory : INodeConverterFactory, new() {
            RegisterFactory(new TFactory());
        }

        public static void RegisterFactory(INodeConverterFactory factory) {
            NodeConverterFactories.Add(factory);
        }

        public static bool RegisterConverter(INodeConverter converter, bool replaceIfExists = false) {
            if((replaceIfExists == false) && CachedNodeConverters.ContainsKey(converter.Type))
                return false;

            CachedNodeConverters[converter.Type] = converter;
            return true;
        }


        public static bool TryCreateConverter(Type instanceType, out INodeConverter converter) {
            if(CachedNodeConverters.TryGetValue(instanceType, out converter))
                return true;

            if(TryCreateNodeConverter(instanceType, out converter)) {
                converter = CachedNodeConverters.GetOrAdd(instanceType, converter);
                return true;
            }

            converter = null;
            return false;
        }

        private static bool TryCreateNodeConverter(Type instanceType, out INodeConverter converter) {
            var dataType = instanceType.ToDataType();
            var nodeType = GetNodeType(dataType);
            foreach(var converterFactory in NodeConverterFactories) {
                if(converterFactory.TryCreateConverter(nodeType, dataType, out converter))
                    return true;
            }
            converter = null;
            return false;
        }
        #endregion
    }
}
