using System.IO;

namespace d9.utl;
/// <summary>
/// An <see cref="ILogComponent"/> which wraps a <see cref="TextWriter"/> and allows it to be disposed.
/// </summary>
public class TextWriterLogComponent
    : ILogComponent, IDisposable, IAsyncDisposable
{
    private readonly TextWriter _writer;
    private bool _disposed = false;
    /// <summary>
    /// Creates a <see cref="TextWriterLogComponent"/> from the specified <paramref name="textWriter"/>.
    /// </summary>
    /// <param name="textWriter">The <see cref="TextWriter"/> from which to create the component.</param>
    /// <param name="synchronize">If <see langword="true"/>, the underlying writer will be a <see href="https://learn.microsoft.com/en-us/dotnet/api/system.io.textwriter.synchronized">synchronized</see> wrapper and therefore thread-safe.</param>
    public TextWriterLogComponent(TextWriter textWriter, bool synchronize = false)
        => _writer = synchronize? TextWriter.Synchronized(textWriter) : textWriter;
    /// <summary>
    /// Creates a <see cref="TextWriterLogComponent"/> from the specified <paramref name="stream"/> by creating a <see cref="StreamWriter"/> which writes to it.
    /// </summary>
    /// <param name="stream">The <see cref="Stream"/> from which to create the component.</param>
    /// <remarks><b>NOTE:</b> disposing this component will dispose the <see cref="StreamWriter"/> it creates, and therefore the <see cref="Stream"/> you pass in.</remarks>
    /// <param name="synchronize"><inheritdoc cref="TextWriterLogComponent(TextWriter, bool)" path="/param[@name='synchronize']"/></param>
    public TextWriterLogComponent(Stream stream, bool synchronize = false) : this(new StreamWriter(stream), synchronize) { }
    /// <inheritdoc cref="ILogComponent.Write(object?)"/>
    public Task Write(object? obj)
        => _writer.WriteAsync($"{obj}");
    /// <inheritdoc cref="ILogComponent.WriteLine(object?)"/>
    public Task WriteLine(object? obj)
        => _writer.WriteLineAsync($"{obj}");
    /// <summary>
    /// Implements <see cref="IDisposable"/>.
    /// </summary>
    public void Dispose()
    {
        if (!_disposed)
        {
            _disposed = true;
            _writer.Dispose();
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
            _disposed = true;
            await _writer.DisposeAsync();
        }
        GC.SuppressFinalize(this);
    }
}