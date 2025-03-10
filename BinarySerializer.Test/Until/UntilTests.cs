using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinarySerialization.Test.Until
{
    [TestClass()]
    public class UntilTests : TestBase
    {
        [TestMethod()]
        public void TestUntilConst()
        {
            UntilTestClass<string> expected = new()
            {
                Items = ["unless", "someone", "like", "you"],
                AfterItems = "a whole awful lot"
            };
            UntilTestClass<string> actual = Roundtrip(expected);

            Assert.AreEqual(expected.Items.Count, actual.Items.Count);
            Assert.AreEqual(expected.AfterItems, actual.AfterItems);
        }

        [TestMethod()]
        public void PrimitiveTestUntilConst()
        {
            UntilTestClass<int> expected = new() { Items = [3, 2, 1], AfterItems = "a whole awful lot" };
            UntilTestClass<int> actual = Roundtrip(expected);

            Assert.AreEqual(expected.Items.Count, actual.Items.Count);
            Assert.AreEqual(expected.AfterItems, actual.AfterItems);
        }
    }
}