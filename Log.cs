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
        public enum Mode
        {
            WriteImmediate,
            Stream,
            // NYI
            // Flush
        }
        /// <summary>
        /// The streamwriter used by the log file. If null, the log file is not written to.
        /// </summary>
        /// <remarks>TODO: options to periodically or on close flush to the log file without a streamwriter.</remarks>
        private readonly StreamWriter? Stream = null;
        private readonly string? Path = null;
        private readonly IConsole? Console = null;
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
        public void Dispose()
        {
            Stream?.Flush();
            Stream?.Close();
        }
        /// <summary>
        /// Writes the specified object (converted to a <see langword="string"/> with <see cref="StringUtils.PrintNull(object?, string)"/>) to the console
        /// and the log file, if any, <b>without</b> a trailing newline character.
        /// </summary>
        /// <param name="obj">The object to write.</param>
        public void Write(object? obj)
        {
            if (Console is not null)
            {
                Console.Write(obj.PrintNull());
            }
            else System.Console.Write(obj.PrintNull());
            if(Stream is not null) Stream?.Write(obj.PrintNull());
        }
        /// <summary>
        /// Writes the specified object (converted to a <see langword="string"/> with <see cref="StringUtils.PrintNull(object?, string)"/>) to the console
        /// and the log file, if any, <b>with</b> a trailing newline character.
        /// </summary>
        /// <param name="obj">The object to write.</param>
        public void WriteLine(object? obj)
        {
            if (Console is not null)
            {
                Console.WriteLine(obj.PrintNull());
            }
            else System.Console.WriteLine(obj.PrintNull());
            Stream?.WriteLine(obj.PrintNull());
        }        
    }
}
