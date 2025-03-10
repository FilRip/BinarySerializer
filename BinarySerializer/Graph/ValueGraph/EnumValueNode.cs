using System;
using System.Threading;
using System.Threading.Tasks;

using BinarySerialization.Graph.TypeGraph;
using BinarySerialization.Helpers;
using BinarySerialization.Streams;

namespace BinarySerialization.Graph.ValueGraph;

internal class EnumValueNode(ValueNode parent, string name, TypeNode typeNode) : ValueValueNode(parent, name, typeNode)
{
    internal override void SerializeOverride(BoundedStream stream, EventShuttle eventShuttle)
    {
        EnumInfo enumInfo = GetEnumInfo();
        object value = GetEnumValue(enumInfo);
        Serialize(stream, value, enumInfo.SerializedType, enumInfo.EnumValueLength);
    }

    private object GetEnumValue(EnumInfo enumInfo)
    {
        object value = enumInfo.EnumValues != null ? enumInfo.EnumValues[(Enum)BoundValue] : BoundValue;
        return value;
    }

    internal override void DeserializeOverride(BoundedStream stream, SerializationOptions options, EventShuttle eventShuttle)
    {
        EnumInfo enumInfo = GetEnumInfo();
        Deserialize(stream, enumInfo.SerializedType, enumInfo.EnumValueLength);
        SetValueFromEnum();
    }

    internal override Task SerializeOverrideAsync(BoundedStream stream, EventShuttle eventShuttle, CancellationToken cancellationToken)
    {
        EnumInfo enumInfo = GetEnumInfo();
        object value = GetEnumValue(enumInfo);
        return SerializeAsync(stream, value, enumInfo.SerializedType, enumInfo.EnumValueLength, cancellationToken);
    }

    internal override async Task DeserializeOverrideAsync(BoundedStream stream, SerializationOptions options, EventShuttle eventShuttle,
        CancellationToken cancellationToken)
    {
        EnumInfo enumInfo = GetEnumInfo();
        await DeserializeAsync(stream, enumInfo.SerializedType, enumInfo.EnumValueLength, cancellationToken)
            .ConfigureAwait(false);
        SetValueFromEnum();
    }

    private EnumInfo GetEnumInfo()
    {
        EnumTypeNode localTypeNode = (EnumTypeNode)TypeNode;
        EnumInfo enumInfo = localTypeNode.EnumInfo;
        return enumInfo;
    }

    private void SetValueFromEnum()
    {
        EnumInfo enumInfo = GetEnumInfo();
        object value = GetValue(enumInfo.SerializedType);

        if (enumInfo.ValueEnums != null)
        {
            string stringValue = (string)value;
            value = enumInfo.ValueEnums[stringValue];
        }

        object underlyingValue = value.ConvertTo(enumInfo.UnderlyingType);

        Value = Enum.ToObject(TypeNode.BaseSerializedType, underlyingValue);
    }
}