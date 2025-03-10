using System;

namespace BinarySerialization.Crc;

internal abstract class CrcBase
{
    protected const int BitsPerByte = 8;

    protected static readonly int TableSize = (int)Math.Pow(2, BitsPerByte);
    protected static readonly object TableLock = new();
}
