using System.Linq;
using DotLogix.Core.Nodes;
using NUnit.Framework;

namespace NodeTests
{
    [TestFixture]
    public class SelectSingleNodeTest
    {
        private NodeMap _node1;
        private NodeMap _node11;
        private NodeList _node12;
        private NodeValue _node111;
        private NodeValue _node112;
        private NodeValue _node121;
        private NodeValue _node122;

        public void OneTimeSetup()
        {
            _node1 = new NodeMap();
            _node11 = _node1.CreateMap("11");
            _node12 = _node1.CreateList("12");
            
            _node111 = _node11.CreateValue("111");
            _node112 = _node11.CreateValue("112");
                       
            _node121 = _node12.CreateValue();
            _node122 = _node12.CreateValue();
        }

        #region SelectReleative

        [Test]
        public void SelectSingleNode_SelectRoot()
        {
            var map = new NodeMap();
            map.CreateValue("value_01");
            var nestedList = map.CreateList("value_02");
            nestedList.CreateValue();

            var nestedMap = map.CreateMap("value_03");
            nestedMap.CreateValue("child_01");

            Assert.That(map.SelectNode("~"), Is.EqualTo(map));
            Assert.That(map.SelectNode("value_01/~"), Is.EqualTo(map));
            Assert.That(map.SelectNode("value_02/~"), Is.EqualTo(map));
            Assert.That(map.SelectNode("value_02/0/~"), Is.EqualTo(map));
            Assert.That(map.SelectNode("value_03/~"), Is.EqualTo(map));
            Assert.That(map.SelectNode("value_03/child_01/~"), Is.EqualTo(map));
        }

        [Test]
        public void SelectSingleNode_SelectParent()
        {
            var map = new NodeMap();
            map.CreateValue("value_01");
            var nestedList = map.CreateList("value_02");
            nestedList.CreateValue();

            var nestedMap = map.CreateMap("value_03");
            nestedMap.CreateValue("child_01");

            Assert.That(map.SelectNode(".."), Is.Null);
            Assert.That(map.SelectNode("value_01/.."), Is.EqualTo(map));
            Assert.That(map.SelectNode("value_02/.."), Is.EqualTo(map));
            Assert.That(map.SelectNode("value_02/0/.."), Is.EqualTo(nestedList));
            Assert.That(map.SelectNode("value_03/.."), Is.EqualTo(map));
            Assert.That(map.SelectNode("value_03/child_01/.."), Is.EqualTo(nestedMap));
        }

        [Test]
        public void SelectSingleNode_SelectSelf()
        {
            var map = new NodeMap();
            var child1 = map.CreateValue("value_01");
            var child2 = map.CreateMap("value_02");
            var child3 = map.CreateList("value_03");

            Assert.That(map.SelectNode<NodeValue>("value_01/."), Is.EqualTo(child1));
            Assert.That(map.SelectNode<NodeMap>("value_02/."), Is.EqualTo(child2));
            Assert.That(map.SelectNode<NodeList>("value_03/."), Is.EqualTo(child3));
        }

        #endregion

        #region SelectChild

        [Test]
        public void SelectSingleNode_Map_SelectInvalidPath()
        {
            var map = new NodeMap();
            map.CreateValue("value_01");

            Assert.That(map.SelectNode<NodeValue>("value_02"), Is.Null);
        }

        [Test]
        public void SelectSingleNode_List_SelectInvalidType()
        {
            var map = new NodeMap();
            map.CreateValue("value_01");

            Assert.That(map.SelectNode<NodeMap>("value_01"), Is.Null);
        }

        [Test]
        public void SelectSingleNode_Map_SelectChild()
        {
            var map = new NodeMap();
            var child1 = map.CreateValue("value_01");
            var child2 = map.CreateValue("value_02");

            Assert.That(map.SelectNode<NodeValue>("value_01"), Is.EqualTo(child1));
            Assert.That(map.SelectNode<NodeValue>("value_02"), Is.EqualTo(child2));
        }


        [Test]
        public void SelectSingleNode_Map_SelectNestedChild()
        {
            var map = new NodeMap();
            var childMap = map.CreateMap("child");
            var child1 = childMap.CreateValue("value_01");
            var child2 = childMap.CreateValue("value_02");

            Assert.That(map.SelectNode<NodeValue>("child/value_01"), Is.EqualTo(child1));
            Assert.That(map.SelectNode<NodeValue>("child/value_02"), Is.EqualTo(child2));
        }

        [Test]
        public void SelectSingleNode_List_SelectChild()
        {
            var nodeList = new NodeList();
            var child1 = nodeList.CreateValue();
            var child2 = nodeList.CreateValue();

            Assert.That(nodeList.SelectNode<NodeValue>("0"), Is.EqualTo(child1));
            Assert.That(nodeList.SelectNode<NodeValue>("1"), Is.EqualTo(child2));
        }

