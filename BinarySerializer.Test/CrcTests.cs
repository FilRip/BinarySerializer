using BinarySerialization.Crc;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinarySerialization.Test
{
    [TestClass()]
    public class CrcTests
    {
        private static readonly ushort Crc16Polynomial = 0x1021;
        private static readonly uint Crc32Polynomial = 0xedb88320;

        [TestMethod()]
        public void Crc16Test()
        {
            Crc16 crc = new(Crc16Polynomial, 0xffff);
            TestCrc16(crc, "hello world", 0xefeb);
        }

        [TestMethod()]
        public void Crc16RemainderReflectedTest()
        {
            Crc16 crc = new(Crc16Polynomial, 0xffff) { IsRemainderReflected = true };
            TestCrc16(crc, "hello world", 0xd7f7);
        }

        [TestMethod()]
        public void Crc16DataReflectedTest()
        {
            Crc16 crc = new(Crc16Polynomial, 0xffff) { IsDataReflected = true };
            TestCrc16(crc, "hello world", 0x9f8a);
        }

        [TestMethod()]
        public void Crc16DataReflectedRemainderReflectedTest()
        {
            Crc16 crc = new(Crc16Polynomial, 0xffff) { IsDataReflected = true, IsRemainderReflected = true };
            TestCrc16(crc, "hello world", 0x51f9);
        }

        [TestMethod()]
        public void Crc32Test()
        {
            Crc32 crc = new(Crc32Polynomial, 0xffffffff);
            byte[] messageData = System.Text.Encoding.ASCII.GetBytes("hello world");
            crc.Compute(messageData, 0, messageData.Length);
            uint final = crc.ComputeFinal();
            Assert.AreEqual(0xfd11ac49, final);
        }

        [TestMethod()]
        public void Crc32MultipleCallsTest()
        {
            Crc32 crc = new(Crc32Polynomial, 0xffffffff);
            byte[] messageData = System.Text.Encoding.ASCII.GetBytes("hello");
            crc.Compute(messageData, 0, messageData.Length);
            messageData = System.Text.Encoding.ASCII.GetBytes(" ");
            crc.Compute(messageData, 0, messageData.Length);
            messageData = System.Text.Encoding.ASCII.GetBytes("world");
            crc.Compute(messageData, 0, messageData.Length);
            uint final = crc.ComputeFinal();
            Assert.AreEqual(0xfd11ac49, final);
            final = crc.ComputeFinal();
            Assert.AreEqual(0xfd11ac49, final);
        }

        [TestMethod()]
        public void Crc32NoDataReflectTest()
        {
            Crc32 crc = new(Crc32Polynomial, 0xffffffff) { IsDataReflected = false };
            byte[] messageData = System.Text.Encoding.ASCII.GetBytes("hello world");
            crc.Compute(messageData, 0, messageData.Length);
            uint final = crc.ComputeFinal();
            Assert.AreEqual(0xf8485336, final);
        }

        [TestMethod()]
        public void Crc32NoRemainderReflectTest()
        {
            Crc32 crc = new(Crc32Polynomial, 0xffffffff) { IsRemainderReflected = false };
            byte[] messageData = System.Text.Encoding.ASCII.GetBytes("hello world");
            crc.Compute(messageData, 0, messageData.Length);
            uint final = crc.ComputeFinal();
            Assert.AreEqual(0x923588bf, final);
        }

        [TestMethod()]
        public void Crc32NoDataReflectNoRemainderReflectTest()
        {
            Crc32 crc = new(Crc32Polynomial, 0xffffffff) { IsDataReflected = false, IsRemainderReflected = false };
            byte[] messageData = System.Text.Encoding.ASCII.GetBytes("hello world");
            crc.Compute(messageData, 0, messageData.Length);
            uint final = crc.ComputeFinal();
            Assert.AreEqual((uint)0x6cca121f, final);
        }

        // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
        private static void TestCrc16(Crc16 crc, string value, ushort expected)
        {
            byte[] messageData = System.Text.Encoding.ASCII.GetBytes(value);
            crc.Compute(messageData, 0, messageData.Length);
            ushort final = crc.ComputeFinal();
            Assert.AreEqual(expected, final);
        }
    }
}