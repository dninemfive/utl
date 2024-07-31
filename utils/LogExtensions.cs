namespace d9.utl;

/// <summary>
/// Miscellaneous utilities.
/// </summary>
public static class LogExtensions
{
    /// <summary>
    /// Whether or not to perform debug prints.
    /// </summary>
    public static readonly bool DebugEnabled = CommandLineArgs.GetFlag("debug");
    /// <summary>
    /// <paramref name="log"/> the given <paramref name="obj"/> only if the program was run with the <c>--debug</c> flag.
    /// </summary>
    /// <param name="log">The log to write to.</param>
    /// <param name="obj">The object to write to the log.</param>
    public static void DebugLog(this Log log, object? obj)
    {
        if (DebugEnabled)
            log.WriteLine(obj);
    }
}