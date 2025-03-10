using System;
using System.Linq;

using BinarySerialization.Graph.TypeGraph;

namespace BinarySerialization.Graph.ValueGraph;

internal class ArrayValueNode(ValueNode parent, string name, TypeNode typeNode) : CollectionValueNode(parent, name, typeNode)
{
    private object _cachedValue;

    public override object Value
    {
        get
        {
            /* For creating serialization contexts quickly */
            if (_cachedValue != null)
            {
                return _cachedValue;
            }

            ArrayTypeNode localTypeNode = (ArrayTypeNode)TypeNode;
            Array array = Array.CreateInstance(localTypeNode.ChildType, Children.Count);
            object[] childValues = [.. Children.Select(child => child.Value)];
            Array.Copy(childValues, array, childValues.Length);
            return array;
        }

        set
        {
            if (Children.Count > 0)
            {
                throw new InvalidOperationException("Value already set.");
            }

            if (value == null)
            {
                return;
            }

            ArrayTypeNode localTypeNode = (ArrayTypeNode)TypeNode;

            Array array = (Array)value;

            long? count = GetConstFieldCount();

            if (count != null)
            {
                /* Pad out const-sized array */
                Array valueArray = Array.CreateInstance(localTypeNode.ChildType, (int)count);
                Array.Copy(array, valueArray, array.Length);
                array = valueArray;
            }

            System.Collections.Generic.IEnumerable<ValueNode> children = array.Cast<object>().Select(childValue =>
            {
                ValueNode child = CreateChildSerializer();
                child.Value = childValue;
                return child;
            });

            Children.AddRange(children);

            /* For creating serialization contexts quickly */
            _cachedValue = value;
        }
    }
}