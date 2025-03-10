using System;

namespace BinarySerialization.Attributes;

/// <summary>
///     Used in conjunction with one or more Subtype attributes to specify the default type to use during deserialization.
/// </summary>
/// <remarks>
///     Initializes a new instance of <see cref="SubtypeDefaultAttribute" />.
/// </remarks>
/// <param name="subtype">The default subtype.</param>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class SubtypeDefaultAttribute(Type subtype) : SubtypeDefaultBaseAttribute(subtype)
{
}