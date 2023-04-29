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
        public static readonly string BaseFolderPath = CommandLineArgs.TryGetDirectory(nameof(BaseFolderPath)) ?? Environment.CurrentDirectory;
        public static T? TryLoad<T>(string? path, bool suppressWarnings = false)
        {
            if (path is null || !File.Exists(path))
            {
                if (!suppressWarnings) Utils.DebugLog($"Failed to load config at path `{path}`: path does not point to an existing file!");
                return default;
            }
            try
            {
                return JsonSerializer.Deserialize<T>(File.ReadAllText(path));
            } catch (Exception e)
            {
                if (!suppressWarnings) Utils.DebugLog($"Failed to load config at path `{path}`: {e.Message}");
                return default;
            }
        }
        public static bool IsValid(this IValidityCheck? ivc) => ivc is not null && ivc.IsValid;
    }
    public interface IValidityCheck
    {
        public abstract bool IsValid { get; }
    }
}
