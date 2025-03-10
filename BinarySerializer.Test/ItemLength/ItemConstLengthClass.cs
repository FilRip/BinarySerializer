using System.Collections.Generic;

using BinarySerialization.Attributes;

namespace BinarySerialization.Test.ItemLength
{
    public class ItemConstLengthClass
    {
        [ItemLength(3)]
        public List<string> List { get; set; }
    }
}