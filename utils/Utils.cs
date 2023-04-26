using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace d9.utl
{
    public static class Utils
    {
        /// <summary>
        /// The path to the log file for this program.<br/><br/> If <see langword="null"/>, logs will only be output to the console.
        /// </summary>
        public static readonly string LogPath = CommandLineArgs.TryGet(nameof(LogPath).LowerFirst(), CommandLineArgs.Parsers.FirstNonNullOrEmptyString) 
                                             ?? $"{DateTime.Now.FileNameFormat()}.log";
        /// <summary>
        /// Whether or not to perform debug prints.
        /// </summary>
        public static readonly bool DebugEnabled = CommandLineArgs.GetFlag("debug");
        /// <summary>
        /// Logs an object to the console and, if <see cref="LogPath"/> is not <see langword="null"/>, writes it to the log file.<br/>
        /// Uses <see cref="StringUtils.PrintNull(object?, string)"/>, and therefore produces a non-empty line if a <see langword="null"/> is passed in.
        /// </summary>
        /// <param name="obj">The object to write.</param>
        /// <param name="altPath">The path to an alternate log file than the default..</param>
        public static void Log(object? obj, string? altPath = null)
        {
            string message = obj.PrintNull();
            Console.WriteLine(message);
            string? path = altPath ?? LogPath;
            if (path is not null) File.AppendAllText(path, $"{message}\n");
        }
        /// <summary>
        /// <see cref="Log"/>s the given object if <see cref="DebugEnabled"/> is <see langword="true"/>. Otherwise, does nothing.
        /// </summary>
        /// <param name="obj"><inheritdoc cref="Log" path="/param[@name='obj']"/></param>
        public static void DebugLog(object? obj)
        {
            if (DebugEnabled) Log(obj);
        }
        public static T Sieve<T>(Func<T, bool> lambda, T @default, params T[] ts)
            => ts.FirstOrDefault(lambda, @default);
    }
}
