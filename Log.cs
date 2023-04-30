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
    public class Log : IDisposable
    {
        /// <summary>
        /// The way a <see cref="Log"/> writes to a file.
        /// </summary>
        public enum Mode
        {
            /// <summary>
            /// Opens the file, appends the text, and closes it again. Less efficient but potentially more reliable.
            /// </summary>
            WriteImmediate,
            /// <summary>
            /// Writes to the file as it goes. More efficient.
            /// </summary>
            Stream,
            // NYI
            // Flush
        }
        private readonly StreamWriter? Stream = null;
        private readonly string? Path = null;
        private readonly IConsole? Console = null;
        /// <summary>
        /// Creates and, if applicable, initializes a new logging session.
        /// </summary>
        /// <param name="path">The path to the log file to write to. If not specified, the log will only write to the console.</param>
        /// <param name="console">The <see cref="IConsole"/> to write to. If not specified, will write to <see cref="System.Console"/> by default.</param>
        /// <param name="overwrite">If <see langword="true"/>, will overwrite any file at <c><paramref name="path"/></c> if one exists. 
        /// If <see langword="false"/>, will instead append to any such file.</param>
        /// <param name="mode">The <see cref="Mode"/> to use when writing to the file.</param>
        public Log(string? path = null, IConsole? console = null, bool overwrite = true, Mode mode = Mode.Stream)
        {
            Path = path;
            if (Path is not null)
            {
                if(overwrite || !File.Exists(Path)) File.WriteAllText(Path, string.Empty);
                if(mode is Mode.Stream) Stream = File.AppendText(Path);
            }
            Console = console;
        }
        /// <summary>
        /// Flushes and closes the <see cref="StreamWriter">stream</see> used to write to the log file, if any.
        /// </summary>
        /// <remarks>Implements <see cref="IDisposable.Dispose()"/>.</remarks>
        public void Dispose()
        {
            Stream?.Flush();
            Stream?.Close();
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Writes the specified object (converted to a <see langword="string"/> with <see cref="StringUtils.PrintNull(object?, string)"/>) to the console
        /// and the log file, if any, <b>without</b> a trailing newline character.
        /// </summary>
        /// <param name="obj">The object to write.</param>
        public void Write(object? obj)
        {
            string s = obj.PrintNull();
            if (Console is not null)
            {
                Console.Write(s);
            }
            else System.Console.Write(s);
            WriteToFile(s);
        }
        /// <summary>
        /// Writes the specified object (converted to a <see langword="string"/> with <see cref="StringUtils.PrintNull(object?, string)"/>) to the console
        /// and the log file, if any, <b>with</b> a trailing newline character.
        /// </summary>
        /// <remarks>Doesn't call <see cref="Write(object?)"/> in case the <see cref="Console"/> handles writing lines differently.</remarks>
        /// <param name="obj">The object to write.</param>
        public void WriteLine(object? obj)
        {
            string s = obj.PrintNull();
            if (Console is not null)
            {
                Console.WriteLine(s);
            }
            else System.Console.WriteLine(s);
            WriteToFile($"{s}\n");
        }
        /// <summary>
        /// Writes the specified string to a file, either the Stream if applicable, to the Path,
        /// or does nothing if neither is specified.
        /// </summary>
        /// <param name="s">The string to write.</param>
        private void WriteToFile(string s)
        {
            if (Stream is not null)
            {
                Stream?.Write(s);
            }
            else if (Path is not null)
            {
                File.WriteAllText(Path, s);
            }
        }
    }
}
