// ==================================================
// Copyright 2018(C) , DotLogix
// File:  NodeContainer.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  16.07.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Collections.Generic;
using System.Linq;
using DotLogix.Core.Extensions;
#endregion

namespace DotLogix.Core.Nodes {
    public abstract class NodeContainer : Node {
        public abstract int ChildCount { get; }

        internal NodeContainer() { }

        #region Helper
        protected override ICollection<string> GetFormattingMembers() {
            var list = base.GetFormattingMembers();
            list.Add($"ChildCount: {ChildCount}");
            return list;
        }
        #endregion

        #region Children
        public abstract IEnumerable<Node> Children();

        public IEnumerable<TNode> Children<TNode>() {
            return Children().OfType<TNode>();
        }
        #endregion

        #region Descendants
        public IEnumerable<Node> Descendants() {
            var currentLevel = new List<IEnumerable<Node>> {Children()};
            var nextLevel = new List<IEnumerable<Node>>();

            do {
                foreach(var node in currentLevel.Balance()) {
                    yield return node;
                    if(!(node is NodeContainer nodeContainer))
                        continue;
                    nextLevel.Add(nodeContainer.Children());
                }

                var temp = currentLevel;
                currentLevel = nextLevel;
                nextLevel = temp;
                nextLevel.Clear();
            } while(currentLevel.Count > 0);
        }

        public IEnumerable<TNode> Descendants<TNode>() where TNode : Node {
            return Descendants().OfType<TNode>();
        }

        public TNode NearestDescendant<TNode>() where TNode : Node {
            return Descendants<TNode>().FirstOrDefault();
        }
        #endregion
    }
}
