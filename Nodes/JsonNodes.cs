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
using DotLogix.Core.Nodes.Processor;
#endregion

namespace DotLogix.Core.Nodes {
    /// <summary>
    /// A static class providing extension methods for <see cref="Node"/>
    /// </summary>
    public static class JsonNodes {
        private const int StringBuilderCapacity = 256;

        #region ToNode
        /// <summary>
        /// Convert json text to node
        /// </summary>
        public static TNode ToNode<TNode>(string json) where TNode : Node {
            return (TNode)ToNode(json);
        }

        /// <summary>
        /// Convert json text to node asynchronously
        /// </summary>
        public static async ValueTask<TNode> ToNodeAsync<TNode>(TextReader reader) where TNode : Node {
            var task = ToNodeAsync(reader);
            return (TNode)(task.IsCompletedSuccessfully ? task.Result : await task);
        }

        /// <summary>
        /// Convert json text to node asynchronously
        /// </summary>
        public static async ValueTask<TNode> ToNodeAsync<TNode>(Stream stream, Encoding encoding) where TNode : Node {
            var task = ToNodeAsync(stream, encoding);
            return (TNode)(task.IsCompletedSuccessfully ? task.Result : await task);
        }

        /// <summary>
        /// Convert json text to node
        /// </summary>
        public static Node ToNode(string json) {
            var reader = new JsonNodeReader(json);
            var writer = new NodeWriter();

            var task = reader.CopyToAsync(writer);
            if(task.IsCompletedSuccessfully == false)
                task.GetAwaiter().GetResult();
            return writer.Root;
        }

        /// <summary>
        /// Convert json text to node asynchronously
        /// </summary>
        public static async ValueTask<Node> ToNodeAsync(TextReader reader) {
            var jsonReader = new JsonNodeReader(reader);
            var writer = new NodeWriter();

            var task = jsonReader.CopyToAsync(writer);
            if(task.IsCompletedSuccessfully == false)
                await task;
            return writer.Root;
        }

        /// <summary>
        /// Convert json text to node asynchronously
        /// </summary>
        public static async ValueTask<Node> ToNodeAsync(Stream stream, Encoding encoding) {
            using(var reader = new StreamReader(stream, encoding ?? Encoding.UTF8)) {
                var task = ToNodeAsync(reader);
                return task.IsCompletedSuccessfully ? task.Result : await task;
            }
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
            return task.IsCompletedSuccessfully ? task.Result : task.ConfigureAwait(false).GetAwaiter().GetResult();
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
        public static async ValueTask<string> ToJsonAsync(object value, Type instanceType, JsonFormatterSettings formatterSettings = null) {
            using(var writer = new StringWriter(new StringBuilder(StringBuilderCapacity))) {
                var task = ToJsonAsync(value, instanceType, writer, formatterSettings);
                if(task.IsCompletedSuccessfully == false)
                    await task;
                return writer.ToString();
            }
        }

        /// <summary>
        /// Convert an object to json text asynchronously
        /// </summary>
        public static async ValueTask ToJsonAsync(object value, Type instanceType, Stream stream, Encoding encoding, JsonFormatterSettings formatterSettings = null) {
            using(var writer = new StreamWriter(stream, encoding)) {
                var task = ToJsonAsync(value, instanceType, writer, formatterSettings);
                if(task.IsCompletedSuccessfully == false)
                    await task;
            }
        }

        /// <summary>
        /// Convert an object to json text asynchronously
        /// </summary>
        public static async ValueTask ToJsonAsync(object value, Type instanceType, TextWriter writer, JsonFormatterSettings formatterSettings = null) {
            var nodeWriter = new JsonNodeWriter(writer, formatterSettings, StringBuilderCapacity);

            ValueTask task;
            if(value is Node node) {
                var nodeReader = new NodeReader(node);
                task = nodeReader.CopyToAsync(nodeWriter);
            } else {
                task = Nodes.WriteToAsync(null, value, instanceType, nodeWriter, formatterSettings ?? JsonFormatterSettings.Default);
            }
            if(task.IsCompletedSuccessfully == false)
                await task;
        }
        #endregion

        #region FromJson
        /// <summary>
        /// Convert json text to an instance of object
        /// </summary>
        public static TInstance FromJson<TInstance>(string json, ConverterSettings settings = null) {
            return (TInstance)FromJson(json, typeof(TInstance), settings);
        }

        /// <summary>
        /// Convert json text to an instance of object asynchronously
        /// </summary>
        public static async ValueTask<TInstance> FromJsonAsync<TInstance>(Stream stream, Encoding encoding, ConverterSettings settings = null) {
            var task = FromJsonAsync(stream, encoding, typeof(TInstance), settings);
            return (TInstance)(task.IsCompletedSuccessfully ? task.Result : await task);
        }

        /// <summary>
        /// Convert json text to an instance of object asynchronously
        /// </summary>
        public static async ValueTask<TInstance> FromJsonAsync<TInstance>(TextReader reader, ConverterSettings settings = null) {
            var task = FromJsonAsync(reader, typeof(TInstance), settings);
            return (TInstance)(task.IsCompletedSuccessfully ? task.Result : await task);
        }

        /// <summary>
        /// Convert json text to an instance of object
        /// </summary>
        public static object FromJson(string json, Type instanceType, ConverterSettings settings = null) {
            return ToNode(json).ToObject(instanceType, settings);
        }

        /// <summary>
        /// Convert json text to an instance of object asynchronously
        /// </summary>
        public static async ValueTask<object> FromJsonAsync(TextReader reader, Type instanceType, ConverterSettings settings = null) {
            var task = ToNodeAsync(reader);
            return (task.IsCompletedSuccessfully ? task.Result : await task).ToObject(instanceType, settings);
        }

        /// <summary>
        /// Convert json text to an instance of object asynchronously
        /// </summary>
        public static async ValueTask<object> FromJsonAsync(Stream stream, Encoding encoding, Type instanceType, ConverterSettings settings = null) {
            using(var reader = new StreamReader(stream, encoding ?? Encoding.UTF8)) {
                var task = FromJsonAsync(reader, instanceType, settings);
                return task.IsCompletedSuccessfully ? task.Result : await task;
            }
        }
        #endregion
    }
}
