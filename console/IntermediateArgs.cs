using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
#pragma warning disable IDE1006
namespace d9.utl.console
{
    /// <summary>
    /// Parses console _args formatted as an approximation of Unix console _args into a structure representing arguments and their values.
    /// <br/><br/>
    /// </summary>
    /// <remarks>
    /// For example,
    /// <br/><c>program.exe -- asdf -f --arg1 69 --exampleList a b c d --arg2 42</c><br/>
    /// is parsed to
    /// <br/>
    /// <code>
    /// IntermediateArgs {
    ///   _args: {
    ///     // note that dictionaries are unordered; i sorted this for convenience
    ///     arg1: ["69"]
    ///     arg2: ["42"]
    ///     exampleList: ["a", "b", "c", "d"]
    ///   }
    ///   flags: ['f']
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
        /// The flags set on the program. Note that flags can repeat, and that the <see cref="ConsoleFlagAttribute">default implementation</see>
        /// counts occurrences of empty lists in <see cref="_args">_args</see> corresponding to its alias as occurrences of that key.
        /// </summary>
        public IEnumerable<char> Flags => _flags;
        /// <inheritdoc cref="Flags"/>
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
        /// <param name="args">The unparsed _args, corresponding to the <see langword="_args"/> keyword or the <c>_args</c> argument to a program's
        /// <c>Main(<see langword="string"/>[] _args)</c> method.</param>
        public IntermediateArgs(string[] args)
        {
            string? currentKey = null;
            static ArgCase getCase(string arg)
            {
                if (arg == "--") return ArgCase.Terminator;
                if (FlagMatcher().IsMatch(arg)) return ArgCase.Flag;
                if (ArgMatcher().IsMatch(arg)) return ArgCase.Arg;
                return ArgCase.Value;
            };
            int pos = 1;
            foreach(string arg in args)
            {
                switch(getCase(arg))
                {
                    case ArgCase.Terminator:
                        if (currentKey is not null) currentKey = null;
                        else _warnings.Add((pos++, $"Encountered \"--\" but no currentKey to close."));
                        break;
                    case ArgCase.Flag:
                        currentKey = null;
                        _flags.Add(arg[1]);
                        break;
                    case ArgCase.Arg:
                        currentKey = arg[2..];
                        if (!_args.ContainsKey(currentKey)) _args[currentKey] = new List<string>();
                        break;
                    case ArgCase.Value:
                        if (currentKey is not null) _args[currentKey].Add(arg);
                        else _warnings.Add((pos, $"Encountered value \"{arg}\" but no currentKey to add it to."));
                        break;
                }
            }
        }
        /// <summary>
        /// Gets the values corresponding to the specified key, if any.
        /// </summary>
        /// <param name="key">The key to look for in the underlying dictionary.</param>
        /// <returns>If the key was present at least once in the _args, a non-null IEnumerable, with corresponding values, if there were any, in
        /// order of appearance.<br/><br/>If the key was <em>not</em> present in the _args, <see langword="null"/>.</returns>
        public IEnumerable<string>? this[string key] => _args.TryGetValue(key, out ICollection<string>? value) ? value : null;
        public bool ContainsKey(string key) => _args.ContainsKey(key);
        // gets error CS8795, but this is a bug: see 
        [GeneratedRegex("-.")]
        private static partial Regex FlagMatcher();
        // see above comment
        [GeneratedRegex("--.+")]
        private static partial Regex ArgMatcher();
    }
}
