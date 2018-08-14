// ==================================================
// Copyright 2018(C) , DotLogix
// File:  NodesTest.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  16.07.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Linq;
using DotLogix.Core.Nodes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endregion

namespace Test {
    [TestClass]
    public class NodesTest {
        [TestMethod]
        public void ValueNodeTest() {
            var nodeValue = new NodeValue("1");

            Assert.AreEqual("1", nodeValue.Value);
            Assert.AreEqual("1", nodeValue.GetValue<string>());
            Assert.AreEqual(1, nodeValue.GetValue<int>());
        }

        [TestMethod]
        public void MapNodeTest() {
            var nodeMap = new NodeMap();
            var valueNode = nodeMap.CreateValue("TestValueNode", "1");

            var receivedNode = nodeMap.GetChild<NodeValue>("TestValueNode");
            Assert.AreEqual(valueNode, receivedNode);
            Assert.AreEqual("1", receivedNode.GetValue<string>());
            Assert.AreEqual(1, receivedNode.GetValue<int>());
        }

        [TestMethod]
        public void ListNodeTest() {
            var nodeList = new NodeList();
            var valueNode = nodeList.CreateValue("1");

            var receivedNode = nodeList.GetChild<NodeValue>(0);

            Assert.AreEqual(valueNode, receivedNode);
            Assert.AreEqual("1", receivedNode.GetValue<string>());
            Assert.AreEqual(1, receivedNode.GetValue<int>());
        }

        [TestMethod]
        public void MapNodeNavigationTest() {
            var nodeMap = new NodeMap();
            var valueNode1 = nodeMap.CreateValue("TestValueNode1");
            var valueNode2 = nodeMap.CreateValue("TestValueNode2");
            var valueNode3 = nodeMap.CreateValue("TestValueNode3");

            Assert.AreEqual(valueNode1.Previous, null);
            Assert.AreEqual(valueNode1.Next, valueNode2);

            Assert.AreEqual(valueNode2.Previous, valueNode1);
            Assert.AreEqual(valueNode2.Next, valueNode3);

            Assert.AreEqual(valueNode3.Previous, valueNode2);
            Assert.AreEqual(valueNode3.Next, null);

            nodeMap.RemoveChild("TestValueNode3");
            Assert.AreEqual(null, valueNode2.Next);
            Assert.AreEqual(null, valueNode3.Ancestor);
            valueNode3 = nodeMap.CreateValue("TestValueNode3");

            Assert.AreEqual(valueNode2.Next, valueNode3);

            nodeMap.RemoveChild(valueNode2);
            Assert.AreEqual(valueNode1.Next, valueNode3);
            Assert.AreEqual(valueNode3.Previous, valueNode1);

            var valueNode3Replace = new NodeValue(10);
            nodeMap.ReplaceChild("TestValueNode3", valueNode3Replace);

            Assert.AreEqual(valueNode3Replace, valueNode1.Next);
            Assert.AreEqual(valueNode1, valueNode3Replace.Previous);
            Assert.AreEqual(null, valueNode3.Previous);
            Assert.AreEqual(null, valueNode3.Ancestor);
        }

        [TestMethod]
        public void ListNodeNavigationTest() {
            var nodeList = new NodeList();
            var valueNode1 = nodeList.CreateValue();
            var valueNode2 = nodeList.CreateValue();
            var valueNode3 = nodeList.CreateValue();

            Assert.AreEqual(null, valueNode1.Previous);
            Assert.AreEqual(valueNode2, valueNode1.Next);

            Assert.AreEqual(valueNode1, valueNode2.Previous);
            Assert.AreEqual(valueNode3, valueNode2.Next);

            Assert.AreEqual(valueNode2, valueNode3.Previous);
            Assert.AreEqual(null, valueNode3.Next);

            nodeList.RemoveChild(2);
            Assert.AreEqual(null, valueNode2.Next);
            Assert.AreEqual(null, valueNode3.Ancestor);
            valueNode3 = nodeList.CreateValue();

            Assert.AreEqual(valueNode3, valueNode2.Next);

            nodeList.RemoveChild(valueNode2);
            Assert.AreEqual(valueNode1.Next, valueNode3);
            Assert.AreEqual(valueNode3.Previous, valueNode1);

            var valueNode3Replace = new NodeValue(10);
            nodeList.ReplaceChild(1, valueNode3Replace);

            Assert.AreEqual(valueNode3Replace, valueNode1.Next);
            Assert.AreEqual(valueNode1, valueNode3Replace.Previous);
            Assert.AreEqual(null, valueNode3.Previous);
            Assert.AreEqual(null, valueNode3.Ancestor);


            nodeList.InsertChild(1, valueNode2);
            Assert.AreEqual(null, valueNode1.Previous);
            Assert.AreEqual(valueNode2, valueNode1.Next);

            Assert.AreEqual(valueNode1, valueNode2.Previous);
            Assert.AreEqual(valueNode3Replace, valueNode2.Next);

            Assert.AreEqual(valueNode2, valueNode3Replace.Previous);
            Assert.AreEqual(null, valueNode3Replace.Next);
        }

