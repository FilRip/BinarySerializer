using System;

using BinarySerialization.Attributes;

namespace BinarySerialization.Test.Misc
{
    public sealed class ImmutableClass2 : IEquatable<ImmutableClass2>
    {
        public static readonly ImmutableClass2 Broadcast = new(0xFFFF);
        public static readonly ImmutableClass2 CoordinatorAddress = new(0);

        public ImmutableClass2()
        {
        }

        public ImmutableClass2(ulong value)
        {
            Value = value;
        }

        public ImmutableClass2(uint high, uint low)
        {
            High = high;
            Low = low;
        }

        public ulong Value
        {
            get => ((ulong)High << 32) + Low;

            set
            {
                High = (uint)((value & 0xFFFFFFFF00000000UL) >> 32);
                Low = (uint)(value & 0x00000000FFFFFFFFUL);
            }
        }

        [Ignore()]
        public uint High { get; set; }

        [Ignore()]
        public uint Low { get; set; }

        public bool Equals(ImmutableClass2 other)
        {
            return Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            if (obj is ImmutableClass2 ic)
                return this.Equals(ic);
            else
                return false;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value.ToString("X16");
        }
    }
}