namespace d9.utl;
/// <summary>
/// An <see cref="ILogComponent"/> which just wraps two delegate methods for <paramref name="write"/> and <paramref name="writeLine"/>.
/// </summary>
/// <param name="write">An <c>Action</c> corresponding to writing the specified output.</param>
/// <param name="writeLine">An <c>Action</c> corresponding to writing a line consisting of the specified output.</param>
public class DelegateLogComponent(Action<object?> write, Action<object?> writeLine)
    : ILogComponent
{
    /// <inheritdoc cref="ILogComponent.Write(object?)"/>
    public Task Write(object? obj)
    {
        write(obj);
        return Task.CompletedTask;
    }
    /// <inheritdoc cref="ILogComponent.WriteLine(object?)"/>
    public Task WriteLine(object? obj)
    {
        writeLine(obj);
        return Task.CompletedTask;
    }
}
