using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace d9.utl
{
    /// <summary>
    /// Handles synchronously logging to a file and to stdout.
    /// </summary>
    public static class Log
    {
        /// <summary>
        /// The streamwriter used by the log file. If null, the log file is not written to.
        /// </summary>
        /// <remarks>TODO: options to periodically or on close flush to the log file without a streamwriter.</remarks>
        private static StreamWriter? LogFile { get; set; } = null;
        /// <summary>
        /// Opens a log file at the specified path.
        /// </summary>
        /// <param name="path">The path the log file should be created at.</param>
        /// <param name="overwrite">If <see langword="true"/>, the existence of a file at the specified <c>path</c> is ignored.</param>
        /// <param name="openMessage">The message, if any, to write immediately after creating the log file.</param>
        public static void OpenLog(string path, bool overwrite = true, string? openMessage = null)
        {
            if (overwrite || !File.Exists(path)) File.WriteAllText(path, string.Empty);
            LogFile = File.AppendText(path);
            if (openMessage is not null) WriteLine(openMessage);
        }
        /// <summary>
        /// Closes the currently-open log file.
        /// </summary>
        /// <param name="closeMessage">The message to write when closing the log file, if any.</param>
        public static void CloseLog(string? closeMessage = null)
        {
            if(LogFile is null)
            {
                WriteLine($"Attempted to close log file with message {closeMessage}, but one was not open.");
                return;
            }
            WriteLine(closeMessage);
            LogFile.Flush();
            LogFile.Close();
        }
        /// <summary>
        /// Writes the specified object (converted to a <see langword="string"/> with <see cref="StringUtils.PrintNull(object?, string)"/>) to the console
        /// and the log file, if any, <b>without</b> a trailing newline character.
        /// </summary>
        /// <param name="obj">The object to write.</param>
        public static void Write(object? obj)
        {
            Console.Write(obj.PrintNull());
            LogFile?.Write(obj.PrintNull());
        }
        /// <summary>
        /// Writes the specified object (converted to a <see langword="string"/> with <see cref="StringUtils.PrintNull(object?, string)"/>) to the console
        /// and the log file, if any, <b>with</b> a trailing newline character.
        /// </summary>
        /// <param name="obj">The object to write.</param>
        public static void WriteLine(object? obj)
        {
            Console.WriteLine(obj.PrintNull());
            LogFile?.WriteLine(obj.PrintNull());
        }
    }
}
