using DotLogix.Core.Utils.Naming;
using NUnit.Framework;

namespace Core.Tests.NamingStrategies {
    [TestFixture]
    public class KebapCaseNamingStrategyTests {

        private static readonly string[][] TestCases = {
            new[]{ "testname", "testname" },
            new[]{ "TestName", "test-name" },
            new[]{ "testName", "test-name" },
            new[]{ "TESTName", "test-name" },
            new[]{ "testNAME", "test-name" },
            new[]{ "test_name", "test-name" },
            new[]{ "Test_Name", "test-name" },
            new[]{ "test_Name", "test-name" },
            new[]{ "TEST_Name", "test-name" },
            new[]{ "test_NAME", "test-name" },
            new[]{ "__testname", "testname" },
            new[]{ "__TestName", "test-name" },
            new[]{ "__testName", "test-name" },
            new[]{ "__TESTName", "test-name" },
            new[]{ "__testNAME", "test-name" },
            new[]{ "test0name", "test-0-name" },
            new[]{ "Test0Name", "test-0-name" },
            new[]{ "test0Name", "test-0-name" },
            new[]{ "TEST0Name", "test-0-name" },
            new[]{ "test0NAME", "test-0-name" },
            new[]{ "test_0name", "test-0-name" },
            new[]{ "Test_0Name", "test-0-name" },
            new[]{ "test_0Name", "test-0-name" },
            new[]{ "TEST_0Name", "test-0-name" },
            new[]{ "test_0NAME", "test-0-name" },
            new[]{ "test0_name", "test-0-name" },
            new[]{ "Test0_Name", "test-0-name" },
            new[]{ "test0_Name", "test-0-name" },
            new[]{ "TEST0_Name", "test-0-name" },
            new[]{ "test0_NAME", "test-0-name" },
            new[]{ "test001", "test-001" },
            new[]{ "Test001", "test-001" },
            new[]{ "TEST001", "test-001" },
            new[]{ "001test", "001-test" },
            new[]{ "001Test", "001-test" },
            new[]{ "001TEST", "001-test" },
        };

        private static readonly INamingStrategy Strategy = DotLogix.Core.Utils.Naming.NamingStrategies.KebapCase;

        [TestCaseSource(nameof(TestCases))]
        public void Rewrite_ReturnsExpected(string value, string expected) {
            Assert.AreEqual(expected, Strategy.Rewrite(value));
        }

    }
}