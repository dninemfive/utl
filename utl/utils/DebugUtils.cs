using System.Runtime.CompilerServices;

namespace d9.utl;

/// <summary>
/// Miscellaneous utilities.
/// </summary>
public static class DebugUtils
{
    /// <summary>
    /// Converts an exception to an (ideally) single-line summary of what happened. 
    /// </summary>
    /// <param name="exception">The exception to summarize.</param>
    /// <param name="caller">See <see cref="CallerMemberNameAttribute"/>.</param>
    public static string Summary(this Exception exception, [CallerMemberName] string caller = "")
        => $"{caller} {exception.GetType().Name}: {exception.Message}";
}