using Config.Net;
using d9.utl;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Microsoft.Extensions.Logging;
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
    public IGoogleAuthConfig Config { get; private set; }
    /// <summary>
    /// The log to which this context will write. If <see langword="null"/>, no log will be written to.
    /// </summary>
    public ILogger? Log { get; private set; }
    /// <summary>
    /// Creates a GoogleServiceContext from the specified <paramref name="config"/> and <paramref name="log"/>.
    /// </summary>
    /// <param name="config"><inheritdoc cref="Config" path="/summary"/></param>
    /// <param name="log"><inheritdoc cref="Log" path="/summary"/></param>
    public GoogleServiceContext(IGoogleAuthConfig config, ILogger? log = null)
    {
        Config = config;
        Log = log;
    }
    /// <summary>
    /// Creates a GoogleServiceContext by loading the config from the specified <paramref name="configPath"/> and using the specified <paramref name="log"/>.
    /// </summary>
    /// <param name="configPath">The path from which the <see cref="Config"/> will be loaded.</param>
    /// <param name="log"><inheritdoc cref="Log" path="/summary"/></param>
    internal GoogleServiceContext(string configPath, ILogger? log = null) 
        : this(new ConfigurationBuilder<IGoogleAuthConfig>().UseJsonFile(configPath).Build(), log) { }
    /// <summary>
    /// Tries to load a config at the specified path, catching and optionally logging any thrown errors.
    /// </summary>
    /// <param name="configPath">The path to the config to load.</param>
    /// <param name="log">A log which will print any errors and be passed to the new object.</param>
    /// <returns>
    /// A new <see cref="GoogleServiceContext"/> if a valid config was successfully loaded, or 
    /// <see langword="null"/> otherwise.
    /// </returns>
    public static GoogleServiceContext? TryLoad(string configPath, ILogger? log = null)
    {
        try
        {
            IGoogleAuthConfig config = new ConfigurationBuilder<IGoogleAuthConfig>().UseJsonFile(configPath).Build();
            if(!config.IsValid(out string? reasons))
            {
                log?.LogError("A GoogleAuthConfig was found at `{path}`, but it was invalid for the following reasons:\n`{reasons}`", configPath, reasons.Indent());
            } 
            else
            {
                return new(config, log);
            }
        }
        catch(Exception e)
        {
            log?.LogError("{errorMessage}", e.Summary());
        }
        return null;
    }
    /// <summary>
    /// Gets the Google Auth certificate from the (privately-stored) key and password files.
    /// </summary>
    /// <remarks>Largely a copy of code from <see href="https://www.daimto.com/google-drive-authentication-c/">this example</see>.<br/>
    /// <br/> Apparently the password is always <c>notasecret</c> and that can't be changed, which is strange.</remarks>
    public X509Certificate2 Certificate
    {
        get
        {
            string keyPath = Config.KeyPath.AbsoluteOrInBaseFolder();
            try
            {
                return X509CertificateLoader.LoadPkcs12FromFile(keyPath, "notasecret", X509KeyStorageFlags.Exportable);
            } 
            catch(Exception e)
            {
                throw new Exception($"Could not load certificate at path `{keyPath}`: {e.Summary()}");
            }
        }
    }
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
    /// Constructs a <see cref="BaseClientService.Initializer"/> with the specified <paramref name="scopes"/> and using this context's <see cref="IGoogleAuthConfig.AppName"/>.
    /// </summary>
    /// <param name="scopes">The scopes with which to create the initializer.</param>
    public BaseClientService.Initializer InitializerFor(params string[] scopes)
        => new()
        {
            HttpClientInitializer = Credential(scopes),
            ApplicationName = Config.AppName
        };
}
