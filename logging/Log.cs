namespace d9.utl;

/// <summary>
/// Wrapper for writing to multiple locations simultaneously, for example a file and stdout.
/// </summary>
public partial class Log(params ILogComponent[] components) : IDisposable
{
    private IEnumerable<ILogComponent> _components = components;
    private bool _disposed = false;
    public static Log ConsoleAndFile(string fileName)
        => new(Components.Console, Components.OpenFile(fileName));
    public async Task Write(object? obj)
    {
        foreach (ILogComponent component in _components)
            await component.Write(obj);
    }
    public async Task WriteLine(object? obj)
    {
        foreach (ILogComponent component in _components)
            await component.WriteLine(obj);
    }
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
                foreach (ILogComponent component in _components)
                    if (component is IDisposable disposable)
                        disposable.Dispose();
            _disposed = true;
        }
    }
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
