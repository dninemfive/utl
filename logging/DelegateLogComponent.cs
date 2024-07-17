namespace d9.utl;
public class DelegateLogComponent(Action<object?> write, Action<object?> writeLine)
    : ILogComponent
{
    public Task Write(object? obj)
    {
        write(obj);
        return Task.CompletedTask;
    }
    public Task WriteLine(object? obj)
    {
        writeLine(obj);
        return Task.CompletedTask;
    }
}
