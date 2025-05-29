namespace d9.utl;
/// <summary>
/// An interface representing an object which can be written to as part of a <see cref="Log"/>.
/// </summary>
public interface ILogComponent
{
    /// <summary>
    /// Writes to this object's output.
    /// </summary>
    /// <param name="obj">What to write to the output.</param>
    /// <returns>A <see cref="Task"/> indicating completion.</returns>
    public Task Write(object? obj);
    /// <summary>
    /// Writes a line to this object's output.
    /// </summary>
    /// <param name="obj">What to write to the output.</param>
    /// <returns>A <see cref="Task"/> indicating completion.</returns>
    public Task WriteLine(object? obj);
}