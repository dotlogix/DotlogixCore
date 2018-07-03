using System;
using System.Collections.Generic;
using System.Text;

namespace DotLogix.Core.Nodes
{
    public static class NodeExtensions {
        #region NodeMap
        public static TNode CreateNode<TNode>(this NodeMap nodeMap, string name) where TNode : Node, new() {
            var node = new TNode { InternalName = name };
            nodeMap.AddChild(node);
            return node;
        }

        public static NodeMap CreateMap(this NodeMap nodeMap, string name) {
            var node = new NodeMap(name);
            nodeMap.AddChild(node);
            return node;
        }

        public static NodeList CreateList(this NodeMap nodeMap, string name)
        {
            var node = new NodeList(name);
            nodeMap.AddChild(node);
            return node;
        }


        public static NodeValue CreateValue(this NodeMap nodeMap, string name, object value = null) {
            var node = new NodeValue(name, value);
            nodeMap.AddChild(node);
            return node;
        }

        public static object GetChildValue(this NodeMap nodeMap, string name) {
            return nodeMap.GetChild<NodeValue>(name).Value;
        }

        public static object GetChildValue(this NodeMap nodeMap, string name, Type type)
        {
            return nodeMap.GetChild<NodeValue>(name).GetValue(type);
        }

        public static TValue GetChildValue<TValue>(this NodeMap nodeMap, string name) {
            var nodeValue = nodeMap.GetChild<NodeValue>(name);
            return nodeValue == null ? default(TValue) : nodeValue.GetValue<TValue>();
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
            value = default(TValue);
            return false;
        }

        public static void MergeChild(this NodeMap nodeMap, string name, NodeValue nodeValue) {
            if(nodeValue.HasAncestor)
                throw new InvalidOperationException("This node already has a parent");

            if(nodeMap.TryGetChild(name, out var existing)) {
                switch(existing.Type) {
                    case NodeTypes.Empty:
                    case NodeTypes.Value:
                    case NodeTypes.Map:
                        nodeMap.RemoveChild(existing);

                        var list = nodeMap.CreateList(name);
                        list.AddChild(existing);
                        list.AddChild(nodeValue);
                        break;
                    case NodeTypes.List:
                        var existingList = (NodeList)existing;
                        existingList.AddChild(nodeValue);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public static void MergeChild(this NodeMap nodeMap, string name, NodeMap nodeValue)
        {
            if (nodeValue.HasAncestor)
                throw new InvalidOperationException("This node already has a parent");

            if (nodeMap.TryGetChild(name, out var existing))
            {
                switch (existing.Type)
                {
                    case NodeTypes.Empty:
                    case NodeTypes.Value:
                    case NodeTypes.Map:
                        nodeMap.RemoveChild(existing);

                        var list = nodeMap.CreateList(name);
                        list.AddChild(existing);
                        list.AddChild(nodeValue);
                        break;
                    case NodeTypes.List:
                        var existingList = (NodeList)existing;
                        existingList.AddChild(nodeValue);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        #endregion

        #region NodeList

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

        public static object GetChildValue(this NodeList nodeList, int index)
        {
            return nodeList.GetChild<NodeValue>(index).Value;
        }

        public static object GetChildValue(this NodeList nodeList, int index, Type type)
        {
            return nodeList.GetChild<NodeValue>(index).GetValue(type);
        }

        public static TValue GetChildValue<TValue>(this NodeList nodeList, int index)
        {
            var nodeValue = nodeList.GetChild<NodeValue>(index);
            return nodeValue == null ? default(TValue) : nodeValue.GetValue<TValue>();
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
            value = default(TValue);
            return false;
        }
        #endregion
    }
}
