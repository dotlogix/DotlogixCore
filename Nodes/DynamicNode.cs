using System;
using System.Dynamic;
using DotLogix.Core.Extensions;
using DotLogix.Core.Nodes.Processor;

namespace DotLogix.Core.Nodes
{
    /// <summary>
    /// Creates a dynamic object with a internal hierarchy structure cast-able to matching types
    /// </summary>
    public class DynamicNode : DynamicObject
    {
        private readonly ConverterSettings _settings;

        /// <summary>
        /// The internal node
        /// </summary>
        public Node Node { get; }

        private DynamicNode(Node node, ConverterSettings settings = null) {
            this._settings = settings ?? new ConverterSettings();
            Node = node;
        }

        /// <inheritdoc />
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
            result = new DynamicNode(childNode, _settings);
            return true;
        }

        /// <inheritdoc />
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (!(Node is NodeMap nodeMap))
            {
                result = null;
                return false;
            }
            var childNode = nodeMap.GetChild(_settings.NamingStrategy?.TransformName(binder.Name) ?? binder.Name);
            result = new DynamicNode(childNode, _settings);
            return true;
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (!(Node is NodeMap nodeMap))
                return false;

            var node = ToNode(value);
            nodeMap.AddOrReplaceChild(binder.Name, node);
            return true;
        }

        /// <inheritdoc />
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
                result = Node.ToObject(binder.ReturnType, _settings);
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
                    return Nodes.ToNode(value, _settings);
            }
        }

        /// <summary>
        /// Creates a dynamic object using a existing node
        /// </summary>
        public static dynamic From(Node node, ConverterSettings settings = null)
        {
            return new DynamicNode(node, settings);
        }

        /// <summary>
        /// Creates a dynamic object using a new node map
        /// </summary>
        public static dynamic Map(ConverterSettings settings = null)
        {
            return new DynamicNode(new NodeMap(), settings);
        }

        /// <summary>
        /// Creates a dynamic object using a ne node list
        /// </summary>
        public static dynamic List(ConverterSettings settings = null)
        {
            return new DynamicNode(new NodeList(), settings);
        }
    }
}
