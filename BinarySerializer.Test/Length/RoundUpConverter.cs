using System;

using BinarySerialization.Interfaces;

namespace BinarySerialization.Test.Length
{
    public class RoundUpConverter : IValueConverter
    {
        public object Convert(object value, object parameter, BinarySerializationContext context)
        {
            ArgumentNullException.ThrowIfNull(value);

            ArgumentNullException.ThrowIfNull(parameter);

            ulong v = System.Convert.ToUInt64(value);
            ulong m = System.Convert.ToUInt64(parameter);
            return v + (m - v % m) % m;
        }

        public object ConvertBack(object value, object parameter, BinarySerializationContext context)
        {
            throw new NotSupportedException();
        }
    }
}
