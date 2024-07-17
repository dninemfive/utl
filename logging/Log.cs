namespace d9.utl;

/// <summary>
/// Wrapper for writing to multiple locations simultaneously, for example a file and stdout.
/// </summary>
public partial class Log(params ILogComponent[] components) : IDisposable, IAsyncDisposable
{
    private readonly IEnumerable<ILogComponent> _components = components;
    private bool _disposed = false;
    /// <summary>
    /// Produces a <c>Log</c> which writes to the standard <see cref="Console"/> and to a
    /// newly-created file with the specified <paramref name="fileName"/>.
    /// </summary>
    /// <param name="fileName">The path to the file the <c>Log</c> will write to.</param>
    /// <returns>A <c>Log</c> as described above.</returns>
    /// <remarks><b>NOTE:</b> this will <b>overwrite</b> any existing file at the specified path!</remarks>
    public static Log ConsoleAndFile(string fileName)
        => new(Components.Console, Components.OpenFile(fileName));
    /// <summary>
    /// Asynchronously writes to all of this <c>Log</c>'s components.
    /// </summary>
    /// <param name="obj">The item to write.</param>
    /// <returns>A <see cref="Task"/> indicating completion.</returns>
    public async Task Write(object? obj)
    {
        foreach (ILogComponent component in _components)
            await component.Write(obj);
    }
    /// <summary>
    /// Asynchronously writes a line to all of this <c>Log</c>'s components.
    /// </summary>
    /// <param name="obj">The item to write.</param>
    /// <returns>A <see cref="Task"/> indicating completion.</returns>
    public async Task WriteLine(object? obj)
    {
        foreach (ILogComponent component in _components)
            await component.WriteLine(obj);
    }
    /// <summary>
    /// Implements <see cref="IDisposable"/>.
    /// </summary>
    public void Dispose()
    {
        if (!_disposed)
            foreach (ILogComponent component in _components)
                if (component is IDisposable disposable)
                    disposable.Dispose();
        GC.SuppressFinalize(this);
    }
    /// <summary>
    /// Implements <see cref="IAsyncDisposable"/>.
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        if (!_disposed)
            foreach (ILogComponent component in _components)
            {
                if (component is IAsyncDisposable asyncDisposable)
                {
                    await asyncDisposable.DisposeAsync();
                }
                else if (component is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
        GC.SuppressFinalize(this);
    }
}