namespace d9.utl;

/// <summary>
/// Interface describing basic methods a console must have to be used with a <see cref="Log"/> instance.
/// </summary>
public interface IConsole
{
    /// <summary>
    /// Writes to the console <b>without</b> a trailing newline character.
    /// </summary>
    /// <param name="obj">The object to write.</param>
    public void Write(object? obj);
    /// <summary>
    /// Writes to the console <b>with</b> a trailing newline character.
    /// </summary>
    /// <param name="obj"><inheritdoc cref="Write(object?)" path="/param[@name='obj']"/></param>
    public void WriteLine(object? obj);
}
