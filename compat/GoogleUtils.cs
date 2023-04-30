using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Download;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace d9.utl.compat
{
    /// <summary>
    /// Utilities for authenticating and interfacing with Google services.
    /// </summary>
    public static class GoogleUtils
    {
        /// <summary>
        /// Configuration class which loads the necessary variables for Google authentication.
        /// </summary>
        /// <remarks>For more info, see <see href="https://console.cloud.google.com/"/>.</remarks>
        private class GoogleAuthConfig : IValidityCheck
        {
            /// <summary>
            /// Whether or not the <see cref="GoogleAuthConfig"/> has been fully and correctly loaded. Implements <see cref="IValidityCheck"/>.
            /// </summary>
            [JsonIgnore]
            public bool IsValid => !(new string?[] { KeyPath, Email, AppName }.Any(x => x is null));
            // initialized by JsonSerializer
#pragma warning disable CS0649
            /// <summary>
            /// The path to a <see href="https://en.wikipedia.org/wiki/PKCS_12">p12</see> file containing the key for the desired Google service.
            /// </summary>
            [JsonInclude]
            public string? KeyPath;
            /// <summary>
            /// The email associated with the service in OAuth. This is not the email for the account which created the service, but rather the
            /// one provided when you register your project at <see href="https://console.cloud.google.com/"/>.
            /// </summary>
            [JsonInclude]
            public string? Email;
            /// <summary>
            /// The name of the application to authenticate with.
            /// </summary>
            [JsonInclude]
            public string? AppName;
#pragma warning restore CS0649
        }
        /// <summary>
        /// The path to the <see cref="GoogleAuthConfig">config file</see> for Google authentication, provided via command-line argument.
        /// </summary>
        private static readonly string? ConfigPath = CommandLineArgs.TryGet("googleAuth", CommandLineArgs.Parsers.FilePath);
        /// <summary>
        /// The <see cref="GoogleAuthConfig"/> loaded from the file, or <see langword="null"/> if it could not be loaded.
        /// </summary>
        private static readonly GoogleAuthConfig? AuthConfig = Config.TryLoad<GoogleAuthConfig>(ConfigPath);
        /// <summary>
        /// <see langword="true"/> if the auth config is usable or <see langword="false"/> otherwise.
        /// </summary>
        public static bool IsValid => AuthConfig?.IsValid ?? false;
        /// <summary>
        /// The exception thrown when the <see cref="GoogleAuthConfig"/> is not <see cref="IValidityCheck">valid</see>.
        /// </summary>
        private static Exception NoValidAuthConfig(string methodName) =>
            new($"{methodName}: Cannot authenticate with Google because no AuthConfig at path {ConfigPath} could be successfully loaded!");
        /// <summary>
        /// Gets the Google Auth certificate from the (privately-stored) key and password files.
        /// </summary>
        /// <remarks>Largely a copy of code from <see href="https://www.daimto.com/google-drive-authentication-c/">this example</see>.<br/>
        /// <br/> Apparently the password is always <c>notasecret</c> and that can't be changed, which is strange.</remarks>
        private static X509Certificate2 Certificate
        {
            get
            {
                if (!AuthConfig.IsValid()) throw NoValidAuthConfig(nameof(Certificate));
                // AuthConfig and KeyPath are certainly non-null because they're checked by IsValid
                return new(AuthConfig!.KeyPath!, "notasecret", X509KeyStorageFlags.Exportable);
            }
        }
        /// <summary>
        /// Constructs a credential with the specified scopes.
        /// </summary>
        /// <remarks>Largely a copy of code from <see href="https://www.daimto.com/google-drive-authentication-c/">this example</see>.</remarks>
        /// <param name="scopes">The <see href="https://developers.google.com/identity/protocols/oauth2/scopes">Google scopes</see> the credential 
        /// is permitted to use.</param>
        private static ServiceAccountCredential Credential(params string[] scopes)
        {
            if (!AuthConfig.IsValid()) throw NoValidAuthConfig(nameof(Credential));
            return new(new ServiceAccountCredential.Initializer(AuthConfig!.Email) { Scopes = scopes }
                    .FromCertificate(Certificate));
        }
        /// <summary>
        /// Gets a 
        /// <see href="https://googleapis.dev/dotnet/Google.Apis.Calendar.v3/latest/api/Google.Apis.Calendar.v3.CalendarService.html">
        ///     calendar service
        /// </see>
        /// using a <see cref="Credential">Credential</see> scoped to allow all Calendar operations.
        /// </summary>
        public static CalendarService CalendarService
        {
            get
            {
                if (!AuthConfig.IsValid()) throw NoValidAuthConfig(nameof(CalendarService));
                return new(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = Credential(CalendarService.Scope.Calendar),
                    ApplicationName = AuthConfig!.AppName
                });
            }
        }
        /// <summary>
        /// Gets a 
        /// <see href="https://developers.google.com/resources/api-libraries/documentation/drive/v3/csharp/latest/classGoogle_1_1Apis_1_1Drive_1_1v3_1_1DriveService.html">
        ///     drive service
        /// </see>
        /// using a <see cref="Credential">Credential</see> scoped to allow all Drive operations.
        /// </summary>
        public static DriveService DriveService
        {
            get
            {
                if (!AuthConfig.IsValid()) throw NoValidAuthConfig(nameof(DriveService));
                return new(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = Credential(DriveService.Scope.Drive),
                    ApplicationName = AuthConfig!.AppName
                });
            }
        }        
        /// <summary>
        /// Attempts to download a file from a Drive URL to the <paramref name="filePath">specified path</paramref>
        /// and prints whether or not it was successful, as well as the response code.
        /// </summary>
        /// <remarks>The file must be shared, through the Sheets UI, with the <see cref="GoogleAuthConfig.Email">email associated with the service account</see>.</remarks>
        /// <param name="fileId">The Drive ID of the file to download.</param>
        /// <param name="filePath">The path to the file when downloaded.</param>
        /// <param name="mimeType">The type of the file to download. Should be a valid 
        /// <see href="https://developer.mozilla.org/en-US/docs/Web/HTTP/Basics_of_HTTP/MIME_types/Common_types">MIME type</see>.</param>
        /// <returns>The path to the downloaded file, if successfully downloaded, or <see langword="null"/> otherwise.</returns>
        public static string? Download(string fileId, string filePath, string mimeType)
        {
            FilesResource.ExportRequest request = new(DriveService, fileId, mimeType);
            filePath = filePath.AbsolutePath();
            using FileStream fs = new(filePath, FileMode.Create);
            IDownloadProgress progress = request.DownloadWithStatus(fs);
            try
            {
                progress.ThrowOnFailure();
                return filePath;
            }
            catch (Exception e)
            {
                Utils.DebugLog($"Error when downloading file from Drive!\n\t{e.Message}");
                return null;
            }
        }
    }
}
