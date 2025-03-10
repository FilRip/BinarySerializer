using System;

using BinarySerialization.Constants;

namespace BinarySerialization.Attributes;

/// <summary>
///     Specifies an 8-bit checksum for a member or object sub-graph, defined as the 2's compliment of the addition of all
///     bytes.
/// </summary>
/// <remarks>
///     Initializes a new instance of the FieldChecksum class.
/// </remarks>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
public sealed class FieldChecksumAttribute(string checksumPath) : FieldValueAttributeBase(checksumPath)
{

    /// <summary>
    ///     The mode used to compute the checksum.
    /// </summary>
    public ChecksumMode Mode { get; set; }

    /// <summary>
    ///     This is called by the framework to indicate a new operation.
    /// </summary>
    /// <param name="context"></param>
    protected override object GetInitialState(BinarySerializationContext context)
    {
        return default(byte);
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
        byte checksum = (byte)state;

        if (Mode == ChecksumMode.Xor)
        {
            for (int i = offset; i < count; i++)
            {
                checksum ^= buffer[i];
            }
        }
        else
        {
            for (int i = offset; i < count; i++)
            {
                checksum = (byte)(checksum + buffer[i]);
            }
        }

        return checksum;
    }

    /// <summary>
    ///     This is called by the framework to retrieve the final value from computation.
    /// </summary>
    /// <returns></returns>
    protected override object GetFinalValue(object state)
    {
        byte checksum = (byte)state;

        return Mode switch
        {
            ChecksumMode.TwosComplement => (byte)(0x100 - checksum),
            ChecksumMode.Modulo256 => (byte)(checksum % 256),
            ChecksumMode.Xor => (object)checksum,
            _ => throw new ArgumentException(nameof(Mode)),
        };
    }
}