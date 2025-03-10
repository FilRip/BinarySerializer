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
            var expected = new PrivateSetterClass();
#if TESTASYNC
            Assert.ThrowsExactly<AggregateException>(() => _ = Roundtrip(expected));
#else
            Assert.ThrowsException<InvalidOperationException>(() => Roundtrip(expected));
#endif
        }

        [TestMethod()]
        public void ImmutableTest()
        {
            var expected = new ImmutableClass(3, 4);
            var actual = Roundtrip(expected);
            Assert.AreEqual(expected.Value, actual.Value);
        }

        [TestMethod()]
        public void ImmutableTest2()
        {
            var expected = new ImmutableClass2(3, 4);
            var actual = Roundtrip(expected);
            Assert.AreEqual(expected.Value, actual.Value);
        }

        [TestMethod()]
        public void ImmutableWithNullableParametersTest()
        {
            var expected = new ImmutableClass3(33);
            var actual = Roundtrip(expected);
            Assert.AreEqual(expected.Value, actual.Value);
        }

        [TestMethod()]
        public void ImmutableWithNullableParametersAndIgnoreTest()
        {
            var expected = new ImmutableClass4(4, 5);
            var actual = Roundtrip(expected);
            Assert.AreEqual(expected.Header, actual.Header);
            Assert.IsNull(actual.ResponseId);
        }

        [TestMethod()]
        public void ImmutableNoPublicConstructorTest()
        {
            var stream = new MemoryStream([(byte)0x1]);
            Assert.ThrowsExactly<InvalidOperationException>(() => _ = Serializer.Deserialize<ImmutableNoPublicConstructorClass>(stream));
        }

        [TestMethod()]
        public void ImmutableItemsList()
        {
            var expected = new List<ImmutableClass>
            {
                new(1, 2),
                new(3, 4)
            };

            var actual = Roundtrip(expected);

            Assert.AreEqual(expected[0].Value, actual[0].Value);
            Assert.AreEqual(expected[0].Value2, actual[0].Value2);
            Assert.AreEqual(expected[1].Value, actual[1].Value);
            Assert.AreEqual(expected[1].Value2, actual[1].Value2);
        }
    }
}