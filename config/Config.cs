using System.Text.Json;
using System.Text.Json.Serialization;
namespace d9.utl;
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
    /// The default serializer options to use when writing and reading config files.
    /// </summary>
    public static readonly JsonSerializerOptions DefaultSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true,
        IncludeFields = true,
        Converters =
        {
            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
        }
    };
    public static T Load<T>(string path)
        => JsonSerializer.Deserialize<T>(File.ReadAllText(path.AbsoluteOrInBaseFolder()), DefaultSerializerOptions)!;
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
        Utils.DebugLog($"TryLoad<{typeof(T).Name}>({path.PrintNull()}, {suppressWarnings})");
        T? failWithMessage(string msg)
        {
            if (!suppressWarnings) Utils.DebugLog($"Failed to load config at path `{path}`: {msg}!");
            return default;
        }
        path = path?.AbsoluteOrInBaseFolder();
        if (path is null || !File.Exists(path)) return failWithMessage("path does not point to an existing file");
        try
        {
            return JsonSerializer.Deserialize<T>(File.ReadAllText(path), DefaultSerializerOptions);
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