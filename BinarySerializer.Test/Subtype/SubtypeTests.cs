using System;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinarySerialization.Test.Subtype
{
    [TestClass()]
    public class SubtypeTests : TestBase
    {
        [TestMethod()]
        public void SubtypeTest()
        {
            SubtypeClass expected = new() { Field = new SubclassB { SomethingForClassB = 33 }, Field2 = new SubclassA() };
            SubtypeClass actual = Roundtrip(expected);

            Assert.AreEqual(SubclassType.B, actual.Subtype);
            Assert.IsTrue(actual.Field is SubclassB);
        }

        [TestMethod()]
        public void SubSubtypeTest()
        {
            SubtypeClass expected = new()
            {
                Field = new SubSubclassC(3)
                {
                    SomeSuperStuff = 1,
                    SomethingForClassB = 2
                },
                Field2 = new SubclassA()
            };
            SubtypeClass actual = Roundtrip(expected);

            Assert.AreEqual(SubclassType.C, actual.Subtype);
            Assert.IsTrue(actual.Field is SubSubclassC);
            Assert.AreEqual(actual.Field.SomeSuperStuff, expected.Field.SomeSuperStuff);
            Assert.AreEqual(((SubSubclassC)actual.Field).SomethingForClassB,
                ((SubSubclassC)expected.Field).SomethingForClassB);
            Assert.AreEqual(((SubSubclassC)actual.Field).SomethingForClassC,
                ((SubSubclassC)expected.Field).SomethingForClassC);
        }

        [TestMethod()]
        public void RecoverableMissingSubtypeTest()
        {
            RecoverableMissingSubtypeClass<SuperclassContainer> expected = new()
            {
                Items =
                [
                    new() {Value = new SubclassA()},
                    new() {Value = new SubclassB()},
                    new() {Value = new SubSubclassC(33)}
                ]
            };

            MemoryStream stream = new();

            Serializer.Serialize(stream, expected);

            stream.Position = 0;

            RecoverableMissingSubtypeClass<SuperclassContainerWithMissingSubclass> actual =
                Serializer.Deserialize<RecoverableMissingSubtypeClass<SuperclassContainerWithMissingSubclass>>(stream);

            System.Collections.Generic.List<SuperclassContainerWithMissingSubclass> actualItems = actual.Items;

            Assert.AreEqual(typeof(SubclassA), actualItems[0].Value.GetType());
            Assert.IsNull(actualItems[1].Value);
            Assert.AreEqual(typeof(SubSubclassC), actualItems[2].Value.GetType());
        }

        [TestMethod()]
        public void MissingSubtypeTest()
        {
            IncompleteSubtypeClass expected = new() { Field = new SubclassB() };
#if TESTASYNC
            Assert.ThrowsExactly<AggregateException>(() => _ = Roundtrip(expected));
#else
            Assert.ThrowsException<InvalidOperationException>(() => Roundtrip(expected));
#endif
        }

        [TestMethod()]
        public void SubtypeDefaultTest()
        {
            byte[] data = [0x0, 0x1, 0x2, 0x3, 0x4, 0x5];
            DefaultSubtypeContainerClass actual = Deserialize<DefaultSubtypeContainerClass>(data);
            Assert.AreEqual(typeof(DefaultSubtypeClass), actual.Value.GetType());
        }

        [TestMethod()]
        public void InvalidSubtypeDefaultTest()
        {
            byte[] data = [0x0, 0x1, 0x2, 0x3, 0x4, 0x5];
#if TESTASYNC
            Assert.ThrowsExactly<AggregateException>(() => _ =
#else
            Assert.ThrowsException<InvalidOperationException>(() =>
#endif
            Deserialize<InvalidDefaultSubtypeContainerClass>(data));

        }

        [TestMethod()]
        public void DefaultSubtypeForwardTest()
        {
            DefaultSubtypeContainerClass expected = new()
            {
                Value = new SubclassA()
            };

            DefaultSubtypeContainerClass actual = Roundtrip(expected);

            Assert.AreEqual(1, actual.Indicator);
            Assert.AreEqual(typeof(SubclassA), actual.Value.GetType());
        }

        [TestMethod()]
        public void DefaultSubtypeAllowOnSerialize()
        {
            DefaultSubtypeContainerClass expected = new()
            {
                Indicator = 33,
                Value = new DefaultSubtypeClass()
            };

            DefaultSubtypeContainerClass actual = Roundtrip(expected);

            Assert.AreEqual(33, actual.Indicator);
            Assert.AreEqual(typeof(DefaultSubtypeClass), actual.Value.GetType());
        }

        [TestMethod()]
        public void AncestorSubtypeBindingTest()
        {
            AncestorSubtypeBindingContainerClass expected = new()
            {
                AncestorSubtypeBindingClass =
                    new AncestorSubtypeBindingClass
                    {
                        InnerClass = new AncestorSubtypeBindingInnerClass { Value = "hello" }
                    }
            };

            AncestorSubtypeBindingContainerClass actual = Roundtrip(expected);
            Assert.AreEqual(((AncestorSubtypeBindingClass)expected.AncestorSubtypeBindingClass).InnerClass.Value,
                ((AncestorSubtypeBindingClass)actual.AncestorSubtypeBindingClass).InnerClass.Value);
        }

        [TestMethod()]
        public void SubtypeAsSourceTest()
        {
            SubtypeAsSourceClass expected = new() { Superclass = new SubclassA(), Name = "Alice" };
            SubtypeAsSourceClass actual = Roundtrip(expected);
            Assert.AreEqual(expected.Name, actual.Name);
        }

        [TestMethod()]
        public void IncompatibleBindingsTest()
        {
            IncompatibleBindingsClass expected = new();

#if TESTASYNC
            Assert.ThrowsExactly<AggregateException>(() => _ = Roundtrip(expected));
#else
            Assert.ThrowsException<InvalidOperationException>(() => Roundtrip(expected));
#endif
        }

        [TestMethod()]
        public void InvalidSubtypeTest()
        {
            InvalidSubtypeClass expected = new();
#if TESTASYNC
            Assert.ThrowsExactly<AggregateException>(() => _ = Roundtrip(expected));
#else
            Assert.ThrowsException<InvalidOperationException>(() => Roundtrip(expected));
#endif
        }

        [TestMethod()]
        public void NonUniqueSubtypesTest()
        {
            NonUniqueSubtypesClass expected = new();
#if TESTASYNC
            Assert.ThrowsExactly<AggregateException>(() => _ = Roundtrip(expected));
#else
            Assert.ThrowsException<InvalidOperationException>(() => Roundtrip(expected));
#endif
        }

        [TestMethod()]
        public void NonUniqueSubtypeValuesTest()
        {
            NonUniqueSubtypeValuesClass expected = new();

#if TESTASYNC
            Assert.ThrowsExactly<AggregateException>(() => _ = Roundtrip(expected));
#else
            Assert.ThrowsException<InvalidOperationException>(() => Roundtrip(expected));
#endif
        }

        [TestMethod()]
        public void DefaultSubtypeOnlyTest()
        {
            SubtypeDefaultOnlyClass actual = Deserialize<SubtypeDefaultOnlyClass>([0x4, 0x1, 0x2, 0x3, 0x4, 05]);
            Assert.AreEqual(0x4, actual.Key);
            Assert.AreEqual(typeof(DefaultSubtypeClass), actual.Value.GetType());
        }

        [TestMethod()]
        public void MultipleBindingModesTest()
        {
            MixedBindingModesClass forward = new()
            {
                Value = new SubclassB()
            };

            MixedBindingModesClass actual = Roundtrip(forward, [0x2, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0]);
            Assert.AreEqual(typeof(SubclassA), actual.Value.GetType());
        }

        [TestMethod()]
        public void UnorderedSubtypeTest()
        {
            SuperclassContainerWithNoBinding expected = new()
            {
                Superclass = new UnorderedSubtype()
            };

#if TESTASYNC
            Assert.ThrowsExactly<AggregateException>(() => _ = Roundtrip(expected));
#else
            Assert.ThrowsException<InvalidOperationException>(() => Roundtrip(expected));
#endif
        }

        [TestMethod()]
        public void IgnoredSubtypeTest()
        {
            SuperclassContainerWithNoBinding expected = new()
            {
                Superclass = new IgnoredSubtype()
            };

            SuperclassContainerWithNoBinding actual = Roundtrip(expected);
            Assert.IsNull(actual.Superclass);
        }
    }
}