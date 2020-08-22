// ==================================================
// Copyright 2018(C) , DotLogix
// File:  JsonNodes.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.IO;
using System.Text;
using DotLogix.Core.Extensions;
using DotLogix.Core.Nodes.Processor;
#endregion

namespace DotLogix.Core.Nodes {
    /// <summary>
    /// A static class providing extension methods for <see cref="Node"/>
    /// </summary>
    public static class JsonUtils {
        private const int StringBuilderCapacity = 256;

        #region ToNode
        /// <summary>
        /// Convert json text to node
        /// </summary>
        public static TNode ToNode<TNode>(string json, JsonReaderOptions options = default) where TNode : Node {
            return (TNode)ToNode(json, options);
        }

        /// <summary>
        /// Convert json text to node asynchronously
        /// </summary>
        public static TNode ToNode<TNode>(TextReader reader, JsonReaderOptions options = default) where TNode : Node
        {
            return (TNode) ToNode(reader, options);
        }

        /// <summary>
        /// Convert json text to node asynchronously
        /// </summary>
        public static TNode ToNode<TNode>(Stream stream, Encoding encoding, JsonReaderOptions options = default) where TNode : Node {
            return (TNode) ToNode(stream, encoding, options);
        }

        /// <summary>
        /// Convert json text to node
        /// </summary>
        public static Node ToNode(string json, JsonReaderOptions options = default) {
            using var reader = new JsonNodeReader(json, options);
            using var writer = new NodeWriter();
            reader.CopyTo(writer);
            return writer.Root;
        }

        /// <summary>
        /// Convert json text to node asynchronously
        /// </summary>
        public static Node ToNode(TextReader reader, JsonReaderOptions options = default) {
            using var jsonReader = new JsonNodeReader(reader, options);
            using var writer = new NodeWriter();

            jsonReader.CopyTo(writer);
            return writer.Root;
        }

        /// <summary>
        /// Convert json text to node asynchronously
        /// </summary>
        public static Node ToNode(Stream stream, Encoding encoding, JsonReaderOptions options = default)
        {
            using var reader = new StreamReader(stream, encoding ?? Encoding.UTF8);
            return ToNode(reader, options);
        }
        #endregion

        #region ToJson
        /// <summary>
        /// Convert a node to json text
        /// </summary>
        public static string ToJson(this Node value, JsonFormatterSettings formatterSettings = null) {
            return ToJson(value, value?.GetType(), formatterSettings);
        }

        /// <summary>
        /// Convert an object to json text
        /// </summary>
        public static string ToJson(object value, JsonFormatterSettings formatterSettings = null) {
            return ToJson(value, value?.GetType(), formatterSettings);
        }


        /// <summary>
        /// Convert an object to json text
        /// </summary>
        public static void ToJson(object value, Stream stream, Encoding encoding, JsonFormatterSettings formatterSettings = null) {
            ToJson(value, value?.GetType(), stream, encoding, formatterSettings);
        }

        /// <summary>
        /// Convert an object to json text
        /// </summary>
        public static void ToJson(object value, TextWriter writer, JsonFormatterSettings formatterSettings = null) {
            ToJson(value, value?.GetType(), writer, formatterSettings);
        }

        /// <summary>
        /// Convert an object to json text
        /// </summary>
        public static string ToJson(object value, Type instanceType, JsonFormatterSettings formatterSettings = null)
        {
            formatterSettings ??= JsonFormatterSettings.Default;
            formatterSettings.WriteOptions |= JsonWriterOptions.Sync;

            using var writer = new StringWriter(new StringBuilder(StringBuilderCapacity));
            ToJson(value, instanceType, writer, formatterSettings);
            return writer.ToString();
        }

        /// <summary>
        /// Convert an object to json text
        /// </summary>
        public static void ToJson(object value, Type instanceType, Stream stream, Encoding encoding, JsonFormatterSettings formatterSettings = null)
        {
            using var writer = new StreamWriter(stream, encoding);
            ToJson(value, instanceType, writer, formatterSettings);
        }

        /// <summary>
        /// Convert an object to json text asynchronously
        /// </summary>
        public static void ToJson(object value, Type instanceType, TextWriter writer, JsonFormatterSettings formatterSettings = null) {
            using var nodeWriter = new JsonNodeWriter(writer, formatterSettings, StringBuilderCapacity);

            if(value is Node node) {
                using var nodeReader = new NodeReader(node);
                nodeReader.CopyTo(nodeWriter);
            } else {
                NodeUtils.WriteTo(null, value, instanceType, nodeWriter, formatterSettings ?? JsonFormatterSettings.Default);
            }
        }
        #endregion

        #region FromJson
        /// <summary>
        /// Convert json text to an instance of object
        /// </summary>
        public static TInstance FromJson<TInstance>(string json, JsonFormatterSettings settings = null) {
            return (TInstance)FromJson(json, typeof(TInstance), settings);
        }

        /// <summary>
        /// Convert json text to an instance of object asynchronously
        /// </summary>
        public static TInstance FromJson<TInstance>(Stream stream, Encoding encoding, JsonFormatterSettings settings = null) {
            return (TInstance) FromJson(stream, encoding, typeof(TInstance), settings);
        }

        /// <summary>
        /// Convert json text to an instance of object asynchronously
        /// </summary>
        public static TInstance FromJson<TInstance>(TextReader reader, JsonFormatterSettings settings = null) {
            return (TInstance) FromJson(reader, typeof(TInstance), settings);
        }

        /// <summary>
        /// Convert json text to an instance of object
        /// </summary>
        public static object FromJson(string json, Type instanceType, JsonFormatterSettings settings = null) {
            settings ??= JsonFormatterSettings.Default;

            using var jsonReader = new JsonNodeReader(json, settings.ReadOptions);
            if(settings.Resolver.TryResolve(instanceType, out var typeSettings))
                return typeSettings.Converter.Read(jsonReader, settings);

            return instanceType.GetDefaultValue();
        }

        /// <summary>
        /// Convert json text to an instance of object asynchronously
        /// </summary>
        public static object FromJson(TextReader reader, Type instanceType, JsonFormatterSettings settings = null) {
            settings ??= JsonFormatterSettings.Default;

            using var jsonReader = new JsonNodeReader(reader, settings.ReadOptions);

            if (settings.Resolver.TryResolve(instanceType, out var typeSettings))
                return typeSettings.Converter
                                   .Read(jsonReader, settings)
                                   ;

            return instanceType.GetDefaultValue();
        }

        /// <summary>
        /// Convert json text to an instance of object asynchronously
        /// </summary>
        public static object FromJson(Stream stream, Encoding encoding, Type instanceType, JsonFormatterSettings settings = null)
        {
            using var reader = new StreamReader(stream, encoding ?? Encoding.UTF8);
            return FromJson(reader, instanceType, settings);
        }

        #endregion
    }
}
