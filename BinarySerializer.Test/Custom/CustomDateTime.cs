using System;
using System.IO;

using BinarySerialization.Attributes;
using BinarySerialization.Constants;
using BinarySerialization.Interfaces;

namespace BinarySerialization.Test.Custom
{
    public class CustomDateTime : IBinarySerializable
    {
        [Ignore()]
        public DateTime Value;

        public void Serialize(Stream stream, Endianness endianness, BinarySerializationContext serializationContext)
        {
            BinaryWriter writer = new(stream);
            writer.Write((ushort)Value.Year);
            writer.Write((byte)Value.Month);
            writer.Write((byte)Value.Day);
            writer.Write((byte)Value.Hour);
            writer.Write((byte)Value.Minute);
            writer.Write((byte)Value.Second);
            writer.Write((ushort)Value.Millisecond);
        }

        public void Deserialize(Stream stream, Endianness endianness, BinarySerializationContext serializationContext)
        {
            BinaryReader reader = new(stream);
            ushort year = reader.ReadUInt16();
            byte month = reader.ReadByte();
            byte day = reader.ReadByte();
            byte hour = reader.ReadByte();
            byte minute = reader.ReadByte();
            byte second = reader.ReadByte();
            ushort ms = reader.ReadUInt16();

            Value = new DateTime(year, month, day, hour, minute, second, DateTimeKind.Local).AddMilliseconds(ms);
        }
    }
}
