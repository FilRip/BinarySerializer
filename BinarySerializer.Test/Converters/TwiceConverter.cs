using BinarySerialization.Interfaces;

namespace BinarySerialization.Test.Converters
{
    public class TwiceConverter : IValueConverter
    {
        public object Convert(object value, object parameter, BinarySerializationContext context)
        {
            var a = System.Convert.ToDouble(value);
            return a * 2;
        }

        public object ConvertBack(object value, object parameter, BinarySerializationContext context)
        {
            var a = System.Convert.ToDouble(value);
            return a / 2;
        }
    }
}
