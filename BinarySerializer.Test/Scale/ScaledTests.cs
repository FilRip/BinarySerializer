using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinarySerialization.Test.Scale
{
    [TestClass()]
    public class ScaledTests : TestBase
    {
        [TestMethod()]
        public void ScaleTest()
        {
            ScaledValueClass expected = new() { Value = 3 };
            ScaledValueClass actual = Roundtrip(expected, [0x6, 0, 0, 0]);
            Assert.AreEqual(expected.Value, actual.Value);
        }

        [TestMethod()]
        public void ScaleIntTest()
        {
            ScaledIntValueClass expected = new() { Value = 3 };
            ScaledIntValueClass actual = Roundtrip(expected, [0x6, 0, 0, 0]);
            Assert.AreEqual(expected.Value, actual.Value);
        }

        [TestMethod()]
        public void NegativeScaleTest()
        {
            ScaledValueClass expected = new() { Value = -3 };
            ScaledValueClass actual = Roundtrip(expected, [0xFA, 0xFF, 0xFF, 0xFF]);
            Assert.AreEqual(expected.Value, actual.Value);
        }

        [TestMethod()]
        public void BigEndianScaleTest()
        {
            ScaledValueClass expected = new() { Value = 3 };
            ScaledValueClass actual = RoundtripBigEndian(expected, [0, 0, 0, 6]);
            Assert.AreEqual(expected.Value, actual.Value);
        }
    }
}
