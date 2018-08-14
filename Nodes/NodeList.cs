// ==================================================
// Copyright 2018(C) , DotLogix
// File:  NodeList.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  16.07.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using DotLogix.Core.Extensions;
using DotLogix.Core.Utils.Patterns;
#endregion

namespace DotLogix.Core.Nodes {
    public sealed class NodeList : NodeContainer {
        private readonly List<Node> _nodeList = new List<Node>();
        public override NodeTypes Type => NodeTypes.List;
        public override int ChildCount => _nodeList.Count;

        public Node this[int index] {
            get => _nodeList[index];
            set => ReplaceChild(index, value);
        }

        #region ReplaceChild
        public void ReplaceChild(int index, Node childNode) {
            if(childNode == null)
                throw new ArgumentNullException(nameof(childNode));

            if(childNode.Ancestor != null)
                throw new ArgumentException("Node already has an ancestor", nameof(childNode));

            var prevChild = _nodeList[index];
            _nodeList[index] = childNode;
            SyncNode(childNode, this, prevChild.Previous, prevChild.Next);
            ClearNode(prevChild);
        }
        #endregion


        #region IndexOfChild
        public int IndexOfChild(Node childNode) {
            if(childNode == null)
                throw new ArgumentNullException(nameof(childNode));
            return _nodeList.IndexOf(childNode);
        }
        #endregion

        #region Children
        public override IEnumerable<Node> Children() {
            return _nodeList;
        }
        #endregion

        #region Clear
        public void Clear() {
            foreach(var node in _nodeList)
                ClearNode(node);
            _nodeList.Clear();
        }
        #endregion

        #region GetChild
        public Node GetChild(int index) {
            return index.LaysBetween(0, _nodeList.Count - 1) ? _nodeList[index] : null;
        }

        public TNode GetChild<TNode>(int index) where TNode : Node {
            return index.LaysBetween(0, _nodeList.Count - 1) ? _nodeList[index] as TNode : null;
        }

        public bool TryGetChild(int index, out Node childNode) {
            if(index.LaysBetween(0, _nodeList.Count - 1)) {
                childNode = _nodeList[index];
                return true;
            }

            childNode = null;
            return false;
        }

        public bool TryGetChild<TNode>(int index, out TNode childNode) {
            if(index.LaysBetween(0, _nodeList.Count - 1) && _nodeList[index] is TNode typedNode) {
                childNode = typedNode;
                return true;
            }

            childNode = default;
            return false;
        }
        #endregion

        #region AddChild
        public void AddChild(Node childNode) {
            if(childNode == null)
                throw new ArgumentNullException(nameof(childNode));

            if(childNode.Ancestor != null)
                throw new ArgumentException("Node already has an ancestor", nameof(childNode));

            Apply(_nodeList.Count, childNode);
            _nodeList.Add(childNode);
        }

        public void AddChildren(IEnumerable<Node> childNodes) {
            foreach(var childNode in childNodes)
                AddChild(childNode);
        }

        public void InsertChild(int index, Node childNode) {
            if(childNode == null)
                throw new ArgumentNullException(nameof(childNode));

            if(childNode.Ancestor != null)
                throw new ArgumentException("Node already has an ancestor", nameof(childNode));

            _nodeList.Insert(index, childNode);
            Apply(index, childNode);
        }
        #endregion

        #region RemoveChild
        public void RemoveChild(int index) {
            var child = _nodeList[index];
            _nodeList.RemoveAt(index);

            SyncNode(child.Previous, child.Next);
            ClearNode(child);
        }

        public bool RemoveChild(Node childNode) {
            if(childNode == null)
                throw new ArgumentNullException(nameof(childNode));

            if(_nodeList.Remove(childNode) == false)
                return false;

            SyncNode(childNode.Previous, childNode.Next);
            ClearNode(childNode);
            return true;
        }
        #endregion

        #region FirstChild
        public Node FirstChild() {
            return _nodeList.Count > 0 ? _nodeList[0] : null;
        }

        public TNode FirstChild<TNode>() where TNode : Node {
            for(var i = 0; i < _nodeList.Count; i++) {
                if(_nodeList[i] is TNode typedNode)
                    return typedNode;
            }

            return null;
        }
        #endregion

        #region LastChild
        public TNode LastChild<TNode>() where TNode : Node {
            for(var i = _nodeList.Count - 1; i >= 0; i--) {
                if(_nodeList[i] is TNode typedNode)
                    return typedNode;
            }
            return null;
        }

        public Node LastChild() {
            return _nodeList.Count > 0 ? _nodeList[_nodeList.Count - 1] : null;
        }
        #endregion

        #region NthChild
        public TNode NthChild<TNode>(int index) where TNode : Node {
            return NthChild(index) as TNode;
        }

        public Node NthChild(int index) {
            if(index < 0)
                index += _nodeList.Count;
            if(index < 0)
                return null;
            return _nodeList.Count > index ? _nodeList[_nodeList.Count - 1] : null;
        }
        #endregion

        #region Helper
        private void Apply(int index, Node node) {
            var prev = index > 0 ? _nodeList[index - 1] : null;
            var next = index < (_nodeList.Count - 1) ? _nodeList[index + 1] : null;

            SyncNode(node, this, prev, next);
        }

        protected override void AddMatchingNodes(string pattern, List<Node> newResults) {
            if(Range.TryParse(pattern, out var range) == false)
                return;

            var childCount = ChildCount;

            var min = range.Min ?? 0;
            var max = range.Max ?? (childCount - 1);

            min = ((min % childCount) + childCount) % childCount;
            max = ((max % childCount) + childCount) % childCount;

            for(var i = min; i <= max; i++)
                newResults.Add(GetChild(i));
        }
        #endregion
    }
}
