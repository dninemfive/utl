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
        public static readonly string BaseFolderPath = Path.GetFullPath(CommandLineArgs.Get(nameof(BaseFolderPath), CommandLineArgs.Parsers.FirstNonNullOrEmptyString));
        private static readonly Dictionary<string, Dictionary<string, object>> _configs = new();
        public delegate T? Parser<T>(object obj);
        public static class Parsers 
        {
            public static Parser<string> String => s => s.ToString();
        }
        public static T? TryGet<T>(string variableName, string configPath, Parser<T> parser)
        {
            if (!_configs.ContainsKey(configPath)) return parser(_configs[configPath][variableName]);
            return default;
        }
        public static T Get<T>(string variableName, string configPath, Parser<T> parser, Exception? exception = null)
            => TryGet(variableName, configPath, parser) ?? throw exception ?? new Exception($"Failed to get config variable {variableName} in config file `{configPath}`!");
        public static void Load(string path)
        {
            path = path.AbsolutePath();
            if (_configs.ContainsKey(path)) Utils.DebugLog($"Reloading config at {path}.");
            try
            {
                _configs[path] = JsonSerializer.Deserialize<Dictionary<string, object>>(File.ReadAllText(path))!;
            } catch(Exception e)
            {
                Utils.DebugLog($"Caughted exception while deserializing in `Config.Load({path})`: {e.Message}");
            }
            // potential thing: if infinite loop occurs when trying to load, add a null value for the given config?
        }
        public static void Load(string key, Dictionary<string, object> dict)
        {
            _configs[key] = dict;
        }
    }
}
