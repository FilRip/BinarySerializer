using BinarySerialization.Attributes;

namespace BinarySerialization.Test.Enums
{
    public enum NamedEnumValues
    {
        [SerializeAsEnum("Alpha")] A,
        [SerializeAsEnum("Bravo")] B,
        [SerializeAsEnum] C
    }
}