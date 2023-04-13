using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace d9.utl.console
{
    /// <summary>
    /// Implements <see cref="IConsoleArg"/> for boolean flags which can be specified either with their full variable name
    /// or with a single-character abbreviation.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ConsoleFlagAttribute : Attribute, IConsoleArg
    {
        /// <summary>
        /// The single-character alias which, when present, activates this flag.
        /// </summary>
        public char Alias { get; private set; }
        /// <inheritdoc cref="IConsoleArg.DefaultValue"/>
        public object DefaultValue => _defaultValue;
        /// <summary>
        /// Backing variable for <see cref="DefaultValue"/>. Separate in order to be a <see langword="bool"/> and therefore support the <c>!</c>
        /// operator.
        /// </summary>
        private readonly bool _defaultValue;
        /// <summary>
        /// Creates a <c>ConsoleFlagAttribute</c> with the specified characteristics.
        /// </summary>
        /// <param name="alias"><inheritdoc cref="Alias" path="/summary"/></param>
        /// <param name="defaultValue"><inheritdoc cref="DefaultValue" path="/summary"/></param>
        public ConsoleFlagAttribute(char alias, bool defaultValue = false)
        {
            Alias = alias;
            _defaultValue = defaultValue;
        }
        /// <summary>
        /// Gets the value for the flag from the console _args.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="key"><inheritdoc cref="IConsoleArg.Parse(IntermediateArgs, string)" path="/params[@name=key]"/></param>
        /// <returns><see langword="true"/> if the flag alias was used at least once or the <c>key</c> was present at all in the arguments.</returns>
        public object Parse(IntermediateArgs args, string key) => args.Flags.Any(c => c == Alias) || args[key] is not null ? _defaultValue : !_defaultValue;
    }
}
