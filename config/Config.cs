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
    /// <summary>
    /// Loads a JSON config file at the specified <paramref name="path"/>, allowing any errors to be thrown.
    /// </summary>
    /// <typeparam name="T">The type defining the config to load.</typeparam>
    /// <param name="path">The path to the file to load.</param>
    /// <returns></returns>
    public static T Load<T>(string path)
        => JsonSerializer.Deserialize<T>(File.ReadAllText(path.AbsoluteOrInBaseFolder()), DefaultSerializerOptions)!;
    /// <summary>
    /// Tries to load a JSON config file at the specified <c><paramref name="path"/></c>, catching any errors in case
    /// the file does not exist or is malformed.
    /// </summary>
    /// <typeparam name="T"><inheritdoc cref="Load{T}(string)" path="/typeparam[@name='T']"/></typeparam>
    /// <param name="path"><inheritdoc cref="Load{T}(string)" path="/param[@name='path']"/></param>
    /// <param name="suppressWarnings">If <see langword="false"/> and the program has the Debug flag applied, any caught exceptions will be printed to the <see cref="Utils.DefaultLog"/>.</param>
    /// <returns>A <typeparamref name="T"/> instance loaded from the specified path, if successful, or <see langword="default"/>(<typeparamref name="T"/>) otherwise.</returns>
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
}