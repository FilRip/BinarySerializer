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
            var stream = new UnreadableStream();
            var serializer = new BinarySerializer();
            Assert.ThrowsExactly<IOException>(() => _ = serializer.Deserialize<int>(stream));
        }
    }
}