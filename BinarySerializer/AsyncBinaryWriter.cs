using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using BinarySerialization.Streams;

namespace BinarySerialization;

internal class AsyncBinaryWriter(BoundedStream output, Encoding encoding, byte paddingValue) : BinaryWriter(output, encoding)
{
    private readonly Encoding _encoding = encoding;
    private readonly byte _paddingValue = paddingValue;

    public BoundedStream OutputStream { get; } = output;

    public Task WriteAsync(byte value, FieldLength fieldLength, CancellationToken cancellationToken)
    {
        byte[] data = [value];
        return WriteAsync(data, fieldLength, cancellationToken);
    }

    public Task WriteAsync(char value, FieldLength fieldLength, CancellationToken cancellationToken)
    {
        byte[] data = _encoding.GetBytes([value]);
        return WriteAsync(data, fieldLength, cancellationToken);
    }

    public Task WriteAsync(sbyte value, FieldLength fieldLength, CancellationToken cancellationToken)
    {
        byte[] data = [(byte)value];
        return WriteAsync(data, fieldLength, cancellationToken);
    }

    public Task WriteAsync(short value, FieldLength fieldLength, CancellationToken cancellationToken)
    {
        byte[] data = BitConverter.GetBytes(value);
        return WriteAsync(data, fieldLength, cancellationToken);
    }

    public Task WriteAsync(ushort value, FieldLength fieldLength, CancellationToken cancellationToken)
    {
        byte[] data = BitConverter.GetBytes(value);
        return WriteAsync(data, fieldLength, cancellationToken);
    }

    public Task WriteAsync(int value, FieldLength fieldLength, CancellationToken cancellationToken)
    {
        byte[] data = BitConverter.GetBytes(value);
        return WriteAsync(data, fieldLength, cancellationToken);
    }

    public Task WriteAsync(uint value, FieldLength fieldLength, CancellationToken cancellationToken)
    {
        byte[] data = BitConverter.GetBytes(value);
        return WriteAsync(data, fieldLength, cancellationToken);
    }

    public Task WriteAsync(long value, FieldLength fieldLength, CancellationToken cancellationToken)
    {
        byte[] data = BitConverter.GetBytes(value);
        return WriteAsync(data, fieldLength, cancellationToken);
    }

    public Task WriteAsync(ulong value, FieldLength fieldLength, CancellationToken cancellationToken)
    {
        byte[] data = BitConverter.GetBytes(value);
        return WriteAsync(data, fieldLength, cancellationToken);
    }

    public Task WriteAsync(float value, FieldLength fieldLength, CancellationToken cancellationToken)
    {
        byte[] data = BitConverter.GetBytes(value);
        return WriteAsync(data, fieldLength, cancellationToken);
    }

    public Task WriteAsync(double value, FieldLength fieldLength, CancellationToken cancellationToken)
    {
        byte[] data = BitConverter.GetBytes(value);
        return WriteAsync(data, fieldLength, cancellationToken);
    }

    public Task WriteAsync(byte[] data, FieldLength fieldLength, CancellationToken cancellationToken)
    {
        Resize(ref data, fieldLength);

        FieldLength length = fieldLength ?? data.Length;
        return OutputStream.WriteAsync(data, length, cancellationToken);
    }

    public void Write(byte value, FieldLength fieldLength)
    {
        byte[] data = [value];
        Write(data, fieldLength);
    }

    public void Write(sbyte value, FieldLength fieldLength)
    {
        byte[] data = [(byte)value];
        Write(data, fieldLength);
    }

    public void Write(short value, FieldLength fieldLength)
    {
        byte[] data = BitConverter.GetBytes(value);
        Write(data, fieldLength);
    }

    public void Write(ushort value, FieldLength fieldLength)
    {
        byte[] data = BitConverter.GetBytes(value);
        Write(data, fieldLength);
    }

    public void Write(int value, FieldLength fieldLength)
    {
        byte[] data = BitConverter.GetBytes(value);
        Write(data, fieldLength);
    }

    public void Write(uint value, FieldLength fieldLength)
    {
        byte[] data = BitConverter.GetBytes(value);
        Write(data, fieldLength);
    }

    public void Write(long value, FieldLength fieldLength)
    {
        byte[] data = BitConverter.GetBytes(value);
        Write(data, fieldLength);
    }

    public void Write(ulong value, FieldLength fieldLength)
    {
        byte[] data = BitConverter.GetBytes(value);
        Write(data, fieldLength);
    }

    public void Write(float value, FieldLength fieldLength)
    {
        byte[] data = BitConverter.GetBytes(value);
        Write(data, fieldLength);
    }

    public void Write(double value, FieldLength fieldLength)
    {
        byte[] data = BitConverter.GetBytes(value);
        Write(data, fieldLength);
    }

    public void Write(byte[] data, FieldLength fieldLength)
    {
        Resize(ref data, fieldLength);

        FieldLength length = fieldLength ?? data.Length;
        OutputStream.Write(data, length);
    }

    private void Resize(ref byte[] data, FieldLength length)
    {
        if (length == null)
        {
            return;
        }

        int dataLength = data.Length;
        int totalByteCount = (int)length.TotalByteCount;

        if (dataLength == totalByteCount)
        {
            return;
        }

        Array.Resize(ref data, totalByteCount);

        if (_paddingValue == default)
        {
            return;
        }

        for (int i = dataLength; i < totalByteCount; i++)
        {
            data[i] = _paddingValue;
        }
    }
}
