using BinarySerialization.Attributes;

namespace BinarySerialization.Test.Ignore
{
    public class IgnoreBindingClass
    {
        [Ignore()]
        public int Length { get; set; } = 4;

        [FieldLength("Length")]
        public string Value { get; set; }
    }
}
