using d9.utl.console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace d9.utl.console
{
    /// <summary>
    /// Implements <see cref="IConsoleArg{T}"/> for a generic type as an attribute on a field or property in a <see cref="ConsoleArgs"/> class.
    /// </summary>
    /// <typeparam name="T">The type of the variable to set.</typeparam>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ConsoleArgAttribute<T> : Attribute, IConsoleArg<T>, IUntypedConsoleArg
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
        public object? Value => TryGet(out T? val) ? val : DefaultValue;
    }
}
