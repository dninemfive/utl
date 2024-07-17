namespace d9.utl;
/// <summary>
/// An <see cref="ILogComponent"/> which wraps a <see cref="Stream"/> and <see cref="StreamWriter"/>
/// and allows them to be disposed.
/// </summary>
public class StreamLogComponent
    : ILogComponent, IDisposable, IAsyncDisposable
{
    private Stream _stream;
    private StreamWriter _streamWriter;
    private bool _disposed = false;
    /// <summary>
    /// Creates a <c>StreamLogComponent</c> from the specified <paramref name="stream"/>.
    /// </summary>
    /// <param name="stream">The <see cref="Stream"/> from which to create the component.</param>
    public StreamLogComponent(Stream stream)
    {
        _stream = stream;
        _streamWriter = new(_stream);
    }
    /// <summary>
    /// Creates a <c>StreamLogComponent</c> from the specified <paramref name="streamWriter"/>.
    /// </summary>
    /// <param name="streamWriter">The <see cref="StreamWriter"/> from which to create the component.</param>
    public StreamLogComponent(StreamWriter streamWriter)
    {
        _stream = streamWriter.BaseStream;
        _streamWriter = streamWriter;
    }
    /// <inheritdoc cref="ILogComponent.Write(object?)"/>
    public Task Write(object? obj)
        => _streamWriter.WriteAsync($"{obj}");
    /// <inheritdoc cref="ILogComponent.WriteLine(object?)"/>
    public Task WriteLine(object? obj)
        => _streamWriter.WriteLineAsync($"{obj}");
    /// <summary>
    /// Implements <see cref="IDisposable"/>.
    /// </summary>
    public void Dispose()
    {
        if (!_disposed)
        {
            _streamWriter.Dispose();
            _stream.Dispose();
        }
        GC.SuppressFinalize(this);
    }
    /// <summary>
    /// Implements <see cref="IAsyncDisposable"/>.
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        if (!_disposed)
        {
            await _streamWriter.DisposeAsync();
            await _stream.DisposeAsync();
        }
        GC.SuppressFinalize(this);
    }
}