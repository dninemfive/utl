using d9.utl.config;

namespace d9.utl;
public partial class CommandLineArgs
{
    public partial class Parsers
    {
        public static d9.utl.CommandLineArgs.Parser<string> Raw => (args, name) => args[name];
        public static Parser<bool> Flag => (args, name) => [args[name].Any() || args[name.First()]];
    }
}