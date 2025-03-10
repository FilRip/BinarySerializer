using System;
using System.IO;

using BinarySerialization.Streams;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinarySerialization.Test.Streams
{
    [TestClass()]
    public class StreamTests : TestBase
    {
        [TestMethod()]
        public void StreamTest()
        {
            MemoryStream stream = new(System.Text.Encoding.ASCII.GetBytes("StreamValue"));
            StreamClass expected = new() { Field = stream };
            StreamClass actual = Roundtrip(expected);
            Assert.AreEqual(stream.Length, actual.Field.Length);
        }

        [TestMethod()]
        public void BoundedStreamSetLengthThrowsNotSupported()
        {
            Assert.ThrowsExactly<NotSupportedException>(() => new BoundedStream(new MemoryStream(), string.Empty).SetLength(0));
        }

        [TestMethod()]
        public void BoundedStreamToStringIsName()
        {
            const string name = "Name";
            BoundedStream stream = new(new MemoryStream(), name);
            Assert.AreEqual(name, stream.ToString());
        }
    }
}