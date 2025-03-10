using System;

namespace BinarySerialization.Attributes;

/// <summary>
/// Used to denote the type of a subtype factory object that implements ISubtypeFactory.
/// </summary>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public sealed class SubtypeFactoryAttribute(string path, Type factoryType) : SubtypeFactoryBaseAttribute(path, factoryType)
{
}