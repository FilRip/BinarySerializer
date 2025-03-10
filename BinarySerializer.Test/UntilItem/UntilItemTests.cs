using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinarySerialization.Test.UntilItem
{
    [TestClass()]
    public class UntilItemTests : TestBase
    {
        [TestMethod()]
        public void UntilItemConstTest()
        {
            List<UntilItemClass> items =
            [
                new() {
                    Name = "Alice",
                    LastItem = "Nope",
                    Description = "She's just a girl in the world"
                },
                new() {
                    Name = "Bob",
                    LastItem = "Not yet",
                    Description = "Well, he's just this guy, you know?"
                },
                new() {
                    Name = "Charlie",
                    LastItem = "Yep", // this is only needed for bound case but it's that or reproduce it a bunch of times
                    Description = "What??  That's a great idea!",
                    Type = EUntilItem.End
                }
            ];

            UntilItemContainer expected = new() { Items = items, ItemsLastItemExcluded = items, BoundItems = items, EnumTerminationItems = items };

            UntilItemContainer actual = Roundtrip(expected);

            Assert.AreEqual(expected.Items.Count, actual.Items.Count);
            Assert.AreEqual(expected.ItemsLastItemExcluded.Count - 1, actual.ItemsLastItemExcluded.Count);
        }

        [TestMethod()]
        public void UntilItemBoundTest()
        {
            List<UntilItemClass> items =
            [
                new() {
                    Name = "Alice",
                    LastItem = "Nope",
                    Description = "She's just a girl in the world"
                },
                new() {
                    Name = "Bob",
                    LastItem = "Not yet",
                    Description = "Well, he's just this guy, you know?"
                },
                new() {
                    Name = "Charlie",
                    LastItem = "Yep",
                    Description = "What??  That's a great idea!",
                    Type = EUntilItem.End
                }
            ];

            UntilItemContainer expected = new() { Items = items, ItemsLastItemExcluded = items, BoundItems = items, EnumTerminationItems = items };

            UntilItemContainer actual = Roundtrip(expected);

            Assert.AreEqual(expected.BoundItems.Count, actual.BoundItems.Count);
            Assert.AreEqual(expected.BoundItems[2].LastItem, actual.SerializeUntilField);
        }

        [TestMethod()]
        public void UntilItemEnumTest()
        {
            List<UntilItemClass> items =
            [
                new() {
                    Name = "Alice",
                    LastItem = "Nope",
                    Description = "She's just a girl in the world"
                },
                new() {
                    Name = "Bob",
                    LastItem = "Not yet",
                    Description = "Well, he's just this guy, you know?"
                },
                new() {
                    Name = "Charlie",
                    LastItem = "Yep",
                    Description = "What??  That's a great idea!",
                    Type = EUntilItem.End
                }
            ];

            UntilItemContainer expected = new() { Items = items, ItemsLastItemExcluded = items, BoundItems = items, EnumTerminationItems = items };

            UntilItemContainer actual = Roundtrip(expected);

            Assert.AreEqual(expected.EnumTerminationItems.Count, actual.EnumTerminationItems.Count);
        }

        [TestMethod()]
        public void UntilItemDeferredTest()
        {
            UntilItemContainerDeferred expected = new()
            {
                Sections =
                [
                    new() {
                        Header = new UntilItemSimpleClass {Type = EUntilItem.Header},
                        Items =
                        [
                            new(),
                            new()
                        ]
                    },
                    new() {
                        Header = new UntilItemSimpleClass {Type = EUntilItem.Header},
                        Items =
                        [
                            new(),
                            new()
                        ]
                    },
                ]
            };

            UntilItemContainerDeferred actual = Roundtrip(expected,
            [
                2,0,0,0,0,0,
                2,0,0,0,0,0
            ]);

            Assert.AreEqual(expected.Sections.Count, actual.Sections.Count);
            Assert.AreEqual(expected.Sections[0].Items.Count, actual.Sections[0].Items.Count);
            Assert.AreEqual(expected.Sections[1].Items.Count, actual.Sections[1].Items.Count);
        }
    }
}