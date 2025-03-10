using System;

namespace BinarySerialization.CustomEventArgs;

/// <summary>
///     Provides data for the <see cref="BinarySerializer.MemberSerializing" /> and
///     <see cref="BinarySerializer.MemberDeserializing" /> events.
/// </summary>
/// <remarks>
///     Initializes a new instance of the MemberSerializingEventArgs class with the member name.
/// </remarks>
/// <param name="memberName">The name of the member.</param>
/// <param name="context">The current serialization context.</param>
/// <param name="offset">The current offset in the stream relative to the start of the overall operation.</param>
/// <param name="localOffset">The current object-local offset in the stream.</param>
public class MemberSerializingEventArgs(string memberName, BinarySerializationContext context, FieldLength offset, FieldLength localOffset) : EventArgs
{

    /// <summary>
    ///     The name of the member.
    /// </summary>
    public string MemberName { get; } = memberName;

    /// <summary>
    ///     The current serialization context.
    /// </summary>
    public BinarySerializationContext Context { get; } = context;

    /// <summary>
    ///     The global location in the stream relative to the initial operation.
    /// </summary>
    public FieldLength Offset { get; } = offset;

    /// <summary>
    /// The object-local location in the stream relative to the parent object.
    /// </summary>
    public FieldLength LocalOffset { get; } = localOffset;
}