using DotLogix.Core.Nodes;
using NUnit.Framework;

namespace NodeTests
{
    [TestFixture]
    public class NodeValueTest : NodeValueTestBase<NodeValue>
    {
        [Test]
        public void Node_HasTypeEmpty() {
            var node = CreateNode();
            Assert.That(node.Type, Is.EqualTo(NodeTypes.Empty));
        }

        [Test]
        public void Node_WithValue_HasTypeValue() {
            var node = CreateNode(5);
            Assert.That(node.Type, Is.EqualTo(NodeTypes.Value));
        }

        [TestCase("value")]
        [TestCase(5)]
        [TestCase(true)]
        public void Node_WithValue_HasValue(object value) {
            var node = CreateNode(value);
            Assert.That(node.Value, Is.EqualTo(value));
        }

        [TestCase(5, 5l)]
        [TestCase("5", 5)]
        [TestCase(5, "5")]
        [TestCase("value", "value")]
        [TestCase("true", true)]
        public void Node_ConvertValue_ReturnsExpected(object nodeValue, object expectedValue) {
            var node = CreateNode(nodeValue);
            Assert.That(node.ConvertValue(expectedValue.GetType()), Is.EqualTo(expectedValue));
        }
        
        [TestCase(5, 5l)]
        [TestCase("5", 5)]
        [TestCase(5, "5")]
        [TestCase("value", "value")]
        [TestCase("true", true)]
        public void Node_TryConvertValue_ReturnsExpected(object nodeValue, object expectedValue) {
            var node = CreateNode(nodeValue);
            Assert.That(node.TryConvertValue(expectedValue.GetType(), out var converted), Is.True);
            Assert.That(converted, Is.EqualTo(expectedValue));
        }

        [TestCase(int.MaxValue, (byte)2)]
        [TestCase("5", true)]
        public void Node_TryConvertInvalidValue_ReturnsFalse(object nodeValue, object expectedValue) {
            var node = CreateNode(nodeValue);
            Assert.That(node.TryConvertValue(expectedValue.GetType(), out _), Is.False);
        }
        

        protected override NodeValue CreateNode()
        {
            return new NodeValue(null);
        }

        protected NodeValue CreateNode(object value)
        {
            return new NodeValue(value);
        }
    }
}