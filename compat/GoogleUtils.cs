using Google.Apis.Auth.OAuth2;
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
    internal class GoogleUtils
    {
        private class GoogleAuthConfig : IValidityCheck
        {
            [JsonIgnore]
            public bool IsValid => !(new string?[] { KeyPath, Email, FileId, AppName }.Any(x => x is null));
            [JsonInclude]
            public readonly string? KeyPath;
            [JsonInclude]
            public readonly string? Email;
            [JsonInclude]
            public readonly string? FileId;
            [JsonInclude]
            public readonly string? AppName;
        }
        // todo: equivalent to TryGetDirectory for files
        private static readonly string? ConfigPath = CommandLineArgs.TryGet("googleAuth", CommandLineArgs.Parsers.FirstNonNullOrEmptyString);
        private static readonly GoogleAuthConfig? AuthConfig = Config.TryLoad<GoogleAuthConfig>(ConfigPath, true);
        private static Exception NoValidAuthConfig = new($"Cannot authenticate with google because AuthConfig at path {ConfigPath}!");
        /// <summary>
        /// Gets the Google Auth certificate from the (privately-stored) key and password files.
        /// </summary>
        /// <remarks>Largely a copy of code from <see href="https://www.daimto.com/google-drive-authentication-c/">this example</see>.<br/>
        /// <br/> Apparently the password is always <c>notasecret</c> and that can't be changed, which is strange.</remarks>
        private static X509Certificate2 Certificate
        {
            get
            {
                if (!AuthConfig.IsValid()) throw NoValidAuthConfig;
                // AuthConfig and KeyPath are certainly non-null because they're checked by IsValid
                return new(AuthConfig!.KeyPath!, "notasecret", X509KeyStorageFlags.Exportable);
            }
        }
        /// <summary>
        /// Constructs a ServiceAccountCredential initializer from the <see cref="Certificate"/>.
        /// </summary>
        /// <remarks>Largely a copy of code from <see href="https://www.daimto.com/google-drive-authentication-c/">this example</see>.</remarks>
        private static ServiceAccountCredential.Initializer CredentialInitializer
        {
            get
            {
                if (!AuthConfig.IsValid()) throw NoValidAuthConfig;
                // AuthConfig and Email are certainly non-null because they're checked by IsValid
                return new ServiceAccountCredential.Initializer(AuthConfig!.Email) { Scopes = new[] { DriveService.Scope.Drive } }
                    .FromCertificate(Certificate);
            }
        }
        /// <summary>
        /// Constructs a credential using the <see cref="CredentialInitializer"/>.
        /// </summary>
        /// <remarks>Largely a copy of code from <see href="https://www.daimto.com/google-drive-authentication-c/">this example</see>.</remarks>
        public static ServiceAccountCredential Credential => new(CredentialInitializer);
        /// <summary>
        /// Gets the Drive service using the <see cref="Credential"/> previously established.
        /// </summary>
        public static DriveService DriveService
        {
            get
            {
                if (!AuthConfig.IsValid()) throw NoValidAuthConfig;
                return new(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = Credential,
                    ApplicationName = AuthConfig!.AppName
                });
            }
        }
        /// <summary>
        /// Attempts to download the data TSV file from a Drive URL to the <see cref="Paths.BaseFolder">default base folder</see> 
        /// and prints whether or not it was successful, as well as the response code.
        /// </summary>
        /// <remarks>The file must be shared, through the Sheets UI, with the <see cref="GoogleAuthConfig.Email">email associated with the service account</see>.</remarks>
        /// <param name="fileId">The <see cref="GoogleAuthConfig.FileId">Sheets ID</see> of the file to download.</param>
        /// <param name="filename">The name the file should have when downloaded.</param>
        /// <returns>The path to the downloaded file, if successfully downloaded, or <see langword="null"/> otherwise.</returns>
        public static string? DownloadTsv(string fileId, string filename)
        {
            FilesResource.ExportRequest request = new(DriveService, fileId, "text/tab-separated-values");
            using FileStream fs = new(filename.AbsolutePath(), FileMode.Create);
            IDownloadProgress progress = request.DownloadWithStatus(fs);
            try
            {
                progress.ThrowOnFailure();
                return filename.AbsolutePath();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error when downloading file from Drive!\n\t{e.Message}");
                return null;
            }
        }
    }
}
