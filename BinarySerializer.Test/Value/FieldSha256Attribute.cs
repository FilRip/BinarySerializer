using System;
using System.Security.Cryptography;

using BinarySerialization.Attributes;

namespace BinarySerialization.Test.Value
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public class FieldSha256Attribute(string valuePath) : FieldValueAttributeBase(valuePath)
    {
        protected override object GetInitialState(BinarySerializationContext context)
        {
            return IncrementalHash.CreateHash(HashAlgorithmName.SHA256);
        }

        protected override object GetUpdatedState(object state, byte[] buffer, int offset, int count)
        {
            var sha = (IncrementalHash)state;
            sha.AppendData(buffer, offset, count);
            return sha;
        }

        protected override object GetFinalValue(object state)
        {
            var sha = (IncrementalHash)state;
            return sha.GetHashAndReset();
        }
    }
}