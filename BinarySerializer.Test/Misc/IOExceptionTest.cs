using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinarySerialization.Test.Misc
{
    [TestClass()]
    public class IOExceptionTest
    {
        [TestMethod()]
        public void ShouldThrowIOExceptionNotInvalidOperationExceptionTest()
        {
            UnreadableStream stream = new();
            BinarySerializer serializer = new();
            Assert.ThrowsExactly<IOException>(() => _ = serializer.Deserialize<int>(stream));
        }
    }
}