using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using BinarySerialization.Constants;
using BinarySerialization.Graph.TypeGraph;
using BinarySerialization.Helpers;
using BinarySerialization.Streams;

namespace BinarySerialization.Graph.ValueGraph;

internal abstract class CollectionValueNode(ValueNode parent, string name, TypeNode typeNode) : CollectionValueNodeBase(parent, name, typeNode)
{
    internal override void SerializeOverride(BoundedStream stream, EventShuttle eventShuttle)
    {
        List<ValueNode> serializableChildren = [.. GetSerializableChildren()];

        SetTerminationValue(serializableChildren);

        foreach (ValueNode child in serializableChildren)
        {
            if (stream.IsAtLimit)
            {
                break;
            }

            BoundedStream childStream = new(stream, Name, GetConstFieldItemLength);

            child.Serialize(childStream, eventShuttle);
        }

        SerializeTermination(stream, eventShuttle);
    }

    internal override async Task SerializeOverrideAsync(BoundedStream stream, EventShuttle eventShuttle, CancellationToken cancellationToken)
    {
        List<ValueNode> serializableChildren = [.. GetSerializableChildren()];

        SetTerminationValue(serializableChildren);

        foreach (ValueNode child in serializableChildren)
        {
            if (stream.IsAtLimit)
            {
                break;
            }

            BoundedStream childStream = new(stream, Name, GetConstFieldItemLength);

            await child.SerializeAsync(childStream, eventShuttle, true, cancellationToken)
                .ConfigureAwait(false);
        }

        await SerializeTerminationAsync(stream, eventShuttle, cancellationToken)
            .ConfigureAwait(false);
    }

    internal override void DeserializeOverride(BoundedStream stream, SerializationOptions options, EventShuttle eventShuttle)
    {
        object terminationValue = GetTerminationValue();
        ValueNode terminationChild = GetTerminationChild();
        object itemTerminationValue = GetItemTerminationValue();
        IEnumerable<long> itemLengths = GetItemLengths();

        using IEnumerator<long> itemLengthEnumerator = itemLengths?.GetEnumerator();
        long count = GetFieldCount() ?? long.MaxValue;

        for (long i = 0; i < count && !EndOfStream(stream); i++)
        {
            if (IsTerminated(stream, terminationChild, terminationValue, options, eventShuttle))
            {
                break;
            }

            itemLengthEnumerator?.MoveNext();

            // TODO this doesn't allow for deferred eval of endianness in the case of jagged arrays
            // probably extremely rare but still...
            long? itemLength = itemLengthEnumerator?.Current;
            BoundedStream childStream = itemLength == null
                ? new BoundedStream(stream, Name)
                : new BoundedStream(stream, Name, () => itemLength);

            ValueNode child = CreateChildSerializer();

            using (StreamResetter streamResetter = new(childStream))
            {
                child.Deserialize(childStream, options, eventShuttle);

                if (child.Value == null)
                {
                    break;
                }

                if (IsTerminated(child, itemTerminationValue))
                {
                    ProcessLastItem(streamResetter, child);
                    break;
                }

                streamResetter.CancelReset();
            }

            Children.Add(child);
        }
    }

    internal override async Task DeserializeOverrideAsync(BoundedStream stream, SerializationOptions options, EventShuttle eventShuttle,
        CancellationToken cancellationToken)
    {
        object terminationValue = GetTerminationValue();
        ValueNode terminationChild = GetTerminationChild();
        object itemTerminationValue = GetItemTerminationValue();
        IEnumerable<long> itemLengths = GetItemLengths();

        using IEnumerator<long> itemLengthEnumerator = itemLengths?.GetEnumerator();
        long count = GetFieldCount() ?? long.MaxValue;

        for (long i = 0; i < count && !EndOfStream(stream); i++)
        {
            if (IsTerminated(stream, terminationChild, terminationValue, options, eventShuttle))
            {
                break;
            }

            itemLengthEnumerator?.MoveNext();

            // TODO this doesn't allow for deferred eval of endianness in the case of jagged arrays
            // probably extremely rare but still...
            long? itemLength = itemLengthEnumerator?.Current;
            BoundedStream childStream = itemLength == null
                ? new BoundedStream(stream, Name)
                : new BoundedStream(stream, Name, () => itemLength);

            ValueNode child = CreateChildSerializer();

            using (StreamResetter streamResetter = new(childStream))
            {
                await child.DeserializeAsync(childStream, options, eventShuttle, cancellationToken)
                    .ConfigureAwait(false);

                if (child.Value == null)
                {
                    break;
                }

                if (IsTerminated(child, itemTerminationValue))
                {
                    ProcessLastItem(streamResetter, child);
                    break;
                }

                streamResetter.CancelReset();
            }

            Children.Add(child);
        }
    }

