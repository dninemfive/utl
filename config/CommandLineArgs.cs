using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
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
        /// <param name="flag">If the <see cref="IntermediateArgs._flags">flag</see> specified for the variable in question is present, 
        /// <see langword="true"/>; otherwise, <see langword="false"/>.</param>
        /// <returns>An object of type <typeparamref name="T"/>, if parsing was successful, or <see langword="null"/> if parsing was not successful.</returns>
        public delegate T? Parser<T>(IEnumerable<string>? values, bool flag);
        /// <summary>
        /// Predefined <see cref="Parser{T}">parsers</see> for command-line args.
        /// </summary>
        public static class Parsers
        {
            #region basic parsers
            /// <summary>
            /// Selects the first <see langword="string"/> among the values which is not <see langword="null"/> and whose length is greater than 0.
            /// </summary>
            /// <remarks>Ignores the <c>flag</c> argument.</remarks>
            public static Parser<string> FirstNonNullOrEmptyString => (values, _) => values?.SkipWhile(x => string.IsNullOrEmpty(x)).First();
            /// <summary>
            /// Returns the potentially <see langword="null"/> <see cref="IEnumerable{T}">IEnumerable</see>&lt;<see langword="string"/>&gt; corresponding
            /// to the actual values passed when specifying the given variable.
            /// </summary>
            /// <remarks>Ignores the <c>flag</c> argument.</remarks>
            public static Parser<IEnumerable<string>?> Raw => (values, _) => values;
            #endregion basic parsers
            #region primitive types
            /// <summary>
            /// Parses the <see cref="FirstNonNullOrEmptyString">first non-null-or-empty <see langword="string"/></see> to a <see langword="double"/>.
            /// </summary>
            /// <remarks>Ignores the <c>flag</c> argument.</remarks>
            public static Parser<double?> Double
                => (values, _) => double.TryParse(FirstNonNullOrEmptyString(values, false), out double result) ? result : null;            
            /// <summary>
            /// Returns <inheritdoc cref="GetFlag(string, char?)" path="/returns"/>
            /// </summary>
            public static Parser<bool> Flag => (enumerable, flag) => enumerable is not null || flag;
            #endregion primitive types
            #region time
            /// <summary>
            /// Parses the <see cref="FirstNonNullOrEmptyString">first non-null-or-empty <see langword="string"/></see> to a <see cref="DateTime"/>
            /// using the type's <see cref="DateTime.TryParse(string?, out System.DateTime)">default parser</see>.
            /// </summary>
            /// <remarks>Ignores the <c>flag</c> argument.</remarks>
            public static Parser<DateTime?> DateTime
                => (values, _) => System.DateTime.TryParse(FirstNonNullOrEmptyString(values, false), out DateTime result) ? result : null;
            /// <summary>
            /// Parses the <see cref="FirstNonNullOrEmptyString">first non-null-or-empty <see langword="string"/></see> to a <see cref="TimeSpan"/>
            /// using the type's <see cref="TimeSpan.TryParse(string?, out System.TimeSpan)">default parser</see>.
            /// </summary>
            /// <remarks>Ignores the <c>flag</c> argument.</remarks>
            public static Parser<TimeSpan?> TimeSpan
                => (values, _) => System.TimeSpan.TryParse(FirstNonNullOrEmptyString(values, false), out TimeSpan result) ? result : null;
            /// <summary>
            /// Parses the <see cref="FirstNonNullOrEmptyString">first non-null-or-empty <see langword="string"/></see> to a <see langword="double"/>,
            /// then converts that into a <see cref="TimeSpan"/> using the specified delegate.
            /// </summary>
            /// <remarks>Intended for use with <see cref="TimeSpan"/>'s methods which parse from <see langword="double"/>s, such as
            /// <see cref="TimeSpan.FromMinutes(double)"/>.<br/>Ignores the <c>flag</c> argument.</remarks>
            /// <param name="parser">The parser to use.</param>
            /// <returns>A <see cref="TimeSpan"/> as produced by the <c><paramref name="parser"/></c>.</returns>
            public static Parser<TimeSpan?> UsingParser(Func<double, TimeSpan> parser)
                => delegate (IEnumerable<string>? values, bool _)
                {
                    double? d = Double(values, false);
                    if (d is not null) return parser(d.Value);
                    return null;
                };
            #endregion time
            #region filesystem
            /// <summary>
            /// <para>Checks that the <see cref="FirstNonNullOrEmptyString">first non-null-or-empty <see langword="string"/></see> is a path to a folder.</para>
            /// <list type="bullet">
            /// <item>If the folder exists, returns the path.</item>
            /// <item>If the folder does not exist but the path is valid, creates the folder and returns the path.</item>
            /// <item>Otherwise, returns <see langword="null"/>.</item>
            /// </list>
            /// <para>If the result is not <see langword="null"/>, it is guaranteed to be a path pointing to a folder which exists.</para>
            /// </summary>
            public static Parser<string?> FolderPath => delegate (IEnumerable<string>? enumerable, bool _)
            {
                string? possiblePath = FirstNonNullOrEmptyString(enumerable, false);
                if (possiblePath is null) return null;
                string path;
                try
                {
                    path = Path.GetFullPath(possiblePath);
                    // creating the directory throws an error if a file exists there
                    if(!Directory.Exists(path)) Directory.CreateDirectory(path);
                }
                catch (Exception e)
                {
                    Utils.DebugLog(e);
                    return null;
                }
                return path;
            };
            /// <summary>
            /// <para>Checks that the <see cref="FirstNonNullOrEmptyString">first non-null-or-empty <see langword="string"/></see> is a path to an existing file.
            /// If the file exists, returns the path; returns <see langword="null"/> otherwise.</para>
            /// <para>If the result is not <see langword="null"/>, it is guaranteed to be a path pointing to a folder which exists.</para>
            /// </summary>
            public static Parser<string?> FilePath => delegate (IEnumerable<string>? enumerable, bool _)
            {
                string? possiblePath = FirstNonNullOrEmptyString(enumerable, false);
                if (possiblePath is null) return null;
                string path;
                try
                {
                    path = Path.GetFullPath(possiblePath);
                }
                catch (Exception e)
                {
                    Utils.DebugLog(e);
                    return null;
                }
                if (!File.Exists(path)) return null;
                return path;
            };
            #endregion filesystem
        }
        /// <summary>
        /// Loads the command-line <see langword="args"/> and parses them into <see cref="IntermediateArgs">an intermediate state</see>.
        /// </summary>
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
        /// Gets the specified <see cref="GetFlag(string, char?)">command-line flag</see>.
        /// </summary>
        /// <remarks>Currently, conflicting flag identifiers are <b>not</b> detected, so be careful that variables which are not supposed to be equivalent
        /// do not share their abbreviations.</remarks>
        /// <param name="argName">The name of the flag to get.</param>
        /// <param name="flag">The single-character abbreviation for the flag. If not specified, defaults to the lowercase equivalent of the first character
        /// of the specified <c><paramref name="argName"/></c>.</param>
        /// <returns><see langword="true"/> if the variable was defined at least once or its corresponding <see cref="IntermediateArgs.this[char]">flag</see>
        /// is present in the arguments, or <see langword="false"/> otherwise.</returns>
        public static bool GetFlag(string argName, char? flag = null)
            => Parsers.Flag(_intermediateArgs[argName], _intermediateArgs[flag ?? argName.First().ToLower()]);
        /// <summary>
        /// Gets the value of the argument named <c><paramref name="argName"/></c>, if present, and <b>throws an exception</b> if the argument is not found.
        /// </summary>
        /// <remarks><inheritdoc cref="TryGet{T}(string, Parser{T})" path="/remarks"/></remarks>
        /// <typeparam name="T"><inheritdoc cref="TryGet{T}(string, Parser{T})" path="/typeparam[@name='T']"/></typeparam>
        /// <param name="argName"><inheritdoc cref="TryGet{T}(string, Parser{T})" path="/param[@name='argName']"/></param>
        /// <param name="parser"><inheritdoc cref="TryGet{T}(string, Parser{T})" path="/param[@name='parser']"/></param>
        /// <param name="exception">The exception to throw if the variable is not found. If not specified, a generic <see cref="Exception"/> is thrown.</param>
        /// <returns>The value of the argument named <c><paramref name="argName"/></c>, if it exists.</returns>
        public static T Get<T>(string argName, Parser<T> parser, Exception? exception = null)
            => TryGet(argName, parser) ?? throw exception ?? new Exception($"Tried to get command-line argument {argName}, but it was not found!");
    }
}
