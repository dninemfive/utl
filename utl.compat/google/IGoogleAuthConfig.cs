namespace d9.utl.compat.google;
/// <summary>
/// Configuration class which loads the necessary variables for Google authentication.
/// </summary>
/// <remarks>For more info, see <see href="https://console.cloud.google.com/"/>.</remarks>
public interface IGoogleAuthConfig
{
    /// <summary>
    /// The path to a <see href="https://en.wikipedia.org/wiki/PKCS_12">p12</see> file containing the key for the desired Google service.
    /// </summary>
    [InvalidIfNotFile]
    public string KeyPath { get; }
    /// <summary>
    /// The email associated with the service in OAuth. This is not the email for the account which created the service, but rather the
    /// one provided when you register your project at <see href="https://console.cloud.google.com/"/>.
    /// </summary>
    [InvalidIfNull]
    public string Email { get; }
    /// <summary>
    /// The name of the application to authenticate with.
    /// </summary>
    [InvalidIfNull]
    public string AppName { get; }
}