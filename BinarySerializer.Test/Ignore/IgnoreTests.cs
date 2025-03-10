using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinarySerialization.Test.Ignore
{
    [TestClass()]
    public class IgnoreTests : TestBase
    {
        [TestMethod()]
        public void IgnoreObjectTest()
        {
            IgnoreObjectClass expected = new() { FirstField = 1, IgnoreMe = "hello", LastField = 2 };
            IgnoreObjectClass actual = Roundtrip(expected, 8);

            Assert.AreEqual(expected.FirstField, actual.FirstField);
            Assert.IsNull(actual.IgnoreMe);
            Assert.AreEqual(expected.LastField, actual.LastField);
        }

        [TestMethod()]
        public void IgnoreBindingTest()
        {
            IgnoreBindingClass expected = new() { Value = "Hello" };
            IgnoreBindingClass actual = Roundtrip(expected);

            Assert.AreEqual(expected.Value, actual.Value);
            Assert.AreEqual(4, expected.Length);
        }

        [TestMethod()]
        public void IgnoreMemberTest()
        {
            IgnoreMemberClass expected = new() { IgnoreMe = "ignore me" };
            IgnoreMemberClass actual = Roundtrip(expected);

            Assert.IsNull(actual.IgnoreMe);
        }
    }
}