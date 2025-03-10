using System;

namespace BinarySerialization.Attributes;

/// <summary>
///     Used in conjunction with one or more ItemSubtype attributes to specify the default type to use during
///     deserialization.
/// </summary>
/// <remarks>
///     Initializes a new instance of <see cref="SubtypeDefaultAttribute" />.
/// </remarks>
/// <param name="subtype">The default subtype.</param>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public sealed class ItemSubtypeDefaultAttribute(Type subtype) : SubtypeDefaultBaseAttribute(subtype)
{
}