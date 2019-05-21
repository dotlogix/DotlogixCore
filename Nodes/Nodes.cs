// ==================================================
// Copyright 2018(C) , DotLogix
// File:  Nodes.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  16.07.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using DotLogix.Core.Extensions;
using DotLogix.Core.Nodes.Converters;
using DotLogix.Core.Nodes.Factories;
using DotLogix.Core.Nodes.Processor;
using DotLogix.Core.Types;
#endregion

namespace DotLogix.Core.Nodes {
    public static class Nodes {
        private static readonly ConcurrentDictionary<Type, IAsyncNodeConverter> CachedNodeConverters = new ConcurrentDictionary<Type, IAsyncNodeConverter>();

        private static readonly List<INodeConverterFactory> NodeConverterFactories = new List<INodeConverterFactory>();

        static Nodes() {
            NodeConverterFactories.Add(new ObjectNodeConverterFactory());
            NodeConverterFactories.Add(new OptionalNodeConverterFactory());
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
        public static ValueTask WriteToAsync(object instance, IAsyncNodeWriter writer) {
            return WriteToAsync(null, instance, instance?.GetType(), writer);
        }

        public static ValueTask WriteToAsync(object instance, Type instanceType, IAsyncNodeWriter writer) {
            return WriteToAsync(null, instance, instanceType, writer);
        }

        public static ValueTask WriteToAsync(string name, object instance, IAsyncNodeWriter writer) {
            return WriteToAsync(name, instance, instance?.GetType(), writer);
        }

        public static ValueTask WriteToAsync(string name, object instance, Type instanceType, IAsyncNodeWriter writer) {
            if(instance == null) {
                return writer.WriteValueAsync(name, null);
            }

            if(instance is Node node) {
                var reader = new NodeReader(node);
                return reader.CopyToAsync(writer);
            }

            if(instanceType == null)
                throw new ArgumentNullException(nameof(instanceType));

            if(TryCreateConverter(instanceType, out var converter))
                return converter.WriteAsync(instance, name, writer);

            return writer.WriteValueAsync(name, null);
        }
        #endregion

        #region ToNode
        public static Node ToNode(object instance, ConverterSettings settings = null) {
            return ToNode(null, instance, instance?.GetType(), settings);
        }

        public static Node ToNode(object instance, Type instanceType, ConverterSettings settings = null) {
            return ToNode(null, instance, instanceType, settings);
        }

        internal static Node ToNode(string name, object instance, ConverterSettings settings = null) {
            return ToNode(name, instance, instance?.GetType(), settings);
        }

        internal static Node ToNode(string name, object instance, Type instanceType, ConverterSettings settings = null) {
            if(instance is Node node)
                return node;
            var nodeWriter = new NodeWriter(settings);
            WriteToAsync(name, instance, instanceType, nodeWriter);
            return nodeWriter.Root;
        }
        #endregion

        #region ToObject
        public static T ToObject<T>(this Node node, ConverterSettings settings = null) {
            if(node == null)
                return default;
            
            return (T)ToObject(node, typeof(T), settings);
        }

        public static object ToObject(this Node node, Type type, ConverterSettings settings = null) {
            if(node == null)
                return type.GetDefaultValue();

            if(type.IsInstanceOfType(node))
                return node;

            return TryCreateConverter(type, out var converter)
                       ? converter.ConvertToObject(node, settings ?? ConverterSettings.Default)
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

        public static bool RegisterConverter(IAsyncNodeConverter converter, bool replaceIfExists = false) {
            if((replaceIfExists == false) && CachedNodeConverters.ContainsKey(converter.Type))
                return false;

            CachedNodeConverters[converter.Type] = converter;
            return true;
        }


        public static bool TryCreateConverter(Type instanceType, out IAsyncNodeConverter converter) {
            if(CachedNodeConverters.TryGetValue(instanceType, out converter))
                return true;

            if(TryCreateNodeConverter(instanceType, out converter)) {
                converter = CachedNodeConverters.GetOrAdd(instanceType, converter);
                return true;
            }

            converter = null;
            return false;
        }

        private static bool TryCreateNodeConverter(Type instanceType, out IAsyncNodeConverter converter) {
            var converterAttribute = instanceType.GetCustomAttribute<NodeConverterAttribute>();
            if(converterAttribute != null) {
                converter = converterAttribute.CreateNodeConverter();
                return true;
            }

            var dataType = instanceType.ToDataType();
            var nodeType = GetNodeType(dataType);
            for(var i = NodeConverterFactories.Count - 1; i >= 0; i--) {
                var converterFactory = NodeConverterFactories[i];
                if(converterFactory.TryCreateConverter(nodeType, dataType, out converter))
                    return true;
            }
            converter = null;
            return false;
        }
        #endregion
    }
}
