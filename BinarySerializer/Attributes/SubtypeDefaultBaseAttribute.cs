using System;

namespace BinarySerialization.Attributes;

/// <summary>
///     Used in conjunction with one or more Subtype attributes to specify the default type to use during deserialization.
/// </summary>
/// <remarks>
///     Initializes a new instance of <see cref="SubtypeDefaultBaseAttribute" />.
/// </remarks>
/// <param name="subtype">The default subtype.</param>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public abstract class SubtypeDefaultBaseAttribute(Type subtype) : Attribute
{

    /// <summary>
    ///     The default subtype.  This type must be assignable to the field type.
    /// </summary>
    public Type Subtype { get; } = subtype;
}