using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace d9.utl.console
{
    /// <summary>
    /// Defines a method of getting a value from a set of console _args.
    /// </summary>
    public interface IConsoleArg
    {
        public string Key { get; }
        public string Description { get; }
    }
}
