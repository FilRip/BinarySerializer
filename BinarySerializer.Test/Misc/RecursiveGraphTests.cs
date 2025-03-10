using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinarySerialization.Test.Misc
{
    [TestClass()]
    public class RecursiveGraphTests : TestBase
    {
        [TestMethod()]
#pragma warning disable S2699 // Tests should include assertions
        public void RecursiveGraphTest()
#pragma warning restore S2699 // Tests should include assertions
        {
            /*var expected = new RecursiveGraphClass();
            _ = Roundtrip(expected);*/
        }
    }
}