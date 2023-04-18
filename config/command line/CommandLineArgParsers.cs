using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace d9.utl
{
    /// <summary>
    /// A delegate type which supports initializing a command-line arg using its <see cref="CommandLineArgAttribute">attribute</see>
    /// and the <see cref="IntermediateArgs"/> parsed from the command-line arguments passed to the program at startup.
    /// </summary>
    /// <typeparam name="T">The type this parser parses to.</typeparam>
    /// <param name="cla">A <see cref="CommandLineArgAttribute"/> corresponding to the variable to be initialized.</param>
    /// <param name="ia">The <see cref="IntermediateArgs"/> parsed from the command-line arguments passed to the program at startup.</param>
    /// <returns>The value, if any, parsed from the given variables. If no value was successfully parsed, returns <see langword="null"/>.</returns>
    public delegate T? CommandLineArgParser<T>(CommandLineArgAttribute cla, IntermediateArgs ia);
    /// <summary>
    /// Static class which loads and initializes <see cref="CommandLineArgParser{T}">command-line argument parsers</see> to be used to initialize
    /// <see cref="CommandLineArgAttribute">command-line args</see>.
    /// </summary>
    [HasCommandLineArgParsers]
    public static class CommandLineArgParsers
    {
        private static readonly Dictionary<(string, Type), CommandLineArgParser<object>> _consoleArgParsers = new();
        private static readonly Exception MemberMustBeStaticException 
            = new($"Command-line argument parsers must be static fields or properties with static getters.");
        static CommandLineArgParsers()
        {
            if (!CommandLineArgs.Initialized) throw new Exception($"Somehow, the console args are not initialized.");
            static Exception keyAlreadyPresentException((string name, Type type) key)
                => new($"Attempted to add Command-line argument parser with key ({key.name}, {key.type.Name}), but the key was already present!");
            foreach (Type t in ReflectionUtils.AllLoadedTypesWithAttribute<HasCommandLineArgParsersAttribute>())
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
        /// <summary>
        /// Parses <see langword="bool"/>s from command-line args, returning <see langword="true"/> if the argument's <see cref="CommandLineArgAttribute.Key">key</see>
        /// or its <see cref="CommandLineArgAttribute.Alias">alias</see> is present at least once.
        /// </summary>
        [CommandLineArgParser("flag")]
        public static readonly CommandLineArgParser<bool> Flag = delegate (CommandLineArgAttribute cla, IntermediateArgs ia)
        {
            if (cla.Alias != Constants.NullCharacter && ia.Flags.Contains(cla.Alias)) return true;
            return ia.ContainsKey(cla.Key);
        };
        /// <summary>
        /// Returns the first <see langword="string"/> from the values corresponding to the <see cref="CommandLineArgAttribute">key</see> of the given arg.
        /// </summary>
        public static readonly CommandLineArgParser<string> FirstString = delegate (CommandLineArgAttribute cla, IntermediateArgs ia)
        {
            return ia[cla.Key]?.First() ?? null;
        };
        /// <summary>
        /// Uses <see cref="IParsable{TSelf}"/> to parse a generic type, selecting <see langword="string"/>s from the values 
        /// corresponding to the <see cref="CommandLineArgAttribute">key</see> using the given <c><paramref name="stringSelector"/></c>
        /// </summary>
        /// <typeparam name="T">The type to parse to.</typeparam>
        /// <param name="stringSelector">A function which selects a string from among the given values to be used to parse.</param>
        /// <returns>A <see cref="CommandLineArgParser{T}"/> which parses generically as described above.</returns>
        public static CommandLineArgParser<T> Generic<T>(Func<IEnumerable<string>?, string?>? stringSelector = null) where T : IParsable<T>
        {
            stringSelector ??= x => x?.First();
            return delegate (CommandLineArgAttribute cla, IntermediateArgs ia)
            {
                string? s = stringSelector(ia[cla.Key]);
                if (s is null) return default;
                return T.Parse(s, null);
            };
        }
        /// <summary>
        /// Gets the parser with the specified <c><paramref name="key"/></c> for the specified <c><paramref name="type"/></c>.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static CommandLineArgParser<object>? Get(string key, Type type) => _consoleArgParsers[(key, type)];
        public static CommandLineArgParser<T>? Get<T>(string key, Type type) => _consoleArgParsers[(key, type)] as CommandLineArgParser<T>;
    }
    
}
