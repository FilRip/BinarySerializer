﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinarySerialization.Test.Ignore
{
    [TestClass()]
    public class IgnoreTests : TestBase
    {
        [TestMethod()]
        public void IgnoreObjectTest()
        {
            var expected = new IgnoreObjectClass { FirstField = 1, IgnoreMe = "hello", LastField = 2 };
            var actual = Roundtrip(expected, 8);

            Assert.AreEqual(expected.FirstField, actual.FirstField);
            Assert.IsNull(actual.IgnoreMe);
            Assert.AreEqual(expected.LastField, actual.LastField);
        }

        [TestMethod()]
        public void IgnoreBindingTest()
        {
            var expected = new IgnoreBindingClass { Value = "Hello" };
            var actual = Roundtrip(expected);

            Assert.AreEqual(expected.Value, actual.Value);
            Assert.AreEqual(4, expected.Length);
        }

        [TestMethod()]
        public void IgnoreMemberTest()
        {
            var expected = new IgnoreMemberClass { IgnoreMe = "ignore me" };
            var actual = Roundtrip(expected);

            Assert.IsNull(actual.IgnoreMe);
        }
    }
}