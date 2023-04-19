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
        public static readonly string FolderPath = CommandLineArgs.Get("baseFolderPath", CommandLineArgs.Parsers.FirstNonNullString);
        static Config()
        {
        }
    }
}
