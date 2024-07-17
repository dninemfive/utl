using DelegatePair = (d9.utl.Log.Delegate write, System.Action dispose);

namespace d9.utl;

/// <summary>
/// Wrapper for writing to multiple locations simultaneously, for example a file and stdout.
/// </summary>
public class Log : IDisposable
{
    /// <summary>
    /// Describes the concept of asynchronously writing an arbitrary object.
    /// </summary>
    /// <param name="msg">The object to write.</param>
    /// <returns>A <see cref="Task"/> which can be <see langword="await"/>ed.</returns>
    public delegate Task Delegate(object? msg);
    private IEnumerable<Delegate> _delegates;
    private IEnumerable<Action> _disposals;
    private bool _disposedValue;

    public Log(params DelegatePair[] pairs)
    {

    }
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
                foreach (Action dispose in _disposals)
                    dispose();
            _disposedValue = true;
        }
    }
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
