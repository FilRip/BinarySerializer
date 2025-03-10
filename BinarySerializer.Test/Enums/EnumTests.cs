﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinarySerialization.Test.Enums
{
    [TestClass()]
    public class EnumTests : TestBase
    {
        [TestMethod()]
        public void BasicEnumTest()
        {
            var expected = new BaseTypeEnumClass { Field = BaseTypeEnumValues.B };
            var actual = Roundtrip(expected, sizeof(BaseTypeEnumValues));

            Assert.AreEqual(expected.Field, actual.Field);
        }

        [TestMethod()]
        public void BasicSignedEnumTest()
        {
            var expected = new BaseTypeSignedEnumClass { Field = BaseTypeSignedEnumValues.NegativeValue };
            var actual = Roundtrip(expected, sizeof(BaseTypeSignedEnumValues));

            Assert.AreEqual(expected.Field, actual.Field);
        }

        [TestMethod()]
        public void EnumAsStringTest()
        {
            var expected = new BaseTypeEnumAsStringClass { Field = BaseTypeEnumValues.B };
            var actual = Roundtrip(expected, [(byte)'B', 0x0]);

            Assert.AreEqual(expected.Field, actual.Field);
        }

        [TestMethod()]
        public void EnumAsStringTest2()
        {
            var expected = new BaseTypeEnumAsStringClass2 { Field = BaseTypeEnumValues.B };
            var actual = Roundtrip(expected, [(byte)'B', 0x1]);

            Assert.AreEqual(expected.Field, actual.Field);
        }

        [TestMethod()]
        public void NamedEnumTest()
        {
            var expected = new NamedEnumClass { Field = NamedEnumValues.B };
            var actual = Roundtrip(expected, System.Text.Encoding.UTF8.GetBytes("Bravo" + char.MinValue));

            Assert.AreEqual(expected.Field, actual.Field);
        }

        [TestMethod()]
        public void NamedEnumTest2()
        {
            var expected = new NamedEnumClass { Field = NamedEnumValues.C };
            var actual = Roundtrip(expected, System.Text.Encoding.UTF8.GetBytes("C\0"));

            Assert.AreEqual(expected.Field, actual.Field);
        }

        [TestMethod()]
        public void NullableEnumTest()
        {
            var expected = new NullableEnumClass { Field = BaseTypeEnumValues.B };
            var actual = Roundtrip(expected);

            Assert.AreEqual(expected.Field, actual.Field);
        }

        [TestMethod()]
        public void NegativeEnumTest()
        {
            var expected = new NegativeEnumClass { Value = ENegative.A };
            var actual = Roundtrip(expected, [(byte)0xff, (byte)0xff]);

            Assert.AreEqual(expected.Value, actual.Value);
        }
    }
}