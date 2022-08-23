using DotLogix.Core.Utils.Naming;
using NUnit.Framework;

namespace Core.Tests.NamingStrategies {
    [TestFixture]
    public class SnakeCaseNamingStrategyTests {

        private static readonly string[][] TestCases = {
            new[]{ "testname", "testname" },
            new[]{ "TestName", "test_name" },
            new[]{ "testName", "test_name" },
            new[]{ "TESTName", "test_name" },
            new[]{ "testNAME", "test_name" },
            new[]{ "test_name", "test_name" },
            new[]{ "Test_Name", "test_name" },
            new[]{ "test_Name", "test_name" },
            new[]{ "TEST_Name", "test_name" },
            new[]{ "test_NAME", "test_name" },
            new[]{ "__testname", "testname" },
            new[]{ "__TestName", "test_name" },
            new[]{ "__testName", "test_name" },
            new[]{ "__TESTName", "test_name" },
            new[]{ "__testNAME", "test_name" },
            new[]{ "test0name", "test_0_name" },
            new[]{ "Test0Name", "test_0_name" },
            new[]{ "test0Name", "test_0_name" },
            new[]{ "TEST0Name", "test_0_name" },
            new[]{ "test0NAME", "test_0_name" },
            new[]{ "test_0name", "test_0_name" },
            new[]{ "Test_0Name", "test_0_name" },
            new[]{ "test_0Name", "test_0_name" },
            new[]{ "TEST_0Name", "test_0_name" },
            new[]{ "test_0NAME", "test_0_name" },
            new[]{ "test0_name", "test_0_name" },
            new[]{ "Test0_Name", "test_0_name" },
            new[]{ "test0_Name", "test_0_name" },
            new[]{ "TEST0_Name", "test_0_name" },
            new[]{ "test0_NAME", "test_0_name" },
            new[]{ "test001", "test_001" },
            new[]{ "Test001", "test_001" },
            new[]{ "TEST001", "test_001" },
            new[]{ "001test", "001_test" },
            new[]{ "001Test", "001_test" },
            new[]{ "001TEST", "001_test" },
        };

        private static readonly INamingStrategy Strategy = DotLogix.Core.Utils.Naming.NamingStrategies.SnakeCase;

        [TestCaseSource(nameof(TestCases))]
        public void Rewrite_ReturnsExpected(string value, string expected) {
            Assert.AreEqual(expected, Strategy.Rewrite(value));
        }

    }
}