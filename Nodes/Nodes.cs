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
        public static INodeConverterResolver DefaultResolver => CreateDefaultResolver();

        private static INodeConverterResolver CreateDefaultResolver() {
            var resolver = new NodeConverterResolver();
            resolver.Register(new ObjectNodeConverterFactory());
            resolver.Register(new OptionalNodeConverterFactory());
            resolver.Register(new ListNodeConverterFactory());
            resolver.Register(new KeyValuePairNodeConverterFactory());
            resolver.Register(new ValueNodeConverterFactory());
            return resolver;
        }

        static Nodes() {
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
        public static ValueTask WriteToAsync(object instance, IAsyncNodeWriter writer, ConverterSettings settings) {
            return WriteToAsync(null, instance, instance?.GetType(), writer, settings);
        }

        public static ValueTask WriteToAsync(object instance, Type instanceType, IAsyncNodeWriter writer, ConverterSettings settings) {
            return WriteToAsync(null, instance, instanceType, writer, settings);
        }

        public static ValueTask WriteToAsync(string name, object instance, IAsyncNodeWriter writer, ConverterSettings settings) {
            return WriteToAsync(name, instance, instance?.GetType(), writer, settings);
        }

        public static async ValueTask WriteToAsync(string name, object instance, Type instanceType, IAsyncNodeWriter writer, ConverterSettings settings) {
            if(instance == null) {
                await writer.WriteValueAsync(name, null).ConfigureAwait(false);
                return;
            }

            if(instance is Node node) {
                var reader = new NodeReader(node);
                await reader.CopyToAsync(writer).ConfigureAwait(false);
                return;
            }

            if(instanceType == null)
                throw new ArgumentNullException(nameof(instanceType));

            if(settings.Resolver.TryResolve(instanceType, out var typeSettings)) {
                await typeSettings.Converter.WriteAsync(instance, name, writer, settings).ConfigureAwait(false);
                return;
            }

            await writer.WriteValueAsync(name, null).ConfigureAwait(false);
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

            if(settings == null)
                settings = ConverterSettings.Default;
            var nodeWriter = new NodeWriter(settings);
            WriteToAsync(name, instance, instanceType, nodeWriter, settings);
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

            if(type == typeof(object))
                return DynamicNode.From(node);

            if(settings == null)
                settings = JsonFormatterSettings.Idented;

            return settings.Resolver.TryResolve(type, out var typeSettings)
                       ? typeSettings.Converter.ConvertToObject(node, settings)
                       : type.GetDefaultValue();
        }
        #endregion
    }
}
