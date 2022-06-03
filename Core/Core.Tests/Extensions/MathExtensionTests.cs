using DotLogix.Core.Extensions;
using NUnit.Framework;

namespace CoreTests.Extensions {
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
        
        [TestCase(0U, 1)]
        [TestCase(1U, 1)]
        [TestCase(1337U, 4)]
        [TestCase(100000U, 6)]
        [TestCase(uint.MaxValue, 10)]
        public void DigitCountUnsigned(uint value, int expectedCount) {
            Assert.That(value.DigitCount(), Is.EqualTo(expectedCount));
        }
        
        [TestCase(0L, 1)]
        [TestCase(1L, 1)]
        [TestCase(1337L, 4)]
        [TestCase(-1337L, 5)]
        [TestCase(100000L, 6)]
        [TestCase(-100000L, 7)]
        [TestCase(int.MaxValue, 10)]
        public void DigitCountLong(long value, int expectedCount) {
            Assert.That(value.DigitCount(), Is.EqualTo(expectedCount));
        }
        
        [TestCase(0UL, 1)]
        [TestCase(1UL, 1)]
        [TestCase(1337UL, 4)]
        [TestCase(100000UL, 6)]
        [TestCase(uint.MaxValue, 10)]
        public void DigitCountUnsignedLong(ulong value, int expectedCount) {
            Assert.That(value.DigitCount(), Is.EqualTo(expectedCount));
        }
    }
}
