using System.Collections.Generic;

namespace BinarySerialization.Test.Misc
{
    public class DictionaryMemberClass
    {
        public DictionaryMemberClass()
        {
            Field = [];
        }

        public Dictionary<string, string> Field { get; set; }
    }
}