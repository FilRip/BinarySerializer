using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinarySerialization.Test.Misc
{
    [TestClass()]
    public class SkipableTests : TestBase
    {
        [TestMethod()]
        public void SkipTest()
        {
            var actual = Deserialize<SkipableContainerClass>([]);
            Assert.IsNull(actual.Skipable);
        }
    }
}
