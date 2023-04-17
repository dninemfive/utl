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
    public delegate T? CommandLineArgParser<T>(CommandLineArgAttribute cla, IntermediateArgs ia);
    public class CommandLineArgParserAttribute : Attribute
    {
        public string Name { get; private set; }
        public CommandLineArgParserAttribute(string name)
        {
            Name = name;
        }
    }
    public static class ConsoleArgParsers
    {
        private static readonly Dictionary<(string, Type), CommandLineArgParser<object>> _consoleArgParsers = new();
        static ConsoleArgParsers()
        {
            foreach(Type t in ReflectionUtils.AllLoadedTypesWithAttribute(typeof(HasConsoleArgParsersAttribute)))
            {
                foreach(FieldInfo fi in t.GetFields())
                {
                    CommandLineArgParserAttribute? clap = fi.GetCustomAttribute<CommandLineArgParserAttribute>();
                    if(clap is not null)
                    {
                        if (!fi.IsStatic) throw new Exception($"Command-line argument parsers must be static fields or properties.");
                        (string name, Type type) key = (clap.Name, fi.FieldType);
                        if (_consoleArgParsers.ContainsKey(key)) 
                            throw new Exception($"Attempted to add Command-line argument parser with key ({key.name}, {key.type.Name}), but the key was already present!");
                        if (fi.GetValue(null) is CommandLineArgParser<object> clap2)
                        {
                            _consoleArgParsers.Add(key, clap2);
                        }
                        else throw new Exception($"Variable {fi.Name} has a CommandLineArgParser attribute, but it is not of the correct type!");
                    }
                }
            }
        }
        [CommandLineArgParser("flag")]
        public static readonly CommandLineArgParser<bool> Flag = delegate (CommandLineArgAttribute cla, IntermediateArgs ia)
        {
            if (cla.Alias is not null && ia.Flags.Contains(cla.Alias.Value)) return true;
            return ia.ContainsKey(cla.Key);
        };
        public static readonly CommandLineArgParser<string> FirstString = delegate (CommandLineArgAttribute cla, IntermediateArgs ia)
        {
            return ia[cla.Key]?.First() ?? null;
        };
        public static CommandLineArgParser<object>? Get(string key, Type type) => _consoleArgParsers[(key, type)];
        public static CommandLineArgParser<T>? Get<T>(string key, Type type) => _consoleArgParsers[(key, type)] as CommandLineArgParser<T>;
    }
    
}
