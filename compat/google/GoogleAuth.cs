using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Download;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;
using d9.utl;

namespace d9.utl.compat.google;

/// <summary>
/// Utilities for authenticating and interfacing with Google services.
/// </summary>
public partial class GoogleAuth
{
    public GoogleAuthConfig Config { get; private set; }
    public GoogleAuth(GoogleAuthConfig config)
    {
        Config = config;
    }
    public GoogleAuth(string configPath) : this(utl.Config.Load<GoogleAuthConfig>(configPath)) { }
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
    public BaseClientService.Initializer InitializerFor(params string[] scopes)
        => new()
        {
            HttpClientInitializer = Credential(scopes),
            ApplicationName = Config.AppName
        };
}
