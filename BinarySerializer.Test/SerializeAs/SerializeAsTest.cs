using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinarySerialization.Test.SerializeAs
{
    [TestClass()]
    public class SerializeAsTest : TestBase
    {
        [TestMethod()]
        public void SerializeIntAsSizedStringTest()
        {
            SizedStringClass<int> expected = new() { Value = 33 };
            SizedStringClass<int> actual = Roundtrip(expected, System.Text.Encoding.UTF8.GetBytes(expected.Value.ToString()));

            Assert.AreEqual(expected.Value, actual.Value);
        }

        [TestMethod()]
        public void SerializeAsLengthPrefixedStringTest()
        {
            LengthPrefixedStringClass expected = new() { Value = new string('c', ushort.MaxValue) };
            LengthPrefixedStringClass actual = Roundtrip(expected, ushort.MaxValue + 3);

            Assert.AreEqual(expected.Value, actual.Value);
        }

        [TestMethod()]
        public void SerializeAsWithPaddingValue()
        {
            PaddingValueClass expected = new() { Value = "hi" };
            PaddingValueClass actual = Roundtrip(expected, [0x68, 0x69, 0x33, 0x33, 0x33]);

            Assert.AreEqual(expected.Value, actual.Value.Trim((char)0x33));
        }

        [TestMethod()]
        public void CollectionPaddingValue()
        {
            CollectionPaddingValue expected = new()
            {
                Items =
                [
                    "a", "b"
                ]
            };

            CollectionPaddingValue actual = Roundtrip(expected, [(byte)'a', (byte)' ', (byte)'b', (byte)' ']);

            System.Collections.Generic.List<string> actualItems = [.. actual.Items.Select(i => i.Trim())];
            CollectionAssert.AreEqual(expected.Items, actualItems);
        }
    }
}