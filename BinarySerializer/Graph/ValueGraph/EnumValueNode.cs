﻿using System;
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
        var enumInfo = GetEnumInfo();
        var value = GetEnumValue(enumInfo);
        Serialize(stream, value, enumInfo.SerializedType, enumInfo.EnumValueLength);
    }

    private object GetEnumValue(EnumInfo enumInfo)
    {
        var value = enumInfo.EnumValues != null ? enumInfo.EnumValues[(Enum)BoundValue] : BoundValue;
        return value;
    }

    internal override void DeserializeOverride(BoundedStream stream, SerializationOptions options, EventShuttle eventShuttle)
    {
        var enumInfo = GetEnumInfo();
        Deserialize(stream, enumInfo.SerializedType, enumInfo.EnumValueLength);
        SetValueFromEnum();
    }

    internal override Task SerializeOverrideAsync(BoundedStream stream, EventShuttle eventShuttle, CancellationToken cancellationToken)
    {
        var enumInfo = GetEnumInfo();
        var value = GetEnumValue(enumInfo);
        return SerializeAsync(stream, value, enumInfo.SerializedType, enumInfo.EnumValueLength, cancellationToken);
    }

    internal override async Task DeserializeOverrideAsync(BoundedStream stream, SerializationOptions options, EventShuttle eventShuttle,
        CancellationToken cancellationToken)
    {
        var enumInfo = GetEnumInfo();
        await DeserializeAsync(stream, enumInfo.SerializedType, enumInfo.EnumValueLength, cancellationToken)
            .ConfigureAwait(false);
        SetValueFromEnum();
    }

    private EnumInfo GetEnumInfo()
    {
        var localTypeNode = (EnumTypeNode)TypeNode;
        var enumInfo = localTypeNode.EnumInfo;
        return enumInfo;
    }

    private void SetValueFromEnum()
    {
        var enumInfo = GetEnumInfo();
        var value = GetValue(enumInfo.SerializedType);

        if (enumInfo.ValueEnums != null)
        {
            var stringValue = (string)value;
            value = enumInfo.ValueEnums[stringValue];
        }

        var underlyingValue = value.ConvertTo(enumInfo.UnderlyingType);

        Value = Enum.ToObject(TypeNode.BaseSerializedType, underlyingValue);
    }
}