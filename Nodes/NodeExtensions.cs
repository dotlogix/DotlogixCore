// ==================================================
// Copyright 2018(C) , DotLogix
// File:  NodeExtensions.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  16.07.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DotLogix.Core.Extensions;
using DotLogix.Core.Nodes.Processor;
#endregion

namespace DotLogix.Core.Nodes {
    public static class NodeExtensions {
        public static dynamic ToDynamic(this NodeContainer container) {
            return DynamicNode.From(container);
        }

        #region NodeMap
        #region Create
        public static TNode CreateNode<TNode>(this NodeMap nodeMap, string name) where TNode : Node, new() {
            var node = new TNode();
            nodeMap.AddChild(name, node);
            return node;
        }

        public static NodeMap CreateMap(this NodeMap nodeMap, string name) {
            var node = new NodeMap();
            nodeMap.AddChild(name, node);
            return node;
        }

        public static NodeList CreateList(this NodeMap nodeMap, string name) {
            var node = new NodeList();
            nodeMap.AddChild(name, node);
            return node;
        }


        public static NodeValue CreateValue(this NodeMap nodeMap, string name, object value = null) {
            var node = new NodeValue(value);
            nodeMap.AddChild(name, node);
            return node;
        }
        #endregion

        #region Get
        public static object GetChildValue(this NodeMap nodeMap, string name) {
            return GetChildValue(nodeMap, name, default(object));
        }

        public static object GetChildValue(this NodeMap nodeMap, string name, object defaultValue) {
            var nodeValue = nodeMap.GetChild<NodeValue>(name);
            return nodeValue != null ? nodeValue.Value : defaultValue;
        }

        public static object GetChildValue(this NodeMap nodeMap, string name, Type type) {
            return GetChildValue(nodeMap, name, default(object));
        }

        public static object GetChildValue(this NodeMap nodeMap, string name, Type type, object defaultValue) {
            var nodeValue = nodeMap.GetChild<NodeValue>(name);
            return nodeValue != null ? nodeValue.GetValue(type, defaultValue) : defaultValue;
        }

        public static TValue GetChildValue<TValue>(this NodeMap nodeMap, string name) {
            return GetChildValue(nodeMap, name, default(TValue));
        }

        public static TValue GetChildValue<TValue>(this NodeMap nodeMap, string name, TValue defaultValue) {
            var nodeValue = nodeMap.GetChild<NodeValue>(name);
            return nodeValue == null ? default : nodeValue.GetValue(defaultValue);
        }

        public static bool TryGetChildValue(this NodeMap nodeMap, string name, out object value) {
            if(nodeMap.TryGetChild<NodeValue>(name, out var node)) {
                value = node.Value;
                return true;
            }
            value = null;
            return false;
        }

        public static bool TryGetChildValue(this NodeMap nodeMap, string name, Type type, out object value) {
            if(nodeMap.TryGetChild<NodeValue>(name, out var node)) {
                value = node.GetValue(type);
                return true;
            }
            value = null;
            return false;
        }

        public static bool TryGetChildValue<TValue>(this NodeMap nodeMap, string name, out TValue value) {
            if(nodeMap.TryGetChild<NodeValue>(name, out var node)) {
                value = node.GetValue<TValue>();
                return true;
            }
            value = default;
            return false;
        }
        #endregion

        #region Flatten
        public static NodeMap Flatten(this NodeContainer nodeContainer, string separator="_") {
            var flatMap = new NodeMap();
            FlattenRecursive(flatMap, nodeContainer, null, separator);
            return flatMap;
        }

        private static void FlattenRecursive(NodeMap flatMap, NodeContainer container, string previousPath, string separator) {
            var childContainers = new List<NodeContainer>();
            foreach(var child in container.Children()) {
                switch(child) {
                    case NodeContainer nodeContainer:
                        childContainers.Add(nodeContainer);
                        break;
                    case NodeValue nodeValue:
                        var path = previousPath == null ? child.Name ?? child.Index.ToString() : string.Concat(previousPath, separator, child.Name ?? child.Index.ToString());
                        flatMap.CreateValue(path, nodeValue.Value);
                        break;
                }
            }
            if(childContainers.Count <= 0)
                return;

            foreach(var child in childContainers) {
                var path = previousPath == null ? child.Name ?? child.Index.ToString() : string.Concat(previousPath, separator, child.Name ?? child.Index.ToString());
                FlattenRecursive(flatMap, child, path, separator);
            }
        }
        #endregion

