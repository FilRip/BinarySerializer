using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinarySerialization.Test.BitLength
{
    [TestClass()]
    public class BitLengthTests : TestBase
    {
        [TestMethod()]
        public void LengthTest()
        {
            BitLengthClass expected = new()
            {
                A = 0b1_0110_1110_1111_0111_1101,
                B = 0b111,
                C = (TypeCode)0b1101,
                Internal = new InternalBitLengthClass { Value = 0b1111 },
                Internal2 = new InternalBitLengthValueClass { Value = 0b1010, Value2 = 0b0101 }
            };

            BitLengthClass actual = Roundtrip(expected, [0x7d, 0xef, 0xf6, 0xfd, 0x5a]);
            Assert.AreEqual(expected.A, actual.A);
            Assert.AreEqual(expected.B, actual.B);
            Assert.AreEqual(expected.C, actual.C);
            Assert.AreEqual(expected.Internal.Value, actual.Internal.Value);
            Assert.AreEqual(0b1010, actual.Internal2.Value);
        }

        [TestMethod()]
        public void LengthOverrunTest()
        {
            InternalBitLengthValueClass expectedHighBits = new()
            {
                Value = 0,
                Value2 = 0x7FFFFFFF,
            };
            InternalBitLengthValueClass actualHighBits = Roundtrip(expectedHighBits, [0xF0]);
            Assert.AreEqual(expectedHighBits.Value & 0x0F, actualHighBits.Value);
            Assert.AreEqual(expectedHighBits.Value2 & 0x0F, actualHighBits.Value2);

            InternalBitLengthValueClass expectedLowBits = new()
            {
                Value = 0x7FFFFFFF,
                Value2 = 0,
            };
            InternalBitLengthValueClass actualLowBits = Roundtrip(expectedLowBits, [0x0F]);
            Assert.AreEqual(expectedLowBits.Value & 0x0F, actualLowBits.Value);
            Assert.AreEqual(expectedLowBits.Value2 & 0x0F, actualLowBits.Value2);

            BitLengthOverflowLsbClass expectedLsbBits = new()
            {
                Value = 0b1_01,
                Value2 = 0b10_1101,
                Value3 = 0b1_10
            };
            BitLengthOverflowLsbClass actualLsbBits = Roundtrip(expectedLsbBits, [0b1011_0101]);
            Assert.AreEqual(expectedLsbBits.Value & 0b11, actualLsbBits.Value);
            Assert.AreEqual(expectedLsbBits.Value2 & 0b1111, actualLsbBits.Value2);
            Assert.AreEqual(expectedLsbBits.Value3 & 0b11, actualLsbBits.Value3);

            BitLengthOverflowMsbClass expectedMsbBits = new()
            {
                Value = 0b101,
                Value2 = 0b101101,
                Value3 = 0b110
            };
            BitLengthOverflowLsbClass actualMsbBits = Roundtrip(expectedLsbBits, [0b1011_0101]);
            Assert.AreEqual(expectedMsbBits.Value & 0b11, actualMsbBits.Value);
            Assert.AreEqual(expectedMsbBits.Value2 & 0b1111, actualMsbBits.Value2);
            Assert.AreEqual(expectedMsbBits.Value3 & 0b11, actualMsbBits.Value3);
        }

        //[TestMethod()]
        public static void LengthTestBE()
        {
            BitLengthClass expected = new()
            {
                A = 0b1_0110_1110_1111_0111_1101,
                B = 0b111,
                C = (TypeCode)0b1101,
                Internal = new InternalBitLengthClass { Value = 0b1111 },
                Internal2 = new InternalBitLengthValueClass { Value = 0b1010, Value2 = 0b0101 }
            };

            BitLengthClass actual = RoundtripBigEndian(expected, [0x7d, 0xef, 0xf6, 0xfd, 0x5a]);
            Assert.AreEqual(expected.A, actual.A);
            Assert.AreEqual(expected.B, actual.B);
            Assert.AreEqual(expected.C, actual.C);
            Assert.AreEqual(expected.Internal.Value, actual.Internal.Value);
            Assert.AreEqual(0b1010, actual.Internal2.Value);
        }

        [TestMethod()]
        public void TestBitLengthValue()
        {
            BitLengthValueClass expected = new()
            {
                Value = new InternalBitLengthValueClass
                {
                    Value = 1,
                    Value2 = 1
                },
                Value2 = 0x11
            };

            BitLengthValueClass actual = Roundtrip(expected);

            Assert.AreEqual(expected.Value.Value, actual.Value.Value);
            Assert.AreEqual(expected.Value.Value2, actual.Value.Value2);
            Assert.AreEqual(actual.Crc2, actual.Crc);
        }

        [TestMethod()]
        public void BitLengthBoolTest()
        {
            List<BitLengthBoolClass> expected =
            [
                new() {Value = true},
                new() {Value = false},
                new() {Value = true},
                new() {Value = false},
                new() {Value = true},
                new() {Value = false},
                new() {Value = true},
                new() {Value = false}
            ];

            List<BitLengthBoolClass> actual = Roundtrip(expected, [0x55]);

            for (int i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected[i].Value, actual[i].Value);
            }
        }

        [TestMethod()]
        public void BoundBitLengthTest()
        {
            BoundBitLengthClass expected = new()
            {
                Items =
                [
                    new() {Value = 1},
                    new() {Value = 2}
                ]
            };

            BoundBitLengthClass actual = Roundtrip(expected);
            Assert.AreEqual(8, actual.BitLength);
            Assert.AreEqual(expected.Items.Count, actual.Items.Count);
            Assert.AreEqual(expected.Items[0].Value, actual.Items[0].Value);
            Assert.AreEqual(expected.Items[1].Value, actual.Items[1].Value);
        }
    }
}

