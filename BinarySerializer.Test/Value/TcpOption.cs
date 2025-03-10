using BinarySerialization.Attributes;

namespace BinarySerialization.Test.Value
{
    public class TcpOption
    {
        [FieldOrder(0)]
        public TcpOptionKind Kind { get; set; }

        [FieldOrder(1)]
        [SerializeWhen(nameof(Kind), TcpOptionKind.End, Constants.ComparisonOperator.NotEqual)]
        [SerializeWhen(nameof(Kind), TcpOptionKind.NoOp, Constants.ComparisonOperator.NotEqual)]
        public byte Length { get; set; }

        [FieldOrder(2)]
        [FieldLength(nameof(Length))]
        [SerializeWhen(nameof(Kind), TcpOptionKind.End, Constants.ComparisonOperator.NotEqual)]
        [SerializeWhen(nameof(Kind), TcpOptionKind.NoOp, Constants.ComparisonOperator.NotEqual)]
        public byte[] Option { get; set; }
    }
}