        #endregion

        #region NodeList
        #region Create
        public static TNode CreateNode<TNode>(this NodeList nodeList) where TNode : Node, new() {
            var node = new TNode();
            nodeList.AddChild(node);
            return node;
        }

        public static NodeMap CreateMap(this NodeList nodeList) {
            var node = new NodeMap();
            nodeList.AddChild(node);
            return node;
        }

        public static NodeList CreateList(this NodeList nodeList) {
            var node = new NodeList();
            nodeList.AddChild(node);
            return node;
        }


        public static NodeValue CreateValue(this NodeList nodeList, object value = null) {
            var node = new NodeValue(value);
            nodeList.AddChild(node);
            return node;
        }
        #endregion

        #region Get
        public static object GetChildValue(this NodeList nodeList, int index) {
            return GetChildValue(nodeList, index, default(object));
        }

        public static object GetChildValue(this NodeList nodeList, int index, object defaultValue) {
            var nodeValue = nodeList.GetChild<NodeValue>(index);
            return nodeValue != null ? nodeValue.Value : defaultValue;
        }

        public static object GetChildValue(this NodeList nodeList, int index, Type type) {
            return GetChildValue(nodeList, index, default(object));
        }

        public static object GetChildValue(this NodeList nodeList, int index, Type type, object defaultValue) {
            var nodeValue = nodeList.GetChild<NodeValue>(index);
            return nodeValue != null ? nodeValue.GetValue(type, defaultValue) : defaultValue;
        }

        public static TValue GetChildValue<TValue>(this NodeList nodeList, int index) {
            return GetChildValue(nodeList, index, default(TValue));
        }

        public static TValue GetChildValue<TValue>(this NodeList nodeList, int index, TValue defaultValue) {
            var nodeValue = nodeList.GetChild<NodeValue>(index);
            return nodeValue == null ? default : nodeValue.GetValue(defaultValue);
        }

        public static bool TryGetChildValue(this NodeList nodeList, int index, out object value) {
            if(nodeList.TryGetChild<NodeValue>(index, out var node)) {
                value = node.Value;
                return true;
            }
            value = null;
            return false;
        }

        public static bool TryGetChildValue(this NodeList nodeList, int index, Type type, out object value) {
            if(nodeList.TryGetChild<NodeValue>(index, out var node)) {
                value = node.GetValue(type);
                return true;
            }
            value = null;
            return false;
        }

        public static bool TryGetChildValue<TValue>(this NodeList nodeList, int index, out TValue value) {
            if(nodeList.TryGetChild<NodeValue>(index, out var node)) {
                value = node.GetValue<TValue>();
                return true;
            }
            value = default;
            return false;
        }
        #endregion
        #endregion

        public static Node Clone(this Node node) {
            var reader = new NodeReader(node);
            var writer = new NodeWriter();
            var task = reader.CopyToAsync(writer);
            task.ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();
            return writer.Root;
        }

        public static TNode Clone<TNode>(this TNode node) where TNode : Node {
            return (TNode)Clone((Node)node);
        }

        public static async Task<TNode> ReadNodeAsync<TNode>(this IAsyncNodeReader reader) where TNode : Node
        {
            return (TNode)(await ReadNodeAsync(reader).ConfigureAwait(false));
        }
        
        public static async Task<Node> ReadNodeAsync(this IAsyncNodeReader reader)
        {
            var writer = new NodeWriter();
            do
            {
                var operation = await reader.ReadOperationAsync().ConfigureAwait(true);
                await writer.WriteOperationAsync(operation).ConfigureAwait(true);
            } while (!writer.IsComplete);

            return writer.Root;
        }
        
        public static async Task WriteNodeAsync(this IAsyncNodeWriter writer, Node node)
        {
            var reader = new NodeReader(node);
            await reader.CopyToAsync(writer).ConfigureAwait(true);
        }
    }
}
