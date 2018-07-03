// ==================================================
// Copyright 2018(C) , DotLogix
// File:  Node.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotLogix.Core.Extensions;
using DotLogix.Core.Nodes.Processor;
#endregion

namespace DotLogix.Core.Nodes {
    public abstract class Node {
        private const string PathSeperator = "\\";
        private const string RootCharacter = "~";
        private const string SelfCharacter = ".";
        private const string AncestorCharacter = "..";
        private const string AnyCharacter = "*";
        private static readonly char[] PathSeperators = {'\\', '/'};
        private static readonly char[] OptionSeperators = {'|'};

        private NodeCollection _ancestor;
        internal string InternalName;
        public NodeTypes Type { get; internal set; }

        public string Path {
            get {
                if(IsRoot)
                    return PathSeperator.ToString();

                var nodeStack = new Stack<Node>();
                nodeStack.Push(this);
                foreach(var ancestor in Ancestors())
                    nodeStack.Push(ancestor);
                nodeStack.Pop(); // Skip the first element on stack, because it is the root element
                var stringBuilder = new StringBuilder(RootCharacter);
                while(nodeStack.Count > 0) {
                    stringBuilder.Append(PathSeperator);
                    var currentNode = nodeStack.Pop();
                    switch(currentNode.Ancestor.Type) {
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
        public bool IsRoot => Ancestor == null;

        public NodeCollection Ancestor {
            get => _ancestor;
            internal set {
                if(value == _ancestor)
                    return;
                if((value != null) && (_ancestor != null))
                    throw new InvalidOperationException("This node already has a parent");
                _ancestor = value;
            }
        }

        public bool HasAncestor => Ancestor != null;
        public Node Previous { get; internal set; }
        public bool HasPrevious => Previous != null;
        public Node Next { get; internal set; }
        public bool HasNext => Next != null;

        protected Node(NodeTypes type) {
            Type = type;
        }

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
            while((currentNode = currentNode.Previous) != null)
                yield return currentNode;
        }

        public IEnumerable<Node> SiblingsAfter() {
            var currentNode = this;
            while((currentNode = currentNode.Next) != null)
                yield return currentNode;
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
            while((currentNode = currentNode.Ancestor) != null)
                yield return currentNode;
        }

        public IEnumerable<TNode> Ancestors<TNode>() where TNode : Node {
            return Ancestors().OfType<TNode>();
        }

        public TNode NearestAncestor<TNode>() where TNode : Node {
            return Ancestors<TNode>().FirstOrDefault();
        }

        public virtual Node SelectNode(string path) {
            return SelectNodes(path).FirstOrDefault();
        }

        public TNode SelectNode<TNode>(string path) where TNode : Node
        {
            return SelectNode(path) as TNode;
        }

        public virtual IEnumerable<Node> SelectNodes(string path)
        {
            return SelectNodes(path.Split(PathSeperators));
        }

        public IEnumerable<Node> SelectNodes(string[] pathParts) {
            return SelectNodes(pathParts, 0, pathParts.Length);
        }

        public IEnumerable<Node> SelectNodes(string[] pathParts, int start, int count) {
            if (pathParts == null)
                throw new ArgumentNullException(nameof(pathParts));
            if(start < 0 || start >= pathParts.Length)
                throw new ArgumentOutOfRangeException(nameof(start));
            if (count < 0 || start+count > pathParts.Length)
                throw new ArgumentOutOfRangeException(nameof(start));

            IEnumerable<Node> results = null;
            for(int i = start; i < start+count; i++) {
                var pathPart = pathParts[i];
                var newResults = new List<Node>();
                foreach (var currentNode in results ?? new []{this})
                {
                    switch (pathPart)
                    {
                        case "":
                        case SelfCharacter:
                            newResults.Add(currentNode);
                            break;
                        case RootCharacter:
                            newResults.Add(currentNode.Root);
                            break;
                        case AncestorCharacter:
                            if (currentNode.Ancestor == null)
                                break;
                            newResults.Add(currentNode.Ancestor);
                            break;
                        case AnyCharacter:
                            switch (Type)
                            {
                                case NodeTypes.Empty:
                                case NodeTypes.Value:
                                    newResults.Add(currentNode);
                                    break;
                                case NodeTypes.List:
                                case NodeTypes.Map:
                                    newResults.AddRange(((NodeCollection)currentNode).Children());
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                            break;
                        default:
                            var options = pathPart.Split(OptionSeperators, StringSplitOptions.RemoveEmptyEntries);
                            foreach(var option in options) {
                                currentNode.AddMatchingNodes(option, newResults);
                            }
                            break;
                    }
                }
                results = newResults;
            }
            return results ?? Enumerable.Empty<Node>();
        }

        public virtual IEnumerable<TNode> SelectNodes<TNode>(string path)
        {
            return SelectNodes(path).OfType<TNode>();
        }

        public IEnumerable<TNode> SelectNodes<TNode>(string[] pathParts) {
            return SelectNodes(pathParts, 0, pathParts.Length).OfType<TNode>();
        }

        public IEnumerable<TNode> SelectNodes<TNode>(string[] pathParts, int start, int count) {
            return SelectNodes(pathParts, start, count).OfType<TNode>();
        }

        protected virtual void AddMatchingNodes(string pattern, List<Node> newResults) {
            
        }

        protected virtual string CalculatePath() {
            if(IsRoot)
                return RootCharacter;

            switch(Ancestor.Type) {
                case NodeTypes.List:
                    return string.Concat(Ancestor.Path, PathSeperator, Index.ToString());
                case NodeTypes.Map:
                    return string.Concat(Ancestor.Path, PathSeperator, Name);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override string ToString() {
            if(HasAncestor) {
                switch(Ancestor.Type) {
                    case NodeTypes.List:
                        return $"Node{Type} {{Index: {Index}, Path: {Path}}}";
                    case NodeTypes.Map:
                        return $"{Type} {{Name: {Name}, Path: {Path}}}";
                }
            }
            return $"Node{Type} {{Path: {Path}}}";
        }
    }
}
