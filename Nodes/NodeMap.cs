// ==================================================
// Copyright 2018(C) , DotLogix
// File:  NodeMap.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace DotLogix.Core.Nodes {
    public class NodeMap : NodeCollection {
        private readonly Dictionary<string, Node> _nodeDict = new Dictionary<string, Node>(StringComparer.OrdinalIgnoreCase);
        public Node this[string name]
        {
            get => GetChild(name);
            set
            {
                if (value == null)
                {
                    RemoveChild(name);
                } else {
                    value.Ancestor = this;
                    value.InternalName = name;
                    ReplaceChild(value);
                }
            }
        }
        public NodeMap() : base(NodeTypes.Map) { }

        public NodeMap(string name) : this() {
            InternalName = name;
        }

        public Node GetChild(string name) {
            return _nodeDict.TryGetValue(name, out var node) ? node : null;
        }

        public TNode GetChild<TNode>(string name) where TNode : Node {
            return GetChild(name) as TNode;
        }

        public bool TryGetChild(string name, out Node childNode)
        {
            return (childNode = GetChild(name)) != null;
        }

        public bool TryGetChild<TNode>(string name, out TNode childNode) where TNode : Node
        {
            return (childNode = GetChild<TNode>(name)) != null;
        }

        public bool RemoveChild(string name) {
            var node = GetChild(name);
            return (node != null) && RemoveChild(node);
        }

        public Node PopChild(string name) {
            var node = GetChild(name);
            if(node == null)
                return null;

            RemoveChild(name);
            return node;
        }

        public TNode PopChild<TNode>(string name) where TNode : Node {
            if(!(GetChild(name) is TNode node))
                return null;

            RemoveChild(name);
            return node;
        }

        public void ReplaceChild(Node node) {
            var oldNode = GetChild(node.Name);
            ReplaceChild(oldNode, node);
        }

        public override void AddChild(Node node) {
            _nodeDict.Add(node.Name, node);
            base.AddChild(node);
        }

        public virtual void AddChild(string name, Node node) {
            node.Ancestor = this;
            node.InternalName = name;
            _nodeDict.Add(node.Name, node);
            base.AddChild(node);
        }

        public override bool RemoveChild(Node node) {
            return base.RemoveChild(node) && _nodeDict.Remove(node.Name);
        }

        public override void RenameChild(Node node, string newName) {
            if(node == null)
                throw new ArgumentNullException(nameof(node));
            if(node.Ancestor != this)
                throw new InvalidOperationException("This node is not a child of this collection");

            _nodeDict.Remove(node.Name);
            _nodeDict.Add(newName, node);
            base.RenameChild(node, newName);
        }

        public override void ReplaceChild(Node oldNode, Node newNode) {
            base.ReplaceChild(oldNode, newNode);
            _nodeDict.Remove(oldNode.Name);
            _nodeDict.Add(newNode.Name, newNode);
        }

        public void RenameChild(string oldName, string newName) {
            var node = GetChild(oldName);
            RenameChild(node, newName);
        }

        protected override void AddMatchingNodes(string pattern, List<Node> newResults) {
            if(TryGetChild(pattern, out var child))
                newResults.Add(child);
        }
    }
}
