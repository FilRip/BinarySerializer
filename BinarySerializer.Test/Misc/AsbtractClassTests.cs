
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinarySerialization.Test.Misc
{
    [TestClass()]
    public class AsbtractClassTests : TestBase
    {
        [TestMethod()]
        public void AbstractClassTest()
        {
            AbstractClassContainer container = new() { Content = new DerivedClass() };
            Roundtrip(container, 4);
        }
    }
}