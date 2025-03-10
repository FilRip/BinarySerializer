using System.IO;

using BinarySerialization.Constants;
using BinarySerialization.Interfaces;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinarySerialization.Test.Custom
{
    public class CustomWithContextClass : IBinarySerializable
    {
        public void Serialize(Stream stream, Endianness endianness,
            BinarySerializationContext serializationContext)
        {
            Assert.AreEqual(typeof(CustomWithContextContainerClass), serializationContext.ParentType);
            //Assert.AreEqual("context", serializationContext.ParentContext.ParentValue);
            // TODO check root context
        }

        public void Deserialize(Stream stream, Endianness endianness,
            BinarySerializationContext serializationContext)
        {
            Assert.AreEqual(typeof(CustomWithContextContainerClass), serializationContext.ParentType);
        }
    }
}