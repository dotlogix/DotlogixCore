// ==================================================
// Copyright 2018(C) , DotLogix
// File:  Node.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  16.07.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotLogix.Core.Extensions;
#endregion

namespace DotLogix.Core.Nodes {
    public abstract class Node {
        protected const string PathSeperator = "\\";
        protected const string RootCharacter = "~";
        protected const string SelfCharacter = ".";
        protected const string AncestorCharacter = "..";
        protected const string AnyCharacter = "*";
        protected const string DescendantsCharacter = "**";
        protected static readonly char[] PathSeperators = {'\\', '/'};
        protected static readonly char[] OptionSeperators = {'|'};

        #region Enumerate
        private static IEnumerable<Node> Enumerate(Node currentNode, Func<Node, Node> selectNextFunc) {
            while((currentNode = selectNextFunc(currentNode)) != null)
                yield return currentNode;
        }
        #endregion


        public override string ToString() {
            var sb = new StringBuilder(GetType().Name);
            sb.Append(" {");
            sb.AppendJoin(", ", GetFormattingMembers());
            sb.Append("}");
            return sb.ToString();
        }

        protected virtual ICollection<string> GetFormattingMembers() {
            var list = new List<string>();
            if(Ancestor == null)
                list.Add("Root");
            else {
                switch(Ancestor.Type) {
                    case NodeTypes.Map:
                        list.Add($"Name: {Name}");
                        break;
                    case NodeTypes.List:
                        list.Add($"Index: {((NodeList)Ancestor).IndexOfChild(this)}");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            return list;
        }

        #region Root
        public Node Root {
            get {
                NodeContainer ancestor;
                var current = this;
                while((ancestor = current.Ancestor) != null)
                    current = ancestor;
                return current;
            }
        }

        public bool IsRoot => Ancestor == null;
        #endregion

        #region Ancestor
        public NodeContainer Ancestor { get; private set; }
        public bool HasAncestor => Ancestor != null;
        #endregion

        #region Previous
        public Node Previous { get; private set; }
        public bool HasPrevious => Previous != null;
        #endregion

        #region Next
        public Node Next { get; private set; }
        public bool HasNext => Next != null;
        #endregion

        #region Misc
        public abstract NodeTypes Type { get; }
        public string Path => CalculatePath();
        public string Name { get; internal set; }
        public int Index => Ancestor is NodeList list ? list.IndexOfChild(this) : -1;

        private string CalculatePath() {
            var pathParts = new Stack<string>();
            var currentNode = this;
            NodeContainer ancestor;
            while((ancestor = currentNode.Ancestor) != null) {
                switch(ancestor.Type) {
                    case NodeTypes.Map:
                        pathParts.Push(currentNode.Name);
                        break;
                    case NodeTypes.List:
                        pathParts.Push(((NodeList)ancestor).IndexOfChild(currentNode).ToString());
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                currentNode = ancestor;
            }

            pathParts.Push(RootCharacter);
            return string.Join(PathSeperator, pathParts);
        }
        #endregion

        #region Siblings
        public IEnumerable<Node> SiblingsBefore() {
            return Enumerate(this, n => n.Previous);
        }

        public IEnumerable<Node> SiblingsAfter() {
            return Enumerate(this, n => n.Next);
        }

        public IEnumerable<Node> Siblings(CombineMode combineMode = CombineMode.RoundRobin, bool preferPrevious = true) {
            return preferPrevious
                       ? SiblingsBefore().Combine(SiblingsAfter(), combineMode)
                       : SiblingsAfter().Combine(SiblingsBefore(), combineMode);
        }

        public IEnumerable<TNode> SiblingsBefore<TNode>() where TNode : Node {
            return SiblingsBefore().OfType<TNode>();
        }

        public IEnumerable<TNode> SiblingsAfter<TNode>() where TNode : Node {
            return SiblingsAfter().OfType<TNode>();
        }

        public IEnumerable<TNode> Siblings<TNode>(CombineMode combineMode = CombineMode.RoundRobin, bool preferPrevious = true) where TNode : Node {
            return Siblings(combineMode, preferPrevious).OfType<TNode>();
        }

        public Node NearestSibling(bool preferPrevious = true) {
            if(preferPrevious)
                return Previous ?? Next;
            return Next ?? Previous;
        }

        public TNode NearestSibling<TNode>(bool preferPrevious = true) where TNode : Node {
            return Siblings(CombineMode.RoundRobin, preferPrevious).OfType<TNode>().FirstOrDefault();
        }
        #endregion

        #region Ancestors
        public IEnumerable<Node> Ancestors() {
            return Enumerate(this, n => n.Ancestor);
        }

        public IEnumerable<TNode> Ancestors<TNode>() where TNode : Node {
            return Ancestors().OfType<TNode>();
        }

        public TNode NearestAncestor<TNode>() where TNode : Node {
            return Ancestors<TNode>().FirstOrDefault();
        }
        #endregion

        #region SelectNodes
        public virtual Node SelectNode(string path) {
            return SelectNodes(path).FirstOrDefault();
        }

        public TNode SelectNode<TNode>(string path) where TNode : Node {
            return SelectNode(path) as TNode;
        }

        public virtual IEnumerable<Node> SelectNodes(string path) {
            return SelectNodes(path.Split(PathSeperators));
        }

        public IEnumerable<Node> SelectNodes(string[] pathParts) {
            return SelectNodes(pathParts, 0, pathParts.Length);
        }

        public IEnumerable<Node> SelectNodes(string[] pathParts, int start, int count) {
            if(pathParts == null)
                throw new ArgumentNullException(nameof(pathParts));
            if((start < 0) || (start >= pathParts.Length))
                throw new ArgumentOutOfRangeException(nameof(start));
            if((count < 0) || ((start + count) > pathParts.Length))
                throw new ArgumentOutOfRangeException(nameof(start));

            IEnumerable<Node> results = null;
            for(var i = start; i < (start + count); i++) {
                var pathPart = pathParts[i];
                var newResults = new List<Node>();
                foreach(var currentNode in results ?? new[] {this}) {
                    switch(pathPart) {
                        case "":
                        case SelfCharacter:
                            newResults.Add(currentNode);
                            break;
                        case RootCharacter:
                            newResults.Add(currentNode.Root);
                            break;
                        case AncestorCharacter:
                            if(currentNode.Ancestor == null)
                                break;
                            newResults.Add(currentNode.Ancestor);
                            break;
                        case AnyCharacter:
                            switch(currentNode.Type) {
                                case NodeTypes.Empty:
                                case NodeTypes.Value:
                                    //newResults.Add(currentNode);
                                    break;
                                case NodeTypes.Map:
                                case NodeTypes.List:
                                    newResults.AddRange(((NodeContainer)currentNode).Children());
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                            break;
                        case DescendantsCharacter:
                            switch (currentNode.Type)
                            {
                                case NodeTypes.Empty:
                                case NodeTypes.Value:
                                    //newResults.Add(currentNode);
                                    break;
                                case NodeTypes.Map:
                                case NodeTypes.List:
                                    newResults.AddRange(((NodeContainer)currentNode).Descendants());
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                            break;
                        default:
                            var options = pathPart.Split(OptionSeperators, StringSplitOptions.RemoveEmptyEntries);
                            foreach(var option in options)
                                currentNode.AddMatchingNodes(option, newResults);
                            break;
                    }
                }
                results = newResults;
            }
            return results ?? Enumerable.Empty<Node>();
        }

        public virtual IEnumerable<TNode> SelectNodes<TNode>(string path) {
            return SelectNodes(path).OfType<TNode>();
        }

        public IEnumerable<TNode> SelectNodes<TNode>(string[] pathParts) {
            return SelectNodes(pathParts, 0, pathParts.Length).OfType<TNode>();
        }

        public IEnumerable<TNode> SelectNodes<TNode>(string[] pathParts, int start, int count) {
            return SelectNodes(pathParts, start, count).OfType<TNode>();
        }

        protected virtual void AddMatchingNodes(string pattern, List<Node> newResults) { }
        #endregion

        #region Helper
        protected static void SyncNode(Node node, NodeContainer ancestor, Node prev, Node next, string name = null) {
            node.Ancestor = ancestor;
            node.Name = name;
            SyncNode(node, next);
            SyncNode(prev, node);
        }

        protected static void SyncNode(Node prev, Node next) {
            if(prev != null)
                prev.Next = next;

            if(next != null)
                next.Previous = prev;
        }

        protected static void ClearNode(Node node) {
            node.Previous = null;
            node.Next = null;
            node.Ancestor = null;
            node.Name = null;
        }
        #endregion
    }
}
