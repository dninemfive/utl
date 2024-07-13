using d9.utl;
namespace d9.utl.unit_tests;

[TestClass]
public class Tests_CommandLineArgs_Parsers_Generic
{
    [TestMethod]
    public void Test_Parsables()
    {
        CommandLineArgs.Parser<IEnumerable<double?>> parser = CommandLineArgs.Parsers.Parsables<double?>();
    }
}