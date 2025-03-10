using System;

namespace BinarySerialization.Attributes;

/// <summary>
///     Used as the abstract base for deriving field value attributes.
/// </summary>
/// <remarks>
///     Initializes a new instance of the FieldValue class with a path pointing to a binding source member.
///     <param name="valuePath">A path to the source member.</param>
/// </remarks>
#pragma warning disable S3376 // Attribute, EventArgs, and Exception type names should end with the type being extended
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
public abstract class FieldValueAttributeBase(string valuePath) : FieldBindingBaseAttribute(valuePath)
#pragma warning restore S3376 // Attribute, EventArgs, and Exception type names should end with the type being extended
{

    /// <summary>
    ///     Override to indicate to the framework the expected input block size for this attribute.
    /// </summary>
    public virtual int BlockSize => 81920;

    internal virtual object GetInitialStateInternal(BinarySerializationContext context)
    {
        return GetInitialState(context);
    }

    internal object GetUpdatedStateInternal(object state, byte[] buffer, int offset, int count)
    {
        return GetUpdatedState(state, buffer, offset, count);
    }

    internal object GetFinalValueInternal(object state)
    {
        return GetFinalValue(state);
    }

    /// <summary>
    ///     This is called by the framework to indicate a new operation.
    /// </summary>
    /// <param name="context"></param>
    protected abstract object GetInitialState(BinarySerializationContext context);

    /// <summary>
    ///     This is called one or more times by the framework to add data to the computation.
    /// </summary>
    /// <param name="state"></param>
    /// <param name="buffer"></param>
    /// <param name="offset"></param>
    /// <param name="count"></param>
    protected abstract object GetUpdatedState(object state, byte[] buffer, int offset, int count);

    /// <summary>
    ///     This is called by the framework to retrieve the final value from computation.
    /// </summary>
    /// <returns></returns>
    protected abstract object GetFinalValue(object state);
}