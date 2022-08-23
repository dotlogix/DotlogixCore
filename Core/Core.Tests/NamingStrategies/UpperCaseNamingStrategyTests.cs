using DotLogix.Core.Utils.Naming;
using NUnit.Framework;

namespace Core.Tests.NamingStrategies; 

[TestFixture]
public class UpperCaseNamingStrategyTests {

    private static readonly string[][] TestCases = {
        new[]{ "testname", "TESTNAME" },
        new[]{ "TestName", "TESTNAME" },
        new[]{ "testName", "TESTNAME" },
        new[]{ "TESTName", "TESTNAME" },
        new[]{ "testNAME", "TESTNAME" },
        new[]{ "test_name","TESTNAME" },
        new[]{ "Test_Name","TESTNAME" },
        new[]{ "test_Name","TESTNAME" },
        new[]{ "TEST_Name","TESTNAME" },
        new[]{ "test_NAME","TESTNAME" },
        new[]{ "__testname", "TESTNAME" },
        new[]{ "__TestName", "TESTNAME" },
        new[]{ "__testName", "TESTNAME" },
        new[]{ "__TESTName", "TESTNAME" },
        new[]{ "__testNAME", "TESTNAME" },
        new[]{ "test0name","TEST0NAME" },
        new[]{ "Test0Name","TEST0NAME" },
        new[]{ "test0Name","TEST0NAME" },
        new[]{ "TEST0Name","TEST0NAME" },
        new[]{ "test0NAME","TEST0NAME" },
        new[]{ "test_0name", "TEST0NAME" },
        new[]{ "Test_0Name", "TEST0NAME" },
        new[]{ "test_0Name", "TEST0NAME" },
        new[]{ "TEST_0Name", "TEST0NAME" },
        new[]{ "test_0NAME", "TEST0NAME" },
        new[]{ "test0_name", "TEST0NAME" },
        new[]{ "Test0_Name", "TEST0NAME" },
        new[]{ "test0_Name", "TEST0NAME" },
        new[]{ "TEST0_Name", "TEST0NAME" },
        new[]{ "test0_NAME", "TEST0NAME" },
        new[]{ "test001", "TEST001" },
        new[]{ "Test001", "TEST001" },
        new[]{ "TEST001", "TEST001" },
        new[]{ "001test", "001TEST" },
        new[]{ "001Test", "001TEST" },
        new[]{ "001TEST", "001TEST" },
    };

    private static readonly INamingStrategy Strategy = DotLogix.Core.Utils.Naming.NamingStrategies.UpperCase;

    [TestCaseSource(nameof(TestCases))]
    public void Rewrite_ReturnsExpected(string value, string expected) {
        Assert.AreEqual(expected, Strategy.Rewrite(value));
    }
}