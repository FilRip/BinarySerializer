using System;
using System.Collections.Generic;

using BinarySerialization;
using BinarySerialization.Attributes;
using BinarySerialization.Constants;

namespace BinarySerializer.Performance
{
    [Serializable]
    public class Beer
    {
        [SerializeAs(SerializedType.LengthPrefixedString)]
        [FieldOrder(0)] public string Brand;

#pragma warning disable S1104 // Fields should not have public accessibility
        [NonSerialized, FieldOrder(1)] public byte SortCount;
#pragma warning restore S1104 // Fields should not have public accessibility

        [FieldOrder(2)]
        [FieldCount("SortCount", ConverterType = typeof(TwiceConverter))]
        [FieldCrc16("Crc")]
        public List<SortContainer> Sort;

        [FieldOrder(3)] public float Alcohol;

        [SerializeAs(SerializedType.LengthPrefixedString)]
        [FieldOrder(4)] public string Brewery;

        [FieldOrder(5)]
        public ushort Crc { get; set; }

        [FieldOrder(6)]
        [FieldBitLength(5)]
        public byte WeirdNumber { get; set; }

        [FieldOrder(7)]
        [FieldBitLength(3)]
        public byte WeirdNumber2 { get; set; }

        [FieldOrder(8)]
        [SerializeAs(SerializedType.TerminatedString, StringTerminator = (char)5)]
        public string TerminatedString { get; set; }

        [FieldOrder(9)]
        public Color Color { get; set; }
    }

    [Serializable]
    public class SortContainer
    {
        [NonSerialized]
        [FieldOrder(0)]
#pragma warning disable S1104 // Fields should not have public accessibility
        public byte NameLength;
#pragma warning restore S1104 // Fields should not have public accessibility

        [FieldOrder(1)]
        [FieldLength("NameLength")]
        public string Name;
    }
}
