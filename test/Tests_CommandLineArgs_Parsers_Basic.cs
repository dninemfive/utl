using d9.utl;
namespace d9.utl.unit_tests;

[TestClass]
public class Tests_CommandLineArgs_Parsers_Basic
{
    [TestMethod]
    public void Test_FirstNonEmptyString()
    {
        CommandLineArgs.Parser<string?> parser = CommandLineArgs.Parsers.FirstNonEmptyString;
        Assert.AreEqual("test1", parser(new string[] { "test1" }, false));
        Assert.AreEqual("test2", parser(new string[] { "", "test2", "wronganswer" }, false));
        Assert.AreEqual(null, parser(new string[] { "", "" }, false));
        Assert.AreEqual(null, parser(null, false));
    }
    [TestMethod]
    public void Test_Raw()
    {
        List<List<string>?> testLists = new()
        {
            new() { "test", "test2" },
            new() { },
            null
        };
        CommandLineArgs.Parser<IEnumerable<string>?> parser = CommandLineArgs.Parsers.Raw;
        foreach(List<string>? testList in testLists)
        {
            Assert.AreEqual(testList, parser(testList, false));
        }
    }
    [TestMethod]
    public void Test_Flag()
    {
        CommandLineArgs.Parser<bool> parser = CommandLineArgs.Parsers.Flag;
        Assert.AreEqual(true, parser(null, true));
        Assert.AreEqual(true, parser(new List<string> { }, false));
        Assert.AreEqual(true, parser(new List<string> { }, true));
        Assert.AreEqual(false, parser(null, false));
    }
}