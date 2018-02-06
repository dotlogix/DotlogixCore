// ==================================================
// Copyright 2016(C) , DotLogix
// File:  Node.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.10.2017
// LastEdited:  18.10.2017
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotLogix.Core.Extensions;
using DotLogix.Core.Nodes.Io;
#endregion

namespace DotLogix.Core.Nodes {
    public abstract class Node {
        private const char PathSeperator = '\\';
        private const string RootCharacter = "~";
        private const string SelfCharacter = ".";
        private const string AncestorCharacter = "..";
        private static readonly char[] PathSeperators = { PathSeperator, '/'};
        internal string InternalName;
        public NodeTypes NodeType { get; internal set; }

        protected Node(NodeTypes nodeType) {
            NodeType = nodeType;
        }

        public string Path {
            get {
                if(IsRoot)
                    return PathSeperator.ToString();

                var nodeStack = new Stack<Node>();
                nodeStack.Push(this);
                foreach(var ancestor in Ancestors()) {
                    nodeStack.Push(ancestor);
                }
                nodeStack.Pop(); // Skip the first element on stack, because it is the root element
                var stringBuilder = new StringBuilder(RootCharacter);
                while(nodeStack.Count > 0) {
                    stringBuilder.Append(PathSeperator);
                    var currentNode = nodeStack.Pop();
                    switch(currentNode.Ancestor.NodeType) {
                        case NodeTypes.List:
                            stringBuilder.Append(currentNode.Index);
                            break;
                        case NodeTypes.Map:
                            stringBuilder.Append(currentNode.Name);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                return stringBuilder.ToString();
            }
        }

        public string Name {
            get => InternalName;
            set => Rename(value);
        }

        public int Index => (Ancestor as NodeList)?.IndexOfChild(this) ?? -1;
        public Node Root => Ancestor != null ? Ancestor.Root : this;
        public bool IsRoot => HasAncestor == false;
        private NodeCollection _ancestor;

        public NodeCollection Ancestor {
            get => _ancestor;
            internal set {
                if(value == _ancestor)
                    return;
                if(value != null && _ancestor != null)
                    throw new InvalidOperationException("This node already has a parent");
                _ancestor = value;
            }
        }

        public bool HasAncestor => Ancestor != null;
        public Node Previous { get; internal set; }
        public bool HasPrevious => Previous != null;
        public Node Next { get; internal set; }
        public bool HasNext => Next != null;

        public void Rename(string newName) {
            if(InternalName == newName)
                return;
            Ancestor?.RenameChild(this, newName);
            InternalName = newName;
        }

        public IEnumerable<Node> Siblings(CombineMode mode = CombineMode.Sequential) {
            return EnumerableExtensions.Combine(mode, SiblingsBefore(), SiblingsAfter());
        }

        public IEnumerable<Node> SiblingsBefore() {
            var currentNode = this;
            while((currentNode = currentNode.Previous) != null) {
                yield return currentNode;
            }
        }

        public IEnumerable<Node> SiblingsAfter() {
            var currentNode = this;
            while((currentNode = currentNode.Next) != null) {
                yield return currentNode;
            }
        }

        public IEnumerable<TNode> Siblings<TNode>(CombineMode mode = CombineMode.Sequential) where TNode : Node {
            return Siblings(mode).OfType<TNode>();
        }

        public IEnumerable<TNode> SiblingsBefore<TNode>() where TNode : Node {
            return SiblingsBefore().OfType<TNode>();
        }

        public IEnumerable<TNode> SiblingsAfter<TNode>() where TNode : Node {
            return SiblingsAfter().OfType<TNode>();
        }

        public TNode NearestSibling<TNode>() where TNode : Node {
            return Siblings(CombineMode.RoundRobin).OfType<TNode>().FirstOrDefault();
        }

        public IEnumerable<Node> Ancestors() {
            var currentNode = this;
            while((currentNode = currentNode.Ancestor) != null) {
                yield return currentNode;
            }
        }

        public IEnumerable<TNode> Ancestors<TNode>() where TNode : Node {
            return Ancestors().OfType<TNode>();
        }

        public TNode NearestAncestor<TNode>() where TNode : Node {
            return Ancestors<TNode>().FirstOrDefault();
        }

        public virtual Node SelectNode(string path) {
            if(path == null)
                throw new ArgumentNullException(nameof(path));
            var split = path.Split(PathSeperators, 2, StringSplitOptions.None);
            Node currentNode;
            switch (split[0])
            {
                case "":
                case SelfCharacter:
                    currentNode = this;
                    break;
                case RootCharacter:
                    currentNode = Root;
                    break;
                case AncestorCharacter:
                    currentNode = Ancestor;
                    break;
                default:
                    currentNode = SelectNodeInternal(split[0]);
                    break;
            }

            return split.Length == 1 ? currentNode : currentNode?.SelectNode(split[1]);
        }

        protected virtual Node SelectNodeInternal(string pathToken) {
            return null;
        }

        public TNode SelectNode<TNode>(string path) where TNode : Node {
            return SelectNode(path) as TNode;
        }

        public override string ToString() {
            return JsonNodes.ToJson(this, new JsonNodesFormatter() {Ident = true});
        }
    }
}
