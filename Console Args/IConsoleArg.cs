using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace d9.utl.console
{
    /// <summary>
    /// Represents the concept of getting a specific type from a console arg.
    /// </summary>
    /// <typeparam name="T">The desired type.</typeparam>
    public interface IConsoleArg<T>
    {
        /// <summary>
        /// The name of the variable used in the command line args.
        /// </summary>
        public string Key { get; }
        /// <summary>
        /// Tries to get the value of this variable, if any.
        /// </summary>
        /// <param name="value">The resulting value, or <see cref="DefaultValue">the default</see> if no value was specified.</param>
        /// <returns><see langword="true"/> if a value exists or <see langword="false"/> otherwise.</returns>
        public bool TryGet(out T? value);
        /// <summary>
        /// The default value, if any, to return if the variable was not specified in the console args.
        /// </summary>
        public T? DefaultValue { get; }
    }
    /// <summary>
    /// Implements <see cref="IConsoleArg{T}"/> for a generic type as an attribute on a field or property in a <see cref="ConsoleArgs"/> class.
    /// </summary>
    /// <typeparam name="T">The type of the variable to set.</typeparam>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ConsoleArgAttribute<T> : Attribute, IConsoleArg<T>
    {
        /// <inheritdoc cref="IConsoleArg{T}.Key"/>
        public string Key => "asdf";
        /// <inheritdoc cref="IConsoleArg{T}.DefaultValue"/>
        public T? DefaultValue => default;
        /// <inheritdoc cref="IConsoleArg{T}.TryGet(out T)"/>
        public bool TryGet(out T? value)
        {
            value = DefaultValue;
            return false;
        }
    }
    /// <summary>
    /// Implements <see cref="IConsoleArg{T}"/> for boolean flags which can be specified either with their full <see cref="Key">Key</see>
    /// or with a single-character abbreviation.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ConsoleFlagAttribute : Attribute, IConsoleArg<bool>
    {
        /// <summary>
        /// The single-character abbreviation for this flag.
        /// </summary>
        public char Alias { get; private set; }
        /// <summary>
        /// The default value for this flag.
        /// </summary>
        public bool DefaultValue { get; private set; } = false;
        /// <summary>
        /// The unabbreviated version of this flag.
        /// </summary>
        public string Key => "asdf";
        /// <inheritdoc cref="IConsoleArg{T}.TryGet(out T)"/>
        public bool TryGet(out bool val)
        {
            val = DefaultValue;
            return false;
        }

    }
}
