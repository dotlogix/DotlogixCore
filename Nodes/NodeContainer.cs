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

        public IEnumerable<Node> Descendants()
        {
            return ChildCount != 0
                ? Children().EnumerateRecursive(c => (c as NodeContainer)?.Children())
                : Enumerable.Empty<Node>();
        }

        public IEnumerable<TNode> Descendants<TNode>() where TNode : Node {
            return Descendants().OfType<TNode>();
        }

        #endregion
    }
}
