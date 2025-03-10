using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinarySerialization.Test.Enums
{
    [TestClass()]
    public class EnumTests : TestBase
    {
        [TestMethod()]
        public void BasicEnumTest()
        {
            BaseTypeEnumClass expected = new() { Field = BaseTypeEnumValues.B };
            BaseTypeEnumClass actual = Roundtrip(expected, sizeof(BaseTypeEnumValues));

            Assert.AreEqual(expected.Field, actual.Field);
        }

        [TestMethod()]
        public void BasicSignedEnumTest()
        {
            BaseTypeSignedEnumClass expected = new() { Field = BaseTypeSignedEnumValues.NegativeValue };
            BaseTypeSignedEnumClass actual = Roundtrip(expected, sizeof(BaseTypeSignedEnumValues));

            Assert.AreEqual(expected.Field, actual.Field);
        }

        [TestMethod()]
        public void EnumAsStringTest()
        {
            BaseTypeEnumAsStringClass expected = new() { Field = BaseTypeEnumValues.B };
            BaseTypeEnumAsStringClass actual = Roundtrip(expected, [(byte)'B', 0x0]);

            Assert.AreEqual(expected.Field, actual.Field);
        }

        [TestMethod()]
        public void EnumAsStringTest2()
        {
            BaseTypeEnumAsStringClass2 expected = new() { Field = BaseTypeEnumValues.B };
            BaseTypeEnumAsStringClass2 actual = Roundtrip(expected, [(byte)'B', 0x1]);

            Assert.AreEqual(expected.Field, actual.Field);
        }

        [TestMethod()]
        public void NamedEnumTest()
        {
            NamedEnumClass expected = new() { Field = NamedEnumValues.B };
            NamedEnumClass actual = Roundtrip(expected, System.Text.Encoding.UTF8.GetBytes("Bravo" + char.MinValue));

            Assert.AreEqual(expected.Field, actual.Field);
        }

        [TestMethod()]
        public void NamedEnumTest2()
        {
            NamedEnumClass expected = new() { Field = NamedEnumValues.C };
            NamedEnumClass actual = Roundtrip(expected, System.Text.Encoding.UTF8.GetBytes("C\0"));

            Assert.AreEqual(expected.Field, actual.Field);
        }

        [TestMethod()]
        public void NullableEnumTest()
        {
            NullableEnumClass expected = new() { Field = BaseTypeEnumValues.B };
            NullableEnumClass actual = Roundtrip(expected);

            Assert.AreEqual(expected.Field, actual.Field);
        }

        [TestMethod()]
        public void NegativeEnumTest()
        {
            NegativeEnumClass expected = new() { Value = ENegative.A };
            NegativeEnumClass actual = Roundtrip(expected, [(byte)0xff, (byte)0xff]);

            Assert.AreEqual(expected.Value, actual.Value);
        }
    }
}