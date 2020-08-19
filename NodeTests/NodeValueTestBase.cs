using DotLogix.Core.Nodes;
using NUnit.Framework;

namespace NodeTests {
    public abstract class NodeValueTestBase<T> where T:Node
    {
        [Test]
        public void Node_HasNoAncestor() {
            var node = CreateNode();

            Assert.That(node.HasAncestor, Is.False);
            Assert.That(node.Ancestor, Is.Null);
        }

        [Test]
        public void Node_HasNoSiblings() {
            var node = CreateNode();

            Assert.That(node.HasNext, Is.False);
            Assert.That(node.Next, Is.Null);
            Assert.That(node.HasPrevious, Is.False);
            Assert.That(node.Previous, Is.Null);
        }

        [Test]
        public void Node_IsRoot() {
            var node = CreateNode();
            Assert.That(node.IsRoot, Is.True);
            Assert.That(node.Root, Is.EqualTo(node));
        }

        [Test]
        public void Node_HasIndex_Neg1() {
            var node = CreateNode();
            Assert.That(node.Index, Is.EqualTo(-1));
        }

        [Test]
        public void Node_HasName_Null() {
            var node = CreateNode();
            Assert.That(node.Name, Is.Null);
        }
        [Test]
        public void Node_HasPath_Root() {
            var node = CreateNode();
            Assert.That(node.Path, Is.EqualTo("~"));
        }

        protected abstract T CreateNode();
    }
}
