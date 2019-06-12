// ==================================================
// Copyright 2018(C) , DotLogix
// File:  NodeMap.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  16.07.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DotLogix.Core.Extensions;
using DotLogix.Core.Utils.Patterns;
#endregion

namespace DotLogix.Core.Nodes {
    public class NodeMap : NodeContainer {
        private readonly Dictionary<string, Node> _nodeMap = new Dictionary<string, Node>();

        private Node _last;

        public override NodeTypes Type => NodeTypes.Map;

        public override int ChildCount => _nodeMap.Count;

        public Node this[string name] {
            get => _nodeMap[name];
            set => AddOrReplaceChild(name, value);
        }

        #region Clear
        public void Clear() {
            foreach(var node in _nodeMap.Values)
                ClearNode(node);
            _nodeMap.Clear();
        }
        #endregion

        #region Children
        public override IEnumerable<Node> Children() {
            return _nodeMap.Values;
        }
        #endregion

        protected override void AddMatchingNodes(string pattern, List<Node> newResults) {
            if (pattern.StartsWith("<$") && pattern.EndsWith("$>")) {
                var regex = new Regex(pattern.Substring(2, pattern.Length - 4));
                newResults.AddRange(_nodeMap.Values.Where(n => regex.IsMatch(n.Name)));
            }else if(_nodeMap.TryGetValue(pattern, out var node))
                newResults.Add(node);
        }

        #region GetChild
        public Node GetChild(string name) {
            if(name == null)
                throw new ArgumentNullException(nameof(name));

            return _nodeMap.TryGetValue(name, out var node) ? node : null;
        }

        public TNode GetChild<TNode>(string name) where TNode : Node {
            if(name == null)
                throw new ArgumentNullException(nameof(name));

            return _nodeMap.TryGetValue(name, out var node) ? node as TNode : null;
        }

        public bool TryGetChild(string name, out Node childNode) {
            if(name == null)
                throw new ArgumentNullException(nameof(name));

            return _nodeMap.TryGetValue(name, out childNode);
        }

        public bool TryGetChild<TNode>(string name, out TNode childNode) {
            if(name == null)
                throw new ArgumentNullException(nameof(name));

            if(_nodeMap.TryGetValue(name, out var node) && node is TNode typedNode) {
                childNode = typedNode;
                return true;
            }

            childNode = default;
            return false;
        }
        #endregion

        #region AddChild
        public void AddChild(string name, Node childNode) {
            if(name == null)
                throw new ArgumentNullException(nameof(name));

            if(childNode == null)
                throw new ArgumentNullException(nameof(childNode));

            if(childNode.Ancestor != null)
                throw new ArgumentException("Node already has an ancestor", nameof(childNode));

            SyncNode(childNode, this, _last, null, name);
            _last = childNode;
            _nodeMap.Add(name, childNode);
        }


        public void AddChildren(IEnumerable<KeyValuePair<string, Node>> childProperties) {
            foreach(var childProperty in childProperties)
                AddChild(childProperty.Key, childProperty.Value);
        }
        #endregion

        #region RemoveChild
        public void RemoveChild(string name) {
            var child = _nodeMap[name];

            if(child == _last)
                _last = child.Previous;

            _nodeMap.Remove(name);

            SyncNode(child.Previous, child.Next);
            ClearNode(child);
        }

        public bool RemoveChild(Node childNode) {
            if(childNode == null)
                throw new ArgumentNullException(nameof(childNode));

            if(_nodeMap.TryGetKey(childNode, out var name) == false)
                return false;

            if(childNode == _last)
                _last = childNode.Previous;

            _nodeMap.Remove(name);

            SyncNode(childNode.Previous, childNode.Next);
            ClearNode(childNode);
            return true;
        }
        #endregion

        #region ReplaceChild
        public void AddOrReplaceChild(string name, Node childNode) {
            if(name == null)
                throw new ArgumentNullException(nameof(name));

            if(_nodeMap.TryGetValue(name, out var prevChild))
                ReplaceChild(name, prevChild, childNode);
            else
                AddChild(name, childNode);
        }

        public void ReplaceChild(string name, Node childNode) {
            if(name == null)
                throw new ArgumentNullException(nameof(name));

            if(_nodeMap.TryGetValue(name, out var prevChild))
                ReplaceChild(name, prevChild, childNode);
            else
                throw new KeyNotFoundException("A node with this name does not exist");
        }

        public void ReplaceChild(string name, Node prevChild, Node childNode) {
            if(childNode == null)
                throw new ArgumentNullException(nameof(childNode));

            if(childNode.Ancestor != null)
                throw new ArgumentException("Node already has an ancestor", nameof(childNode));

            if(prevChild == _last)
                _last = childNode;

            _nodeMap[name] = childNode;
            SyncNode(childNode, this, prevChild.Previous, prevChild.Next, name);
            ClearNode(prevChild);
        }
        #endregion

        #region Properties
        public IEnumerable<KeyValuePair<string, TNode>> Properties<TNode>() where TNode : Node {
            return _nodeMap.Where(n => n.Value is TNode).Select(n => new KeyValuePair<string, TNode>(n.Key, (TNode)n.Value));
        }

        public IEnumerable<KeyValuePair<string, Node>> Properties() {
            return _nodeMap;
        }
        #endregion
    }
}
