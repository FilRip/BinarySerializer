﻿namespace BinarySerialization.Test.Value
{
    public class FieldCrc32Class
    {
        [FieldOrder(0)]
        public int Length { get; set; }

        [FieldOrder(1)]
        [FieldLength("Length")]
        [FieldCrc32("Crc")]
        public FieldCrcInternalClass Internal { get; set; }

        [FieldOrder(2)]
        public uint Crc { get; set; }
    }
}