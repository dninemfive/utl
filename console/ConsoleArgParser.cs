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
    [HasCommandLineArgParsers]
    public static class CommandLineArgParsers
    {
        private static readonly Dictionary<(string, Type), CommandLineArgParser<object>> _consoleArgParsers = new();
        private static readonly Exception MemberMustBeStaticException 
            = new($"Command-line argument parsers must be static fields or properties with static getters.");
        static CommandLineArgParsers()
        {
            if (!ConsoleArgs.Initialized) throw new Exception($"Somehow, the console args are not initialized.");
            static Exception keyAlreadyPresentException((string name, Type type) key)
                => new($"Attempted to add Command-line argument parser with key ({key.name}, {key.type.Name}), but the key was already present!");
            foreach (Type t in ReflectionUtils.AllLoadedTypesWithAttribute(typeof(HasCommandLineArgParsersAttribute)))
            {
                foreach((MemberInfo mi, CommandLineArgParserAttribute clap) in t.MembersWithAttribute<CommandLineArgParserAttribute>())
                {
                    Exception notACommandLineArgParserException = new($"Variable {mi.Name} has a CommandLineArgParserAttribute, but it is not a CommandLineArgParser!");
                    if(mi is FieldInfo fi)
                    {
                        if (!fi.IsStatic) throw MemberMustBeStaticException;
                        (string name, Type type) key = (clap.Name, fi.FieldType);
                        if (_consoleArgParsers.ContainsKey(key))
                            throw keyAlreadyPresentException(key);
                        if (fi.GetValue(null) is CommandLineArgParser<object> clap2)
                        {
                            _consoleArgParsers.Add(key, clap2);
                        }
                        else throw notACommandLineArgParserException;
                    } else if(mi is PropertyInfo pi)
                    {
                        if (!(pi.GetMethod?.IsStatic ?? false)) throw MemberMustBeStaticException;
                        (string name, Type type) key = (clap.Name, pi.PropertyType);
                        if (_consoleArgParsers.ContainsKey(key)) throw keyAlreadyPresentException(key);
                        if (pi.GetValue(null) is CommandLineArgParser<object> clap2)
                        {
                            _consoleArgParsers.Add(key, clap2);
                        }
                        else throw notACommandLineArgParserException;
                    }
                }
            }
        }
        [CommandLineArgParser("flag")]
        public static readonly CommandLineArgParser<bool> Flag = delegate (CommandLineArgAttribute cla, IntermediateArgs ia)
        {
            if (cla.Alias != Constants.NullCharacter && ia.Flags.Contains(cla.Alias)) return true;
            return ia.ContainsKey(cla.Key);
        };
        public static readonly CommandLineArgParser<string> FirstString = delegate (CommandLineArgAttribute cla, IntermediateArgs ia)
        {
            return ia[cla.Key]?.First() ?? null;
        };
        public static CommandLineArgParser<T> Generic<T>(Func<IEnumerable<string>?, string?>? func = null) where T : IParsable<T>
        {
            func ??= x => x?.First();
            return delegate (CommandLineArgAttribute cla, IntermediateArgs ia)
            {
                string? s = func(ia[cla.Key]);
                if (s is null) return default;
                return T.Parse(s, null);
            };
        }
        public static CommandLineArgParser<object>? Get(string key, Type type) => _consoleArgParsers[(key, type)];
        public static CommandLineArgParser<T>? Get<T>(string key, Type type) => _consoleArgParsers[(key, type)] as CommandLineArgParser<T>;
    }
    
}
