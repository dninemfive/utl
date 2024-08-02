using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using System.Security.Cryptography.X509Certificates;

namespace d9.utl.compat.google;

/// <summary>
/// A context in which Google Services may operate, providing authentication and logging when creating such services.
/// </summary>
public partial class GoogleServiceContext
{
    /// <summary>
    /// The config which defines this context's authorization.
    /// </summary>
    public GoogleAuthConfig Config { get; private set; }
    /// <summary>
    /// The log to which this context will write. If <see langword="null"/>, no log will be written to.
    /// </summary>
    public Log? Log { get; private set; }
    /// <summary>
    /// Creates a GoogleServiceContext from the specified <paramref name="config"/> and <paramref name="log"/>.
    /// </summary>
    /// <param name="config"><inheritdoc cref="Config" path="/summary"/></param>
    /// <param name="log"><inheritdoc cref="Log" path="/summary"/></param>
    public GoogleServiceContext(GoogleAuthConfig config, Log? log = null)
    {
        Config = config;
        Log = log;
    }
    /// <summary>
    /// Creates a GoogleServiceContext by loading the config from the specified <paramref name="configPath"/> and using the specified <paramref name="log"/>.
    /// </summary>
    /// <param name="configPath">The path from which the <see cref="Config"/> will be loaded.</param>
    /// <param name="log"><inheritdoc cref="Log" path="/summary"/></param>
    public GoogleServiceContext(string configPath, Log? log = null) : this(utl.Config.Load<GoogleAuthConfig>(configPath), log) { }
    /// <summary>
    /// Gets the Google Auth certificate from the (privately-stored) key and password files.
    /// </summary>
    /// <remarks>Largely a copy of code from <see href="https://www.daimto.com/google-drive-authentication-c/">this example</see>.<br/>
    /// <br/> Apparently the password is always <c>notasecret</c> and that can't be changed, which is strange.</remarks>
    public X509Certificate2 Certificate
        => new(Config.KeyPath, "notasecret", X509KeyStorageFlags.Exportable);
    /// <summary>
    /// Constructs a credential with the specified scopes.
    /// </summary>
    /// <remarks>Largely a copy of code from <see href="https://www.daimto.com/google-drive-authentication-c/">this example</see>.</remarks>
    /// <param name="scopes">The <see href="https://developers.google.com/identity/protocols/oauth2/scopes">Google scopes</see> the credential 
    /// is permitted to use.</param>
    public ServiceAccountCredential Credential(params string[] scopes)
        => new(new ServiceAccountCredential.Initializer(Config.Email) { Scopes = scopes }
                                           .FromCertificate(Certificate));
    /// <summary>
    /// Constructs a <see cref="BaseClientService.Initializer"/> with the specified <paramref name="scopes"/> and using this context's <see cref="GoogleAuthConfig.AppName"/>.
    /// </summary>
    /// <param name="scopes">The scopes with which to create the initializer.</param>
    public BaseClientService.Initializer InitializerFor(params string[] scopes)
        => new()
        {
            HttpClientInitializer = Credential(scopes),
            ApplicationName = Config.AppName
        };
}
