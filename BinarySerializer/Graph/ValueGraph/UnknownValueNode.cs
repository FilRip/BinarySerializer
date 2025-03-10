﻿using System;

using BinarySerialization.Graph.TypeGraph;

namespace BinarySerialization.Graph.ValueGraph;

internal class UnknownValueNode(ValueNode parent, string name, TypeNode typeNode) : ObjectValueNode(parent, name, typeNode)
{
    private object _cachedValue;
    private Type _valueType;

    public override object Value
    {
        get
        {
            /* For creating serialization contexts quickly */
            if (_cachedValue != null)
            {
                return _cachedValue;
            }

            return GetValue(child => child.Value);
        }

        set
        {
            if (value == null)
            {
                return;
            }

            _valueType = value.GetType();

            if (_valueType == typeof(object))
            {
                throw new InvalidOperationException("Unable to serialize object.");
            }

            /* Create graph as if parent were creating it */
            var unknownTypeGraph = new RootTypeNode(TypeNode.Parent, _valueType);
            var unknownSerializer = (RootValueNode)unknownTypeGraph.CreateSerializer(Parent);
            unknownSerializer.EndiannessCallback = GetFieldEndianness;
            unknownSerializer.EncodingCallback = GetFieldEncoding;
            unknownSerializer.Value = value;
            Children.Add(unknownSerializer.Child);

            _cachedValue = value;
        }
    }

    protected override Type GetValueTypeOverride()
    {
        return _valueType;
    }
}