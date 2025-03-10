using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinarySerialization.Test.Order
{
    [TestClass()]
    public class OrderTests : TestBase
    {
        [TestMethod()]
        public void OrderTest()
        {
            OrderClass order = new() { First = 1, Second = 2, Name = "Alice" };
            Roundtrip(order, [0x1, 0x2, 0x5, 0x41, 0x6c, 0x69, 0x63, 0x65]);
        }

        [TestMethod()]
#pragma warning disable S2699 // Tests should include assertions
        public void SingleMemberOrderShouldntThrowTest()
#pragma warning restore S2699 // Tests should include assertions
        {
            SingleMemberOrderClass order = new();
            Roundtrip(order);
        }

        [TestMethod()]
        public void MultipleMembersNoOrderAttributeShouldThrowTest()
        {
            MutlipleMembersNoOrderClass order = new();

#if TESTASYNC
            Assert.ThrowsExactly<AggregateException>(() => _ = Roundtrip(order));
#else
            Assert.ThrowsException<InvalidOperationException>(() => Roundtrip(order));
#endif
        }

        [TestMethod()]
        public void MultipleMembersDuplicateOrderAttributeShouldThrowTest()
        {
            MutlipleMembersDuplicateOrderClass order = new();

#if TESTASYNC
            Assert.ThrowsExactly<AggregateException>(() => _ = Roundtrip(order));
#else
            Assert.ThrowsException<InvalidOperationException>(() => Roundtrip(order));
#endif
        }

        [TestMethod()]
        public void BaseClassComesBeforeDerivedClassTest()
        {
            OrderDerivedClass order = new() { First = 1, Second = 2 };
            Roundtrip(order, [0x1, 0x2]);
        }
    }
}