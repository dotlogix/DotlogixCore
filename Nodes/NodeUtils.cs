// ==================================================
// Copyright 2018(C) , DotLogix
// File:  Nodes.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  16.07.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using DotLogix.Core.Extensions;
using DotLogix.Core.Nodes.Formats.Nodes;
using DotLogix.Core.Nodes.Schema;
using DotLogix.Core.Types;
#endregion

namespace DotLogix.Core.Nodes {
    public static class NodeUtils {
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
        public static void WriteTo(object instance, INodeWriter writer, IReadOnlyConverterSettings settings) {
            WriteTo(null, instance, instance?.GetType(), writer, settings);
        }

        public static void WriteTo(object instance, Type instanceType, INodeWriter writer, IReadOnlyConverterSettings settings) {
            WriteTo(null, instance, instanceType, writer, settings);
        }

        public static void WriteTo(string name, object instance, INodeWriter writer, IReadOnlyConverterSettings settings) {
            WriteTo(name, instance, instance?.GetType(), writer, settings);
        }

        public static void WriteTo(string name, object instance, Type instanceType, INodeWriter writer, IReadOnlyConverterSettings settings) {
            if(string.IsNullOrEmpty(name) == false)
                writer.WriteName(name);

            if(instance == null) {
                writer.WriteValue(null);
                return;
            }

            if(instance is Node node) {
                var reader = new NodeReader(node);
                reader.CopyTo(writer);
                return;
            }

            if(instanceType == null)
                throw new ArgumentNullException(nameof(instanceType));

            if(settings.Resolver.TryResolve(instanceType, out var typeSettings)) {
                typeSettings.Converter.Write(instance, writer, settings);
                return;
            }

            writer.WriteValue(null);
        }
        #endregion

        #region ToNode
        public static Node ToNode(object instance, IReadOnlyConverterSettings settings = null) {
            return ToNode(null, instance, instance?.GetType(), settings);
        }

        public static Node ToNode(object instance, Type instanceType, IReadOnlyConverterSettings settings = null) {
            return ToNode(null, instance, instanceType, settings);
        }

        internal static Node ToNode(string name, object instance, Type instanceType, IReadOnlyConverterSettings settings = null) {
            if(instance is Node node)
                return node;

            settings ??= ConverterSettings.Default;
            var nodeWriter = new NodeWriter(settings);
            WriteTo(name, instance, instanceType, nodeWriter, settings);
            return nodeWriter.Root;
        }
        #endregion

        #region ToObject
        public static T ToObject<T>(this Node node, IReadOnlyConverterSettings settings = null) {
            if(node == null)
                return default;
            
            return (T)ToObject(node, typeof(T), settings);
        }

        public static object ToObject(this Node node, Type type, IReadOnlyConverterSettings settings = null) {
            if(node == null)
                return type.GetDefaultValue();

            if(type.IsInstanceOfType(node))
                return node;

            if(type == typeof(object))
                return DynamicNode.From(node);

            settings ??= ConverterSettings.Default;

            return settings.Resolver.TryResolve(type, out var typeSettings)
                       ? typeSettings.Converter.ConvertToObject(node, settings)
                       : type.GetDefaultValue();
        }
        #endregion
    }
}
