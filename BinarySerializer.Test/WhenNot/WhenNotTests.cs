using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinarySerialization.Test.WhenNot
{
    [TestClass()]
    public class WhenNotTests : TestBase
    {
        [TestMethod()]
        public void SimpleTest()
        {
            WhenNotClass expected = new()
            {
                ExcludeValue = true,
                Value = 100
            };

            byte[] data = Serialize(expected);
            Assert.AreEqual(1, data.Length);

            expected.ExcludeValue = false;
            data = Serialize(expected);
            Assert.AreEqual(5, data.Length);
        }
    }
}
