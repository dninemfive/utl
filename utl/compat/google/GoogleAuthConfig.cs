using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.Json.Serialization;

namespace d9.utl.compat.google;
/// <summary>
/// Configuration class which loads the necessary variables for Google authentication.
/// </summary>
/// <remarks>For more info, see <see href="https://console.cloud.google.com/"/>.</remarks>
public class GoogleAuthConfig : IValidityCheck
{
    /// <summary>
    /// Whether or not the <see cref="GoogleAuthConfig"/> has been fully and correctly loaded. Implements <see cref="IValidityCheck"/>.
    /// <br/><br/>Specifically, all fields (<see cref="KeyPath">KeyPath</see>, <see cref="Email">Email</see>, and <see cref="AppName">AppName</see>) must be non-<see langword="null"/>,
    /// and <c>KeyPath</c> must point to an existing file.
    /// </summary>
    /// <param name="reason"><inheritdoc cref="IValidityCheck.IsValid(out string?)" path="/param[@name='reason']"/></param>
    /// <returns><inheritdoc cref="IValidityCheck.IsValid(out string?)" path="/returns"/></returns>
    /// <remarks><b>NOTE:</b> does not check whether <c>KeyPath</c> is a valid key, <c>Email</c> is a valid email, or <c>AppName</c> is a valid app name.</remarks>
    public bool IsValid([NotNullWhen(false)] out string? reason)
    {
        reason = null;
        if (this.AnyFieldsAreNull(out IEnumerable<FieldInfo> nullFields))
        {
            reason = $"{nullFields.Names().NaturalLanguageList("and")} are null";
            return false;
        }
        string? absoluteKeyPath = KeyPath?.AbsoluteOrInBaseFolder();
        if (!File.Exists(absoluteKeyPath))
            reason = $"Could not find authentication key at {absoluteKeyPath}";
        return reason is null;
    }
    /// <summary>
    /// The path to a <see href="https://en.wikipedia.org/wiki/PKCS_12">p12</see> file containing the key for the desired Google service.
    /// </summary>
    [JsonInclude]
    public required string KeyPath;
    /// <summary>
    /// The email associated with the service in OAuth. This is not the email for the account which created the service, but rather the
    /// one provided when you register your project at <see href="https://console.cloud.google.com/"/>.
    /// </summary>
    [JsonInclude]
    public required string Email;
    /// <summary>
    /// The name of the application to authenticate with.
    /// </summary>
    [JsonInclude]
    public required string AppName;
}