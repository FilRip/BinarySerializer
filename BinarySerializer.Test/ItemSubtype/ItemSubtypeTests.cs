using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinarySerialization.Test.ItemSubtype
{
    [TestClass()]
    public class ItemSubtypeTests : TestBase
    {
        [TestMethod()]
        public void ItemSubtypeTest()
        {
            ItemSubtypeClass expected = new()
            {
                Items =
                [
                    new ItemTypeB {Value = 1},
                    new ItemTypeB {Value = 2},
                    new ItemTypeB {Value = 3}
                ]
            };

            ItemSubtypeClass actual = Roundtrip(expected, [2, 1, 2, 3]);

            Assert.AreEqual(2, actual.Indicator);
            Assert.AreEqual(3, actual.Items.Count);
            Assert.AreEqual(expected.Items[0].GetType(), actual.Items[0].GetType());
            Assert.AreEqual(expected.Items[1].GetType(), actual.Items[1].GetType());
            Assert.AreEqual(expected.Items[2].GetType(), actual.Items[2].GetType());
        }

        [TestMethod()]
        public void CustomItemSubtypeTest()
        {
            ItemSubtypeClass expected = new()
            {
                Items =
                [
                    new CustomItem(),
                    new CustomItem()
                ]
            };

            byte[] data = [3, .. CustomItem.Data, .. CustomItem.Data];
            ItemSubtypeClass actual = Roundtrip(expected, data);

            Assert.AreEqual(3, actual.Indicator);
            Assert.AreEqual(2, actual.Items.Count);
            Assert.AreEqual(expected.Items[0].GetType(), actual.Items[0].GetType());
            Assert.AreEqual(expected.Items[1].GetType(), actual.Items[1].GetType());
        }


        [TestMethod()]
        public void DefaultItemSubtypeTest()
        {
            byte[] data = [4, 0, 1, 2, 3];
            ItemSubtypeClass actual = Deserialize<ItemSubtypeClass>(data);

            Assert.AreEqual(4, actual.Indicator);
            Assert.AreEqual(1, actual.Items.Count);
            Assert.AreEqual(typeof(DefaultItemType), actual.Items[0].GetType());
        }

        [TestMethod()]
        public void ItemSubtypeFactoryTest()
        {
            ItemSubtypeFactoryClass expected = new()
            {
                Value =
                [
                    new ItemTypeB(),
                    new ItemTypeB()
                ]
            };

            ItemSubtypeFactoryClass actual = Roundtrip(expected);

            Assert.AreEqual(3, actual.Key);
        }

        [TestMethod()]
        public void ItemSubtypeMixedTest()
        {
            ItemSubtypeMixedClass expected = new()
            {
                Value =
                [
                    new ItemTypeB(),
                    new ItemTypeB()
                ]
            };

            ItemSubtypeMixedClass actual = Roundtrip(expected);

            Assert.AreEqual(2, actual.Key);
        }

        [TestMethod()]
        public void ItemSubtypeFactoryWithDefaultTest()
        {
            byte[] data = [4, 0, 1, 2, 3];
            ItemSubtypeFactoryWithDefaultClass actual = Deserialize<ItemSubtypeFactoryWithDefaultClass>(data);

            Assert.AreEqual(4, actual.Key);
            Assert.AreEqual(1, actual.Items.Count);
            Assert.AreEqual(typeof(DefaultItemType), actual.Items[0].GetType());
        }
    }
}
