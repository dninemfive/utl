namespace d9.utl;
public class StreamLogComponent
    : ILogComponent, IDisposable, IAsyncDisposable
{
    private Stream _stream;
    private StreamWriter _sw;
    private bool _disposed = false;
    public StreamLogComponent(Stream stream)
    {
        _stream = stream;
        _sw = new(_stream);
    }
    public StreamLogComponent(StreamWriter sw)
    {
        _stream = sw.BaseStream;
        _sw = sw;
    }
    public Task Write(object? obj)
        => _sw.WriteAsync($"{obj}");
    public Task WriteLine(object? obj)
        => _sw.WriteLineAsync($"{obj}");
    public void Dispose()
    {
        if(!_disposed)
        {
            _sw.Dispose();
            _stream.Dispose();
        }
    }
    public async ValueTask DisposeAsync()
    {
        if(!_disposed)
        {
            await _sw.DisposeAsync();
            await _stream.DisposeAsync();
        }
    }
}
