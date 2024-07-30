using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace d9.utl.unit_tests;
[TestClass]
public class Tests_StringUtils
{
    [TestMethod]
    public void Test_PrettyPrint()
    {
        IntermediateArgs ia = new("-- asdf -f --arg1 69 --exampleList a b c d --arg2 42".Split(" "));
        Console.WriteLine(ia.PrettyPrint());
    }
}