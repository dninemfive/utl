using d9.utl.console;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace d9.utl.console
{
    public delegate T? ConsoleArgParser<T>(IConsoleArg ica, IntermediateArgs ia);
    public class ConsoleArgParserAttribute : Attribute
    {
        public string Name { get; private set; }
        public ConsoleArgParserAttribute(string name)
        {
            Name = name;
        }
    }
    public static class ConsoleArgParsers
    {
        public static IReadOnlyDictionary<(string, Type), ConsoleArgParser<object>> Dictionary => _consoleArgParsers;
        private static readonly Dictionary<(string, Type), ConsoleArgParser<object>> _consoleArgParsers = new();
        static ConsoleArgParsers()
        {
            foreach(Type t in ReflectionUtils.AllLoadedTypesWithAttribute(typeof(HasConsoleArgParsersAttribute)))
            {
                foreach(FieldInfo fi in t.GetFields())
                {
                    
                }
            }
        }
        public static readonly ConsoleArgParser<bool> Flag = delegate (IConsoleArg ica, IntermediateArgs ia)
        {
            if (ica is ConsoleFlagAttribute cfa)
            {
                return ia.Flags.Contains(cfa.Alias);
            }
            return ia.ContainsKey(ica.Key);
        };
        public static readonly ConsoleArgParser<string> FirstString = delegate (IConsoleArg ica, IntermediateArgs ia)
        {
            return ia[ica.Key]?.First() ?? null;
        };
    }
    [AttributeUsage(AttributeTargets.Class)]
    public class HasConsoleArgParsersAttribute : Attribute { }
}
