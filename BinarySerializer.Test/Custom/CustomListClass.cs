using System.Collections.Generic;
using System.IO;

using BinarySerialization.Constants;
using BinarySerialization.Interfaces;

namespace BinarySerialization.Test.Custom
{
    public class CustomListClass : List<string>, IBinarySerializable
    {
        public void Serialize(Stream stream, Endianness endianness, BinarySerializationContext serializationContext)
        {
            foreach (string item in this)
            {
                byte[] data = System.Text.Encoding.UTF8.GetBytes(item);
                stream.WriteByte((byte)data.Length);
                stream.Write(data, 0, data.Length);
            }
        }

        public void Deserialize(Stream stream, Endianness endianness, BinarySerializationContext serializationContext)
        {
            while (stream.Position < stream.Length)
            {
                int length = stream.ReadByte();
                byte[] data = new byte[length];
                stream.Read(data, 0, data.Length);
                string item = System.Text.Encoding.UTF8.GetString(data);
                Add(item);
            }
        }
    }
}
