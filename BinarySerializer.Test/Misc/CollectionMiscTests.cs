using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinarySerialization.Test.Misc
{
    [TestClass()]
    public class CollectionMiscTests : TestBase
    {
        [TestMethod()]
        public void ListAtRootTest()
        {
            List<string> expected = ["1", "2", "3"];
            List<string> actual = Roundtrip(expected);
            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod()]
        public void ArrayAtRootTest()
        {
            string[] expected = ["a", "b", "c"];
            string[] actual = Roundtrip(expected);
            Assert.AreEqual(expected.Length, actual.Length);
        }
    }
}