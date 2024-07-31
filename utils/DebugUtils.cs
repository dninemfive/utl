using System.Runtime.CompilerServices;

namespace d9.utl;

/// <summary>
/// Miscellaneous utilities.
/// </summary>
public static class DebugUtils
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
    public static async Task DebugLog(this Log? log, object? obj)
    {
        if (DebugEnabled && log is not null)
            await log.WriteLine(obj);
    }
    /// <summary>
    /// Calls <paramref name="action"/> iff <see cref="DebugEnabled"/> is <see langword="true"/>.
    /// </summary>
    /// <typeparam name="T">The type of the argument to <paramref name="action"/>.</typeparam>
    /// <param name="action">The action to call.</param>
    /// <param name="obj">The object to call the action with.</param>
    public static void IfDebug<T>(this Action<T?> action, T? obj)
    {
        if (DebugEnabled)
            action(obj);
    }
    /// <summary>
    /// Converts an exception to an (ideally) single-line summary of what happened. 
    /// </summary>
    /// <param name="exception">The exception to summarize.</param>
    /// <param name="caller">See <see cref="CallerMemberNameAttribute"/>.</param>
    public static string Summary(this Exception exception, [CallerMemberName] string caller = "")
        => $"{caller} {exception.GetType().Name}: {exception.Message}";
}