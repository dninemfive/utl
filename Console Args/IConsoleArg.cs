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
        /// <summary>
        /// The default value, if any, to return if the variable was not specified in the console _args.
        /// </summary>
        public object? DefaultValue { get; }
        /// <summary>
        /// Attempts to get a value for a specified variable name from the specified arguments.
        /// </summary>
        /// <param name="args">The arguments passed to the program, parsed into <see cref="IntermediateArgs"/>.</param>
        /// <param name="key">The name of the variable whose value, if any, to use.</param>
        /// <returns>An object corresponding to that variable's value, if any, or <see langword="null"/> otherwise.</returns>
        public object? Parse(IntermediateArgs args, string key);
    }
}
