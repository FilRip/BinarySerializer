﻿using System;

namespace BinarySerialization.Attributes;

/// <summary>
///     Specifies a value binding for a member or object sub-graph.
/// </summary>
/// <remarks>
///     Initializes a new instance of the FieldValue class.
/// </remarks>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
public sealed class FieldValueAttribute(string valuePath) : FieldValueAttributeBase(valuePath)
{

    /// <summary>
    ///     This is called by the framework to indicate a new operation.
    /// </summary>
    /// <param name="context"></param>
    protected override object GetInitialState(BinarySerializationContext context)
    {
        return context.Value;
    }

    /// <summary>
    ///     This is called one or more times by the framework to add data to the computation.
    /// </summary>
    /// <param name="state"></param>
    /// <param name="buffer"></param>
    /// <param name="offset"></param>
    /// <param name="count"></param>
    protected override object GetUpdatedState(object state, byte[] buffer, int offset, int count)
    {
        return state;
    }

    /// <summary>
    ///     This is called by the framework to retrieve the final value from computation.
    /// </summary>
    /// <returns></returns>
    protected override object GetFinalValue(object state)
    {
        return state;
    }
}