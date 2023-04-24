using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace d9.utl
{
    /// <summary>
    /// Handles automatically loading command-line arguments into variables.
    /// </summary>
    /// <example>
    /// public static readonly string ExampleArg = CommandLineArgs.Get("example", CommandLineArgs.Parsers.FirstNonNullString);
    /// </example>
    public static class CommandLineArgs
    {
        private readonly static IntermediateArgs _intermediateArgs;
        /// <summary>
        /// Defines a parser which operates on the values recorded for a given variable by an <see cref="IntermediateArgs"/> instance and returns
        /// an object of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <param name="values">A potentially <see langword="null"/> <see cref="IEnumerable{T}">IEnumerable</see>&lt;<see langword="string"/>&gt;
        /// corresponding to the values passed after invoking a given variable's name.</param>
        /// <param name="flag">If the <see cref="IntermediateArgs.Flags">flag</see> specified for the variable in question is present, 
        /// <see langword="true"/>; otherwise, <see langword="false"/>.</param>
        /// <returns>An object of type <typeparamref name="T"/>, if parsing was successful, or <see langword="null"/> if parsing was not successful.</returns>
        public delegate T? Parser<T>(IEnumerable<string>? values, bool flag);
        /// <summary>
        /// Predefined <see cref="Parser{T}">parsers</see> for command-line args.
        /// </summary>
        public static class Parsers
        {
            /// <summary>
            /// Selects the first <see langword="string"/> among the values which is not <see langword="null"/> and whose length is greater than 0.
            /// </summary>
            /// <remarks>Ignores the flag variable.</remarks>
            public static Parser<string> FirstNonNullOrEmptyString => (values, _) => values?.SkipWhile(x => string.IsNullOrEmpty(x)).First();
            /// <summary>
            /// Returns <inheritdoc cref="GetFlag(string, char?)" path="/returns"/>
            /// </summary>
            public static Parser<bool> Flag => (enumerable, flag) => enumerable is not null || flag;
            /// <summary>
            /// Returns the potentially <see langword="null"/> <see cref="IEnumerable{T}">IEnumerable</see>&lt;<see langword="string"/>&gt; corresponding
            /// to the exact values passed when specifying the given variable.
            /// </summary>
            public static Parser<IEnumerable<string>?> Raw => (values, _) => values;
            public static Parser<double?> Double
                => (values, _) => double.TryParse(FirstNonNullOrEmptyString(values, false), out double result) ? result : null;
            public static Parser<TimeSpan?> TimeSpan
                => (values, _) => System.TimeSpan.TryParse(FirstNonNullOrEmptyString(values, false), out TimeSpan result) ? result : null;
            public static Parser<TimeSpan?> UsingParser(Func<double, TimeSpan> parser)
                => delegate (IEnumerable<string>? values, bool _)
                {
                    double? d = Double(values, false);
                    if (d is not null) return parser(d.Value);
                    return null;
                };
            public static Parser<DateTime?> DateTime
                => (values, _) => System.DateTime.TryParse(FirstNonNullOrEmptyString(values, false), out DateTime result) ? result : null;
        }
        static CommandLineArgs()
        {
            _intermediateArgs = new(Environment.GetCommandLineArgs()[1..]);
            foreach((int pos, string warning) in _intermediateArgs.Warnings)
            {
                Utils.DebugLog($"Error in command-line args at position {pos}: {warning}");
            }
        }
        /// <summary>
        /// Attempts to get the argument named <c><paramref name="argName"/></c> as type <typeparamref name="T"/> using the specified <c><paramref name="parser"/></c>,
        /// returning <see langword="null"/> if unsuccessful.
        /// </summary>
        /// <typeparam name="T">The type of the variable to get.</typeparam>
        /// <param name="argName">The command-line name of the variable to get.</param>
        /// <param name="parser">The <see cref="Parser{T}"/> used to get the variable's value.</param>
        /// <returns>An object of type <typeparamref name="T"/> if parsing was successful, or <see langword="null"/> otherwise.</returns>
        public static T? TryGet<T>(string argName, Parser<T> parser)
            => parser(_intermediateArgs[argName], false);
        /// <summary>
        /// Gets the specified <see cref="IntermediateArgs.Flags">command-line flag</see>.
        /// </summary>
        /// <param name="argName">The name of the flag to get.</param>
        /// <param name="flag">The single-character abbreviation for the flag. If not specified, defaults to the lowercase equivalent of the first character
        /// of the specified <c><paramref name="argName"/></c>.</param>
        /// <returns><see langword="true"/> if the variable was defined at least once or its corresponding <see cref="IntermediateArgs.Flags">flag</see>
        /// is present in the arguments, or <see langword="false"/> otherwise.</returns>
        public static bool GetFlag(string argName, char? flag = null)
            => Parsers.Flag(_intermediateArgs[argName], _intermediateArgs[flag ?? argName.First().ToLower()]);
        /// <summary>
        /// Gets the value of the argument named <c><paramref name="argName"/></c>, if present, and <b>throws an exception</b> if the argument is not found.
        /// </summary>
        /// <typeparam name="T"><inheritdoc cref="TryGet{T}(string, Parser{T})" path="/typeparam[@name='T']"/></typeparam>
        /// <param name="argName"><inheritdoc cref="TryGet{T}(string, Parser{T})" path="/param[@name='argName']"/></param>
        /// <param name="parser"><inheritdoc cref="TryGet{T}(string, Parser{T})" path="/param[@name='parser']"/></param>
        /// <param name="exception">The exception to throw if the variable is not found. If not specified, a generic <see cref="Exception"/> is thrown.</param>
        /// <returns>The value of the argument named <c><paramref name="argName"/></c>, if it exists.</returns>
        public static T Get<T>(string argName, Parser<T> parser, Exception? exception = null)
            => TryGet(argName, parser) ?? throw exception ?? new Exception($"Tried to get command-line argument {argName}, but it was not found!");
        public static string GetDirectory(string argName)
        {
            string intro = $"Error when trying to get directory with argument {argName}: ";
            string possiblePath = TryGet(argName, Parsers.FirstNonNullOrEmptyString) 
                ?? throw new Exception($"{intro}No non-null-or-empty string was provided!");
            string path;
            try
            {
                path = Path.GetFullPath(possiblePath);
            }
            catch (Exception e)
            {
                throw new Exception($"{intro}Path was not valid: {e.Message}");
            }
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            // https://stackoverflow.com/a/1395226
            if (!File.GetAttributes(path).HasFlag(FileAttributes.Directory))
            {
                throw new Exception($"{intro} Path `{path}` is a file, not a folder!");
            }
            return path;
        }
    }
}
