using System;
using System.Collections.Generic;
using System.Linq;
using DotLogix.Core.Extensions;

namespace DotLogix.Core.Nodes {
    public abstract class NodeCollection : Node
    {
        protected List<Node> NodeList { get; } = new List<Node>();
        public int ChildCount => NodeList.Count;

        protected NodeCollection(NodeTypes nodeType) : base(nodeType) { }


        public virtual void AddChild(Node node) {
            if(node == null)
                throw new ArgumentNullException(nameof(node));
            node.Ancestor = this;

            var last = LastChild();
            if(last != null) {
                last.Next = node;
                node.Previous = last;
            }

            NodeList.Add(node);
        }

        public IEnumerable<Node> Children() {
            return NodeList;
        }

        public IEnumerable<TNode> Children<TNode>() where TNode : Node {
            return NodeList.OfType<TNode>();
        }

        public IEnumerable<Node> Decendents()
        {
            foreach (var node in NodeList)
            {
                yield return node;
                var collection = node as NodeCollection;
                if(collection == null)
                    continue;
                foreach (var decendent in collection.Decendents()) {
                    yield return decendent;
                }
            }
        }

        public IEnumerable<TNode> Decendents<TNode>(CombineMode combineMode = CombineMode.Sequential) where TNode : Node {
            return Decendents().OfType<TNode>();
        }

        public Node FirstChild() {
            return NodeList.Count > 0 ? NodeList[0] : null;
        }

        public TNode FirstChild<TNode>() where TNode : Node {
            foreach(var node in NodeList) {
                if(node is TNode typedNode)
                    return typedNode;
            }
            return null;
        }

        public Node LastChild() {
            var count = NodeList.Count - 1;
            return count >= 0 ? NodeList[count] : null;
        }

        public TNode LastChild<TNode>() where TNode : Node
        {
            for(var i = NodeList.Count - 1; i >= 0; i--) {
                if (NodeList[i] is TNode typedNode)
                    return typedNode;
            }
            return null;
        }

        public TNode NearestDecendent<TNode>() where TNode : Node {
            return Decendents().OfType<TNode>().FirstOrDefault();
        }

        public virtual bool RemoveChild(Node node) {
            if(node == null)
                throw new ArgumentNullException(nameof(node));
            if(NodeList.Remove(node) == false)
                return false;
            node.Ancestor = null;

            var next = node.Next;
            var prev = node.Previous;

            if (next != null)
            {
                next.Previous = prev;
                node.Next = null;
            }

            if (prev != null)
            {
                prev.Next = next;
                node.Previous = null;
            }

            return true;
        }

        public virtual void RenameChild(Node node, string newName) {
            if(node == null)
                throw new ArgumentNullException(nameof(node));
            if(node.Ancestor != this)
                throw new InvalidOperationException("This node is not a child of this collection");
            node.InternalName = newName ?? throw new ArgumentNullException(nameof(newName));
        }

        public virtual void ReplaceChild(Node oldNode, Node newNode) {
            var index = NodeList.IndexOf(oldNode);
            if(index == -1)
                throw new InvalidOperationException("This node is not a child of this collection");

            ReplaceChild(oldNode, newNode, index);
        }

        protected void ReplaceChild(Node oldNode, Node newNode, int index) {
            if (oldNode == null)
                throw new ArgumentNullException(nameof(oldNode));
            if (newNode == null)
                throw new ArgumentNullException(nameof(newNode));

            if (oldNode == newNode)
                return;

            if (oldNode.Ancestor != this)
                throw new InvalidOperationException("This node is not a child of this collection");

            newNode.Ancestor = this;
            NodeList[index] = newNode;

            // apply next
            var next = oldNode.Next;
            if(next != null) {
                next.Previous = newNode;
                newNode.Next = next;
            }

            // apply prev
            var prev = oldNode.Previous;
            if(prev != null) {
                prev.Next = newNode;
                newNode.Previous = prev;
            }

            // reset old
            oldNode.Ancestor = null;
            oldNode.Previous = null;
            oldNode.Next = null;
        }
    }
}