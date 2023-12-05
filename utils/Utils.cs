namespace d9.utl;

/// <summary>
/// Miscellaneous utilities.
/// </summary>
public static class Utils
{
    /// <summary>
    /// The default <see cref="utl.Log"/> to print to when <see cref="Log(object?)"/> is used.
    /// </summary>
    public static Log? DefaultLog { private get; set; } = null;
    /// <summary>
    /// Whether or not to perform debug prints.
    /// </summary>
    public static readonly bool DebugEnabled = CommandLineArgs.GetFlag("debug");
    /// <summary>
    /// Prints to the <see cref="DefaultLog">default log</see>, if it has been set, or the <see cref="Console"/> otherwise.
    /// </summary>
    /// <remarks>Uses <see cref="StringUtils.PrintNull(object?, string)"/>, and therefore produces a non-empty line if a <see langword="null"/> is passed in.</remarks>
    /// <param name="obj">The object to write.</param>
    public static void Log(object? obj)
    {
        if (DefaultLog is not null) DefaultLog.WriteLine(obj.PrintNull());
        else Console.WriteLine(obj.PrintNull());
    }
    /// <summary>
    /// <see cref="Log"/>s the given object if <see cref="DebugEnabled"/> is <see langword="true"/>. Otherwise, does nothing.
    /// </summary>
    /// <remarks><inheritdoc cref="Log" path="/remarks"/></remarks>
    /// <param name="obj"><inheritdoc cref="Log" path="/param[@name='obj']"/></param>
    public static void DebugLog(object? obj)
    {
        if (DebugEnabled) Log(obj);
    }
    /// <summary>
    /// Just a wrapper for Linq's <c>FirstOrDefault</c> which takes a <see langword="params"/> argument, making it more readable in
    /// certain circumstances.
    /// </summary>
    /// <remarks>i honestly just wrote this for a meme.</remarks>
    /// <typeparam name="T">The type of the objects to sieve.</typeparam>
    /// <param name="lambda">The function which will sieve the objects. If it returns <see langword="true"/> for an object, the object is 
    /// <see langword="return"/>ed and enumeration stops.</param>
    /// <param name="default">The default value if no object causes the <c><paramref name="lambda"/></c> to return true.</param>
    /// <param name="ts">The <typeparamref name="T"/>s to sieve.</param>
    /// <returns>The first object satisfying <c><paramref name="lambda"/></c>, if any, or <c><paramref name="default"/></c> if none do.</returns>
    public static T Sieve<T>(Func<T, bool> lambda, T @default, params T[] ts)
        => ts.FirstOrDefault(lambda, @default);
}
