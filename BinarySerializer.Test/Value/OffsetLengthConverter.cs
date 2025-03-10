using System;

using BinarySerialization.Interfaces;

namespace BinarySerialization.Test.Value
{
    public class OffsetLengthConverter : IValueConverter
    {
        private const int BaseOffset = 20;

        public object Convert(object value, object parameter, BinarySerializationContext context)
        {
            int offset = System.Convert.ToInt32(value) * 4;
            return offset - BaseOffset;
        }

        public object ConvertBack(object value, object parameter, BinarySerializationContext context)
        {
            int length = System.Convert.ToInt32(value);
            return (int)Math.Ceiling((length + BaseOffset) / 4f);
        }
    }
}
