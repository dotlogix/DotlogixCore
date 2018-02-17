// ==================================================
// Copyright 2018(C) , DotLogix
// File:  NodeList.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Core.Nodes {
    public class NodeList : NodeCollection {
        public Node this[int index] => GetChild(index);

        public NodeList(string name) : this() {
            InternalName = name;
        }

        public NodeList() : base(NodeTypes.List) { }

        public override void AddChild(Node node) {
            if(node.InternalName != null)
                throw new InvalidOperationException("Children in a node list can not have a name");
            base.AddChild(node);
        }

        public virtual TNode CreateChild<TNode>() where TNode : Node, new() {
            var node = new TNode();
            AddChild(node);
            return node;
        }

        public Node GetChild(int index) {
            if((index < 0) || (index > NodeList.Count))
                return null;
            return NodeList[index];
        }

        public TNode GetChild<TNode>(int index) where TNode : Node {
            return GetChild(index) as TNode;
        }

        public void InsertChild(int index, Node node) {
            if(node.InternalName != null)
                throw new InvalidOperationException("Children in a node list can not have a name");
            if(index < 0)
                index = 0;

            var next = GetChild(index);
            var prev = GetChild(index - 1);

            if((next == null) && (prev == null)) {
                AddChild(node);
                return;
            }

            NodeList.Insert(index, node);

            // apply next
            if(next != null) {
                next.Previous = node;
                node.Next = next;
            }

            // apply prev
            if(prev != null) {
                prev.Next = node;
                node.Previous = prev;
            }
        }

        public bool RemoveChild(int index) {
            var node = GetChild(index);
            if(node == null)
                return false;

            NodeList.RemoveAt(index);
            node.Ancestor = null;

            var next = node.Next;
            var prev = node.Previous;

            if(next != null) {
                next.Previous = prev;
                node.Next = null;
            }

            if(prev != null) {
                prev.Next = next;
                node.Previous = null;
            }

            return true;
        }

        public Node PopChild(int index) {
            var node = GetChild(index);
            if(node == null)
                return null;

            RemoveChild(index);
            return node;
        }

        public TNode PopChild<TNode>(int index) where TNode : Node {
            if(!(GetChild(index) is TNode node))
                return null;

            RemoveChild(index);
            return node;
        }

        public void ReplaceChild(int index, Node node) {
            if(node.InternalName != null)
                throw new InvalidOperationException("Children in a node list can not have a name");
            var child = GetChild(index);
            ReplaceChild(child, node, index);
        }

        public int IndexOfChild(Node node) {
            return NodeList.IndexOf(node);
        }

        public override void RenameChild(Node node, string newName) {
            throw new InvalidOperationException("Children in a node list can not have a name");
        }

        public void RenameChild(int index, string newName) {
            throw new InvalidOperationException("Children in a node list can not have a name");
        }

        protected override Node SelectNodeInternal(string pathToken) {
            return int.TryParse(pathToken, out var index) ? GetChild(index) : null;
        }
    }
}
