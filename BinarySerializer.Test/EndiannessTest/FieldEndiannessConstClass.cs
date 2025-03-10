using BinarySerialization.Attributes;
using BinarySerialization.Constants;

namespace BinarySerialization.Test.EndiannessTest
{
    public class FieldEndiannessConstClass
    {
        [FieldEndianness(Endianness.Big)]
        public int Value { get; set; }
    }
}
