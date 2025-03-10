using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace BinarySerialization.Streams;

internal class TapStream(Stream source, Stream tap, string name) : BoundedStream(source, name)
{
    private const string TappingErrorMessage = "Not supported while tapping.";

    private readonly Stream _tap = tap;

    public override bool CanSeek => false;

    public override long Position
    {
        get => base.Position;
        set => throw new InvalidOperationException(TappingErrorMessage);
    }

    protected override bool IsByteBarrier => true;

    public override long Seek(long offset, SeekOrigin origin)
    {
        throw new InvalidOperationException(TappingErrorMessage);
    }

    public override void SetLength(long value)
    {
        throw new InvalidOperationException(TappingErrorMessage);
    }

    protected override void WriteByteAligned(byte[] buffer, int length)
    {
        _tap.Write(buffer, 0, length);
        base.WriteByteAligned(buffer, length);
    }

    protected override async Task WriteByteAlignedAsync(byte[] buffer, int length,
        CancellationToken cancellationToken)
    {
        await _tap.WriteAsync(buffer, 0, length, cancellationToken).ConfigureAwait(false);
        await base.WriteByteAlignedAsync(buffer, length, cancellationToken).ConfigureAwait(false);
    }

    protected override int ReadByteAligned(byte[] buffer, int length)
    {
        int read = base.ReadByteAligned(buffer, length);
        _tap.Write(buffer, 0, read);
        return read;
    }

    protected override async Task<int> ReadByteAlignedAsync(byte[] buffer, int length, CancellationToken cancellationToken)
    {
        int read = await base.ReadByteAlignedAsync(buffer, length, cancellationToken).ConfigureAwait(false);
        await _tap.WriteAsync(buffer, 0, read, cancellationToken).ConfigureAwait(false);
        return read;
    }

    public override async Task FlushAsync(CancellationToken cancellationToken)
    {
        await _tap.FlushAsync(cancellationToken).ConfigureAwait(false);
        await base.FlushAsync(cancellationToken).ConfigureAwait(false);
    }

    public override void Flush()
    {
        _tap.Flush();
        base.Flush();
    }
}