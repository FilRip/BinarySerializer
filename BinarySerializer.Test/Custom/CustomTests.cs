using System;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinarySerialization.Test.Custom
{
    [TestClass()]
    public class CustomTests : TestBase
    {
        [TestMethod()]
        public void TestVaruint()
        {
            Varuint expected = new() { Value = ushort.MaxValue };
            Varuint actual = Roundtrip(expected, 3);

            Assert.AreEqual(expected.Value, actual.Value);
        }

        [TestMethod()]
        public void TestCustomDateTime()
        {
            CustomDateTime expected = new() { Value = new DateTime(1776, 7, 4, 0, 0, 0, DateTimeKind.Local) };
            CustomDateTime actual = Roundtrip(expected);
            Assert.AreEqual(expected.Value, actual.Value);
        }

        [TestMethod()]
#pragma warning disable S2699 // Tests should include assertions
        public void CustomWithContextTest()
#pragma warning restore S2699 // Tests should include assertions
        {
            CustomWithContextContainerClass expected = new() { Value = new CustomWithContextClass() };

            BinarySerializer serializer = new();
            MemoryStream stream = new();

            serializer.Serialize(stream, expected, "context");
            stream.Position = 0;
            serializer.Deserialize<CustomWithContextContainerClass>(stream);
        }

        [TestMethod()]
        public void CustomSourceBindingTest()
        {
            CustomSourceBinding expected = new() { NameLength = new Varuint(), Name = "Alice" };
            int nameLength = System.Text.Encoding.UTF8.GetByteCount(expected.Name);
            CustomSourceBinding actual = Roundtrip(expected, nameLength + 1);
            Assert.AreEqual(expected.Name, actual.Name);
        }

        [TestMethod()]
        public void CustomSourceBindingTest2()
        {
            CustomSourceBinding expected = new()
            {
                NameLength = new Varuint(),
                Name =
                    "This is rather as if you imagine a puddle waking up one morning and thinking, 'This is an interesting world I find myself in — an interesting hole I find myself in — fits me rather neatly, doesn't it? In fact it fits me staggeringly well, must have been made to have me in it!' This is such a powerful idea that as the sun rises in the sky and the air heats up and as, gradually, the puddle gets smaller and smaller, frantically hanging on to the notion that everything's going to be alright, because this world was meant to have him in it, was built to have him in it; so the moment he disappears catches him rather by surprise. I think this may be something we need to be on the watch out for."
            };
            int nameLength = System.Text.Encoding.UTF8.GetByteCount(expected.Name);
            CustomSourceBinding actual = Roundtrip(expected, nameLength + 2);
            Assert.AreEqual(expected.Name, actual.Name);
        }

        [TestMethod()]
        public void CustomSubtypeTest()
        {
            CustomSubtypeContainerClass expected = new()
            {
                Inner = new CustomSubtypeCustomClass
                {
                    Value = 2097151
                }
            };

            CustomSubtypeContainerClass actual = Roundtrip(expected, 150);

            CustomSubtypeCustomClass innerExpected = (CustomSubtypeCustomClass)expected.Inner;
            CustomSubtypeCustomClass innerActual = (CustomSubtypeCustomClass)actual.Inner;

            Assert.AreEqual(innerExpected.Value, innerActual.Value);
        }

        [TestMethod()]
#pragma warning disable S2699 // Tests should include assertions
        public void CustomAttributeTest()
#pragma warning restore S2699 // Tests should include assertions
        {
            CustomWithCustomAttributesContainerClass expected = new()
            {
                Value = new CustomWithCustomAttributes()
            };

            Roundtrip(expected);
        }

        [TestMethod()]
        public void CustomListTest()
        {
            CustomListClass expected = ["hello"];
            CustomListClass actual = Roundtrip(expected);

            CollectionAssert.AreEqual(expected, actual);
        }

        //[TestMethod()]
        //public void CustomIListTest()
        //{
        //    var expected = new CustomIListClass {"hello"};
        //    var actual = Roundtrip(expected);

        //    Assert.AreEqual(expected, actual);
        //}
    }
}