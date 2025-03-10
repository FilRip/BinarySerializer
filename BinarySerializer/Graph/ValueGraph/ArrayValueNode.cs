﻿using System;
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

            var localTypeNode = (ArrayTypeNode)TypeNode;
            var array = Array.CreateInstance(localTypeNode.ChildType, Children.Count);
            var childValues = Children.Select(child => child.Value).ToArray();
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

            var localTypeNode = (ArrayTypeNode)TypeNode;

            var array = (Array)value;

            var count = GetConstFieldCount();

            if (count != null)
            {
                /* Pad out const-sized array */
                var valueArray = Array.CreateInstance(localTypeNode.ChildType, (int)count);
                Array.Copy(array, valueArray, array.Length);
                array = valueArray;
            }

            var children = array.Cast<object>().Select(childValue =>
            {
                var child = CreateChildSerializer();
                child.Value = childValue;
                return child;
            });

            Children.AddRange(children);

            /* For creating serialization contexts quickly */
            _cachedValue = value;
        }
    }
}