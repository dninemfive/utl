using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace d9.utl
{
    /// <summary>
    /// Manages configuration files.
    /// </summary>
    public static class Config
    {
        public static readonly string BaseFolderPath = Path.GetFullPath(CommandLineArgs.Get(nameof(BaseFolderPath), CommandLineArgs.Parsers.FirstNonNullString));
        private static readonly Dictionary<string, Dictionary<string, string>> _configs = new();
        public delegate T? Parser<T>(string s);
        public static class Parsers 
        {
            public static Parser<string> String => s => s;
        }
        public static T? TryGet<T>(string variableName, string configPath, Parser<T> parser)
        {
            if(!_configs.ContainsKey(configPath)) 
        }
        public static Config Load(string path)
        {
            if (_configs.ContainsKey(path)) Utils.DebugLog($"Reloading config at {path}.");
            Dictionary<string, string> cfg = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(path));
            _configs[path] = new();
        }
    }
}
