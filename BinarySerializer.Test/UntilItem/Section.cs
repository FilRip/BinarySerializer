using System.Collections.Generic;

using BinarySerialization.Attributes;
using BinarySerialization.Constants;

namespace BinarySerialization.Test.UntilItem
{
    public class Section
    {
        [FieldOrder(0)]
        public UntilItemSimpleClass Header { get; set; }

        [FieldOrder(1)]
        [ItemSerializeUntil(nameof(UntilItemSimpleClass.Type), EUntilItem.Header, LastItemMode = LastItemMode.Defer)]
        public List<UntilItemSimpleClass> Items { get; set; }
    }
}
