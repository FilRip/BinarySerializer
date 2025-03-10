using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinarySerialization.Test.Context
{
    [TestClass()]
    public class ContextTests
    {
        [TestMethod()]
        public void ContextTest()
        {
            ContextClass contextClass = new();
            BinarySerializer serializer = new();

            Context context = new() { SerializeCondtion = false };

            MemoryStream stream = new();
            serializer.Serialize(stream, contextClass, context);

            context = new Context { SerializeCondtion = true };

            stream = new MemoryStream();
            serializer.Serialize(stream, contextClass, context);

            Assert.AreEqual(sizeof(int), stream.Length);
        }
    }
}