using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinarySerialization.Test.When
{
    [TestClass()]
    public class WhenTests : TestBase
    {
        [TestMethod()]
        public void WhenStringTest()
        {
            WhenStringTestClass expected = new()
            {
                WhatToDo = "PickOne",
                SerializeThis = 1,
                DontSerializeThis = 2,
                SerializeThisNoMatterWhat = 3
            };

            WhenStringTestClass actual = Roundtrip(expected);

            Assert.AreEqual(expected.SerializeThis, actual.SerializeThis);
            Assert.AreNotEqual(expected.DontSerializeThis, actual.DontSerializeThis);
            Assert.AreEqual(expected.SerializeThisNoMatterWhat, actual.SerializeThisNoMatterWhat);
        }

        [TestMethod()]
        public void WhenIntTest()
        {
            WhenIntTestClass expected = new()
            {
                WhatToDo = 2,
                SerializeThis = 100,
                DontSerializeThis = 200,
                SerializeThisNoMatterWhat = 300,
                SerializeThis2 = 400,
                DontSerializeThis2 = 500
            };

            WhenIntTestClass actual = Roundtrip(expected);

            Assert.AreEqual(expected.SerializeThis, actual.SerializeThis);
            Assert.AreNotEqual(expected.DontSerializeThis, actual.DontSerializeThis);
            Assert.AreEqual(expected.SerializeThisNoMatterWhat, actual.SerializeThisNoMatterWhat);
            Assert.AreEqual(expected.SerializeThis2, actual.SerializeThis2);
            Assert.AreNotEqual(expected.DontSerializeThis2, actual.DontSerializeThis2);
        }

        [TestMethod()]
        public void WhenEnumTest()
        {
            WhenEnumTestClass expected = new()
            {
                WhatToDo = 1,
                SerializeThis = 1000,
                DontSerializeThis = 2000,
                SerializeThisNoMatterWhat = 3000
            };

            WhenEnumTestClass actual = Roundtrip(expected);

            Assert.AreEqual(expected.SerializeThis, actual.SerializeThis);
            Assert.AreNotEqual(expected.DontSerializeThis, actual.DontSerializeThis);
            Assert.AreEqual(expected.SerializeThisNoMatterWhat, actual.SerializeThisNoMatterWhat);
        }

        [TestMethod()]
        public void WhenConverterTest()
        {
            WhenConverterTestClass expected = new()
            {
                WhatToDo = 1,
                SerializeThis = 1000,
                DontSerializeThis = 2000,
                SerializeThisNoMatterWhat = 3000
            };

            WhenConverterTestClass actual = Roundtrip(expected);

            Assert.AreEqual(expected.SerializeThis, actual.SerializeThis);
            Assert.AreNotEqual(expected.DontSerializeThis, actual.DontSerializeThis);
            Assert.AreEqual(expected.SerializeThisNoMatterWhat, actual.SerializeThisNoMatterWhat);
        }
    }
}