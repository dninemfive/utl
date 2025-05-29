using d9.utl;
namespace d9.utl.tests;

[TestClass]
public class Tests_MathUtils
{
    [TestMethod]
    public void Test_Clamp()
    {
        Assert.AreEqual(2, 2.Clamp(0, 5));
        Assert.AreEqual(3, 2.Clamp(3, 5));
        Assert.AreEqual(1, 2.Clamp(0, 1));
    }
    [TestMethod]
    public void Test_Mean()
    {
        List<int> ints = new()
        {
            1, 3, 5
        };
        Assert.AreEqual(3, ints.Mean());
        Assert.AreEqual(3, MathUtils.Mean(2, 4));
        // todo: fix
        Assert.AreEqual(3.5, MathUtils.Mean(2, 5));
    }
    private static void AssertParity(bool odd, Func<int, bool> func)
    {
        Assert.AreEqual(odd, func(1));
        Assert.AreEqual(!odd, func(2));
        Assert.AreEqual(!odd, func(0));
    }
    [TestMethod]
    public void Test_IsOdd()
        => AssertParity(true, MathUtils.IsOdd);
    [TestMethod]
    public void Test_IsEven()
        => AssertParity(false, MathUtils.IsEven);
}