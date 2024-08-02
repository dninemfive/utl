using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace d9.utl.config;
public partial class CommandLineArgs
{
    public delegate IEnumerable<T?> Parser<T>(IntermediateArgs args, string varName);
}