    protected override long CountOverride()
    {
        return Children.Count;
    }

    protected override IEnumerable<FieldLength> MeasureItemsOverride()
    {
        NullStream nullStream = new();
        BoundedStream boundedStream = new(nullStream, Name);

        IEnumerable<ValueNode> serializableChildren = GetSerializableChildren();

        return serializableChildren.Select(child =>
        {
            boundedStream.RelativePosition = FieldLength.Zero;
            child.Serialize(boundedStream, null);
            return boundedStream.RelativePosition;
        });
    }

    protected override object GetLastItemValueOverride()
    {
        ValueNode lastItem = Children.LastOrDefault() ?? throw new InvalidOperationException("Unable to determine last item value because collection is empty.");
        ValueValueNode terminationItemChild =
            (ValueValueNode)lastItem.GetChild(TypeNode.ItemSerializeUntilAttribute.ItemValuePath);

        return terminationItemChild.BoundValue;
    }

    private void SetTerminationValue(List<ValueNode> serializableChildren)
    {
        CollectionTypeNode localTypeNode = (CollectionTypeNode)TypeNode;

        if (localTypeNode.ItemSerializeUntilAttribute == null ||
            localTypeNode.ItemSerializeUntilAttribute.LastItemMode != LastItemMode.Include)
        {
            return;
        }

        ValueNode lastChild = serializableChildren.LastOrDefault();

        if (lastChild == null)
        {
            return;
        }

        object itemTerminationValue = TypeNode.ItemSerializeUntilBinding.GetBoundValue(this);
        ValueNode itemTerminationChild = lastChild.GetChild(localTypeNode.ItemSerializeUntilAttribute.ItemValuePath);

        object convertedItemTerminationValue =
            itemTerminationValue.ConvertTo(itemTerminationChild.TypeNode.Type);

        itemTerminationChild.Value = convertedItemTerminationValue;
    }

    private object GetItemTerminationValue()
    {
        object itemTerminationValue = null;
        if (TypeNode.ItemSerializeUntilBinding != null)
        {
            itemTerminationValue = TypeNode.ItemSerializeUntilBinding.GetValue(this);
        }

        return itemTerminationValue;
    }

    private void ProcessLastItem(StreamResetter streamResetter, ValueNode child)
    {
        switch (TypeNode.ItemSerializeUntilAttribute.LastItemMode)
        {
            case LastItemMode.Include:
                streamResetter.CancelReset();
                Children.Add(child);
                break;
            case LastItemMode.Discard:
                streamResetter.CancelReset();
                break;
            case LastItemMode.Defer:
                // stream will reset
                break;
        }
    }

    private IEnumerable<long> GetItemLengths()
    {
        IEnumerable<long> itemLengths = null;
        if (TypeNode.ItemLengthBindings != null)
        {
            object itemLengthValue = TypeNode.ItemLengthBindings.GetValue(this);

            IEnumerable enumerableItemLengthValue = itemLengthValue as IEnumerable;

            itemLengths = enumerableItemLengthValue?.Cast<object>().Select(Convert.ToInt64) ??
                          GetInfiniteSequence(Convert.ToInt64(itemLengthValue));
        }

        return itemLengths;
    }

    private bool IsTerminated(ValueNode child, object itemTerminationValue)
    {
        if (TypeNode.ItemSerializeUntilBinding == null)
        {
            return false;
        }

        ValueNode itemTerminationChild = child.GetChild(TypeNode.ItemSerializeUntilAttribute.ItemValuePath);

        object convertedItemTerminationValue =
            itemTerminationValue.ConvertTo(itemTerminationChild.TypeNode.Type);

        return itemTerminationChild.Value == null ||
               itemTerminationChild.Value.Equals(convertedItemTerminationValue);
    }

    // ReSharper disable IteratorNeverReturns
#pragma warning disable S2190 // Loops and recursions should not be infinite
    private static IEnumerable<long> GetInfiniteSequence(long value)
    {
        while (true)
        {
            yield return value;
        }
    }
#pragma warning restore S2190 // Loops and recursions should not be infinite
    // ReSharper restore IteratorNeverReturns
}