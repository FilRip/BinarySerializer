﻿using System;
using System.Collections;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using BinarySerialization.Constants;
using BinarySerialization.Graph.TypeGraph;
using BinarySerialization.Helpers;
using BinarySerialization.Streams;

namespace BinarySerialization.Graph.ValueGraph;

internal class PrimitiveArrayValueNode(ValueNode parent, string name, TypeNode typeNode) : PrimitiveCollectionValueNode(parent, name, typeNode)
{
    protected override void PrimitiveCollectionSerializeOverride(BoundedStream stream, object boundValue,
        ValueValueNode childSerializer, SerializedType childSerializedType, FieldLength itemLength, long? itemCount)
    {
        var array = (Array)BoundValue;

        // Handle const-sized mismatched collections
        PadArray(ref array, itemCount);

        for (var i = 0; i < array.Length; i++)
        {
            if (stream.IsAtLimit)
            {
                break;
            }

            var value = array.GetValue(i);
            childSerializer.Serialize(stream, value, childSerializedType, itemLength);
        }
    }

    protected override async Task PrimitiveCollectionSerializeOverrideAsync(BoundedStream stream, object boundValue, ValueValueNode childSerializer,
        SerializedType childSerializedType, FieldLength itemLength, long? itemCount, CancellationToken cancellationToken)
    {
        var array = (Array)BoundValue;

        // Handle const-sized mismatched collections
        PadArray(ref array, itemCount);

        for (var i = 0; i < array.Length; i++)
        {
            if (stream.IsAtLimit)
            {
                break;
            }

            var value = array.GetValue(i);
            await childSerializer.SerializeAsync(stream, value, childSerializedType, itemLength, cancellationToken)
                .ConfigureAwait(false);
        }
    }

    protected override object CreateCollection(long size)
    {
        var localTypeNode = (ArrayTypeNode)TypeNode;
        return Array.CreateInstance(localTypeNode.ChildType, (int)size);
    }

    protected override object CreateCollection(IEnumerable enumerable)
    {
        var localTypeNode = (ArrayTypeNode)TypeNode;
        return enumerable.Cast<object>().Select(item => item.ConvertTo(localTypeNode.ChildType)).ToArray();
    }

    protected override void SetCollectionValue(object item, long index)
    {
        var array = (Array)BoundValue;
        var localTypeNode = (ArrayTypeNode)TypeNode;
        array.SetValue(item.ConvertTo(localTypeNode.ChildType), (int)index);
    }

    protected override long CountOverride()
    {
        var array = (Array)BoundValue;

        if (array == null)
        {
            return 0;
        }

        return array.Length;
    }

    private void PadArray(ref Array array, long? itemCount)
    {
        if (itemCount != null && array.Length != itemCount)
        {
            var tempArray = array;
            array = (Array)CreateCollection(itemCount.Value);
            Array.Copy(tempArray, array, Math.Min(tempArray.Length, array.Length));
        }
    }
}