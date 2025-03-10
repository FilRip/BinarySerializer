using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinarySerialization.Test.Misc
{
    [TestClass()]
    public class ImmutableTests : TestBase
    {
        [TestMethod()]

        public void PrivateSetterTest()
        {
            PrivateSetterClass expected = new();
#if TESTASYNC
            Assert.ThrowsExactly<AggregateException>(() => _ = Roundtrip(expected));
#else
            Assert.ThrowsException<InvalidOperationException>(() => Roundtrip(expected));
#endif
        }

        [TestMethod()]
        public void ImmutableTest()
        {
            ImmutableClass expected = new(3, 4);
            ImmutableClass actual = Roundtrip(expected);
            Assert.AreEqual(expected.Value, actual.Value);
        }

        [TestMethod()]
        public void ImmutableTest2()
        {
            ImmutableClass2 expected = new(3, 4);
            ImmutableClass2 actual = Roundtrip(expected);
            Assert.AreEqual(expected.Value, actual.Value);
        }

        [TestMethod()]
        public void ImmutableWithNullableParametersTest()
        {
            ImmutableClass3 expected = new(33);
            ImmutableClass3 actual = Roundtrip(expected);
            Assert.AreEqual(expected.Value, actual.Value);
        }

        [TestMethod()]
        public void ImmutableWithNullableParametersAndIgnoreTest()
        {
            ImmutableClass4 expected = new(4, 5);
            ImmutableClass4 actual = Roundtrip(expected);
            Assert.AreEqual(expected.Header, actual.Header);
            Assert.IsNull(actual.ResponseId);
        }

        [TestMethod()]
        public void ImmutableNoPublicConstructorTest()
        {
            MemoryStream stream = new([(byte)0x1]);
            Assert.ThrowsExactly<InvalidOperationException>(() => _ = Serializer.Deserialize<ImmutableNoPublicConstructorClass>(stream));
        }

        [TestMethod()]
        public void ImmutableItemsList()
        {
            List<ImmutableClass> expected =
            [
                new(1, 2),
                new(3, 4)
            ];

            List<ImmutableClass> actual = Roundtrip(expected);

            Assert.AreEqual(expected[0].Value, actual[0].Value);
            Assert.AreEqual(expected[0].Value2, actual[0].Value2);
            Assert.AreEqual(expected[1].Value, actual[1].Value);
            Assert.AreEqual(expected[1].Value2, actual[1].Value2);
        }
    }
}