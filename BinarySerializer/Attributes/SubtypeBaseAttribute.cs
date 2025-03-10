using System;

namespace BinarySerialization.Attributes;

/// <summary>
///     Used to specify multiple possible derived types.
/// </summary>
/// <remarks>
///     Initializes a new instance of <see cref="SubtypeBaseAttribute" />.
/// </remarks>
/// <param name="valuePath">The path to the binding source.</param>
/// <param name="value">The value to be used in determining if the subtype should be used.</param>
/// <param name="subtype">The subtype to be used.</param>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
public abstract class SubtypeBaseAttribute(string valuePath, object value, Type subtype) : FieldBindingBaseAttribute(valuePath)
{

    /// <summary>
    ///     The value that defines the subtype mapping.
    /// </summary>
    public object Value { get; } = value;

    /// <summary>
    ///     The subtype.
    /// </summary>
    public Type Subtype { get; } = subtype;
}