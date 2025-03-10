using BinarySerialization;
using BinarySerialization.Interfaces;

namespace BinarySerializer.Performance
{
    public class TwiceConverter : IValueConverter
    {
        public object Convert(object value, object parameter, BinarySerializationContext context)
        {
            int a = System.Convert.ToInt32(value);
            return a * 2;
        }

        public object ConvertBack(object value, object parameter, BinarySerializationContext context)
        {
            int a = System.Convert.ToInt32(value);
            return a / 2;
        }
    }
}