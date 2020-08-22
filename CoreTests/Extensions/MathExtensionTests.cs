using DotLogix.Core.Extensions;
using NUnit.Framework;

namespace CoreTests {
    [TestFixture]
    public class MathExtensionTests {
        [TestCase(0, 1)]
        [TestCase(1, 1)]
        [TestCase(1337, 4)]
        [TestCase(-1337, 5)]
        [TestCase(100000, 6)]
        [TestCase(-100000, 7)]
        [TestCase(int.MaxValue, 10)]
        public void DigitCount(int value, int expectedCount) {
            Assert.That(value.DigitCount(), Is.EqualTo(expectedCount));
        }
        
        [TestCase(0u, 1)]
        [TestCase(1u, 1)]
        [TestCase(1337u, 4)]
        [TestCase(100000u, 6)]
        [TestCase(uint.MaxValue, 10)]
        public void DigitCountUnsigned(uint value, int expectedCount) {
            Assert.That(value.DigitCount(), Is.EqualTo(expectedCount));
        }
        
        [TestCase(0l, 1)]
        [TestCase(1l, 1)]
        [TestCase(1337l, 4)]
        [TestCase(-1337l, 5)]
        [TestCase(100000l, 6)]
        [TestCase(-100000l, 7)]
        [TestCase(int.MaxValue, 10)]
        public void DigitCountLong(long value, int expectedCount) {
            Assert.That(value.DigitCount(), Is.EqualTo(expectedCount));
        }
        
        [TestCase(0ul, 1)]
        [TestCase(1ul, 1)]
        [TestCase(1337ul, 4)]
        [TestCase(100000ul, 6)]
        [TestCase(uint.MaxValue, 10)]
        public void DigitCountUnsignedLong(ulong value, int expectedCount) {
            Assert.That(value.DigitCount(), Is.EqualTo(expectedCount));
        }
    }
}
