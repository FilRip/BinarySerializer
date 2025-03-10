using System;

using BinarySerialization.Constants;
using BinarySerialization.Interfaces;

namespace BinarySerialization.Test.EndiannessTest
{
    public class EndiannessConverter : IValueConverter
    {
        public const uint LittleEndiannessMagic = 0x1A2B3C4D;
        public const uint BigEndiannessMagic = 0x4D3C2B1A;

        public object Convert(object value, object parameter, BinarySerializationContext context)
        {
            if (value == null)
                return Endianness.Little;

            var indicator = System.Convert.ToUInt32(value);

            if (indicator == LittleEndiannessMagic)
                return Endianness.Little;

            if (indicator == BigEndiannessMagic)
                return Endianness.Big;

            throw new InvalidOperationException("Invalid endian magic");
        }

        public object ConvertBack(object value, object parameter, BinarySerializationContext context)
        {
            throw new NotSupportedException();
        }
    }
}
