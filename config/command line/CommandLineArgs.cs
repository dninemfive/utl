using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace d9.utl
{
    /// <summary>
    /// Wrapper for a static constructor which initializes variables (fields or properties) tagged with <see cref="CommandLineArgAttribute"/>s
    /// on startup.
    /// </summary>
    /// <remarks>
    /// <u>Both</u> the assembly and the type containing the variable must have the <see cref="HasCommandLineArgsAttribute"/>,
    /// and the variable must itself be <see langword="static"/>.
    /// </remarks>
    public static class CommandLineArgs
    {
        /// <summary>
        /// Dummy variable used to ensure the static constructor runs.
        /// </summary>
        public static bool Initialized => true;
        /// <summary>
        /// Exception thrown if a non-static member has a 
        /// </summary>
        private static readonly Exception MemberMustBeStaticException
            = new($"Command-line argument variables must be static fields or properties with static setters.");
        static CommandLineArgs()
        {
            string[] args = Environment.GetCommandLineArgs()[1..];
            IntermediateArgs intermediateArgs = new(args);
            Utils.DebugLog($"Initializing ConsoleArgs with args `{args.PrettyPrint()}`...");
            foreach (Type type in ReflectionUtils.TypesInAssembliesWithAttribute<HasCommandLineArgParsersAttribute>())
            {
                Utils.DebugLog($"Looking for command-line args in {type.FullName}...");
                foreach ((MemberInfo mi, CommandLineArgAttribute cla) in type.MembersWithAttribute<CommandLineArgAttribute>())
                {
                    Utils.DebugLog($"\t{mi.FullyQualifiedPath()}");
                    Exception noParserException(Type type)
                        => new($"Tried to find CommandLineArgParser<{type.Name}>{cla.ParserKey} for {mi.Name}, but no such parser exists!");
                    if (mi is FieldInfo fi)
                    {
                        if (!fi.IsStatic) throw MemberMustBeStaticException;
                        CommandLineArgParser<object> clap = CommandLineArgParsers.Get(cla.ParserKey, fi.FieldType) ?? throw noParserException(fi.FieldType);
                        fi.SetValue(null, clap(cla, intermediateArgs));
                    }
                    else if (mi is PropertyInfo pi)
                    {
                        if (!(pi.GetGetMethod()?.IsStatic ?? false)) throw MemberMustBeStaticException;
                        CommandLineArgParser<object> clap = CommandLineArgParsers.Get(cla.ParserKey, pi.PropertyType) ?? throw noParserException(pi.PropertyType);
                        pi.SetValue(null, clap(cla, intermediateArgs));
                    }
                }
            }
        }
    }
}
