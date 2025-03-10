using System.Threading;
using System.Threading.Tasks;

using BinarySerialization.Graph.TypeGraph;
using BinarySerialization.Streams;

namespace BinarySerialization.Graph.ValueGraph;

internal abstract class CollectionValueNodeBase(ValueNode parent, string name, TypeNode typeNode) : ValueNode(parent, name, typeNode)
{
    protected ValueNode CreateChildSerializer()
    {
        CollectionTypeNode localTypeNode = (CollectionTypeNode)TypeNode;
        return localTypeNode.Child.CreateSerializer(this);
    }

    protected object GetTerminationValue()
    {
        CollectionTypeNode localTypeNode = (CollectionTypeNode)TypeNode;
        return localTypeNode.TerminationValue;
    }

    protected static bool IsTerminated(BoundedStream stream, ValueNode terminationChild, object terminationValue, SerializationOptions options,
        EventShuttle eventShuttle)
    {
        if (terminationChild != null)
        {
            using StreamResetter streamResetter = new(stream);
            terminationChild.Deserialize(stream, options, eventShuttle);

            if (terminationChild.Value.Equals(terminationValue))
            {
                streamResetter.CancelReset();
                return true;
            }
        }
        return false;
    }

    protected void SerializeTermination(BoundedStream stream, EventShuttle eventShuttle)
    {
        CollectionTypeNode localTypeNode = (CollectionTypeNode)TypeNode;

        if (localTypeNode.TerminationChild != null)
        {
            ValueNode terminationChild = localTypeNode.TerminationChild.CreateSerializer(this);
            terminationChild.Value = localTypeNode.TerminationValue;
            terminationChild.Serialize(stream, eventShuttle);
        }
    }

    protected async Task SerializeTerminationAsync(BoundedStream stream, EventShuttle eventShuttle, CancellationToken cancellationToken)
    {
        CollectionTypeNode localTypeNode = (CollectionTypeNode)TypeNode;

        if (localTypeNode.TerminationChild != null)
        {
            ValueNode terminationChild = localTypeNode.TerminationChild.CreateSerializer(this);
            terminationChild.Value = localTypeNode.TerminationValue;
            await terminationChild.SerializeAsync(stream, eventShuttle, true, cancellationToken)
                .ConfigureAwait(false);
        }
    }

    protected ValueNode GetTerminationChild()
    {
        CollectionTypeNode localTypeNode = (CollectionTypeNode)TypeNode;
        ValueNode terminationChild = localTypeNode.TerminationChild?.CreateSerializer(this);
        return terminationChild;
    }
}