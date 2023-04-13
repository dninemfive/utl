using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace d9.utl.console
{
    /// <summary>
    /// Implements <see cref="IConsoleArg{T}"/> for boolean flags which can be specified either with their full <see cref="Key">Key</see>
    /// or with a single-character abbreviation.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ConsoleFlagAttribute : Attribute, IConsoleArg<bool>, IUntypedConsoleArg
    {
        public char Alias { get; private set; }
        public bool DefaultValue { get; private set; } = false;
        public ConsoleFlagAttribute(char alias, bool defaultValue = false)
        {
            Alias = alias;
            DefaultValue = defaultValue;
        }
    }
}
