using BinarySerialization.Attributes;

namespace BinarySerialization.Test.WhenNot
{
    public class WhenNotClass
    {
        [FieldOrder(0)]
        public bool ExcludeValue { get; set; }

        [FieldOrder(1)]
        [SerializeWhen(nameof(ExcludeValue), true, Constants.ComparisonOperator.NotEqual)]
        public int Value { get; set; }
    }
}
