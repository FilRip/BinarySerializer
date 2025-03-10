﻿using System;
using System.IO;
using System.Linq;

using BinarySerialization.Streams;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinarySerialization.Test
{
    [TestClass()]
    public class StreamletTest
    {
        private static readonly byte[] SourceData = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9];

        private static void AssertRead(int offset, int length)
        {
            MemoryStream source = new(SourceData);
            Streamlet streamlet = new(source, offset, length);

            byte[] block = new byte[SourceData.Length];
            int read = streamlet.Read(block, 0, block.Length);

            long sourceBalance = source.Length - offset;
            long expectedRead = Math.Min(sourceBalance, length);

            Assert.AreEqual(expectedRead, read);

            System.Collections.Generic.IEnumerable<byte> expectedSequence = SourceData.Skip(offset).Take(read);
            System.Collections.Generic.IEnumerable<byte> sequence = block.Take(read);
            Assert.IsTrue(expectedSequence.SequenceEqual(sequence));

            Assert.AreEqual(offset + read, source.Position);
        }

        /*private static void AssertSeek(int offset, int length, int seekOffset, SeekOrigin seekOrigin)
        {
            var source = new MemoryStream(SourceData);
            var streamlet = new Streamlet(source, offset, length);

            streamlet.Seek(seekOffset, seekOrigin);

            long sourcePosition;
            long position;

            switch (seekOrigin)
            {
                case SeekOrigin.Begin:
                case SeekOrigin.Current:
                    sourcePosition = offset + seekOffset;
                    position = seekOffset;
                    break;
                case SeekOrigin.End:
                    sourcePosition = offset + length - seekOffset;
                    position = length - seekOffset;
                    break;
                default:
                    throw new NotSupportedException();
            }

            Assert.AreEqual(sourcePosition, source.Position);
            Assert.AreEqual(position, streamlet.Position);
        }*/

        [TestMethod()]
        public void ReadFullTest()
        {
            AssertRead(0, SourceData.Length);
        }

        [TestMethod()]
        public void ReadPartialTest()
        {
            AssertRead(0, 4);
        }

        [TestMethod()]
        public void ReadPartialOffsetTest()
        {
            AssertRead(2, 4);
        }
    }
}