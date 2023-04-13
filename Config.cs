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
        [ConsoleArg((x) => x?.Where(x => File.Exists(x)).First())]
        public static string ConfigPath { get; }
    }
}
