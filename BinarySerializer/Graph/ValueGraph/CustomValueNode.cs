using System;
using System.Threading;
using System.Threading.Tasks;

using BinarySerialization.Graph.TypeGraph;
using BinarySerialization.Interfaces;
using BinarySerialization.Streams;

namespace BinarySerialization.Graph.ValueGraph;

internal class CustomValueNode(ValueNode parent, string name, TypeNode typeNode) : ObjectValueNode(parent, name, typeNode)
{
    protected override void ObjectSerializeOverride(BoundedStream stream, EventShuttle eventShuttle)
    {
        LazyBinarySerializationContext serializationContext = CreateLazySerializationContext();

        object value = BoundValue;

        if (value == null)
        {
            return;
        }

        if (value is not IBinarySerializable binarySerializable)
        {
            throw new InvalidOperationException("Must implement IBinarySerializable");
        }

        binarySerializable.Serialize(stream, GetFieldEndianness(), serializationContext);
    }

    protected override void ObjectDeserializeOverride(BoundedStream stream, SerializationOptions options, EventShuttle eventShuttle)
    {
        LazyBinarySerializationContext serializationContext = CreateLazySerializationContext();
        IBinarySerializable binarySerializable = CreateBinarySerializable();
        binarySerializable.Deserialize(stream, GetFieldEndianness(), serializationContext);
        Value = binarySerializable;
    }

    protected override Task ObjectDeserializeOverrideAsync(BoundedStream stream, SerializationOptions options, EventShuttle eventShuttle,
        CancellationToken cancellationToken)
    {
        ObjectDeserializeOverride(stream, options, eventShuttle);
        return Task.CompletedTask;
    }

    protected override Task ObjectSerializeOverrideAsync(BoundedStream stream, EventShuttle eventShuttle, CancellationToken cancellationToken)
    {
        ObjectSerializeOverride(stream, eventShuttle);
        return Task.CompletedTask;
    }

    private IBinarySerializable CreateBinarySerializable()
    {
        IBinarySerializable binarySerializable = (IBinarySerializable)Activator.CreateInstance(TypeNode.Type);
        return binarySerializable;
    }
}