using DotLogix.Core.Nodes;
using NUnit.Framework;

namespace NodeTests
{
    [TestFixture]
    public class NodeListTest : NodeValueTestBase<NodeList>
    {
        [Test]
        public void Node_HasTypeEmpty() {
            var root = CreateNode();
            Assert.That(root.Type, Is.EqualTo(NodeTypes.List));
        }

        [Test]
        public void Node_GetChild_NonExisting_ReturnsNull() {
            var root = CreateNode();
            var receivedNode = root.GetChild<NodeList>(0);
            Assert.That(receivedNode, Is.Null);
        }

        [Test]
        public void Node_GetChild_ExistingTypeMismatch_ReturnsNull() {
            var root = CreateNode();
            root.CreateValue("1");
            root.CreateList();
            root.CreateMap();

            var receivedNode1 = root.GetChild<NodeMap>(0);
            var receivedNode2 = root.GetChild<NodeValue>(1);
            var receivedNode3 = root.GetChild<NodeList>(2);

            Assert.That(receivedNode1, Is.Null);
            Assert.That(receivedNode2, Is.Null);
            Assert.That(receivedNode3, Is.Null);
        }

        [Test]
        public void Node_GetChild_Existing_ReturnsExpected() {
            var root = CreateNode();
            var child1 = root.CreateValue("1");
            var child2 = root.CreateList();
            var child3 = root.CreateMap();

            var receivedNode1 = root.GetChild(0);
            var receivedNode2 = root.GetChild(1);
            var receivedNode3 = root.GetChild(2);

            Assert.That(receivedNode1, Is.EqualTo(child1));
            Assert.That(receivedNode2, Is.EqualTo(child2));
            Assert.That(receivedNode3, Is.EqualTo(child3));
        }

        [Test]
        public void Node_CreateChild_ChildCount_EqualsExpected() {
            var root = CreateNode();

            Assert.That(root.ChildCount, Is.EqualTo(0));

            root.CreateValue("1");
            Assert.That(root.ChildCount, Is.EqualTo(1));

            root.CreateList();
            Assert.That(root.ChildCount, Is.EqualTo(2));

            root.CreateMap();
            Assert.That(root.ChildCount, Is.EqualTo(3));
        }

        [Test]
        public void Node_CreateChild_SiblingNavigation() {
            var root = CreateNode();
            var child1 = root.CreateValue("1");
            var child2 = root.CreateList();
            var child3 = root.CreateMap();

            Assert.That(child1.Previous, Is.Null);
            Assert.That(child1.Next, Is.EqualTo(child2));

            Assert.That(child2.Previous, Is.EqualTo(child1));
            Assert.That(child2.Next, Is.EqualTo(child3));

            Assert.That(child3.Previous, Is.EqualTo(child2));
            Assert.That(child3.Next, Is.Null);
        }

        [Test]
        public void Node_CreateChild_InSyncWithParent() {
            var root = CreateNode();
            var child1 = root.CreateValue("1");
            var child2 = root.CreateList();
            var child3 = root.CreateMap();

            Assert.That(child1.Index, Is.EqualTo(0));
            Assert.That(child2.Index, Is.EqualTo(1));
            Assert.That(child3.Index, Is.EqualTo(2));

            Assert.That(child1.Name, Is.Null);
            Assert.That(child2.Name, Is.Null);
            Assert.That(child3.Name, Is.Null);

            Assert.That(child1.Ancestor, Is.EqualTo(root));
            Assert.That(child2.Ancestor, Is.EqualTo(root));
            Assert.That(child3.Ancestor, Is.EqualTo(root));

            Assert.That(child1.Root, Is.EqualTo(root));
            Assert.That(child2.Root, Is.EqualTo(root));
            Assert.That(child3.Root, Is.EqualTo(root));
        }

        [Test]
        public void Node_RemoveChild_ChildCount_EqualsExpected() {
            var root = CreateNode();

            var child1 = root.CreateValue("1");
            var child2 = root.CreateList();
            var child3 = root.CreateMap();
            Assert.That(root.ChildCount, Is.EqualTo(3));

            root.RemoveChild(child1);
            Assert.That(root.ChildCount, Is.EqualTo(2));

            root.RemoveChild(child2);
            Assert.That(root.ChildCount, Is.EqualTo(1));

            root.RemoveChild(child3);
            Assert.That(root.ChildCount, Is.EqualTo(0));
        }

        [Test]
        public void Node_RemovedChild_SiblingNavigation() {
            var root = CreateNode();
            var child1 = root.CreateValue("1");
            var child2 = root.CreateList();
            var child3 = root.CreateMap();

            root.RemoveChild(child1);
            root.RemoveChild(child2);
            root.RemoveChild(child3);

            Assert.That(child1.Previous, Is.Null);
            Assert.That(child1.Next, Is.Null);

            Assert.That(child2.Previous, Is.Null);
            Assert.That(child2.Next, Is.Null);

            Assert.That(child3.Previous, Is.Null);
            Assert.That(child3.Next, Is.Null);
        }

        [Test]
        public void Node_RemovedChild_IsReset() {
            var root = CreateNode();
            var child1 = root.CreateValue("1");
            var child2 = root.CreateList();
            var child3 = root.CreateMap();

            root.RemoveChild(child1);
            root.RemoveChild(child2);
            root.RemoveChild(child3);

            Assert.That(child1.Name, Is.Null);
            Assert.That(child2.Name, Is.Null);
            Assert.That(child3.Name, Is.Null);

            Assert.That(child1.Ancestor, Is.Null);
            Assert.That(child2.Ancestor, Is.Null);
            Assert.That(child3.Ancestor, Is.Null);

            Assert.That(child1.Root, Is.EqualTo(child1));
            Assert.That(child2.Root, Is.EqualTo(child2));
            Assert.That(child3.Root, Is.EqualTo(child3));
        }


        protected override NodeList CreateNode()
        {
            return new NodeList();
        }
    }
}