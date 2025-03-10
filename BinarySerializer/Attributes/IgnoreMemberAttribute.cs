using System;

namespace BinarySerialization.Attributes;

/// <summary>
///     Instructs <see cref="BinarySerializer" /> not to serialize or deserialize the specified public field or property value.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
public sealed class IgnoreMemberAttribute(string name) : Attribute
{
    public string Name { get; } = name;
}