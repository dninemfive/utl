using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace d9.utl
{
    public static class Log
    {
        private static StreamWriter? LogFile { get; set; } = null;
        public static void OpenLog(string path, bool overwrite = true, string? openMessage = null)
        {
            if (overwrite || !File.Exists(path)) File.WriteAllText(path, string.Empty);
            LogFile = File.AppendText(path);
            if (openMessage is not null) WriteLine(openMessage);
        }
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
        public static void Write(object? obj)
        {
            Console.Write(obj.PrintNull());
            LogFile?.Write(obj.PrintNull());
        }
        public static void WriteLine(object? obj)
        {
            Console.WriteLine(obj.PrintNull());
            LogFile?.WriteLine(obj.PrintNull());
        }
    }
}
