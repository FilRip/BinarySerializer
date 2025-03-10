using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinarySerialization.Test.BitOrderTest
{
    [TestClass()]
    public class BitOrderTests : TestBase
    {
        [TestMethod()]
        public void Test()
        {
            BitOrderClass expected = new()
            {
                Value1 = 0x7,
                Value2 = 0x2,
            };

            BitOrderClass actual = Roundtrip(expected, [0x72]);
            Assert.AreEqual(expected.Value1, actual.Value1);
            Assert.AreEqual(expected.Value2, actual.Value2);
        }

        [TestMethod()]
        public void TestForward()
        {
            CipMessageRouterDataForward expected = new()
            {
                Service = CipServiceCodes.GetAttributeSingle,
                Response = false,
            };

            CipMessageRouterDataForward actual = Roundtrip(expected, [0x0E]);
            Assert.AreEqual(expected.Service, actual.Service);
            Assert.AreEqual(expected.Response, actual.Response);
        }

        [TestMethod()]
        public void TestBackward()
        {
            CipMessageRouterDataBackward expected = new()
            {
                Service = CipServiceCodes.GetAttributeSingle,
                Response = false,
            };

            CipMessageRouterDataBackward actual = Roundtrip(expected, [0x0E]);
            Assert.AreEqual(expected.Service, actual.Service);
            Assert.AreEqual(expected.Response, actual.Response);
        }
    }
}
