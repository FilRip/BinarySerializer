using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinarySerialization.Test.Value
{
    [TestClass()]
    public class ValueTests : TestBase
    {
        [TestMethod()]
        public void FieldValueTest()
        {
            FieldValueClass expected = new() { Value = 33 };
            FieldValueClass actual = Roundtrip(expected);

            Assert.AreEqual(expected.Value, actual.Value);
            Assert.AreEqual(actual.Value, actual.ValueCopy);
        }

        [TestMethod()]
        public void Crc16Test()
        {
            FieldCrc16Class expected = new()
            {
                Internal = new FieldCrcInternalClass
                {
                    UshortValue = 1,
                    ByteValue = 2,
                    ArrayValue = [0x3, 0x4],
                    Value = "hello world"
                }
            };

            byte[] expectedData =
            [
                0x10, 0x0, 0x0, 0x0,
                0x01, 0x00,
                0x02,
                0x03, 0x04,
                0x68, 0x65, 0x6c, 0x6c, 0x6f, 0x20, 0x77, 0x6f, 0x72, 0x6c, 0x64,
                0x79, 0xcd
            ];

            FieldCrc16Class actual = Roundtrip(expected, expectedData);
            Assert.AreEqual(0xcd79, actual.Crc);
        }

        [TestMethod()]
        public void CrcTestOneWay()
        {
            FieldCrc16OneWayClass expected = new()
            {
                Internal = new FieldCrcInternalClass
                {
                    UshortValue = 1,
                    ByteValue = 2,
                    ArrayValue = [0x3, 0x4],
                    Value = "hello world"
                }
            };

            byte[] expectedData =
            [
                0x10, 0x0, 0x0, 0x0,
                0x01, 0x00,
                0x02,
                0x03, 0x04,
                0x68, 0x65, 0x6c, 0x6c, 0x6f, 0x20, 0x77, 0x6f, 0x72, 0x6c, 0x64,
                0x00, 0x00
            ];

#if TESTASYNC
            Assert.ThrowsExactly<AggregateException>(() => _ = Roundtrip(expected, expectedData));
#else
            Assert.ThrowsException<InvalidOperationException>(() =>  Roundtrip(expected, expectedData));
#endif
        }

        [TestMethod()]
        public void CrcTestOneWayToSource()
        {
            FieldCrc16OneWayToSourceClass expected = new()
            {
                Internal = new FieldCrcInternalClass
                {
                    UshortValue = 1,
                    ByteValue = 2,
                    ArrayValue = [0x3, 0x4],
                    Value = "hello world"
                }
            };

            byte[] data =
            [
                0x10, 0x0, 0x0, 0x0,
                0x01, 0x00,
                0x02,
                0x03, 0x04,
                0x68, 0x65, 0x6c, 0x6c, 0x6f, 0x20, 0x77, 0x6f, 0x72, 0x6c, 0x64,
                0x00, 0x00
            ];

            FieldCrc16OneWayToSourceClass actual = Deserialize<FieldCrc16OneWayToSourceClass>(data);

            Assert.AreEqual(expected.Internal.Value, actual.Internal.Value);
        }

        [TestMethod()]
        public void Crc32Test()
        {
            FieldCrc32Class expected = new()
            {
                Internal = new FieldCrcInternalClass
                {
                    UshortValue = 1,
                    ByteValue = 2,
                    ArrayValue = [0x3, 0x4],
                    Value = "hello world"
                }
            };

            byte[] expectedData =
            [
                0x10, 0x0, 0x0, 0x0,
                0x01, 0x00,
                0x02,
                0x03, 0x04,
                0x68, 0x65, 0x6c, 0x6c, 0x6f, 0x20, 0x77, 0x6f, 0x72, 0x6c, 0x64,
                0xdf, 0x4d, 0x34, 0xf8
            ];

            FieldCrc32Class actual = Roundtrip(expected, expectedData);
            Assert.AreEqual(0xF8344DDF, actual.Crc);
        }

        [TestMethod()]
        public void Crc16StreamTest()
        {
            StreamValueClass expected = new()
            {
                Data = new MemoryStream([.. Enumerable.Repeat((byte)'A', 100000)])
            };

            StreamValueClass actual = Roundtrip(expected);
            Assert.AreEqual(0xdb9, actual.Crc);
        }

        [TestMethod()]
        public void FieldValueExtensionTest()
        {
            FieldSha256Class expected = new()
            {
                Value = "hello world"
            };

            FieldSha256Class actual = Roundtrip(expected);

            byte[] expectedHash =
                SHA256.Create().ComputeHash(new MemoryStream(System.Text.Encoding.ASCII.GetBytes(expected.Value)));

            Assert.IsTrue(expectedHash.SequenceEqual(actual.Hash));
        }

        [TestMethod()]
        public void EasyMistakeCrcTest()
        {
#if TESTASYNC
            Assert.ThrowsExactly<AggregateException>(() => _ = Roundtrip(new EasyMistakeCrcClass()));
#else
            Assert.ThrowsException<InvalidOperationException>(() => Roundtrip(new EasyMistakeCrcClass()));
#endif
        }

        [TestMethod()]
        public void ChecksumTest()
        {
            FieldChecksumClass expected = new()
            {
                Value = "hello"
            };

            FieldChecksumClass actual = Roundtrip(expected);

            Assert.AreEqual(0xEC, actual.Checksum);
            Assert.AreEqual(0x14, actual.ModuloChecksum);
            Assert.AreEqual(0x62, actual.XorChecksum);
        }

        [TestMethod()]
        public void MultiValueFieldTest()
        {
            FieldCrc16MultiFieldClass expected = new() { Value1 = 0x1, Value2 = 0x0201, Value3 = 0x2 };
            FieldCrc16MultiFieldClass actual = Roundtrip(expected);

            Assert.AreEqual(actual.Crc2, actual.Crc);
        }

        [TestMethod()]
        public void NestedCrcTest()
        {
            NestedCrcClass expected = new() { Value = "hello" };
            NestedCrcClass actual = Roundtrip(expected);
            Assert.AreEqual(0xd26e, actual.Crc);
        }

        [TestMethod()]
        public void OuterCrcTest()
        {
            OuterCrcClass value = new() { NestedCrc = new NestedCrcClass { Value = "hello" } };
            OuterCrcClass actual = Roundtrip(value);
            Assert.AreEqual(0xd26e, actual.NestedCrc.Crc);
            Assert.AreEqual(0x91f8, actual.Crc);
        }
    }
}