using System;
using System.Text;
using DotLogix.Core.Nodes.Io;

namespace DotLogix.Core.Nodes {
    public static class JsonNodes {
        private const int StringBuilderCapacity=256;

        #region ToNode
        public static Node ToNode(string json) {
            var reader = new JsonNodeReader(json);
            var writer = new NodeWriter(false);
            writer.CopyFrom(reader);
            return writer.Root;
        }
        #endregion

        #region ToJson
        public static string ToJson<TInstance>(TInstance instance, JsonNodesFormatter formatter = null) {
            return ToJsonInternal(instance, typeof(TInstance), formatter);
        }

        public static string ToJson(Node node, JsonNodesFormatter formatter = null) {
            var builder = new StringBuilder(StringBuilderCapacity);
            var nodeReader = new NodeReader(node);
            var nodeWriter = new JsonNodeWriter(builder, formatter);
            nodeWriter.CopyFrom(nodeReader);
            return builder.ToString();
        }

        public static string ToJson(object instance, JsonNodesFormatter formatter = null) {
            return ToJsonInternal(instance, instance?.GetType(), formatter);
        }

        public static string ToJson(object instance, Type instanceType, JsonNodesFormatter formatter = null) {
            return ToJsonInternal(instance, instanceType, formatter);
        }

        private static string ToJsonInternal(object instance, Type instanceType, JsonNodesFormatter formatter) {
            var builder = new StringBuilder(StringBuilderCapacity);
            var nodeWriter = new JsonNodeWriter(builder, formatter);
            Nodes.WriteToInternal(null, instance, instanceType, nodeWriter);
            return builder.ToString();
        }
        #endregion

        #region FromJson
        public static TInstance FromJson<TInstance>(string json) {
            return Nodes.ToObject<TInstance>(ToNode(json));
        }

        public static object FromJson(string json, Type instanceType) {
            return Nodes.ToObject(ToNode(json), instanceType);
        }
        #endregion
    }
}