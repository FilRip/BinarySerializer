﻿using System.IO;

using BinarySerialization.Constants;
using BinarySerialization.Interfaces;
using BinarySerialization.Streams;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinarySerialization.Test.Custom
{
    public class CustomSubtypeCustomClass : CustomSubtypeBaseClass, IBinarySerializable
    {
        [BinarySerialization.Attributes.Ignore()]
        public uint Value { get; set; }

        public void Serialize(Stream stream, Endianness endianness,
            BinarySerializationContext serializationContext)
        {
            var boundedStream = (BoundedStream)stream;
            Assert.AreEqual(0, boundedStream.Position);
            Assert.AreEqual(100, (int)boundedStream.MaxLength.ByteCount);

            var varuint = new Varuint { Value = Value };
            varuint.Serialize(stream, endianness, serializationContext);
        }

        public void Deserialize(Stream stream, Endianness endianness,
            BinarySerializationContext serializationContext)
        {
            var varuint = new Varuint { Value = Value };
            varuint.Deserialize(stream, endianness, serializationContext);
            Value = varuint.Value;
        }
    }
}