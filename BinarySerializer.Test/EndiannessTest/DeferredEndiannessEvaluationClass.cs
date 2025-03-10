using BinarySerialization.Attributes;

namespace BinarySerialization.Test.EndiannessTest
{
    public class DeferredEndiannessEvaluationClass
    {
        [FieldOrder(0)]
        [FieldEndianness(nameof(Endianness), typeof(EndiannessConverter))]
        public ushort Value { get; set; }

        [FieldOrder(1)]
        public uint Endianness { get; set; }
    }
}
