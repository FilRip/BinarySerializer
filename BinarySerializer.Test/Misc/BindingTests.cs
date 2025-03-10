﻿using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinarySerialization.Test.Misc
{
    [TestClass()]
    public class BindingTests : TestBase
    {
        [TestMethod()]
        public void InvalidForwardBindingTest()
        {
#if TESTASYNC
            Assert.ThrowsExactly<AggregateException>(() => _ = Roundtrip(new InvalidForwardBindingClass()));
#else
            Assert.ThrowsException<InvalidOperationException>(() => Roundtrip(new InvalidForwardBindingClass()));
#endif
        }
    }
}