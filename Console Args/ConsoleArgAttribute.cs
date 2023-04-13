using d9.utl.console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace d9.utl.console
{
    /// <summary>
    /// Implements <see cref="IConsoleArg"/> for a generic type as an attribute on a field or property in a <see cref="ConsoleArgs"/> class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ConsoleArgAttribute : Attribute, IConsoleArg
    {
        /// <inheritdoc cref="IConsoleArg.DefaultValue"/>
        public object? DefaultValue { get; private set; }
        /// <summary>
        /// A function which takes the values for a console arg, if any, and returns an object, if applicable.
        /// </summary>
        private readonly Func<IEnumerable<string>?, object?> _parser;
        /// <summary>
        /// Creates a <c>ConsoleArgAttribute</c> with the specified characteristics.
        /// </summary>
        /// <param name="parser"><inheritdoc cref="_parser" path="/summary"/></param>
        /// <param name="defaultValue"><inheritdoc cref="DefaultValue" path="/summary"/></param>
        public ConsoleArgAttribute(Func<IEnumerable<string>?, object?> parser, object? defaultValue = null)
        {
            _parser = parser;
            DefaultValue = defaultValue;
        }
        /// <inheritdoc cref="IConsoleArg.Parse(IntermediateArgs, string)"/>
        public object? Parse(IntermediateArgs ia, string key) => _parser(ia[key]);
    }
}
