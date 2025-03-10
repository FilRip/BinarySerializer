using System;

namespace BinarySerialization.Attributes;

/// <summary>
///     Provides the <see cref="BinarySerializer" /> with information used to serialize the decorated member.
/// </summary>
/// <remarks>
///     Initializes a new instance of the <see cref="FieldOrderAttribute" /> class.
/// </remarks>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public sealed class FieldOrderAttribute(int order) : Attribute
{

    /// <summary>
    ///     The order to be observed when serializing or deserializing
    ///     this member compared to other members in the parent object.
    /// </summary>
    public int Order { get; set; } = order;
}