using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace d9.utl;
/// <summary>
/// Utilities related to configuring the program using json files.
/// </summary>
public static class Config
{
    /// <summary>
    /// The base folder path, passed in via command-line args, or the directory from which the
    /// program is executed if such a path is not specified.
    /// </summary>
    /// <remarks>
    /// It is intended that configuration files go into this folder or a subfolder, but this is not required.
    /// </remarks>
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
    /// Tries to load a JSON config file at the specified <c><paramref name="path"/></c>, catching
    /// any errors in case the file does not exist or is malformed.
    /// </summary>
    /// <typeparam name="T"><inheritdoc cref="Load{T}(string)" path="/typeparam[@name='T']"/></typeparam>
    /// <param name="path"><inheritdoc cref="Load{T}(string)" path="/param[@name='path']"/></param>
    /// <param name="errorMessage">
    /// If the config is not successfully loaded, contains a message describing why it was not successful;
    /// if it is successful, it will be <see langword="null"/>.
    /// </param>
    /// <returns>
    /// A <typeparamref name="T"/> instance loaded from the specified path, if successful, or <see
    /// langword="default"/>(<typeparamref name="T"/>) otherwise.
    /// </returns>
    public static T? TryLoad<T>(string? path, out string? errorMessage)
    {
        errorMessage = null;
        T? failWithMessage(string input, out string output)
        {
            output = $"Failed to load config at path `{path}`: {input}!";
            return default;
        }
        path = path?.AbsoluteOrInBaseFolder();
        if (path is null || !File.Exists(path))
            return failWithMessage("path does not point to an existing file", out errorMessage);
        try
        {
            return JsonSerializer.Deserialize<T>(File.ReadAllText(path), DefaultSerializerOptions);
        }
        catch (Exception e)
        {
            return failWithMessage($"{e.GetType().Name}: {e.Message}", out errorMessage);
        }
    }
}