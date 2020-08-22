using System;
using DotLogix.Core.Nodes.Processor;
using NUnit.Framework;

namespace NodeTests.Utils {
    [TestFixture]
    public class JsonStringUtilsTest {

        [TestCase('0', true)]
        [TestCase('5', true)]
        [TestCase('a', true)]
        [TestCase('f', true)]
        [TestCase('A', true)]
        [TestCase('F', true)]
        [TestCase('y', false)]
        public void IsHex_ReturnsExpected(int value, bool expected)
        {
            Assert.That(JsonStrings.IsHex(value), Is.EqualTo(expected));
        }
        
        [TestCase('0', 0)]
        [TestCase('5', 5)]
        [TestCase('a', 10)]
        [TestCase('f', 15)]
        [TestCase('A', 10)]
        [TestCase('F', 15)]
        public void HexToInt_ReturnsExpected(char value, int expected)
        {
            Assert.That(JsonStrings.HexToInt(value), Is.EqualTo(expected));
        }

        [TestCase('x')]
        [TestCase('g')]
        public void HexToInt_Invalid_Throws(char value)
        {
            Assert.That(() => JsonStrings.HexToInt(value), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [TestCase(0, '0')]
        [TestCase(5, '5')]
        [TestCase(10, 'a')]
        [TestCase(15, 'f')]
        public void IntToHex_ReturnsExpected(int value, char expected)
        {
            Assert.That(JsonStrings.IntToHex(value), Is.EqualTo(expected));
        }

        [TestCase(-1)]
        [TestCase(16)]
        public void IntToHex_Invalid_Throws(int value)
        {
            Assert.That(() => JsonStrings.IntToHex(value), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [TestCase("0", 0L)]
        [TestCase("12345", 12345L)]
        [TestCase("-1234567", -1234567L)]
        public void ParseLong_Valid_ReturnsExpected(string value, long expected)
        {
            Assert.That(JsonStrings.ParseInt64(value), Is.EqualTo(expected));
        }


        [TestCase("qwerty")]
        [TestCase("01AF")]
        [TestCase("0.5")]
        [TestCase("0,5")]
        [TestCase("")]
        public void ParseLong_Invalid_Throws(string value)
        {
            Assert.That(() => JsonStrings.ParseInt64(value), Throws.Exception);
        }

        [TestCase(12345L, "12345")]
        [TestCase(-12345L, "-12345")]
        [TestCase(-1L, "-1")]
        [TestCase(0L, "0")]
        public void FormatLong_Valid_ReturnsExpected(long value, string expected)
        {
            Assert.That(JsonStrings.FormatLong(value), Is.EqualTo(expected));
        }

        [TestCase(0UL, "0")]
        [TestCase(12345UL, "12345")]
        public void FormatULong_Valid_ReturnsExpected(ulong value, string expected)
        {
            Assert.That(JsonStrings.FormatULong(value), Is.EqualTo(expected));
        }


        [TestCase("0", 0UL)]
        [TestCase("12345", 12345UL)]
        public void ParseULong_Valid_ReturnsExpected(string value, ulong expected)
        {
            Assert.That(JsonStrings.ParseUInt64(value), Is.EqualTo(expected));
        }


        [TestCase("qwerty")]
        [TestCase("01AF")]
        [TestCase("0.5")]
        [TestCase("0,5")]
        [TestCase("-1234567")]
        [TestCase("")]
        public void ParseULong_Invalid_Throws(string value)
        {
            Assert.That(() => JsonStrings.ParseUInt64(value), Throws.Exception);
        }

        [TestCase("0", 0d)]
        [TestCase("1", 1d)]
        [TestCase("-1", -1d)]
        [TestCase("12345", 12345d)]
        [TestCase("-1234567", -1234567d)]
        [TestCase("1E5", 1E5d)]
        [TestCase("-1E5", -1E5d)]
        [TestCase("0.1", 0.1d)]
        [TestCase("-0.1", -0.1d)]
        [TestCase("1.5e5", 1.5E5d)]
        [TestCase("1.5E5", 1.5E5d)]
        [TestCase("-1.5e5", -1.5E5d)]
        [TestCase("-1.5E5", -1.5E5d)]
        [TestCase("1.5E-5", 1.5E-5d)]
        [TestCase("-1.5E-5", -1.5E-5d)]
        [TestCase("-1.5E+5", -1.5E5d)]
        public void ParseDouble_Valid_ReturnsExpected(string value, double expected)
        {
            Assert.That(JsonStrings.ParseDouble(value), Is.EqualTo(expected).Within(0.1));
        }

        
        [TestCase("qwerty")]
        [TestCase("01AF")]
        [TestCase("0.5.1")]
        [TestCase("0,5")]
        [TestCase("35.3E-5.2")]
        [TestCase("")]
        public void ParseDouble_Invalid_Throws(string value) {
            Assert.That(() => JsonStrings.ParseDouble(value), Throws.Exception);
        }
    }
}
