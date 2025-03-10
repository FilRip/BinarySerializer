using System;

namespace BinarySerialization.Exceptions;

/// <summary>
///     Represents an binding exception.
/// </summary>
public class BindingException : Exception
{
    internal BindingException(string message) : base(message)
    {
    }
}