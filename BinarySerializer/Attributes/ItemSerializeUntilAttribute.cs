using System;

using BinarySerialization.Constants;
using BinarySerialization.Interfaces;

namespace BinarySerialization.Attributes;

/// <summary>
///     Specifies an explicit list termination condition when it is defined as part of an item in the collection.
/// </summary>
/// <remarks>
///     Initializes a new instance of the <see cref="ItemSerializeUntilAttribute" /> class with a
///     path to the member within the item to be used as a termination condition.
/// </remarks>
/// <param name="itemValuePath">
///     The path to the member within the item to be used as a
///     termination condition.
/// </param>
/// <param name="constValue">The value to use in the termination comparison.</param>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public sealed class ItemSerializeUntilAttribute(string itemValuePath, object constValue = null) : FieldBindingBaseAttribute, IConstAttribute
{

    /// <summary>
    ///     Gets or sets the path to the binding source property inside the child item.
    /// </summary>
    public string ItemValuePath { get; set; } = itemValuePath;

    /// <summary>
    ///     The value to use in the termination comparison.  If the item value referenced in the value path
    ///     matches this value, serialization of the collection will be terminated.
    /// </summary>
    public object ConstValue { get; set; } = constValue;

    /// <summary>
    ///     Used to specify whether the terminating item should be included in the collection, discarded,
    ///     or whether processing of the underlying data should be deferred.
    /// </summary>
    public LastItemMode LastItemMode { get; set; }

    /// <summary>
    ///     Get constant value or null if not constant.
    /// </summary>
    public object GetConstValue()
    {
        return ConstValue;
    }
}