using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace d9.utl
{
    /// <summary>
    /// Debug stuff
    /// </summary>
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
        public static void Log(object? obj)
        {
            string message = obj.PrintNull();
            Console.WriteLine(message);
            if (LogPath is not null) File.AppendAllText(LogPath, message);
        }
        /// <summary>
        /// <see cref="Log"/>s the given object if <see cref="DebugEnabled"/> is <see langword="true"/>. Otherwise, does nothing.
        /// </summary>
        /// <param name="obj"><inheritdoc cref="Log" path="/param[@name='obj']"/></param>
        public static void DebugLog(object? obj)
        {
            if (DebugEnabled) Log(obj);
        }
    }
}
