using System;
using System.Dynamic;
using DotLogix.Core.Extensions;
using DotLogix.Core.Nodes.Processor;

namespace DotLogix.Core.Nodes
{
    public class DynamicNode : DynamicObject
    {
        private readonly ConverterSettings _settings;
        public NodeContainer Node { get; }

        private DynamicNode(NodeContainer node, ConverterSettings settings = null) {
            this._settings = settings ?? new ConverterSettings();
            Node = node;
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            if (indexes.Length != 1)
            {
                result = null;
                return false;
            }

            var objIndex = indexes[0];
            Node childNode;
            switch (Node)
            {
                case NodeList nodeList when objIndex is int index:
                    childNode = nodeList.GetChild(index);
                    break;
                case NodeMap nodeMap when objIndex is string name:
                    childNode = nodeMap.GetChild(_settings.NamingStrategy?.TransformName(name) ?? name);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            result = ToValue(childNode);
            return true;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (!(Node is NodeMap nodeMap))
            {
                result = null;
                return false;
            }
            var childNode = nodeMap.GetChild(_settings.NamingStrategy?.TransformName(binder.Name) ?? binder.Name);
            result = ToValue(childNode);
            return true;
        }

        public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
        {
            if (indexes.Length != 1)
                return false;

            var node = ToNode(value);
            var objIndex = indexes[0];
            switch (Node)
            {
                case NodeList nodeList when objIndex is int index:
                    nodeList.ReplaceChild(index, node);
                    break;
                case NodeMap nodeMap when objIndex is string name:
                    nodeMap.AddOrReplaceChild(name, node);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (!(Node is NodeMap nodeMap))
                return false;

            var node = ToNode(value);
            nodeMap.AddOrReplaceChild(binder.Name, node);
            return true;
        }

        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            if(binder.ReturnType.IsAssignableTo<Node>()) {
                if(Node.GetType().IsAssignableTo(binder.ReturnType)) {
                    result = Node;
                    return true;
                }
                result = null;
                return false;
            }

            try
            {
                result = Nodes.ToObject(Node, binder.ReturnType, _settings);
                return true;
            }
            catch
            {
                result = binder.ReturnType.GetDefaultValue();
                return false;
            }
        }

        private Node ToNode(object value)
        {
            switch (value)
            {
                case Node node:
                    return node;
                case DynamicNode dynNode:
                    return dynNode.Node;
                default:
                    return Nodes.ToNode(value);
            }
        }

        private object ToValue(Node node)
        {
            switch (node.Type)
            {
                case NodeTypes.Empty:
                case NodeTypes.Value:
                    return ((NodeValue)node).Value;
                case NodeTypes.List:
                case NodeTypes.Map:
                    return new DynamicNode((NodeContainer)node);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static dynamic From(NodeContainer nodeContainer)
        {
            return new DynamicNode(nodeContainer);
        }

        public static dynamic Map()
        {
            return new DynamicNode(new NodeMap());
        }

        public static dynamic List()
        {
            return new DynamicNode(new NodeList());
        }
    }
}
