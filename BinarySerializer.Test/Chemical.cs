using BinarySerialization.Attributes;
using BinarySerialization.Constants;

namespace BinarySerialization.Test
{
    public abstract class Chemical(string formula)
    {
        [SerializeAs(SerializedType.TerminatedString)]
        public string Formula { get; set; } = formula;
    }
}