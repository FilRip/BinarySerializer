using System;

using BinarySerialization.Interfaces;

namespace BinarySerialization.Attributes;

/// <summary>
///     Specifies an explicit list termination condition when the condition is defined external to the items in
///     the collection (e.g. a null-terminated list).
/// </summary>
/// <remarks>
///     Initializes a new instance of the <see cref="SerializeUntilAttribute" /> class with a terminating constValue.
/// </remarks>
/// <param name="constValue"></param>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public sealed class SerializeUntilAttribute(object constValue) : FieldBindingBaseAttribute, IConstAttribute
{

    /// <summary>
    ///     The terminating constValue.
    /// </summary>
    public object ConstValue { get; set; } = constValue;

    /// <summary>
    ///     Get constant value or null if not constant.
    /// </summary>
    public object GetConstValue()
    {
        return ConstValue;
    }
}