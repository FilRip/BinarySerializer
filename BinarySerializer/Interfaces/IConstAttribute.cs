namespace BinarySerialization.Interfaces;

internal interface IConstAttribute : IBindableFieldAttribute
{
    object GetConstValue();
}