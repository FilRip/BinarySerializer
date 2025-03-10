using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinarySerialization.Test.Misc
{
    [TestClass()]
    public class MismatchBindingTests : TestBase
    {
        [TestMethod()]
        public void MismatchBindingTest()
        {
            MismatchBindingClass expected = new() { Name1 = "Alice", Name2 = "Bob" };

#if TESTASYNC
            Assert.ThrowsExactly<AggregateException>(() => _ = Roundtrip(expected));
#else
            Assert.ThrowsException<InvalidOperationException>(() => Roundtrip(expected));
#endif
        }
    }
}