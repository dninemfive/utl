﻿using System.Text.RegularExpressions;

#pragma warning disable IDE1006

namespace d9.utl;
/// <summary>
/// Parses console <see langword="args"/> formatted as an approximation of Unix console arguments
/// into a structure representing arguments and their values. <br/><br/>
/// </summary>
/// <remarks>
/// For example, <br/><c>program.exe -- asdf -f --arg1 69 --exampleList a b c d --arg2 42</c><br/>
/// is parsed to <br/>
/// <code>
/// IntermediateArgs {
///   _args: {
///     arg1: ["69"]
///     arg2: ["42"]
///     exampleList: ["a", "b", "c", "d"]
///   },
///   flags: ['f'],
///   warnings: [
///     "position 1: Encountered "--" but no currentKey to close.",
///     "position 2: Encountered value "asdf" but no currentKey to add it to."
///   ]
/// }
/// </code>
/// </remarks>
public partial record IntermediateArgs
{
    /// <summary>
    /// The arguments without dashes, organized by their keys.
    /// </summary>
    private Dictionary<string, ICollection<string>> _args { get; set; } = new();
    /// <summary>
    /// The flags set on the program. Note that flags can repeat, and that the <see
    /// cref="CommandLineArgs.GetFlag(string, char?)">default implementation</see> counts
    /// occurrences of empty but non- <see langword="null"/> collections in <see
    /// cref="_args">_args</see> corresponding to its alias as occurrences of that key.
    /// </summary>
    private List<char> _flags { get; set; } = new();
    /// <summary>
    /// Any warnings generated due to syntax errors in the arguments and their respective positions.
    /// </summary>
    public IEnumerable<(int position, string message)> Warnings => _warnings;
    /// <inheritdoc cref="Warnings"/>
    private List<(int position, string message)> _warnings { get; set; } = new();
    /// <summary>
    /// Used to simplify parsing code in the constructor.
    /// </summary>
    private enum ArgCase { Terminator, Flag, Arg, Value }
    /// <summary>
    /// Constructs a new IntermediateArgs instance from the unparsed arguments passed to the program.
    /// </summary>
    /// <param name="args">
    /// The unparsed arguments, corresponding to the <see langword="args"/> keyword or the
    /// <c>args</c> argument to a program's <c>Main( <see langword="string"/>[] args)</c> method.
    /// </param>
    public IntermediateArgs(string[] args)
    {
        string? currentKey = null;
        static ArgCase getCase(string arg)
        {
            if (arg == "--")
                return ArgCase.Terminator;
            if (ArgMatcher().IsMatch(arg))
                return ArgCase.Arg;
            if (FlagMatcher().IsMatch(arg))
                return ArgCase.Flag;
            return ArgCase.Value;
        };
        int pos = 1;
        foreach (string arg in args)
        {
            switch (getCase(arg))
            {
                case ArgCase.Terminator:
                    if (currentKey is not null)
                        currentKey = null;
                    else
                        _warnings.Add((pos++, $"Encountered \"--\" but no currentKey to close."));
                    break;
                case ArgCase.Flag:
                    currentKey = null;
                    _flags.Add(arg[1]);
                    break;
                case ArgCase.Arg:
                    currentKey = arg[2..];
                    if (!_args.ContainsKey(currentKey))
                        _args[currentKey] = new List<string>();
                    break;
                case ArgCase.Value:
                    if (currentKey is not null)
                        _args[currentKey].Add(arg);
                    else
                        _warnings.Add((pos, $"Encountered value \"{arg}\" but no currentKey to add it to."));
                    break;
            }
        }
    }
    /// <summary>
    /// Gets the values corresponding to the specified key, if any.
    /// </summary>
    /// <param name="key">The key to look for in the underlying dictionary.</param>
    /// <returns>
    /// If the key was present at least once in the args, a non- <see langword="null"/> IEnumerable,
    /// with corresponding values, if there were any, in order of appearance. <br/><br/> If the key
    /// was <b>not</b> present in the args, <see langword="null"/>.
    /// </returns>
    public IEnumerable<string>? this[string key] => _args.TryGetValue(key, out ICollection<string>? value) ? value : null;
    /// <summary>
    /// Gets the value of a given flag character.
    /// </summary>
    /// <param name="flag">
    /// The single-character abbreviation of a <see cref="CommandLineArgs.GetFlag(string,
    /// char?)">flag</see> whose presence to check.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the flag was specified using its abbreviation, or <see
    /// langword="false"/> otherwise.
    /// </returns>
    public bool this[char flag] => _flags.Contains(flag);
    /// <summary>
    /// Tells whether the given key corresponds to the name of an argument which was passed to the program.
    /// </summary>
    /// <param name="key">The key to look for.</param>
    /// <returns><see langword="true"/> if the key was found, or <see langword="false"/> otherwise.</returns>
    public bool ContainsKey(string key) => _args.ContainsKey(key);
    /// <summary>
    /// Represents these intermediate args as a human-readable <see langword="string"/>.
    /// </summary>
    /// <returns>
    /// A <see langword="string"/> listing the args and flags specified when the program was executed.
    /// </returns>
    public override string ToString() => $"IntermediateArgs {_args.Select(x => $"<{x.Key}: {x.Value.ListNotation()}>").ListNotation()} {_flags.ListNotation()}";
    // gets error CS8795, but this is a bug: see
    [GeneratedRegex("^-.$")]
    private static partial Regex FlagMatcher();
    // see above comment
    [GeneratedRegex("^--.+$")]
    private static partial Regex ArgMatcher();
}