using d9.utl.console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace d9.utl
{
    public static class Config
    {
        [CommandLineArg("configPath")]
        public static string ConfigPath { get; }
    }
}
