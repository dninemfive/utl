using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace d9.utl
{
    /// <summary>
    /// Manages configuration files.
    /// </summary>
    public static class Config
    {
        /// <summary>
        /// The path to the default config.
        /// </summary>
        [CommandLineArg("configPath", "FirstString")]
        public static string ConfigPath { get; }
        static Config()
        {
            _ = CommandLineArgs.Initialized;
            if(ConfigPath is null || !File.Exists(ConfigPath))
            {
                if (File.Exists("/config.cfg")) ConfigPath = "/config.cfg";
                else throw new Exception($"Was not able to find config at `{ConfigPath.PrintNull()}` or `/config.cfg`.");
            }
        }
    }
}
