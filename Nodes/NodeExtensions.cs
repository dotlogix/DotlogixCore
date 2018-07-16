using System;
using System.Collections.Generic;
using System.Text;

namespace DotLogix.Core.Nodes
{
    public static class NodeExtensions {
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

        public static NodeList CreateList(this NodeMap nodeMap, string name)
        {
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

        public static object GetChildValue(this NodeMap nodeMap, string name, Type type)
        {
            return GetChildValue(nodeMap, name, default(object));
        }

        public static object GetChildValue(this NodeMap nodeMap, string name, Type type, object defaultValue)
        {
            var nodeValue = nodeMap.GetChild<NodeValue>(name);
            return nodeValue != null ? nodeValue.GetValue(type, defaultValue) : defaultValue;
        }

        public static TValue GetChildValue<TValue>(this NodeMap nodeMap, string name)
        {
            return GetChildValue(nodeMap, name, default(TValue));
        }

        public static TValue GetChildValue<TValue>(this NodeMap nodeMap, string name, TValue defaultValue) {
            var nodeValue = nodeMap.GetChild<NodeValue>(name);
            return nodeValue == null ? default : nodeValue.GetValue(defaultValue);
        }

        public static bool TryGetChildValue(this NodeMap nodeMap, string name, out object value)
        {
            if (nodeMap.TryGetChild<NodeValue>(name, out var node))
            {
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

        public static bool TryGetChildValue<TValue>(this NodeMap nodeMap, string name, out TValue value)
        {
            if (nodeMap.TryGetChild<NodeValue>(name, out var node))
            {
                value = node.GetValue<TValue>();
                return true;
            }
            value = default;
            return false;
        }

        #endregion
        #endregion

        #region NodeList

        #region Create

        public static TNode CreateNode<TNode>(this NodeList nodeList) where TNode : Node, new()
        {
            var node = new TNode();
            nodeList.AddChild(node);
            return node;
        }

        public static NodeMap CreateMap(this NodeList nodeList)
        {
            var node = new NodeMap();
            nodeList.AddChild(node);
            return node;
        }

        public static NodeList CreateList(this NodeList nodeList)
        {
            var node = new NodeList();
            nodeList.AddChild(node);
            return node;
        }


        public static NodeValue CreateValue(this NodeList nodeList, object value = null)
        {
            var node = new NodeValue(value);
            nodeList.AddChild(node);
            return node;
        }

        #endregion

        #region Get

        public static object GetChildValue(this NodeList nodeList, int index)
        {
            return GetChildValue(nodeList, index, default(object));
        }

        public static object GetChildValue(this NodeList nodeList, int index, object defaultValue)
        {
            var nodeValue = nodeList.GetChild<NodeValue>(index);
            return nodeValue != null ? nodeValue.Value : defaultValue;
        }

        public static object GetChildValue(this NodeList nodeList, int index, Type type)
        {
            return GetChildValue(nodeList, index, default(object));
        }

        public static object GetChildValue(this NodeList nodeList, int index, Type type, object defaultValue)
        {
            var nodeValue = nodeList.GetChild<NodeValue>(index);
            return nodeValue != null ? nodeValue.GetValue(type, defaultValue) : defaultValue;
        }

        public static TValue GetChildValue<TValue>(this NodeList nodeList, int index)
        {
            return GetChildValue(nodeList, index, default(TValue));
        }

        public static TValue GetChildValue<TValue>(this NodeList nodeList, int index, TValue defaultValue)
        {
            var nodeValue = nodeList.GetChild<NodeValue>(index);
            return nodeValue == null ? default : nodeValue.GetValue(defaultValue);
        }

        public static bool TryGetChildValue(this NodeList nodeList, int index, out object value)
        {
            if (nodeList.TryGetChild<NodeValue>(index, out var node))
            {
                value = node.Value;
                return true;
            }
            value = null;
            return false;
        }

        public static bool TryGetChildValue(this NodeList nodeList, int index, Type type, out object value)
        {
            if (nodeList.TryGetChild<NodeValue>(index, out var node))
            {
                value = node.GetValue(type);
                return true;
            }
            value = null;
            return false;
        }

        public static bool TryGetChildValue<TValue>(this NodeList nodeList, int index, out TValue value)
        {
            if (nodeList.TryGetChild<NodeValue>(index, out var node))
            {
                value = node.GetValue<TValue>();
                return true;
            }
            value = default;
            return false;
        }

        #endregion

        #endregion
    }
}
