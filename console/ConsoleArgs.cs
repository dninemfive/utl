using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace d9.utl.console
{
    public static class ConsoleArgs
    {

        public static bool Initialized => true;
        private static readonly Exception MemberMustBeStaticException
            = new($"Command-line argument variables must be static fields or properties with static setters.");
        static ConsoleArgs()
        {
            string[] args = Environment.GetCommandLineArgs()[1..];
            IntermediateArgs intermediateArgs = new(args);
            Utils.DebugLog($"Initializing ConsoleArgs with args `{args.PrettyPrint()}`.");
            foreach(Type type in ReflectionUtils.TypesInAssembliesWithAttribute(typeof(HasCommandLineArgsAttribute))) {
                foreach((MemberInfo mi, CommandLineArgAttribute cla) in type.MembersWithAttribute<CommandLineArgAttribute>())
                {
                    Exception noParserException(Type type) 
                        => new($"Tried to find CommandLineArgParser<{type.Name}>{cla.ParserKey} for {mi.Name}, but no such parser exists!");
                    if (mi is FieldInfo fi)
                    {
                        if (!fi.IsStatic) throw MemberMustBeStaticException;
                        CommandLineArgParser<object> clap = CommandLineArgParsers.Get(cla.ParserKey, fi.FieldType) ?? throw noParserException(fi.FieldType);
                        fi.SetValue(null, clap(cla, intermediateArgs));
                    } else if(mi is PropertyInfo pi)
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
