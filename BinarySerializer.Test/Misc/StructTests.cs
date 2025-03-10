﻿using System;

using BinarySerialization.Attributes;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinarySerialization.Test.Misc
{
    public struct Sample
    {
        [FieldOrder(0)]
        public float A;

        [FieldOrder(1)]
        public float B;
    }

    [TestClass()]
    public class StructTests : TestBase
    {
        [TestMethod()]
        public void TestStruct()
        {
            var expected = new Sample();

#if TESTASYNC
            Assert.ThrowsExactly<AggregateException>(() => _ = Roundtrip(expected));
#else
            Assert.ThrowsException<InvalidOperationException>(() => Roundtrip(expected));
#endif
        }
    }
}
