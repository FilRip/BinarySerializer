namespace BinarySerialization.CustomEventArgs;

/// <summary>
///     Provides data for the <see cref="BinarySerializer.MemberSerialized" /> and
///     <see cref="BinarySerializer.MemberDeserialized" /> events.
/// </summary>
/// <remarks>
///     Initializes a new instance of the MemberSerializedEventArgs class with the member name and value.
/// </remarks>
/// <param name="memberName">The name of the member.</param>
/// <param name="value">The value of the member.</param>
/// <param name="context">The current serialization context.</param>
/// <param name="offset">The current offset in the stream relative to the start of the overall operation.</param>
/// <param name="localOffset">The current object-local offset in the stream.</param>
public class MemberSerializedEventArgs(string memberName, object value, BinarySerializationContext context,
    FieldLength offset, FieldLength localOffset) : MemberSerializingEventArgs(memberName, context, offset, localOffset)
{

    /// <summary>
    ///     The member value.
    /// </summary>
    public object Value { get; } = value;
}
