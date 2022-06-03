using DotLogix.Core.Utils.Naming;
using NUnit.Framework;

namespace CoreTests.NamingStrategies {
    [TestFixture]
    public class UpperSnakeCaseNamingStrategyTests {

        private static readonly string[][] TestCases = {
            new[]{ "testname", "TESTNAME" },
            new[]{ "TestName", "TEST_NAME" },
            new[]{ "testName", "TEST_NAME" },
            new[]{ "TESTName", "TEST_NAME" },
            new[]{ "testNAME", "TEST_NAME" },
            new[]{ "test_name","TEST_NAME" },
            new[]{ "Test_Name","TEST_NAME" },
            new[]{ "test_Name","TEST_NAME" },
            new[]{ "TEST_Name","TEST_NAME" },
            new[]{ "test_NAME","TEST_NAME" },
            new[]{ "__testname", "TESTNAME" },
            new[]{ "__TestName", "TEST_NAME" },
            new[]{ "__testName", "TEST_NAME" },
            new[]{ "__TESTName", "TEST_NAME" },
            new[]{ "__testNAME", "TEST_NAME" },
            new[]{ "test0name","TEST_0_NAME" },
            new[]{ "Test0Name","TEST_0_NAME" },
            new[]{ "test0Name","TEST_0_NAME" },
            new[]{ "TEST0Name","TEST_0_NAME" },
            new[]{ "test0NAME","TEST_0_NAME" },
            new[]{ "test_0name", "TEST_0_NAME" },
            new[]{ "Test_0Name", "TEST_0_NAME" },
            new[]{ "test_0Name", "TEST_0_NAME" },
            new[]{ "TEST_0Name", "TEST_0_NAME" },
            new[]{ "test_0NAME", "TEST_0_NAME" },
            new[]{ "test0_name", "TEST_0_NAME" },
            new[]{ "Test0_Name", "TEST_0_NAME" },
            new[]{ "test0_Name", "TEST_0_NAME" },
            new[]{ "TEST0_Name", "TEST_0_NAME" },
            new[]{ "test0_NAME", "TEST_0_NAME" },
            new[]{ "test001", "TEST_001" },
            new[]{ "Test001", "TEST_001" },
            new[]{ "TEST001", "TEST_001" },
            new[]{ "001test", "001_TEST" },
            new[]{ "001Test", "001_TEST" },
            new[]{ "001TEST", "001_TEST" },
        };

        private static readonly INamingStrategy Strategy = DotLogix.Core.Utils.Naming.NamingStrategies.UpperSnakeCase;

        [TestCaseSource(nameof(TestCases))]
        public void Rewrite_ReturnsExpected(string value, string expected) {
            Assert.AreEqual(expected, Strategy.Rewrite(value));
        }




































    }
}
