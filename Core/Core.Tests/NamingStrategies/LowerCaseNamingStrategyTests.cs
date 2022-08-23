using DotLogix.Core.Utils.Naming;
using NUnit.Framework;

namespace Core.Tests.NamingStrategies {
    [TestFixture]
    public class LowerCaseNamingStrategyTests {

        private static readonly string[][] TestCases = {
            new[]{ "testname", "testname" },
            new[]{ "TestName", "testname" },
            new[]{ "testName", "testname" },
            new[]{ "TESTName", "testname" },
            new[]{ "testNAME", "testname" },
            new[]{ "test_name", "testname" },
            new[]{ "Test_Name", "testname" },
            new[]{ "test_Name", "testname" },
            new[]{ "TEST_Name", "testname" },
            new[]{ "test_NAME", "testname" },
            new[]{ "__testname", "testname" },
            new[]{ "__TestName", "testname" },
            new[]{ "__testName", "testname" },
            new[]{ "__TESTName", "testname" },
            new[]{ "__testNAME", "testname" },
            new[]{ "test0name", "test0name" },
            new[]{ "Test0Name", "test0name" },
            new[]{ "test0Name", "test0name" },
            new[]{ "TEST0Name", "test0name" },
            new[]{ "test0NAME", "test0name" },
            new[]{ "test_0name", "test0name" },
            new[]{ "Test_0Name", "test0name" },
            new[]{ "test_0Name", "test0name" },
            new[]{ "TEST_0Name", "test0name" },
            new[]{ "test_0NAME", "test0name" },
            new[]{ "test0_name", "test0name" },
            new[]{ "Test0_Name", "test0name" },
            new[]{ "test0_Name", "test0name" },
            new[]{ "TEST0_Name", "test0name" },
            new[]{ "test0_NAME", "test0name" },
            new[]{ "test001", "test001" },
            new[]{ "Test001", "test001" },
            new[]{ "TEST001", "test001" },
            new[]{ "001test", "001test" },
            new[]{ "001Test", "001test" },
            new[]{ "001TEST", "001test" },
        };

        private static readonly INamingStrategy Strategy = DotLogix.Core.Utils.Naming.NamingStrategies.LowerCase;

        [TestCaseSource(nameof(TestCases))]
        public void Rewrite_ReturnsExpected(string value, string expected) {
            Assert.AreEqual(expected, Strategy.Rewrite(value));
        }

    }
}