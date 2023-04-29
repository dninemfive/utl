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
    /// Utilities related to configuring the program using json files.
    /// </summary>
    public static class Config
    {
        /// <summary>
        /// The base folder path, passed in via command-line args, or the directory from which the program is executed if such a path is not specified.
        /// </summary>
        /// <remarks>It is intended that configuration files go into this folder or a subfolder, but this is not required.</remarks>
        public static readonly string BaseFolderPath = CommandLineArgs.TryGet(nameof(BaseFolderPath), CommandLineArgs.Parsers.FolderPath)
                                                    ?? Environment.CurrentDirectory;
        /// <summary>
        /// Tries to load a json file at the specified <c><paramref name="path"/></c>, catching any errors in case
        /// the file does not exist or is malformed.
        /// </summary>
        /// <typeparam name="T">The type to try to load the file as.</typeparam>
        /// <param name="path">The path to the file to load.</param>
        /// <param name="suppressWarnings">If <see langword="false"/>, warnings are printed, in debug mode only, if the file is not successfully loaded.</param>
        /// <returns>A <typeparamref name="T"/> instance loaded from the specified path, if successful, or <see langword="null"/> otherwise.</returns>
        public static T? TryLoad<T>(string? path, bool suppressWarnings = false)
        {
            T? failWithMessage(string msg)
            {
                if (!suppressWarnings) Utils.DebugLog($"Failed to load config at path `{path}`: {msg}!");
                return default;
            }
            path = path?.AbsolutePath();
            if (path is null || !File.Exists(path)) return failWithMessage("path does not point to an existing file");
            try
            {
                return JsonSerializer.Deserialize<T>(File.ReadAllText(path));
            } catch (Exception e)
            {
                return failWithMessage(e.Message);
            }
        }
        /// <summary>
        /// Checks that the specified object is not <see langword="null"/> and that it is <see cref="IValidityCheck">valid</see>.
        /// </summary>
        /// <param name="ivc">The object whose validity to check.</param>
        /// <returns><see langword="true"/> if the object is non-<see langword="null"/> and valid, or <see langword="false"/> otherwise.</returns>
        public static bool IsValid(this IValidityCheck? ivc) => ivc is not null && ivc.IsValid;
    }
    /// <summary>
    /// Provides a way to check whether an object which may not have been initialized correctly, e.g. a 
    /// <see cref="Config.TryLoad{T}(string?, bool)">config file loaded from json</see>, was in fact loaded correctly.
    /// </summary>
    public interface IValidityCheck
    {
        /// <summary>
        /// <see langword="true"/> if this object is fully and properly initialized, or <see langword="false"/> otherwise.
        /// </summary>
        public abstract bool IsValid { get; }
    }
}
