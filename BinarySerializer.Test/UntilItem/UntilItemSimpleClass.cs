using BinarySerialization.Attributes;

namespace BinarySerialization.Test.UntilItem
{
    public class UntilItemSimpleClass
    {
        [FieldOrder(0)]
        public EUntilItem Type { get; set; }

        [FieldOrder(1)]
        public byte Value { get; set; }
    }
}
