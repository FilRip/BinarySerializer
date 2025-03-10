namespace BinarySerialization.Test.Misc
{
#pragma warning disable S3453 // Classes should not have only "private" constructors
    public class ImmutableNoPublicConstructorClass
#pragma warning restore S3453 // Classes should not have only "private" constructors
    {
        private ImmutableNoPublicConstructorClass()
        {
        }

        // ReSharper disable once UnassignedGetOnlyAutoProperty
        public byte Value { get; }
    }
}
