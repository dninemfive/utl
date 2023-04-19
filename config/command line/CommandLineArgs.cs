using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace d9.utl
{
    public static class CommandLineArgs
    {
        private readonly static IntermediateArgs _intermediateArgs;
        public delegate T? Parser<T>(IEnumerable<string>? values, bool flag);
        public static class Parsers
        {
            public static Parser<string> FirstNonNullString => (values, _) => values?.SkipWhile(x => string.IsNullOrEmpty(x)).First();
            public static Parser<bool> Flag => (enumerable, flag) => enumerable is not null || flag;
            public static Parser<IEnumerable<string>?> Raw => (values, _) => values;
        }
        static CommandLineArgs()
        {
            _intermediateArgs = new(Environment.GetCommandLineArgs()[1..]);
            foreach((int pos, string warning) in _intermediateArgs.Warnings)
            {
                Utils.DebugLog($"Error in command-line args at position {pos}: {warning}");
            }
        }
        public static T? TryGet<T>(string argName, Parser<T> parser)
            => parser(_intermediateArgs[argName], false);
        public static bool GetFlag(string argName, char? flag = null)
            => Parsers.Flag(_intermediateArgs[argName], _intermediateArgs[flag ?? argName.First().ToLower()]);
        public static T Get<T>(string argName, Parser<T> parser, Exception? exception = null)
            => TryGet(argName, parser) ?? throw exception ?? new Exception($"Tried to get command-line argument {argName}, but it was not found!");
    }
}
