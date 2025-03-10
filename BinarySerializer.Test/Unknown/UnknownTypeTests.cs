using System.IO;

using BinarySerialization.Exceptions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinarySerialization.Test.Unknown
{
    [TestClass()]
    public class UnknownTypeTests : TestBase
    {
        [TestMethod()]
        public void UnknownTypeSerializationTest()
        {
            UnknownTypeClass unknownTypeClass = new() { Field = "hello" };

            BinarySerializer serializer = new();

            MemoryStream stream = new();
            serializer.Serialize(stream, unknownTypeClass);
            string result = System.Text.Encoding.ASCII.GetString(stream.ToArray());
            Assert.AreEqual("hello\0", result);
        }

        [TestMethod()]
        public void SubtypesOnUnknownTypeFieldShouldThrowBindingException()
        {
            InvalidUnknownTypeClass unknownTypeClass = new() { Field = "hello" };

            BinarySerializer serializer = new();

            MemoryStream stream = new();
            Assert.ThrowsExactly<BindingException>(() => serializer.Serialize(stream, unknownTypeClass));
        }

        [TestMethod()]
        public void BindingAcrossUnknownBoundaryTest()
        {
            BindingAcrossUnknownBoundaryChildClass childClass = new() { Subfield = "hello" };
            BindingAcrossUnknownBoundaryClass unknownTypeClass = new()
            {
                Field = childClass
            };

            BinarySerializer serializer = new();

            MemoryStream stream = new();
            serializer.Serialize(stream, unknownTypeClass);

            byte[] data = stream.ToArray();

            Assert.AreEqual((byte)childClass.Subfield.Length, data[0]);
        }

        [TestMethod()]
        public void UnknownSubtypeTest()
        {
            string s = "hello";

            UnknownSubtypeContainer expected = new()
            {
                Unknown = new ClassB
                {
                    Value = s
                }
            };

            UnknownSubtypeContainer actual = Roundtrip(expected);

            Assert.IsInstanceOfType(actual.Unknown, typeof(ClassB));
            ClassB value = (ClassB)actual.Unknown;
            Assert.AreEqual(s, value.Value);
        }
    }
}