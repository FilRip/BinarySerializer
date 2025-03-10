using System.Collections.Generic;

using BinarySerialization.Attributes;

namespace BinarySerialization.Test.ItemLength
{
    public class LimitedItemLengthClassClass
    {
        [ItemLength(3)]
        public List<LimitedItemLengthClassInnerClass> InnerClasses { get; set; }
    }
}