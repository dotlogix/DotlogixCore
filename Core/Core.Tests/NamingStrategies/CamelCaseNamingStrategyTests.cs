using DotLogix.Core.Utils.Naming;
using NUnit.Framework;

namespace Core.Tests.NamingStrategies; 

[TestFixture]
public class CamelCaseNamingStrategyTests {

    private static readonly string[][] TestCases = {
        new[]{ "testname", "testname" },
        new[]{ "TestName", "testName" },
        new[]{ "testName", "testName" },
        new[]{ "TESTName", "testName" },
        new[]{ "testNAME", "testName" },
        new[]{ "test_name", "testName" },
        new[]{ "Test_Name", "testName" },
        new[]{ "test_Name", "testName" },
        new[]{ "TEST_Name", "testName" },
        new[]{ "test_NAME", "testName" },
        new[]{ "__testname", "testname" },
        new[]{ "__TestName", "testName" },
        new[]{ "__testName", "testName" },
        new[]{ "__TESTName", "testName" },
        new[]{ "__testNAME", "testName" },
        new[]{ "test0name", "test0Name" },
        new[]{ "Test0Name", "test0Name" },
        new[]{ "test0Name", "test0Name" },
        new[]{ "TEST0Name", "test0Name" },
        new[]{ "test0NAME", "test0Name" },
        new[]{ "test_0name", "test0Name" },
        new[]{ "Test_0Name", "test0Name" },
        new[]{ "test_0Name", "test0Name" },
        new[]{ "TEST_0Name", "test0Name" },
        new[]{ "test_0NAME", "test0Name" },
        new[]{ "test0_name", "test0Name" },
        new[]{ "Test0_Name", "test0Name" },
        new[]{ "test0_Name", "test0Name" },
        new[]{ "TEST0_Name", "test0Name" },
        new[]{ "test0_NAME", "test0Name" },
        new[]{ "test001", "test001" },
        new[]{ "Test001", "test001" },
        new[]{ "TEST001", "test001" },
        new[]{ "001test", "001Test" },
        new[]{ "001Test", "001Test" },
        new[]{ "001TEST", "001Test" },
    };

    private static readonly INamingStrategy Strategy = DotLogix.Core.Utils.Naming.NamingStrategies.CamelCase;

    [TestCaseSource(nameof(TestCases))]
    public void Rewrite_ReturnsExpected(string value, string expected) {
        Assert.AreEqual(expected, Strategy.Rewrite(value));
    }

}