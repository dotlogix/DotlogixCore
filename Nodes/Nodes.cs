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
using DotLogix.Core.Nodes.Io;
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
            WriteToInternal(null, instance, typeof(TInstance), writer);
        }

        public static void WriteTo(object instance, INodeWriter writer) {
            WriteToInternal(null, instance, instance?.GetType(), writer);
        }

        public static void WriteTo(object instance, Type instanceType, INodeWriter writer) {
            WriteToInternal(null, instance, instanceType, writer);
        }

        internal static void WriteToInternal(string name, object instance, Type instanceType, INodeWriter writer) {
            if(instance == null) {
                writer.WriteValue(null, name);
                return;
            }

            if(instanceType == null)
                throw new ArgumentNullException(nameof(instanceType));

            var converter = CreateConverter(instanceType);
            if(converter == null) {
                writer.WriteValue(null, name);
                return;
            }

            converter.Write(instance, name, writer);
        }
        #endregion

        #region ToNode
        public static Node ToNode<TInstance>(TInstance instance) {
            return ToNodeInternal(instance, typeof(TInstance));
        }


        public static Node ToNode(object instance) {
            return ToNodeInternal(instance, instance?.GetType());
        }

        public static Node ToNode(object instance, Type instanceType) {
            return ToNodeInternal(instance, instanceType);
        }

        private static Node ToNodeInternal(object instance, Type instanceType) {
            var nodeWriter = new NodeWriter();
            WriteToInternal(null, instance, instanceType, nodeWriter);
            return nodeWriter.Root;
        }
        #endregion

        #region ToObject
        public static T ToObject<T>(Node node) {
            if((node == null) || (node.NodeType == NodeTypes.Empty))
                return default(T);

            return (T)ToObject(node, typeof(T));
        }

        public static object ToObject(Node node, Type type) {
            if((node == null) || (node.NodeType == NodeTypes.Empty))
                return type.GetDefaultValue();
            var converter = CreateConverter(type);
            if(converter == null)
                return type.GetDefaultValue();

            return converter.ConvertToObject(node);
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


        public static INodeConverter CreateConverter(Type instanceType) {
            var converter = CachedNodeConverters.GetOrAdd(instanceType, CreateNodeConverter);
            //if(converter == null)
            //    throw new InvalidOperationException($"Converter for type {instanceType.GetFriendlyName()} can not be found");
            return converter;
        }

        private static INodeConverter CreateNodeConverter(Type instanceType) {
            var dataType = instanceType.ToDataType();
            var nodeType = GetNodeType(dataType);
            foreach(var converterFactory in NodeConverterFactories) {
                if(converterFactory.TryCreateConverter(nodeType, dataType, out var converter))
                    return converter;
            }
            return null;
        }
        #endregion
    }
}
