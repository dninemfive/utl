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
    public interface IConsoleArg<T> : IUntypedConsoleArg
    {
        /// <summary>
        /// The default value, if any, to return if the variable was not specified in the console args.
        /// </summary>
        public new T? DefaultValue { get; }
        public T? Parse<T>(IntermediateArgs args);
    }
    public interface IUntypedConsoleArg
    {
        public object? DefaultValue { get; }
        public object? Parse(IntermediateArgs args);
    }
}
