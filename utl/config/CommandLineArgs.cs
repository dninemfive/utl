namespace d9.utl;

/// <summary>
/// Handles automatically loading command-line arguments into variables.
/// </summary>
/// <example>public static readonly string ExampleArg = CommandLineArgs.Get("example", CommandLineArgs.Parsers.FirstNonNullString);</example>
public static partial class CommandLineArgs
{
    /// <summary>
    /// The IntermediateArgs instance for this run of the application.
    /// </summary>
    public static IntermediateArgs IntermediateArgs { get; private set; }
    /// <summary>
    /// Defines a parser which operates on the values recorded for a given variable by an <see
    /// cref="IntermediateArgs"/> instance and returns an object of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of the object to return.</typeparam>
    /// <param name="values">
    /// A potentially <see langword="null"/><see cref="IEnumerable{T}">IEnumerable</see>&lt; <see
    /// langword="string"/>&gt; corresponding to the values passed after invoking a given variable's name.
    /// </param>
    /// <param name="flag">
    /// If the <see cref="IntermediateArgs._flags">flag</see> specified for the variable in question
    /// is present, <see langword="true"/>; otherwise, <see langword="false"/>.
    /// </param>
    /// <returns>
    /// An object of type <typeparamref name="T"/>, if parsing was successful, or <see
    /// langword="null"/> if parsing was not successful.
    /// </returns>
    public delegate T? Parser<T>(IEnumerable<string>? values, bool flag);
    /// <summary>
    /// Loads the command-line <see langword="args"/> and parses them into <see
    /// cref="IntermediateArgs">an intermediate state</see>.
    /// </summary>
    static CommandLineArgs()
    {
        IntermediateArgs = new(Environment.GetCommandLineArgs()[1..]);
        //foreach ((int pos, string warning) in IntermediateArgs.Warnings)
        //    DebugUtils.IfDebug(Console.Error.WriteLine, $"Error in command-line args at position {pos}: {warning}");
    }
    /// <summary>
    /// Attempts to get the argument named <c><paramref name="argName"/></c> as type <typeparamref
    /// name="T"/> using the specified <c><paramref name="parser"/></c>, returning <see
    /// langword="null"/> if unsuccessful.
    /// </summary>
    /// <typeparam name="T">The type of the variable to get.</typeparam>
    /// <param name="argName">The command-line name of the variable to get.</param>
    /// <param name="parser">The <see cref="Parser{T}"/> used to get the variable's value.</param>
    /// <returns>
    /// An object of type <typeparamref name="T"/> if parsing was successful, or <see
    /// langword="null"/> otherwise.
    /// </returns>
    public static T? TryGet<T>(string argName, Parser<T> parser)
        => parser(IntermediateArgs[argName], false);
    /// <summary>
    /// Tries to parse a given argument to the specified value type.
    /// </summary>
    /// <typeparam name="T">The type to parse the argument into.</typeparam>
    /// <param name="argName">The name of the argument to parse.</param>
    /// <param name="formatProvider">An <see cref="IFormatProvider"/> to help parse the argument.</param>
    /// <returns></returns>
    public static T? TryParseStruct<T>(string argName, IFormatProvider? formatProvider = null)
        where T : struct, IParsable<T>
        => Parsers.Struct<T>(formatProvider)(IntermediateArgs[argName], false);
    /// <summary>
    /// Tries to parse a given argument to the specified reference type.
    /// </summary>
    /// <typeparam name="T">The type to parse the argument into.</typeparam>
    /// <param name="argName">The name of the argument to parse.</param>
    /// <param name="formatProvider">An <see cref="IFormatProvider"/> to help parse the argument.</param>
    /// <returns></returns>
    public static T? TryParseClass<T>(string argName, IFormatProvider? formatProvider = null)
        where T : class, IParsable<T>
        => Parsers.Class<T>(formatProvider)(IntermediateArgs[argName], false);
    /// <summary>
    /// Gets the specified <see cref="GetFlag(string, char?)">command-line flag</see>.
    /// </summary>
    /// <remarks>
    /// Currently, conflicting flag identifiers are <b>not</b> detected, so be careful that
    /// variables which are not supposed to be equivalent do not share their abbreviations.
    /// </remarks>
    /// <param name="argName">The name of the flag to get.</param>
    /// <param name="flag">
    /// The single-character abbreviation for the flag. If not specified, defaults to the lowercase
    /// equivalent of the first character of the specified <c><paramref name="argName"/></c>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the variable was defined at least once or its corresponding <see
    /// cref="IntermediateArgs.this[char]">flag</see> is present in the arguments, or <see
    /// langword="false"/> otherwise.
    /// </returns>
    public static bool GetFlag(string argName, char? flag = null)
        => Parsers.Flag(IntermediateArgs[argName], IntermediateArgs[flag ?? argName.First().ToLower()]);
    /// <summary>
    /// Gets the value of the argument named <c><paramref name="argName"/></c>, if present, and
    /// <b>throws an exception</b> if the argument is not found.
    /// </summary>
    /// <remarks><inheritdoc cref="TryGet{T}(string, Parser{T})" path="/remarks"/></remarks>
    /// <typeparam name="T"><inheritdoc cref="TryGet{T}(string, Parser{T})" path="/typeparam[@name='T']"/></typeparam>
    /// <param name="argName"><inheritdoc cref="TryGet{T}(string, Parser{T})" path="/param[@name='argName']"/></param>
    /// <param name="parser"><inheritdoc cref="TryGet{T}(string, Parser{T})" path="/param[@name='parser']"/></param>
    /// <param name="exception">
    /// The exception to throw if the variable is not found. If not specified, a generic <see
    /// cref="Exception"/> is thrown.
    /// </param>
    /// <returns>The value of the argument named <c><paramref name="argName"/></c>, if it exists.</returns>
    public static T Get<T>(string argName, Parser<T> parser, Exception? exception = null)
        => TryGet(argName, parser) ?? throw exception ?? new Exception($"Tried to get command-line argument {argName}, but it was not found!");
}