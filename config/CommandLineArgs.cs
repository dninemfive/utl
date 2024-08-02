namespace d9.utl;

/// <summary>
/// Handles automatically loading command-line arguments into variables.
/// </summary>
/// <example>public static readonly string ExampleArg = CommandLineArgs.Get("example", CommandLineArgs.Parsers.FirstNonNullString);</example>
public partial class CommandLineArgs
{
    public static CommandLineArgs Instance
        = new(Environment.GetCommandLineArgs()[1..]);
    /// <summary>
    /// The IntermediateArgs instance for this run of the application.
    /// </summary>
    public IntermediateArgs IntermediateArgs { get; private set; }
    /// <summary>
    /// Loads the command-line <see langword="args"/> and parses them into <see
    /// cref="IntermediateArgs">an intermediate state</see>.
    /// </summary>
    public CommandLineArgs()
    {
        IntermediateArgs = new(Environment.GetCommandLineArgs()[1..]);
        foreach ((int pos, string warning) in IntermediateArgs.Warnings)
            DebugUtils.IfDebug(Console.WriteLine, $"Error in command-line args at position {pos}: {warning}");
    }
    public CommandLineArgs(params string[] args) : this(args, Log.ConsoleAndFile("d9.utl.args.log", true));
    public CommandLineArgs(IEnumerable<string> args, Log? log = null)
    {

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
    public T? TryGet<T>(string argName, Parser<T> parser)
        => parser(IntermediateArgs[argName], false);
    /// <summary>
    /// Tries to parse a given argument to the specified value type.
    /// </summary>
    /// <typeparam name="T">The type to parse the argument into.</typeparam>
    /// <param name="argName">The name of the argument to parse.</param>
    /// <param name="formatProvider">An <see cref="IFormatProvider"/> to help parse the argument.</param>
    /// <returns></returns>
    public T? TryParseStruct<T>(string argName, IFormatProvider? formatProvider = null)
        where T : struct, IParsable<T>
        => Parsers.Struct<T>(formatProvider)(IntermediateArgs[argName], false);
    /// <summary>
    /// Tries to parse a given argument to the specified reference type.
    /// </summary>
    /// <typeparam name="T">The type to parse the argument into.</typeparam>
    /// <param name="argName">The name of the argument to parse.</param>
    /// <param name="formatProvider">An <see cref="IFormatProvider"/> to help parse the argument.</param>
    /// <returns></returns>
    public T? TryParseClass<T>(string argName, IFormatProvider? formatProvider = null)
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
    public bool GetFlag(string argName, char? flag = null)
        => IntermediateArgs[argName].Any() || IntermediateArgs[flag ?? argName.First()];
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
    public T Get<T>(string argName, Parser<T> parser, Exception? exception = null)
        => TryGet(argName, parser) ?? throw exception ?? new Exception($"Tried to get command-line argument {argName}, but it was not found!");
}