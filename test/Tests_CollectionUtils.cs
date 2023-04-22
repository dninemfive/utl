using d9.utl;
namespace d9.utl.unit_tests
{
    [TestClass]
    public class Tests_CollectionUtils
    {
        [TestMethod]
        public void Test_BreakInto()
        {
            List<int> list = new() { 1, 45, 92, 69, 420, 957891, 589 };
            IEnumerable<IEnumerable<int>> broken = list.BreakInto(3);
            Assert.AreEqual(3, broken.Count());
            int n(int index) => broken.ElementAt(index).Count();
            Assert.AreEqual(3, n(0));
            Assert.AreEqual(2, n(1));
            Assert.AreEqual(2, n(2));
        }
    }
}