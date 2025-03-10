using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinarySerialization.Test.Subtype
{
    [TestClass()]
    public class SubtypeFactoryTests : TestBase
    {
        [TestMethod()]
        public void SubtypeFactoryTest()
        {
            SubtypeFactoryClass expected = new()
            {
                Value = new SubclassB()
            };

            SubtypeFactoryClass actual = Roundtrip(expected);
            Assert.AreEqual(2, actual.Key);
            Assert.AreEqual(expected.Value.GetType(), actual.Value.GetType());
        }

        [TestMethod()]
        public void SubtypeMixedTest()
        {
            SubtypeMixedClass expected = new()
            {
                Value = new SubclassB()
            };

            SubtypeMixedClass actual = Roundtrip(expected);
            Assert.AreEqual(2, actual.Key);
            Assert.AreEqual(expected.Value.GetType(), actual.Value.GetType());
        }

        [TestMethod()]
        public void SubtypeMixedTest2()
        {
            SubtypeMixedClass expected = new()
            {
                Value = new SubSubclassC()
            };

            SubtypeMixedClass actual = Roundtrip(expected);
            Assert.AreEqual(3, actual.Key);
            Assert.AreEqual(expected.Value.GetType(), actual.Value.GetType());
        }

        [TestMethod()]
        public void SubtypeFactoryWithDefaultTest()
        {
            byte[] data = [0x0, 0x1, 0x2, 0x3, 0x4, 0x5];
            SubtypeFactoryWithDefaultClass actual = Deserialize<SubtypeFactoryWithDefaultClass>(data);
            Assert.AreEqual(typeof(DefaultSubtypeClass), actual.Value.GetType());
        }

        [TestMethod()]
        public void SubtypeMixedWithDefaultTest()
        {
            byte[] data = [0x0, 0x1, 0x2, 0x3, 0x4, 0x5];
            SubtypeMixedWithDefaultClass actual = Deserialize<SubtypeMixedWithDefaultClass>(data);
            Assert.AreEqual(typeof(DefaultSubtypeClass), actual.Value.GetType());
        }

        [TestMethod()]
        public void InterfaceSubtypeTest()
        {
            InterfaceSubtype expected = new()
            {
                Value = new InterfaceSubclassA
                {
                    Value = "hello",
                    MoreStuff = 4
                }
            };

            InterfaceSubtype actual = Roundtrip(expected);

            Assert.IsInstanceOfType(actual.Value, typeof(InterfaceSubclassA));
        }
    }
}
