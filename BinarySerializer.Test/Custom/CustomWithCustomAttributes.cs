﻿using System.IO;
using System.Linq;
using System.Reflection;

using BinarySerialization.Constants;
using BinarySerialization.Interfaces;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinarySerialization.Test.Custom
{
    public class CustomWithCustomAttributes : IBinarySerializable
    {
        public void Serialize(Stream stream, Endianness endianness, BinarySerializationContext serializationContext)
        {
            var memberInfo = serializationContext.MemberInfo;
            AssertCustomAttribute(memberInfo);
        }

#pragma warning disable S4144 // Methods should not have identical implementations
        public void Deserialize(Stream stream, Endianness endianness, BinarySerializationContext serializationContext)
#pragma warning restore S4144 // Methods should not have identical implementations
        {
            var memberInfo = serializationContext.MemberInfo;
            AssertCustomAttribute(memberInfo);
        }

        private static void AssertCustomAttribute(MemberInfo memberInfo)
        {
            var attributes = memberInfo.CustomAttributes;
            var customAttribute = attributes.Single();
            Assert.AreEqual(typeof(CustomAttribute), customAttribute.AttributeType);
        }
    }
}
