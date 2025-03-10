using System.IO;
using System.Threading;
using System.Threading.Tasks;

using BinarySerialization.Streams;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinarySerialization.Test.ReaderWriterTests
{
    [TestClass()]
    public class AsyncBinaryReaderTests : TestBase
    {
        [TestMethod()]
        // ReSharper disable once InconsistentNaming
        public async Task ReadCharAsyncASCIITest()
        {
            System.Text.Encoding encoding = System.Text.Encoding.ASCII;

            char expected = 'a';
            byte[] data = encoding.GetBytes(expected.ToString());
            MemoryStream stream = new(data);
            BoundedStream boundedStream = new(stream, string.Empty);
            AsyncBinaryReader reader = new(boundedStream, encoding);
            char actual = await reader.ReadCharAsync(CancellationToken.None);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        // ReSharper disable once InconsistentNaming
        public async Task ReadCharAsyncUTF8Test()
        {
            System.Text.Encoding encoding = System.Text.Encoding.UTF8;

            char expected = 'ش';
            byte[] data = encoding.GetBytes(expected.ToString());
            MemoryStream stream = new(data);
            BoundedStream boundedStream = new(stream, string.Empty);
            AsyncBinaryReader reader = new(boundedStream, encoding);
            char actual = await reader.ReadCharAsync(CancellationToken.None);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        // ReSharper disable once InconsistentNaming
        public async Task ReadCharAsyncUTF16Test()
        {
            System.Text.Encoding encoding = System.Text.Encoding.Unicode;

            char expected = 'ش';
            byte[] data = encoding.GetBytes(expected.ToString());
            MemoryStream stream = new(data);
            BoundedStream boundedStream = new(stream, string.Empty);
            AsyncBinaryReader reader = new(boundedStream, encoding);
            char actual = await reader.ReadCharAsync(CancellationToken.None);
            Assert.AreEqual(expected, actual);
        }
    }
}
