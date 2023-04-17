using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace d9.utl.console
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class)]
    public class HasConsoleArgsAttribute : Attribute { }
}
