using System;

using BinarySerialization.Constants;

namespace BinarySerialization.Attributes;

/// <summary>
///     Used to control conditional serialization of members.
/// </summary>
/// <remarks>
///     Initializes a new instance of the <see cref="SerializeWhenAttribute" />.
/// </remarks>
/// <param name="valuePath">The path to the binding source.</param>
/// <param name="value">The value to be used in determining if the condition is true.</param>
/// <param name="operator">The comparison operator used to determine serialization eligibility.</param>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
public class SerializeWhenAttribute(string valuePath, object value, ComparisonOperator @operator = ComparisonOperator.Equal) : FieldBindingBaseAttribute(valuePath)
{

    /// <summary>
    ///     The value to be used in determining if the condition is true.
    /// </summary>
    public object Value { get; set; } = value;

    public ComparisonOperator Operator { get; } = @operator;
}