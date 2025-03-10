using System.IO;
using System.Linq;

using BinarySerialization.Constants;
using BinarySerialization.Interfaces;

namespace BinarySerialization.Test.ItemSubtype
{
    public class CustomItem : IItemSubtype, IBinarySerializable
    {
        internal static readonly byte[] Data = System.Text.Encoding.ASCII.GetBytes("hello");

        public void Serialize(Stream stream, Endianness endianness, BinarySerializationContext serializationContext)
        {
            stream.Write(Data, 0, Data.Length);
        }

        public void Deserialize(Stream stream, Endianness endianness, BinarySerializationContext serializationContext)
        {
            var data = new byte[Data.Length];
            stream.Read(data, 0, data.Length);

            if (!data.SequenceEqual(Data))
                throw new InvalidDataException();
        }
    }
}
