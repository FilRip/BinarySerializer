using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinarySerialization.Test.Converters
{
    [TestClass()]
    public class ConverterTests : TestBase
    {
        [TestMethod()]
        public void ConverterTest()
        {
            ConverterClass expected = new() { Field = "FieldValue" };
            ConverterClass actual = Roundtrip(expected);
            Assert.AreEqual((double)expected.Field.Length / 2, actual.HalfFieldLength);
            Assert.AreEqual(expected.Field, actual.Field);
        }
    }
}