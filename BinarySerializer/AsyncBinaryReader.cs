﻿using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using BinarySerialization.Streams;

namespace BinarySerialization;

public class AsyncBinaryReader(BoundedStream input, Encoding encoding) : BinaryReader(input, encoding)
{
    private readonly Encoding _encoding = encoding;

    public BoundedStream InputStream { get; } = input;

    public override byte ReadByte()
    {
        byte[] b = new byte[sizeof(byte)];
        Read(b, b.Length);
        return b[0];
    }

    public async Task<byte> ReadByteAsync(CancellationToken cancellationToken)
    {
        byte[] b = new byte[sizeof(byte)];
        await ReadAsync(b, b.Length, cancellationToken)
            .ConfigureAwait(false);
        return b[0];
    }

    public async Task<char> ReadCharAsync(CancellationToken cancellationToken)
    {
        Decoder decoder = _encoding.GetDecoder();

        int read;

        char[] chars = new char[1];

        do
        {
            byte b = await ReadByteAsync(cancellationToken);
            byte[] data = [b];

            read = decoder.GetChars(data, 0, data.Length, chars, 0, false);
        } while (read < chars.Length);

        return chars[0];
    }

    public override sbyte ReadSByte()
    {
        byte[] b = new byte[sizeof(sbyte)];
        Read(b, b.Length);
        return (sbyte)b[0];
    }

    public async Task<sbyte> ReadSByteAsync(CancellationToken cancellationToken)
    {
        byte[] b = new byte[sizeof(sbyte)];
        await ReadAsync(b, b.Length, cancellationToken)
            .ConfigureAwait(false);
        return (sbyte)b[0];
    }

    public override ushort ReadUInt16()
    {
        byte[] b = new byte[sizeof(ushort)];
        Read(b, b.Length);
        return BitConverter.ToUInt16(b, 0);
    }

    public async Task<ushort> ReadUInt16Async(CancellationToken cancellationToken)
    {
        byte[] b = new byte[sizeof(ushort)];
        await ReadAsync(b, b.Length, cancellationToken)
            .ConfigureAwait(false);
        return BitConverter.ToUInt16(b, 0);
    }

    public override short ReadInt16()
    {
        byte[] b = new byte[sizeof(short)];
        Read(b, b.Length);
        return BitConverter.ToInt16(b, 0);
    }

    public async Task<short> ReadInt16Async(CancellationToken cancellationToken)
    {
        byte[] b = new byte[sizeof(short)];
        await ReadAsync(b, b.Length, cancellationToken)
            .ConfigureAwait(false);
        return BitConverter.ToInt16(b, 0);
    }

    public override uint ReadUInt32()
    {
        byte[] b = new byte[sizeof(uint)];
        Read(b, b.Length);
        return BitConverter.ToUInt32(b, 0);
    }

    public async Task<uint> ReadUInt32Async(CancellationToken cancellationToken)
    {
        byte[] b = new byte[sizeof(uint)];
        await ReadAsync(b, b.Length, cancellationToken)
            .ConfigureAwait(false);
        return BitConverter.ToUInt32(b, 0);
    }

    public override int ReadInt32()
    {
        byte[] b = new byte[sizeof(int)];
        Read(b, b.Length);
        return BitConverter.ToInt32(b, 0);
    }

    public async Task<int> ReadInt32Async(CancellationToken cancellationToken)
    {
        byte[] b = new byte[sizeof(int)];
        await ReadAsync(b, b.Length, cancellationToken)
            .ConfigureAwait(false);
        return BitConverter.ToInt32(b, 0);
    }

    public override ulong ReadUInt64()
    {
        byte[] b = new byte[sizeof(ulong)];
        Read(b, b.Length);
        return BitConverter.ToUInt64(b, 0);
    }

    public async Task<ulong> ReadUInt64Async(CancellationToken cancellationToken)
    {
        byte[] b = new byte[sizeof(ulong)];
        await ReadAsync(b, b.Length, cancellationToken)
            .ConfigureAwait(false);
        return BitConverter.ToUInt64(b, 0);
    }

    public override long ReadInt64()
    {
        byte[] b = new byte[sizeof(long)];
        Read(b, b.Length);
        return BitConverter.ToInt64(b, 0);
    }

    public async Task<long> ReadInt64Async(CancellationToken cancellationToken)
    {
        byte[] b = new byte[sizeof(long)];
        await ReadAsync(b, b.Length, cancellationToken)
            .ConfigureAwait(false);
        return BitConverter.ToInt64(b, 0);
    }

    public override float ReadSingle()
    {
        byte[] b = new byte[sizeof(float)];
        Read(b, b.Length);
        return BitConverter.ToSingle(b, 0);
    }

    public async Task<float> ReadSingleAsync(CancellationToken cancellationToken)
    {
        byte[] b = new byte[sizeof(float)];
        await ReadAsync(b, b.Length, cancellationToken)
            .ConfigureAwait(false);
        return BitConverter.ToSingle(b, 0);
    }

    public override double ReadDouble()
    {
        byte[] b = new byte[sizeof(double)];
        Read(b, b.Length);
        return BitConverter.ToDouble(b, 0);
    }

    public async Task<double> ReadDoubleAsync(CancellationToken cancellationToken)
    {
        byte[] b = new byte[sizeof(double)];
        await ReadAsync(b, b.Length, cancellationToken)
            .ConfigureAwait(false);
        return BitConverter.ToDouble(b, 0);
    }

    public void Read(byte[] data, FieldLength fieldLength)
    {
        FieldLength length = fieldLength ?? data.Length;
        FieldLength readLength = InputStream.Read(data, length);

        if (readLength == 0)
        {
            throw new EndOfStreamException();
        }
    }

    public async Task ReadAsync(byte[] data, FieldLength fieldLength, CancellationToken cancellationToken)
    {
        FieldLength length = fieldLength ?? data.Length;
        FieldLength readLength = await InputStream.ReadAsync(data, length, cancellationToken);

        if (readLength == 0)
        {
            throw new EndOfStreamException();
        }
    }

    public async Task<byte[]> ReadBytesAsync(int count, CancellationToken cancellationToken)
    {
        byte[] b = new byte[count];
        _ = await BaseStream.ReadAsync(b, 0, b.Length, cancellationToken)
            .ConfigureAwait(false);
        return b;
    }
}