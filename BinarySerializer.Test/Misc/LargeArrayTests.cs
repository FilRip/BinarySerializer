using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinarySerialization.Test.Misc
{
    [TestClass()]
    public class LargeArrayTests
    {
        [TestMethod()]
#pragma warning disable S2699 // Tests should include assertions
        public void LargeArrayTest()
#pragma warning restore S2699 // Tests should include assertions
        {
            /*var ser = new BinarySerializer();
            var data = new byte[65536 * sizeof(int) * 2];
            data[0] = 65;
            data[262145] = 66;
            IntArray64K result = ser.Deserialize<IntArray64K>(data);
            Assert.AreEqual(65, result.Array[0]);
            Assert.AreEqual(66, result.Array2[0]);

            using var ms = new MemoryStream(data);
            result = ser.Deserialize<IntArray64K>(ms);
            Assert.AreEqual(65, result.Array[0]);
            Assert.AreEqual(66, result.Array2[0]);*/
        }
    }
}