        [TestMethod]
        public void NodeSelectTest() {
            var node1 = new NodeMap();
            var node11 = node1.CreateMap("11");
            var node12 = node1.CreateList("12");

            var node111 = node11.CreateValue("111");
            var node112 = node11.CreateValue("112");

            var node121 = node12.CreateValue();
            var node122 = node12.CreateValue();

            Assert.AreEqual(node11, node1.SelectNode<NodeMap>("11"));
            Assert.AreEqual(node12, node1.SelectNode<NodeList>("12"));

            Assert.AreEqual(node111, node1.SelectNode<NodeValue>("11/111"));
            Assert.AreEqual(node112, node1.SelectNode<NodeValue>("11/112"));

            Assert.AreEqual(node121, node1.SelectNode<NodeValue>("12/0"));
            Assert.AreEqual(node122, node1.SelectNode<NodeValue>("12/1"));

            Assert.AreEqual(node122, node1.SelectNode<NodeValue>("12/-1"));

            var nodes = node1.SelectNodes<NodeValue>("12/*").ToList();
            Assert.AreEqual(2, nodes.Count);
            Assert.AreEqual(node121, nodes[0]);
            Assert.AreEqual(node122, nodes[1]);

            nodes = node1.SelectNodes<NodeValue>("12/0..1").ToList();
            Assert.AreEqual(2, nodes.Count);
            Assert.AreEqual(node121, nodes[0]);
            Assert.AreEqual(node122, nodes[1]);

            nodes = node1.SelectNodes<NodeValue>("12/..1").ToList();
            Assert.AreEqual(2, nodes.Count);
            Assert.AreEqual(node121, nodes[0]);
            Assert.AreEqual(node122, nodes[1]);

            Assert.AreEqual(node1, node11.SelectNode<NodeMap>("~"));
            Assert.AreEqual(node12, node11.SelectNode<NodeList>("~/12"));

            Assert.AreEqual(node121.Path, "~\\12\\0");
        }

        [TestMethod]
        public void ObjectToNodeTest() {
            var testClassInstance = new TestClass {
                                                      Property1 = null,
                                                      Property2 = 1,
                                                      Property3 = new TestNestedClass {
                                                                                          Property1 = "TestString2",
                                                                                          Property2 = 2,
                                                                                          Property3 = new[] {1, 2, 3}
                                                                                      }
                                                  };

            var node = Nodes.ToNode(testClassInstance);

            Assert.IsTrue(node is NodeMap);

            var nodeMap = node as NodeMap;

            var property1 = nodeMap.GetChild<NodeValue>("Property1");
            var property2 = nodeMap.GetChild<NodeValue>("Property2");
            var property3 = nodeMap.GetChild<NodeMap>("Property3");
            Assert.IsNotNull(property1);
            Assert.AreEqual(NodeTypes.Empty, property1.Type);
            Assert.IsNotNull(property2);
            Assert.IsNotNull(property3);

            var property31 = property3.GetChild<NodeValue>("Property1");
            var property32 = property3.GetChild<NodeValue>("Property2");
            var property33 = property3.GetChild<NodeList>("Property3");
            Assert.IsNotNull(property31);
            Assert.IsNotNull(property32);
            Assert.IsNotNull(property33);

            //Assert.AreEqual(null, property1.Value);
            Assert.AreEqual(1, property2.Value);
            Assert.AreEqual("TestString2", property31.Value);
            Assert.AreEqual(2, property32.Value);
            Assert.AreEqual(1, property33.GetChild<NodeValue>(0).Value);
            Assert.AreEqual(2, property33.GetChild<NodeValue>(1).Value);
            Assert.AreEqual(3, property33.GetChild<NodeValue>(2).Value);
        }

        public class TestClass {
            public string Property1 { get; set; }
            public int Property2 { get; set; }
            public TestNestedClass Property3 { get; set; }
        }

        public class TestNestedClass {
            public string Property1 { get; set; }
            public int Property2 { get; set; }
            public int[] Property3 { get; set; }
        }
    }
}
