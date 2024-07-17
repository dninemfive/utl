namespace d9.utl;
public interface ILogComponent
{
    public Task Write(object? obj);
    public Task WriteLine(object? obj);
}