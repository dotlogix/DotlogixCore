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
using System.Threading.Tasks;
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
        public static async ValueTask<TNode> ToNodeAsync<TNode>(TextReader reader, JsonReaderOptions options = default) where TNode : Node
        {
            return (TNode) await ToNodeAsync(reader, options).ConfigureAwait(false);
        }

        /// <summary>
        /// Convert json text to node asynchronously
        /// </summary>
        public static async ValueTask<TNode> ToNodeAsync<TNode>(Stream stream, Encoding encoding, JsonReaderOptions options = default) where TNode : Node {
            return (TNode) await ToNodeAsync(stream, encoding, options).ConfigureAwait(false);
        }

        /// <summary>
        /// Convert json text to node
        /// </summary>
        public static Node ToNode(string json, JsonReaderOptions options = default) {
            var reader = new JsonNodeReader(json, options | JsonReaderOptions.Sync);
            var writer = new NodeWriter();
            reader.CopyToAsync(writer)
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();
            return writer.Root;
        }

        /// <summary>
        /// Convert json text to node asynchronously
        /// </summary>
        public static async ValueTask<Node> ToNodeAsync(TextReader reader, JsonReaderOptions options = default) {
            var jsonReader = new JsonNodeReader(reader, options);
            var writer = new NodeWriter();

            await jsonReader.CopyToAsync(writer).ConfigureAwait(false);
            return writer.Root;
        }

        /// <summary>
        /// Convert json text to node asynchronously
        /// </summary>
        public static async ValueTask<Node> ToNodeAsync(Stream stream, Encoding encoding, JsonReaderOptions options = default)
        {
            using var reader = new StreamReader(stream, encoding ?? Encoding.UTF8);
            return await ToNodeAsync(reader, options).ConfigureAwait(false);
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
        /// Convert a node to json text asynchronously
        /// </summary>
        public static ValueTask<string> ToJsonAsync(this Node value, JsonFormatterSettings formatterSettings = null) {
            return ToJsonAsync(value, value?.GetType(), formatterSettings);
        }

        /// <summary>
        /// Convert a node to json text asynchronously
        /// </summary>
        public static ValueTask ToJsonAsync(this Node value, Stream stream, Encoding encoding, JsonFormatterSettings formatterSettings = null) {
            return ToJsonAsync(value, value?.GetType(), stream, encoding, formatterSettings);
        }

        /// <summary>
        /// Convert a node to json text asynchronously
        /// </summary>
        public static ValueTask ToJsonAsync(this Node value, TextWriter writer, JsonFormatterSettings formatterSettings = null) {
            return ToJsonAsync(value, value?.GetType(), writer, formatterSettings);
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
        public static string ToJson(object value, Type instanceType, JsonFormatterSettings formatterSettings = null) {
            var task = ToJsonAsync(value, instanceType, formatterSettings);
            return task.ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();
        }

        /// <summary>
        /// Convert an object to json text asynchronously
        /// </summary>
        public static ValueTask<string> ToJsonAsync(object value, JsonFormatterSettings formatterSettings = null) {
            return ToJsonAsync(value, value?.GetType(), formatterSettings);
        }

        /// <summary>
        /// Convert an object to json text asynchronously
        /// </summary>
        public static ValueTask ToJsonAsync(object value, Stream stream, Encoding encoding, JsonFormatterSettings formatterSettings = null) {
            return ToJsonAsync(value, value?.GetType(), stream, encoding, formatterSettings);
        }

        /// <summary>
        /// Convert an object to json text asynchronously
        /// </summary>
        public static ValueTask ToJsonAsync(object value, TextWriter writer, JsonFormatterSettings formatterSettings = null) {
            return ToJsonAsync(value, value?.GetType(), writer, formatterSettings);
        }

        /// <summary>
        /// Convert an object to json text asynchronously
        /// </summary>
        public static async ValueTask<string> ToJsonAsync(object value, Type instanceType, JsonFormatterSettings formatterSettings = null)
        {
            formatterSettings ??= JsonFormatterSettings.Default;
            formatterSettings.WriteOptions |= JsonWriterOptions.Sync;

            using var writer = new StringWriter(new StringBuilder(StringBuilderCapacity));
            await ToJsonAsync(value, instanceType, writer, formatterSettings).ConfigureAwait(false);
            return writer.ToString();
        }

        /// <summary>
        /// Convert an object to json text asynchronously
        /// </summary>
        public static async ValueTask ToJsonAsync(object value, Type instanceType, Stream stream, Encoding encoding, JsonFormatterSettings formatterSettings = null)
        {
            using var writer = new StreamWriter(stream, encoding);
            await ToJsonAsync(value, instanceType, writer, formatterSettings).ConfigureAwait(false);
        }

        /// <summary>
        /// Convert an object to json text asynchronously
        /// </summary>
        public static async ValueTask ToJsonAsync(object value, Type instanceType, TextWriter writer, JsonFormatterSettings formatterSettings = null) {
            var nodeWriter = new JsonNodeWriter(writer, formatterSettings, StringBuilderCapacity);

            if(value is Node node) {
                var nodeReader = new NodeReader(node);
                await nodeReader.CopyToAsync(nodeWriter).ConfigureAwait(false);
            } else {
                await NodeUtils.WriteToAsync(null, value, instanceType, nodeWriter, formatterSettings ?? JsonFormatterSettings.Default).ConfigureAwait(false);
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
        public static async ValueTask<TInstance> FromJsonAsync<TInstance>(Stream stream, Encoding encoding, JsonFormatterSettings settings = null) {
            return (TInstance) await FromJsonAsync(stream, encoding, typeof(TInstance), settings).ConfigureAwait(false);
        }

        /// <summary>
        /// Convert json text to an instance of object asynchronously
        /// </summary>
        public static async ValueTask<TInstance> FromJsonAsync<TInstance>(TextReader reader, JsonFormatterSettings settings = null) {
            return (TInstance) await FromJsonAsync(reader, typeof(TInstance), settings).ConfigureAwait(false);
        }

        /// <summary>
        /// Convert json text to an instance of object
        /// </summary>
        public static object FromJson(string json, Type instanceType, JsonFormatterSettings settings = null) {
            settings ??= JsonFormatterSettings.Default;
            settings.ReadOptions |= JsonReaderOptions.Sync;

            var jsonReader = new JsonNodeReader(json, settings.ReadOptions);
            if(settings.Resolver.TryResolve(instanceType, out var typeSettings))
                return typeSettings.Converter
                                   .ReadAsync(jsonReader, settings)
                                   .ConfigureAwait(false)
                                   .GetAwaiter()
                                   .GetResult();

            return instanceType.GetDefaultValue();
        }

        /// <summary>
        /// Convert json text to an instance of object asynchronously
        /// </summary>
        public static async ValueTask<object> FromJsonAsync(TextReader reader, Type instanceType, JsonFormatterSettings settings = null) {
            settings ??= JsonFormatterSettings.Default;

            var jsonReader = new JsonNodeReader(reader, settings.ReadOptions);

            if (settings.Resolver.TryResolve(instanceType, out var typeSettings))
                return await typeSettings.Converter
                                   .ReadAsync(jsonReader, settings)
                                   .ConfigureAwait(false);

            return instanceType.GetDefaultValue();
        }

        /// <summary>
        /// Convert json text to an instance of object asynchronously
        /// </summary>
        public static async ValueTask<object> FromJsonAsync(Stream stream, Encoding encoding, Type instanceType, JsonFormatterSettings settings = null)
        {
            using var reader = new StreamReader(stream, encoding ?? Encoding.UTF8);
            return await FromJsonAsync(reader, instanceType, settings).ConfigureAwait(false);
        }

        #endregion
    }
}
