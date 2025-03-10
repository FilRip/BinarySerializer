using System.Collections.Generic;

namespace BinarySerialization.Crc;

internal abstract class Crc<T> : CrcBase
{
    private static readonly Dictionary<T, T[]> Tables = [];

    private readonly T _initialValue;
    private readonly T[] _table;
    private T _crc;

    protected Crc(T polynomial, T initialValue)
    {
        _initialValue = initialValue;

        Reset();

        lock (TableLock)
        {
            if (Tables.TryGetValue(polynomial, out _table))
            {
                return;
            }

            _table = BuildTable(polynomial);

            Tables.Add(polynomial, _table);
        }
    }

    public bool IsDataReflected { get; set; }
    public bool IsRemainderReflected { get; set; }
    public T FinalXor { get; set; }

    protected abstract int Width { get; }

    public void Reset()
    {
        _crc = _initialValue;
    }

    public void Compute(byte[] buffer, int offset, int count)
    {
        uint remainder = ToUInt32(_crc);

        for (int i = offset; i < count; i++)
        {
            byte b = buffer[i];

            if (IsDataReflected)
            {
                b = (byte)Reflect(b, BitsPerByte);
            }

            byte data = (byte)(b ^ remainder >> Width - BitsPerByte);

            remainder = ToUInt32(_table[data]) ^ remainder << BitsPerByte;
        }

        _crc = FromUInt32(remainder);
    }


    public T ComputeFinal()
    {
        T crc = _crc;

        if (IsRemainderReflected)
        {
            crc = FromUInt32(Reflect(ToUInt32(crc), Width));
        }

        return FromUInt32(ToUInt32(crc) ^ ToUInt32(FinalXor));
    }


    protected abstract uint ToUInt32(T value);
    protected abstract T FromUInt32(uint value);

    private T[] BuildTable(T polynomial)
    {
        T[] table = new T[TableSize];

        uint poly = ToUInt32(polynomial);

        int padWidth = Width - BitsPerByte;

        int topBit = 1 << Width - 1;

        // Compute the remainder of each possible dividend.
        for (uint dividend = 0; dividend < table.Length; dividend++)
        {
            // Start with the dividend followed by zeros.
            uint remainder = dividend << padWidth;

            // Perform modulo-2 division, a bit at a time.
            for (uint bit = BitsPerByte; bit > 0; bit--)
            {
                // Try to divide the current data bit.
                if ((remainder & topBit) != 0)
                {
                    remainder = remainder << 1 ^ poly;
                }
                else
                {
                    remainder <<= 1;
                }
            }

            // Store the result into the table.
            table[dividend] = FromUInt32(remainder);
        }

        return table;
    }

    private static uint Reflect(uint value, int bitCount)
    {
        uint reflection = 0;

        // Reflect the data about the center bit.
        for (int bit = 0; bit < bitCount; bit++)
        {
            // If the LSB bit is set, set the reflection of it.
            if ((value & 0x01) != 0)
            {
                reflection |= (uint)(1 << bitCount - 1 - bit);
            }

            value >>= 1;
        }

        return reflection;
    }
}