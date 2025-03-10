using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinarySerialization.Test.ItemLength
{
    [TestClass()]
    public class ItemLengthTests : TestBase
    {
        [TestMethod()]
        public void ItemConstLengthTest()
        {
            ItemConstLengthClass expected = new() { List = [.. new[] { "abc", "def", "ghi" }] };
            ItemConstLengthClass actual = Roundtrip(expected, expected.List.Count * 3);
            Assert.IsTrue(expected.List.SequenceEqual(actual.List));
        }

        [TestMethod()]
        public void ItemBoundLengthTest()
        {
            ItemBoundLengthClass expected = new() { Items = [.. new[] { "abc", "def", "ghi" }] };

            int itemLength = expected.Items[0].Length;
            int expectedLength = sizeof(int) + itemLength * expected.Items.Count;
            ItemBoundLengthClass actual = Roundtrip(expected, expectedLength);

            Assert.AreEqual(itemLength, actual.ItemLength);
            Assert.IsTrue(expected.Items.SequenceEqual(actual.Items));
        }

        [TestMethod()]
        public void ArrayItemBoundLengthTest()
        {
            ArrayItemBoundLengthClass expected = new() { Items = ["abc", "def", "ghi"] };

            ArrayItemBoundLengthClass actual = Roundtrip(expected);

            Assert.AreEqual(expected.Items.Length, actual.ItemLength);
            Assert.AreEqual(expected.Items.Length, actual.Items.Length);
        }

        [TestMethod()]
        public void ItemBoundMismatchLengthTest_ShouldThrowInvalidOperation()
        {
            ItemBoundLengthClass expected = new() { Items = [.. new[] { "abc", "defghi" }] };
#if TESTASYNC
            Assert.ThrowsExactly<AggregateException>(() => _ = Roundtrip(expected));
#else
            Assert.ThrowsExactly<InvalidOperationException>(() => _ = Roundtrip(expected));
#endif
        }

        [TestMethod()]
        public void ItemLengthListOfByteArraysTest()
        {
            ItemLengthListOfByteArrayClass expected = new()
            {
                Arrays = [new byte[3], new byte[3], new byte[3]]
            };

            ItemLengthListOfByteArrayClass actual = Roundtrip(expected);

            Assert.AreEqual(expected.Arrays.Count, actual.Arrays.Count);
        }

        [TestMethod()]
        public void LimitedItemLengthTest()
        {
            LimitedItemLengthClassClass expected = new()
            {
                InnerClasses =
                [
                    new() {Value = "hello"},
                    new() {Value = "world"}
                ]
            };

            byte[] expectedData = System.Text.Encoding.ASCII.GetBytes("he\0wo\0");
            LimitedItemLengthClassClass actual = Roundtrip(expected, expectedData);

            Assert.AreEqual(expected.InnerClasses[0].Value[..2], actual.InnerClasses[0].Value);
        }

        [TestMethod()]
        public void JaggedArrayTest()
        {
            JaggedArrayClass expected = new() { NameArray = ["Alice", "Bob", "Charlie"] };

            JaggedArrayClass actual = Roundtrip(expected);

            System.Collections.Generic.IEnumerable<int> nameLengths = expected.NameArray.Select(name => name.Length);
            Assert.IsTrue(nameLengths.SequenceEqual(actual.NameLengths));
            Assert.IsTrue(expected.NameArray.SequenceEqual(actual.NameArray));
        }

        [TestMethod()]
        public void JaggedListTest()
        {
            JaggedListClass expected = new() { NameList = ["Alice", "Bob", "Charlie"] };
            JaggedListClass actual = Roundtrip(expected);

            System.Collections.Generic.IEnumerable<int> nameLengths = expected.NameList.Select(name => name.Length);
            Assert.IsTrue(nameLengths.SequenceEqual(actual.NameLengths));
            Assert.IsTrue(expected.NameList.SequenceEqual(actual.NameList));
        }

        [TestMethod()]
        public void JaggedDoubleBoundTest()
        {
            JaggedDoubleBoundClass expected = new() { NameArray = ["Alice", "Bob", "Charlie"] };
            expected.NameList = [.. expected.NameArray];

            JaggedDoubleBoundClass actual = Roundtrip(expected);

            System.Collections.Generic.IEnumerable<int> nameLengths = expected.NameArray.Select(name => name.Length);
            Assert.IsTrue(nameLengths.SequenceEqual(actual.NameLengths));
            Assert.IsTrue(expected.NameArray.SequenceEqual(actual.NameArray));
            Assert.IsTrue(expected.NameList.SequenceEqual(actual.NameList));
        }

        [TestMethod()]
        public void JaggedByteArrayTest()
        {
            string[] names = ["Alice", "Bob", "Charlie"];
            JaggedByteArrayClass expected = new()
            {
                NameData = [.. names.Select(name => System.Text.Encoding.ASCII.GetBytes(name))]
            };

            JaggedByteArrayClass actual = Roundtrip(expected);

            System.Collections.Generic.IEnumerable<string> actualNames = actual.NameData.Select(nameData => System.Text.Encoding.ASCII.GetString(nameData));
            Assert.IsTrue(names.SequenceEqual(actualNames));
        }

        [TestMethod()]
        public void JaggedIntArrayTest()
        {
            JaggedIntArrayClass expected = new()
            {
                Arrays = [[1], [2, 2], [3, 3, 3]]
            };

            JaggedIntArrayClass actual = Roundtrip(expected);

            Assert.IsTrue(expected.Arrays[0].SequenceEqual(actual.Arrays[0]));
            Assert.IsTrue(expected.Arrays[1].SequenceEqual(actual.Arrays[1]));
            Assert.IsTrue(expected.Arrays[2].SequenceEqual(actual.Arrays[2]));
        }
    }
}