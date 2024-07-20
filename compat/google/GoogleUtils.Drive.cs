using Google.Apis.Download;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace d9.utl.compat.google;
public partial class GoogleAuth
{
    /// <summary>
    /// Gets a 
    /// <see href="https://developers.google.com/resources/api-libraries/documentation/drive/v3/csharp/latest/classGoogle_1_1Apis_1_1Drive_1_1v3_1_1DriveService.html">
    ///     drive service
    /// </see>
    /// using a <see cref="Credential">Credential</see> scoped to allow all Drive operations.
    /// </summary>
    public DriveService DriveService
        => new(new BaseClientService.Initializer()
        {
            HttpClientInitializer = Credential(DriveService.Scope.Drive),
            ApplicationName = Config!.AppName
        });
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
    public string? Download(string fileId, string filePath, string mimeType)
    {
        FilesResource.ExportRequest request = new(DriveService, fileId, mimeType);
        filePath = filePath.AbsoluteOrInBaseFolder();
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
