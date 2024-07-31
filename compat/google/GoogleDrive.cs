using Google.Apis.Download;
using Google.Apis.Drive.v3;
using Google.Apis.Services;

namespace d9.utl.compat.google;
public class GoogleDrive
    : GoogleServiceWrapper<DriveService>
{
    public GoogleDrive(DriveService service) 
        : base(service) { }
    public GoogleDrive(GoogleAuth auth) 
        : this(new DriveService(auth.InitializerFor(DriveService.Scope.Drive))) { }
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
        FilesResource.ExportRequest request = new(Service, fileId, mimeType);
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
