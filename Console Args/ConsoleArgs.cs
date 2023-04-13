using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace d9.utl.console
{
    /// <summary>
    /// Static class which initializes any variables with a <see cref="ConsoleArgAttribute"/> or <see cref="ConsoleFlagAttribute"/>.
    /// </summary>
    public static class ConsoleArgs
    {
        private static IntermediateArgs? _args = null;
        /// <summary>
        /// Gets the value of the specified console arg using the specified name.
        /// </summary>
        /// <remarks><b>Throws an <see cref="Exception"/> if the class has not been <see cref="Init(string[])">initialized</see>.</b></remarks>
        /// <param name="ica">The <see cref="IConsoleArg"/> to use to initialize the variable.</param>
        /// <param name="key">The name of the variable.</param>
        /// <returns>The corresponding object, if any, or <see langword="null"/> otherwise.</returns>
        /// <exception cref="Exception">Thrown if the class has not been <see cref="Init(string[])">initialized</see>.</exception>
        private static object? Get(IConsoleArg ica, string key) 
            => _args is not null ? ica.Parse(_args, key) 
                                 : throw new Exception("Attempted to get a variable, but ConsoleArgs was not initialized!");
        /// <inheritdoc cref="Get(IConsoleArg, string)"/>
        public static T? Get<T>(IConsoleArg ica, string key) => (T)Get(ica, key)!;
        /// <summary>
        /// Initializes the ConsoleArgs using the specified <see langword="args"/>. <b>Must be called before using either
        /// <see cref="Get(IConsoleArg, string)">Get()</see> method!</b>
        /// </summary>
        /// <param name="args">The <see langword="args"/> to use to initialize the class.</param>
        /// <exception cref="Exception">Thrown if multiple attributes have been applied to the same field.</exception>
        public static void Init(string[] args)
        {
            Utils.DebugLog($"{(_args is null ? "I" : "Rei")}nitializing ConsoleArgs with args `{args.PrettyPrint()}`.");
            _args = new(args);
            // todo: assembly- and type-level attributes to filter faster
            foreach(Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach(Type type in assembly.GetTypes())
                {
                    foreach(FieldInfo field in type.GetFields())
                    {
                        Attribute[] attributes = Attribute.GetCustomAttributes(field, typeof(IConsoleArg));
                        if (!attributes.Any()) continue;
                        if (attributes.Length > 1) throw new Exception($"Can't have multiple console arg attributes on one field!");
                        if(attributes.First() is IConsoleArg ica)
                        {
                            field.SetValue(null, Get(ica, field.Name));
                        }
                    }
                }
            }
        }
    }
}
