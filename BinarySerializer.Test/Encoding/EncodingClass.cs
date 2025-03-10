using BinarySerialization.Attributes;
using BinarySerialization.Constants;

namespace BinarySerialization.Test.Encoding
{
    public class EncodingClass
    {
        [FieldEncoding("windows-1256")]
        [SerializeAs(SerializedType.TerminatedString)]
        public string Name { get; set; }
    }
}