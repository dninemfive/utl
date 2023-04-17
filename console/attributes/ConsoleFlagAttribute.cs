using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace d9.utl.console
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ConsoleFlagAttribute : Attribute, IConsoleArg
    {
        public char Alias { get; private set; }
        public string Key { get; private set; }
        public string Description { get; private set; }
        public ConsoleFlagAttribute(string key, char alias, string description = "")
        {
            Key = key;
            Alias = alias;
            Description = description;
        }
    }
}
