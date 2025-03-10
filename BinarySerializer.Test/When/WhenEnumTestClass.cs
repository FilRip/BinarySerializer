using BinarySerialization.Attributes;

namespace BinarySerialization.Test.When
{
    public class WhenEnumTestClass
    {
        [FieldOrder(8)]
        public int WhatToDo { get; set; }

        [FieldOrder(9)]
        [SerializeWhen(nameof(WhatToDo), EWhen.WhenOne)]
        public int SerializeThis { get; set; }

        [FieldOrder(10)]
        [SerializeWhen(nameof(WhatToDo), EWhen.WhenTwo)]
        public int DontSerializeThis { get; set; }

        [FieldOrder(11)]
        [SerializeWhen(nameof(WhatToDo), EWhen.WhenOne)]
        [SerializeWhen(nameof(WhatToDo), EWhen.WhenTwo)]
        public int SerializeThisNoMatterWhat { get; set; }
    }
}