using System.Collections.Generic;
using System.Linq;
using DotLogix.Core.Extensions;

namespace DotLogix.Core.Nodes
{
    public abstract class NodeContainer : Node
    {

        internal NodeContainer() { }

        public abstract int ChildCount { get; }

        #region Children

        public abstract IEnumerable<Node> Children();

        public IEnumerable<TNode> Children<TNode>()
        {
            return Children().OfType<TNode>();
        }
        #endregion

        #region Descendants
        public IEnumerable<Node> Descendants()
        {
            var currentLevel = new List<IEnumerable<Node>> {Children()};
            List<IEnumerable<Node>> nextLevel = null;

            do {
                foreach (var node in currentLevel.Balance())
                {
                    yield return node;
                    if (!(node is NodeContainer nodeContainer)) continue;

                    if(nextLevel == null)
                        nextLevel = new List<IEnumerable<Node>> {nodeContainer.Children()};
                    else
                        nextLevel.Add(nodeContainer.Children());
                }

                var temp = currentLevel;
                currentLevel = nextLevel;
                nextLevel = temp;
                nextLevel.Clear();
            } while (currentLevel != null && currentLevel.Count > 0);
        }

        public IEnumerable<TNode> Descendants<TNode>() where TNode : Node
        {
            return Descendants().OfType<TNode>();
        }

        public TNode NearestDescendant<TNode>() where TNode : Node
        {
            return Descendants<TNode>().FirstOrDefault();
        }
        #endregion

        #region Helper
        protected override ICollection<string> GetFormattingMembers()
        {
            var list = base.GetFormattingMembers();
            list.Add($"ChildCount: {ChildCount}");
            return list;
        } 
        #endregion
    }
}