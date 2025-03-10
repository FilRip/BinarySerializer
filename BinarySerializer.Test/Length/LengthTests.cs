using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinarySerialization.Test.Length
{
    [TestClass()]
    public class LengthTests : TestBase
    {
        [TestMethod()]
        public void ConstLengthTest()
        {
            ConstLengthClass actual = Roundtrip(new ConstLengthClass { Field = "FieldValue" }, 6);
            Assert.AreEqual("Fie", actual.Field);
        }

        [TestMethod()]
        public void NullStringConstLengthTest()
        {
            ConstLengthClass actual = Roundtrip(new ConstLengthClass(), 6);
            Assert.AreEqual(string.Empty, actual.Field);
            Assert.AreEqual(System.Text.Encoding.ASCII.GetString([0, 0, 0]), actual.Field2);
        }

        [TestMethod()]
        public void LengthBindingTest()
        {
            BoundLengthClass<string> expected = new() { Field = "FieldValue" };
            BoundLengthClass<string> actual = Roundtrip(expected);
            Assert.AreEqual(expected.Field.Length, actual.FieldLengthField);
            Assert.AreEqual(expected.Field, actual.Field);
        }

        [TestMethod()]
        public void LengthBindingTest2()
        {
            BoundLengthClass<byte[]> expected = new() { Field = System.Text.Encoding.ASCII.GetBytes("FieldValue") };
            BoundLengthClass<byte[]> actual = Roundtrip(expected);
            Assert.AreEqual(expected.Field.Length, actual.FieldLengthField);
            Assert.IsTrue(expected.Field.SequenceEqual(actual.Field));
        }

        [TestMethod()]
        public void CollectionConstLengthTest()
        {
            ConstCollectionLengthClass expected = new() { Field = [.. TestSequence] };
            ConstCollectionLengthClass actual = Roundtrip(expected);
            Assert.AreEqual(TestSequence.Length, actual.Field.Count);
            Assert.IsTrue(expected.Field.SequenceEqual(actual.Field));
        }

        [TestMethod()]
        public void CollectionLengthTest()
        {
            BoundLengthClass<List<string>> expected = new() { Field = [.. TestSequence] };
            BoundLengthClass<List<string>> actual = Roundtrip(expected);
            Assert.AreEqual(expected.Field.Count * 2, actual.FieldLengthField);
            Assert.AreEqual(TestSequence.Length, actual.Field.Count);
        }

        [TestMethod()]
        public void EmptyCollectionLengthTest()
        {
            BoundLengthClass<List<string>> expected = new() { Field = [] };
            BoundLengthClass<List<string>> actual = Roundtrip(expected);
            Assert.AreEqual(expected.Field.Count * 2, actual.FieldLengthField);
            Assert.AreEqual(0, actual.Field.Count);
        }

        [TestMethod()]
        public void ComplexFieldLengthTest()
        {
            BoundLengthClass<ConstLengthClass> expected = new()
            {
                Field = new ConstLengthClass { Field = "FieldValue" }
            };
            BoundLengthClass<ConstLengthClass> actual = Roundtrip(expected);
            Assert.AreEqual(3, actual.Field.Field.Length);
            Assert.AreEqual(6, actual.FieldLengthField);
        }

        [TestMethod()]
        public void ContainedCollectionTest()
        {
            BoundLengthClass<ContainedCollection> expected = new()
            {
                Field = new ContainedCollection
                {
                    Collection =
                    [
                        "hello",
                        "world"
                    ]
                }
            };

            BoundLengthClass<ContainedCollection> actual = Roundtrip(expected);
            Assert.AreEqual(2, actual.Field.Collection.Count);
        }

        [TestMethod()]
        public void PaddedLengthTest()
        {
            PaddedLengthClassClass expected = new()
            {
                InnerClass = new PaddedLengthClassInnerClass
                {
                    Value = "hello"
                },
                InnerClass2 = new PaddedLengthClassInnerClass
                {
                    Value = "world"
                }
            };

            PaddedLengthClassClass actual = Roundtrip(expected, 40);

            Assert.AreEqual(expected.InnerClass.Value, actual.InnerClass.Value);
            Assert.AreEqual(expected.InnerClass.Value.Length, actual.InnerClass.ValueLength);
            Assert.AreEqual(expected.InnerClass2.Value, actual.InnerClass2.Value);
            Assert.AreEqual(expected.InnerClass2.Value.Length, actual.InnerClass2.ValueLength);
        }

        [TestMethod()]
        public void EmbeddedConstrainedCollectionTest()
        {
            EmbeddedConstrainedCollectionClass expected = new()
            {
                Inner = new EmbeddedConstrainedCollectionInnerClass
                {
                    Items =
                    [
                        "we",
                        "have",
                        "nothing",
                        "to",
                        "fear"
                    ]
                }
            };

            Roundtrip(expected, 10);
        }

        [TestMethod()]
        public void BoundItemTest()
        {
            BoundItemContainerClass expected = new()
            {
                Items =
                [
                    new() {Name = "Alice"},
                    new() {Name = "Frank"},
                    new() {Name = "Steve"}
                ]
            };

            BoundItemContainerClass actual = Roundtrip(expected);

            Assert.AreEqual(expected.Items[0].Name, actual.Items[0].Name);
            Assert.AreEqual(expected.Items[1].Name, actual.Items[1].Name);
            Assert.AreEqual(expected.Items[2].Name, actual.Items[2].Name);
        }

        [TestMethod()]
        public void MultibindingTest()
        {
            MultibindingClass expected = new() { Value = "hi" };
            MultibindingClass actual = Roundtrip(expected, [0x02, (byte)'h', (byte)'i', 0x02]);

            Assert.AreEqual(2, actual.Length);
            Assert.AreEqual(2, actual.Length2);
        }

        [TestMethod()]
        public void EmptyClassTest()
        {
            EmptyClass expected = new() { Internal = new EmptyInternalClass() };
            EmptyClass actual = Roundtrip(expected);

            Assert.IsNotNull(actual);
            Assert.IsNotNull(actual.Internal);
        }

        [TestMethod()]

        public void InvalidForwardBindingTest()
        {
#if TESTASYNC
            Assert.ThrowsExactly<AggregateException>(() => _ = Roundtrip(new InvalidForwardBindingClass()));
#else
            Assert.ThrowsExactly<InvalidOperationException>(() => _ = Roundtrip(new InvalidForwardBindingClass()));
#endif
        }

        [TestMethod()]
        public void InterfaceAncestoryBindingTest()
        {
            LengthSourceClass expected = new()
            {
                Internal = new InterfaceAncestoryBindingClass
                {
                    Value = "hello"
                }
            };

            LengthSourceClass actual = Roundtrip(expected);

            Assert.AreEqual(expected.Internal.Value, actual.Internal.Value);
        }

        [TestMethod()]
        public void AncestorBindingCollectionItemTest()
        {
            AncestorBindingCollectionClass expected = new()
            {
                Items =
                [
                    new() {
                        Value = "hello"
                    }
                ]
            };

            AncestorBindingCollectionClass actual = Roundtrip(expected);
            Assert.AreEqual(5, actual.ItemLength);
            Assert.AreEqual("hello", actual.Items[0].Value);
        }

        [TestMethod()]
        public void PrimitiveNullByteArrayLengthTest()
        {
            PrimitiveNullArrayLengthTest<byte>();
        }

        [TestMethod()]
        public void PrimitiveNullSByteArrayLengthTest()
        {
            PrimitiveNullArrayLengthTest<sbyte>();
        }

        [TestMethod()]
        public void PrimitiveNullShortArrayLengthTest()
        {
            PrimitiveNullArrayLengthTest<short>();
        }

        [TestMethod()]
        public void PrimitiveNullUShortArrayLengthTest()
        {
            PrimitiveNullArrayLengthTest<ushort>();
        }

        [TestMethod()]
        public void OneWayLengthBindingTest()
        {
            OneWayLengthBindingClass expected = new() { Value = "hi" };
            Roundtrip(expected, [0, (byte)'h', (byte)'i']);
        }

        private static void PrimitiveNullArrayLengthTest<TValue>()
        {
            PrimitiveArrayClass<TValue> expected = new();
            Roundtrip(expected, 5);
        }
    }
}