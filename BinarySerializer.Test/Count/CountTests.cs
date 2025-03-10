using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinarySerialization.Test.Count
{
    [TestClass()]
    public class CountTests : TestBase
    {
        [TestMethod()]
        public void ConstCountTest()
        {
            ConstCountClass<string> actual = Roundtrip(new ConstCountClass<string>
            {
                Field = [.. TestSequence],
                Field2 = [.. TestSequence]
            });

            Assert.AreEqual(3, actual.Field.Count);
            Assert.AreEqual(3, actual.Field2.Length);
        }

        [TestMethod()]
        public void PrimitiveConstCountTest()
        {
            ConstCountClass<int> actual = Roundtrip(new ConstCountClass<int>
            {
                Field = [.. PrimitiveTestSequence],
                Field2 = [.. PrimitiveTestSequence]
            });

            Assert.AreEqual(3, actual.Field.Count);
            Assert.AreEqual(3, actual.Field2.Length);
        }

        [TestMethod()]
        public void CountTest()
        {
            BoundCountClass expected = new()
            {
                Field = [.. TestSequence]
            };

            BoundCountClass actual = Roundtrip(expected);
            Assert.AreEqual(TestSequence.Length, actual.Field.Count);
            Assert.AreEqual(TestSequence.Length, actual.FieldCountField);
            Assert.IsTrue(expected.Field.SequenceEqual(actual.Field));
        }

        [TestMethod()]
        public void ConstCountMismatchTest()
        {
            ConstCountClass<string> actual = Roundtrip(new ConstCountClass<string> { Field = [.. TestSequence.Take(2)] });
            Assert.AreEqual(3, actual.Field.Count);
        }

        [TestMethod()]
        public void PrimitiveConstCountMismatchTest()
        {
            ConstCountClass<int> actual = Roundtrip(new ConstCountClass<int>
            {
                Field = [.. PrimitiveTestSequence.Take(2)],
                Field2 = [.. PrimitiveTestSequence.Take(2)]
            });
            Assert.AreEqual(3, actual.Field.Count);
        }

        [TestMethod()]
        public void PrimitiveListBindingTest()
        {
            PrimitiveListBindingClass expected = new() { Ints = [1, 2, 3] };
            PrimitiveListBindingClass actual = Roundtrip(expected);

            Assert.AreEqual(expected.Ints.Count, actual.ItemCount);
        }

        [TestMethod()]
        public void PrimitiveArrayBindingTest()
        {
            PrimitiveArrayBindingClass expected = new() { Ints = [1, 2, 3] };
            PrimitiveArrayBindingClass actual = Roundtrip(expected);

            Assert.AreEqual(expected.Ints.Length, actual.ItemCount);
        }

        [TestMethod()]
        public void EmptyListBindingTest()
        {
            PrimitiveListBindingClass expected = new();
            PrimitiveListBindingClass actual = Roundtrip(expected);

            Assert.AreEqual(0, actual.Ints.Count);
        }

        [TestMethod()]
        public void EmptyArrayBindingTest()
        {
            PrimitiveArrayBindingClass expected = new();
            PrimitiveArrayBindingClass actual = Roundtrip(expected);

            Assert.AreEqual(0, actual.Ints.Length);
        }

        [TestMethod()]
        public void MultibindingTest()
        {
            MultibindingClass expected = new()
            {
                Items = [.. new[] { "hello", "world" }]
            };

            MultibindingClass actual = Roundtrip(expected);

            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual(2, actual.Count2);
        }

        [TestMethod()]
        public void PaddedConstSizeListTest()
        {
            PaddedConstSizedListClass expected = new()
            {
                Items = []
            };

            PaddedConstSizedListClass actual = Roundtrip(expected);
            Assert.AreEqual(6, actual.Items.Count);
        }

        [TestMethod()]
        public void PrimitiveNullByteArrayTest()
        {
            PrimitiveNullArrayLengthTest<byte>(sizeof(byte));
        }

        [TestMethod()]
        public void PrimitiveNullSByteArrayTest()
        {
            PrimitiveNullArrayLengthTest<sbyte>(sizeof(sbyte));
        }

        [TestMethod()]
        public void PrimitiveNullShortArrayTest()
        {
            PrimitiveNullArrayLengthTest<short>(sizeof(short));
        }

        [TestMethod()]
        public void PrimitiveNullUShortArrayTest()
        {
            PrimitiveNullArrayLengthTest<ushort>(sizeof(ushort));
        }

        [TestMethod()]
        public void PrimitiveNullIntArrayTest()
        {
            PrimitiveNullArrayLengthTest<int>(sizeof(int));
        }

        [TestMethod()]
        public void PrimitiveNullUIntArrayTest()
        {
            PrimitiveNullArrayLengthTest<uint>(sizeof(uint));
        }

        [TestMethod()]
        public void PrimitiveNullLongArrayTest()
        {
            PrimitiveNullArrayLengthTest<uint>(sizeof(uint));
        }

        [TestMethod()]
        public void PrimitiveNullULongArrayTest()
        {
            PrimitiveNullArrayLengthTest<uint>(sizeof(uint));
        }

        [TestMethod()]
        public void PrimitiveNullStringArrayTest()
        {
            PrimitiveNullArrayLengthTest<string>(0);
        }

        private static void PrimitiveNullArrayLengthTest<TValue>(int expectedChildLength)
        {
            PrimitiveArrayConstClass<TValue> expected = new();
            Roundtrip(expected, expectedChildLength * 5);
        }
    }
}