using System;

using BinarySerialization.Attributes;

namespace BinarySerialization.Test
{
    public sealed class Toy : IEquatable<Toy>
    {
        public Toy()
        {
        }

        public Toy(string name, bool last = false)
        {
            Name = name;
            Last = last;
        }

        [FieldOrder(0)]
        public string Name { get; set; }

        [FieldOrder(1)]
        public bool Last { get; set; }

        public bool Equals(Toy other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Name, other.Name) && Last.Equals(other.Last);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Toy)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name?.GetHashCode() ?? 0) * 397) ^ Last.GetHashCode();
            }
        }
    }
}