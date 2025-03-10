using BinarySerialization.Attributes;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinarySerialization.Test.Misc
{
    [TestClass()]
    public class IncompleteObjectTests
    {
        public class SimpleClass
        {
            [FieldOrder(0)]
            public int A { get; set; }

            [FieldOrder(1)]
            public int B { get; set; }
        }

        [TestMethod()]
        public void IncompleteObjectTest()
        {
            BinarySerializer serializer = new()
            {
                Options = SerializationOptions.AllowIncompleteObjects
            };

            SimpleClass actual = serializer.Deserialize<SimpleClass>([0x1, 0x2, 0x3, 0x4]);
            Assert.IsNotNull(actual);
            Assert.AreEqual(0, actual.B);
        }
    }
}
