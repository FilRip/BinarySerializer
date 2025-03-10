using System;
using System.Collections.Generic;
using System.Linq;

using BinarySerialization.Exceptions;

namespace BinarySerialization.Graph;

#pragma warning disable S4035 // Classes implementing "IEquatable<T>" should be sealed
public abstract class Node<TNode>(TNode parent) : IEquatable<Node<TNode>>, IEqualityComparer<TNode> where TNode : Node<TNode>
#pragma warning restore S4035 // Classes implementing "IEquatable<T>" should be sealed
{
    private const char PathSeparator = '.';

    public TNode Parent { get; } = parent;

    public string Name { get; protected set; }

    public List<TNode> Children { get; set; }

    public bool Equals(Node<TNode> other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return string.Equals(Name, other.Name);
    }

    public override bool Equals(object obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;

        return obj.GetType() == GetType() && Equals((Node<TNode>)obj);
    }

    public TNode GetChild(string path)
    {
        var memberNames = path.Split(PathSeparator);

        if (memberNames.Length == 0) throw new BindingException("Path cannot be empty.");

        var child = (TNode)this;
        foreach (var name in memberNames)
        {
            child = child.Children.SingleOrDefault(c => c.Name == name);

            if (child == null) throw new BindingException($"No field found at '{path}'.");
        }

        return child;
    }

    public override string ToString()
    {
        return Name ?? base.ToString();
    }

    public override int GetHashCode()
    {
        // ReSharper disable NonReadonlyMemberInGetHashCode
        return Name != null ? Name.GetHashCode() : 0;
        // ReSharper restore NonReadonlyMemberInGetHashCode
    }

    public bool Equals(TNode x, TNode y)
    {
        if (x is null && y is null)
            return true;
        if (x is null || y is null)
            return false;
        return x.Equals(y);
    }

    public int GetHashCode(TNode obj)
    {
        return obj.GetHashCode();
    }
}