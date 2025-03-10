using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinarySerialization.Test.Offset
{
    [TestClass()]
    public class OffsetTests : TestBase
    {
        [TestMethod()]
        public void ConstOffsetTest()
        {
            ConstOffsetClass expected = new() { Field = "FieldValue" };
            ConstOffsetClass actual = Roundtrip(expected, 100 + expected.Field.Length + 1);
            Assert.AreEqual(expected.Field, actual.Field);
        }

        [TestMethod()]
        public void BoundOffsetTest()
        {
            BoundOffsetClass expected = new() { FieldOffsetField = 1000, Field = "FieldValue" };
            BoundOffsetClass actual = Roundtrip(expected, expected.FieldOffsetField + expected.Field.Length + 1);
            Assert.AreEqual(expected.Field, actual.Field);
        }
    }
}