using BinarySerialization.Attributes;
using BinarySerialization.Constants;

namespace BinarySerialization.Test.Enums
{
    internal class BaseTypeEnumAsStringClass
    {
        [SerializeAs(SerializedType.TerminatedString)]
        public BaseTypeEnumValues Field { get; set; }
    }
}