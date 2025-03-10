using System.Collections.Generic;

using BinarySerialization.Attributes;

namespace BinarySerialization.Test.Count
{
    public class PaddedConstSizedListClass
    {
        [FieldCount(6)]
        public List<PaddedConstSizeListItemClass> Items { get; set; }
    }
}
