using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinarySerialization.Test.Alignment
{
    [TestClass()]
    public class AlignmentTests : TestBase
    {
        [TestMethod()]
        public void AlignmentTest()
        {
            AlignmentClass actual = RoundtripReverse<AlignmentClass>(
            [
                0x2, 0x0, 0x0, 0x0,
                (byte) 'h', (byte) 'i', 0, 0
            ]);

            Assert.AreEqual(2, actual.Length);
            Assert.AreEqual("hi", actual.Value);
        }

        [TestMethod()]
        public void BoundAlignmentTest()
        {
            BoundAlignmentClass actual = RoundtripReverse<BoundAlignmentClass>(
            [
                0x2, 0x4, 0x0, 0x0,
                (byte) 'h', (byte) 'i', 0, 0
            ]);

            Assert.AreEqual(2, actual.Length);
            Assert.AreEqual(4, actual.Alignment);
            Assert.AreEqual("hi", actual.Value);
        }

        [TestMethod()]
        public void LeftAlignmentTest()
        {
            LeftAlignmentClass actual = RoundtripReverse<LeftAlignmentClass>(
            [
                0x1, 0x0, 0x0, 0x0,
                0x2,
                0x3
            ]);

            Assert.AreEqual((byte)1, actual.Header);
            Assert.AreEqual((byte)2, actual.Value);
            Assert.AreEqual((byte)3, actual.Trailer);
        }

        [TestMethod()]
        public void RightAlignmentTest()
        {
            RightAlignmentClass actual = RoundtripReverse<RightAlignmentClass>(
            [
                0x1,
                0x2, 0x0, 0x0,
                0x3
            ]);

            Assert.AreEqual((byte)1, actual.Header);
            Assert.AreEqual((byte)2, actual.Value);
            Assert.AreEqual((byte)3, actual.Trailer);
        }

        [TestMethod()]
        public void MixedAlignmentTest()
        {
            MixedAlignmentClass actual = RoundtripReverse<MixedAlignmentClass>(
            [
                0x1, 0x0, 0x0, 0x0,
                0x2, 0x0,
                0x3
            ]);

            Assert.AreEqual((byte)1, actual.Header);
            Assert.AreEqual((byte)2, actual.Value);
            Assert.AreEqual((byte)3, actual.Trailer);
        }
    }
}
