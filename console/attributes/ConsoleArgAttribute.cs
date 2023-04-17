using d9.utl.console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace d9.utl.console
{
    /// <summary>
    /// Implements <see cref="IConsoleArg"/> for a generic type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ConsoleArgAttribute : Attribute, IConsoleArg
    {
        public string Key { get; private set; }
        public string Description { get; private set; }
        public ConsoleArgAttribute(string key, string description = "")
        {
            Key = key;
            Description = description;
        }
    }
}
