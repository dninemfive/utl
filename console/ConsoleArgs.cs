using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace d9.utl.console
{
    /// <summary>
    /// Static class which initializes any variables with a <see cref="ConsoleArgAttribute"/> or <see cref="ConsoleFlagAttribute"/>.
    /// </summary>
    public static class ConsoleArgs
    {
        private static IntermediateArgs? _args = null;
        private static object? Get(IConsoleArg ica, string key) 
            => _args is not null ? ica.Parse(_args, key) 
                                 : throw new Exception("Attempted to get a variable, but ConsoleArgs was not initialized!");
        /// <inheritdoc cref="Get(IConsoleArg, string)"/>
        public static T? Get<T>(IConsoleArg ica, string key) => (T)Get(ica, key)!;
        static ConsoleArgs()
        {
            string[] args = Environment.GetCommandLineArgs()[1..];
            Utils.DebugLog($"Initializing ConsoleArgs with args `{args.PrettyPrint()}`.");
            // todo: assembly- and type-level attributes to filter faster
            Type hasConsoleArgs = typeof(HasConsoleArgsAttribute);
            foreach(Type type) {
            }
        }
        public static bool HasConsoleArgs(this )
    }
}
