using System.IO;

using BinarySerialization.Constants;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinarySerialization.Test.EndiannessTest
{
    [TestClass()]
    public class EndiannessTests : TestBase
    {
        [TestMethod()]
        public void SerializerEndiannessTest()
        {
            BinarySerializer serializer = new() { Endianness = Endianness.Big };
            EndiannessClass expected = new() { Short = 1 };

            MemoryStream stream = new();
            serializer.Serialize(stream, expected);

            byte[] data = stream.ToArray();

            Assert.AreEqual(0x1, data[1]);
        }

        [TestMethod()]
        public void FieldEndiannessBeTest()
        {
            MemoryStream stream = new();
            BinaryWriter writer = new(stream);

            writer.Write(EndiannessConverter.BigEndiannessMagic);

            writer.Write((byte)0x0);
            writer.Write((byte)0x1);

            byte[] data = stream.ToArray();

            FieldEndiannessClass actual = RoundtripReverse<FieldEndiannessClass>(data);

            Assert.AreEqual(1, actual.Value);
        }

        [TestMethod()]
        public void FieldEndiannessLeTest()
        {
            MemoryStream stream = new();
            BinaryWriter writer = new(stream);

            writer.Write(EndiannessConverter.LittleEndiannessMagic);

            writer.Write((byte)0x1);
            writer.Write((byte)0x0);

            byte[] data = stream.ToArray();

            FieldEndiannessClass actual = RoundtripReverse<FieldEndiannessClass>(data);

            Assert.AreEqual(1, actual.Value);
        }

        [TestMethod()]
        public void FieldEndiannessConstTest()
        {
            FieldEndiannessConstClass expected = new() { Value = 1 };
            FieldEndiannessConstClass actual = Roundtrip(expected, [0x0, 0x0, 0x0, 0x1]);
            Assert.AreEqual(expected.Value, actual.Value);
        }

        [TestMethod()]
        public void DeferredFieldEndiannessBeTest()
        {
            MemoryStream stream = new();
            BinaryWriter writer = new(stream);

            writer.Write((byte)0x0);
            writer.Write((byte)0x1);

            writer.Write(EndiannessConverter.BigEndiannessMagic);

            byte[] data = stream.ToArray();

            DeferredEndiannessEvaluationClass actual = RoundtripReverse<DeferredEndiannessEvaluationClass>(data);

            Assert.AreEqual(1, actual.Value);
        }

        //[TestMethod()]
        //public void InvalidFieldEndiannessConverterTest()
        //{
        //    Assert.Throws<InvalidOperationException>(() => Roundtrip(typeof(FieldEndiannessInvalidConverterClass)));
        //}
    }
}