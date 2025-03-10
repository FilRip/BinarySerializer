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
            var actual = Roundtrip(new ConstCountClass<string>
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
            var actual = Roundtrip(new ConstCountClass<int>
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
            var expected = new BoundCountClass
            {
                Field = [.. TestSequence]
            };

            var actual = Roundtrip(expected);
            Assert.AreEqual(TestSequence.Length, actual.Field.Count);
            Assert.AreEqual(TestSequence.Length, actual.FieldCountField);
            Assert.IsTrue(expected.Field.SequenceEqual(actual.Field));
        }

        [TestMethod()]
        public void ConstCountMismatchTest()
        {
            var actual = Roundtrip(new ConstCountClass<string> { Field = [.. TestSequence.Take(2)] });
            Assert.AreEqual(3, actual.Field.Count);
        }

        [TestMethod()]
        public void PrimitiveConstCountMismatchTest()
        {
            var actual = Roundtrip(new ConstCountClass<int>
            {
                Field = [.. PrimitiveTestSequence.Take(2)],
                Field2 = [.. PrimitiveTestSequence.Take(2)]
            });
            Assert.AreEqual(3, actual.Field.Count);
        }

        [TestMethod()]
        public void PrimitiveListBindingTest()
        {
            var expected = new PrimitiveListBindingClass { Ints = [1, 2, 3] };
            var actual = Roundtrip(expected);

            Assert.AreEqual(expected.Ints.Count, actual.ItemCount);
        }

        [TestMethod()]
        public void PrimitiveArrayBindingTest()
        {
            var expected = new PrimitiveArrayBindingClass { Ints = [1, 2, 3] };
            var actual = Roundtrip(expected);

            Assert.AreEqual(expected.Ints.Length, actual.ItemCount);
        }

        [TestMethod()]
        public void EmptyListBindingTest()
        {
            var expected = new PrimitiveListBindingClass();
            var actual = Roundtrip(expected);

            Assert.AreEqual(0, actual.Ints.Count);
        }

        [TestMethod()]
        public void EmptyArrayBindingTest()
        {
            var expected = new PrimitiveArrayBindingClass();
            var actual = Roundtrip(expected);

            Assert.AreEqual(0, actual.Ints.Length);
        }

        [TestMethod()]
        public void MultibindingTest()
        {
            var expected = new MultibindingClass
            {
                Items = [.. new[] { "hello", "world" }]
            };

            var actual = Roundtrip(expected);

            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual(2, actual.Count2);
        }

        [TestMethod()]
        public void PaddedConstSizeListTest()
        {
            var expected = new PaddedConstSizedListClass
            {
                Items = []
            };

            var actual = Roundtrip(expected);
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
            var expected = new PrimitiveArrayConstClass<TValue>();
            Roundtrip(expected, expectedChildLength * 5);
        }
    }
}