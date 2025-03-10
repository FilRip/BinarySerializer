using System;
using System.Collections;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using BinarySerialization.Constants;
using BinarySerialization.Graph.TypeGraph;
using BinarySerialization.Helpers;
using BinarySerialization.Streams;

namespace BinarySerialization.Graph.ValueGraph;

internal class PrimitiveListValueNode(ValueNode parent, string name, TypeNode typeNode) : PrimitiveCollectionValueNode(parent, name, typeNode)
{
    protected override void PrimitiveCollectionSerializeOverride(BoundedStream stream, object boundValue,
        ValueValueNode childSerializer, SerializedType childSerializedType, FieldLength itemLength, long? itemCount)
    {
        var list = (IList)boundValue;

        // Handle const-sized mismatched collections
        PadList(ref list, itemCount);

        foreach (var value in list)
        {
            if (stream.IsAtLimit)
            {
                break;
            }

            childSerializer.Serialize(stream, value, childSerializedType, itemLength);
        }
    }

    protected override async Task PrimitiveCollectionSerializeOverrideAsync(BoundedStream stream, object boundValue, ValueValueNode childSerializer,
        SerializedType childSerializedType, FieldLength itemLength, long? itemCount, CancellationToken cancellationToken)
    {
        var list = (IList)boundValue;

        // Handle const-sized mismatched collections
        PadList(ref list, itemCount);

        foreach (var value in list)
        {
            if (stream.IsAtLimit)
            {
                break;
            }

            await childSerializer.SerializeAsync(stream, value, childSerializedType, itemLength, cancellationToken)
                .ConfigureAwait(false);
        }
    }

    protected override object CreateCollection(long size)
    {
        var localTypeNode = (ListTypeNode)TypeNode;
        var array = Array.CreateInstance(localTypeNode.ChildType, (int)size);
        return Activator.CreateInstance(localTypeNode.Type, array);
    }

    protected override object CreateCollection(IEnumerable enumerable)
    {
        // don't think this can ever actually happen b/c it would signify a "jagged list", which isn't a real thing
        // TODO probably remove at some point once I verify that
        var localTypeNode = (ListTypeNode)TypeNode;
        return enumerable.Cast<object>().Select(item => item.ConvertTo(localTypeNode.ChildType)).ToList();
    }

    protected override void SetCollectionValue(object item, long index)
    {
        var list = (IList)Value;
        var localTypeNode = (ListTypeNode)TypeNode;
        list[(int)index] = item.ConvertTo(localTypeNode.ChildType);
    }

    protected override long CountOverride()
    {
        var list = (IList)Value;

        if (list == null)
        {
            return 0;
        }

        return list.Count;
    }

    private void PadList(ref IList list, long? itemCount)
    {
        if (itemCount != null && list.Count != itemCount)
        {
            var tempList = list;
            list = (IList)CreateCollection(itemCount.Value);

            for (var i = 0; i < Math.Min(tempList.Count, list.Count); i++)
            {
                list[i] = tempList[i];
            }
        }
    }
}