using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using BinarySerialization.Constants;
using BinarySerialization.Exceptions;
using BinarySerialization.Graph.TypeGraph;
using BinarySerialization.Streams;

namespace BinarySerialization.Graph.ValueGraph;

internal abstract class PrimitiveCollectionValueNode(ValueNode parent, string name, TypeNode typeNode) : CollectionValueNodeBase(parent, name, typeNode)
{
    public override object Value { get; set; }

    public override object BoundValue
    {
        get
        {
            object value;

            if (Bindings.Count > 0)
            {
                value = Bindings[0].Invoke();

                if (value is not IEnumerable enumerableValue)
                {
                    throw new InvalidOperationException(
                        "Complex types cannot be binding sources for scalar values.");
                }

                if (Bindings.Count != 1)
                {
                    List<IEnumerable> bindingValues = [.. Bindings.Select(binding => binding() as IEnumerable)];

                    if (bindingValues.Contains(null))
                    {
                        throw new InvalidOperationException(
                            "Complex types cannot be binding sources for scalar values.");
                    }

                    if (bindingValues.Select(enumerable => enumerable.Cast<object>())
                        .Any(enumerable => !enumerable.SequenceEqual(enumerableValue.Cast<object>())))
                    {
#pragma warning disable S2372 // Exceptions should not be thrown from property getters
                        throw new BindingException("Multiple bindings to a single source must have equivalent target values.");
#pragma warning restore S2372 // Exceptions should not be thrown from property getters
                    }
                }

                value = CreateCollection(enumerableValue);
            }
            else
            {
                value = Value;
            }

            return value;
        }
    }

    internal override void SerializeOverride(BoundedStream stream, EventShuttle eventShuttle)
    {
        ValueValueNode childSerializer = (ValueValueNode)CreateChildSerializer();
        SerializedType childSerializedType = childSerializer.TypeNode.GetSerializedType();

        FieldLength itemLength = GetConstFieldItemLength();

        object boundValue = BoundValue;

        long? count = GetConstFieldCount();

        // handle null value case
        if (boundValue == null)
        {
            if (count != null)
            {
                object defaultValue = TypeNode.GetDefaultValue(childSerializedType);
                for (int i = 0; i < count.Value; i++)
                {
                    childSerializer.Serialize(stream, defaultValue, childSerializedType, itemLength);
                }
            }

            return;
        }

        PrimitiveCollectionSerializeOverride(stream, boundValue, childSerializer, childSerializedType, itemLength,
            count);

        SerializeTermination(stream, eventShuttle);
    }

    internal override async Task SerializeOverrideAsync(BoundedStream stream, EventShuttle eventShuttle, CancellationToken cancellationToken)
    {
        ValueValueNode childSerializer = (ValueValueNode)CreateChildSerializer();
        SerializedType childSerializedType = childSerializer.TypeNode.GetSerializedType();

        FieldLength itemLength = GetConstFieldItemLength();

        object boundValue = BoundValue;

        long? count = GetConstFieldCount();

        // handle null value case
        if (boundValue == null)
        {
            if (count != null)
            {
                object defaultValue = TypeNode.GetDefaultValue(childSerializedType);
                for (int i = 0; i < count.Value; i++)
                {
                    await childSerializer
                        .SerializeAsync(stream, defaultValue, childSerializedType, itemLength, cancellationToken)
                        .ConfigureAwait(false);
                }
            }

            return;
        }

        await PrimitiveCollectionSerializeOverrideAsync(stream, boundValue, childSerializer, childSerializedType, itemLength,
            count, cancellationToken);

        await SerializeTerminationAsync(stream, eventShuttle, cancellationToken);
    }

    internal override void DeserializeOverride(BoundedStream stream, SerializationOptions options, EventShuttle eventShuttle)
    {
        List<object> items = [.. DeserializeCollection(stream, options, eventShuttle)];
        CreateFinalCollection(items);
    }

    internal override async Task DeserializeOverrideAsync(BoundedStream stream, SerializationOptions options, EventShuttle eventShuttle,
        CancellationToken cancellationToken)
    {
        List<object> items = await DeserializeCollectionAsync(stream, options, eventShuttle, cancellationToken).ConfigureAwait(false);
        CreateFinalCollection(items);
    }

    protected abstract void PrimitiveCollectionSerializeOverride(BoundedStream stream, object boundValue,
        ValueValueNode childSerializer, SerializedType childSerializedType, FieldLength itemLength, long? itemCount);

    protected abstract Task PrimitiveCollectionSerializeOverrideAsync(BoundedStream stream, object boundValue,
        ValueValueNode childSerializer, SerializedType childSerializedType, FieldLength itemLength, long? itemCount,
        CancellationToken cancellationToken);

    protected abstract object CreateCollection(long size);
    protected abstract object CreateCollection(IEnumerable enumerable);
    protected abstract void SetCollectionValue(object item, long index);

    protected override object GetLastItemValueOverride()
    {
        throw new InvalidOperationException(
            "Not supported on primitive collections.  Use SerializeUntil attribute.");
    }

    private void CreateFinalCollection(List<object> items)
    {
        int itemCount = items.Count;

        Value = CreateCollection(itemCount);

        /* Copy temp list into final collection */
        for (int i = 0; i < itemCount; i++)
        {
            SetCollectionValue(items[i], i);
        }
    }

    private IEnumerable<object> DeserializeCollection(BoundedStream stream, SerializationOptions options, EventShuttle eventShuttle)
    {
        /* Create single serializer to do all the work */
        ValueValueNode childSerializer = (ValueValueNode)CreateChildSerializer();
        SerializedType childSerializedType = childSerializer.TypeNode.GetSerializedType();

        object terminationValue = GetTerminationValue();
        ValueNode terminationChild = GetTerminationChild();
        long? itemLength = GetFieldItemLength();

        BinaryReader reader = new(stream);
        long count = GetFieldCount() ?? long.MaxValue;

        for (long i = 0; i < count && !EndOfStream(stream); i++)
        {
            if (IsTerminated(stream, terminationChild, terminationValue, options, eventShuttle))
            {
                break;
            }

            childSerializer.Deserialize(reader, childSerializedType, itemLength);
            yield return childSerializer.GetValue(childSerializedType);
        }
    }

    private async Task<List<object>> DeserializeCollectionAsync(BoundedStream stream, SerializationOptions options, EventShuttle eventShuttle,
        CancellationToken cancellationToken)
    {
        List<object> list = [];

        /* Create single serializer to do all the work */
        ValueValueNode childSerializer = (ValueValueNode)CreateChildSerializer();
        SerializedType childSerializedType = childSerializer.TypeNode.GetSerializedType();

        object terminationValue = GetTerminationValue();
        ValueNode terminationChild = GetTerminationChild();
        long? itemLength = GetFieldItemLength();

        AsyncBinaryReader reader = new(stream, GetFieldEncoding());
        long count = GetFieldCount() ?? long.MaxValue;

        for (long i = 0; i < count && !EndOfStream(stream); i++)
        {
            if (IsTerminated(stream, terminationChild, terminationValue, options, eventShuttle))
            {
                break;
            }

            await childSerializer.DeserializeAsync(reader, childSerializedType, itemLength, cancellationToken)
                .ConfigureAwait(false);
            object value = childSerializer.GetValue(childSerializedType);
            list.Add(value);
        }

        return list;
    }
}