        [Test]
        public void SelectSingleNode_List_SelectNestedChild()
        {
            var list = new NodeList();
            var childList = list.CreateList();
            var child1 = childList.CreateValue();
            var child2 = childList.CreateValue();

            Assert.That(list.SelectNode<NodeValue>("0/0"), Is.EqualTo(child1));
            Assert.That(list.SelectNode<NodeValue>("0/1"), Is.EqualTo(child2));
        }

        [Test]
        public void SelectSingleNode_List_SelectLastChild()
        {
            var nodeList = new NodeList();
            var child1 = nodeList.CreateValue();
            var child2 = nodeList.CreateValue();

            Assert.That(nodeList.SelectNode<NodeValue>("-2"), Is.EqualTo(child1));
            Assert.That(nodeList.SelectNode<NodeValue>("-1"), Is.EqualTo(child2));
        }

        [Test]
        public void SelectSingleNode_Mixed_SelectNestedChild()
        {
            var list = new NodeList();
            var childMap = list.CreateMap();
            var child1 = childMap.CreateValue("value_01");
            var child2 = childMap.CreateValue("value_02");

            Assert.That(list.SelectNode<NodeValue>("0/value_01"), Is.EqualTo(child1));
            Assert.That(list.SelectNode<NodeValue>("0/value_02"), Is.EqualTo(child2));
        }

        #endregion

        #region SelectAny

        [Test]
        public void SelectMultipleNode_Map_SelectAny()
        {
            var map = new NodeMap();
            var child1 = map.CreateValue("value_01");
            var child2 = map.CreateValue("value_02");

            CollectionAssert.AreEquivalent(new []{child1, child2}, map.SelectNodes<NodeValue>("*"));
        }

        [Test]
        public void SelectMultipleNode_List_SelectAny()
        {
            var list = new NodeList();
            var child1 = list.CreateValue();
            var child2 = list.CreateValue();

            CollectionAssert.AreEquivalent(new []{child1, child2}, list.SelectNodes<NodeValue>("*"));
        }

        [Test]
        public void SelectMultipleNode_List_SelectRange()
        {
            var list = new NodeList();

            for (var i = 0; i < 10; i++)
            {
                list.CreateValue();
            }

            var children = list.Children<NodeValue>().ToList();
            
            CollectionAssert.AreEquivalent(children.Skip(5), list.SelectNodes<NodeValue>("5.."));
            CollectionAssert.AreEquivalent(children.Take(6), list.SelectNodes<NodeValue>("..5"));
            CollectionAssert.AreEquivalent(children.Skip(5), list.SelectNodes<NodeValue>("-5.."));
            CollectionAssert.AreEquivalent(children.Take(6), list.SelectNodes<NodeValue>("..-5"));
            CollectionAssert.AreEquivalent(children.Skip(3).Take(3), list.SelectNodes<NodeValue>("3..5"));
            CollectionAssert.AreEquivalent(children.Skip(5).Take(3), list.SelectNodes<NodeValue>("-5..-3"));
        }

        [Test]
        public void SelectMultipleNode_Value_SelectAny()
        {
            var map = new NodeValue(null);
            Assert.That(map.SelectNodes<NodeValue>("*"), Is.Empty);
        }
        
        #endregion
        #region SelectDescendants

        [Test]
        public void SelectMultipleNode_Map_SelectDescendants()
        {
            var map = new NodeMap();
            var child1 = map.CreateList("value_01");
            var nestedChild1 = child1.CreateValue();

            var child2 = map.CreateList("value_02");
            var nestedChild2 = child2.CreateValue();

            CollectionAssert.AreEquivalent(new Node[]{child1, child2, nestedChild1, nestedChild2}, map.SelectNodes("**"));
        }

        [Test]
        public void SelectMultipleNode_List_SelectDescendants()
        {
            var list = new NodeList();
            var child1 = list.CreateList();
            var nestedChild1 = child1.CreateValue();

            var child2 = list.CreateList();
            var nestedChild2 = child2.CreateValue();

            CollectionAssert.AreEquivalent(new Node[]{child1, child2, nestedChild1, nestedChild2 }, list.SelectNodes("**"));
        }

        [Test]
        public void SelectMultipleNode_Value_SelectDescendants()
        {
            var map = new NodeValue(null);
            Assert.That(map.SelectNodes<NodeValue>("**"), Is.Empty);
        }

        [Test]
        public void SelectMultipleNode_Map_SelectDescendantsFiltered()
        {
            var map = new NodeMap();
            var child1 = map.CreateList("value_01");
            var nestedChild1 = child1.CreateValue();

            var child2 = map.CreateList("value_02");
            var nestedChild2 = child2.CreateValue();

            CollectionAssert.AreEquivalent(new []{nestedChild1, nestedChild2}, map.SelectNodes<NodeValue>("**"));
        }

        [Test]
        public void SelectMultipleNode_List_SelectDescendantsFiltered()
        {
            var list = new NodeList();
            var child1 = list.CreateList();
            var nestedChild1 = child1.CreateValue();

            var child2 = list.CreateList();
            var nestedChild2 = child2.CreateValue();

            CollectionAssert.AreEquivalent(new []{ nestedChild1, nestedChild2 }, list.SelectNodes<NodeValue>("**"));
        }

        #endregion
    }
}