using Google.Apis.Download;
using Google.Apis.Drive.v3;
using Microsoft.Extensions.Logging;

namespace d9.utl.compat.google;
/// <summary>
/// Wrapper for a Google Drive service with a helper method to download files therefrom.
/// </summary>
public class GoogleDrive
    : GoogleServiceWrapper<DriveService>
{
    /// <inheritdoc cref="GoogleServiceWrapper{T}.GoogleServiceWrapper(T, GoogleServiceContext)"/>
    protected GoogleDrive(DriveService service, GoogleServiceContext context)
        : base(service, context) { }
    /// <summary>
    /// Tries to create a Google Drive wrapper in the specified <paramref name="context"/>.
    /// </summary>
    /// <param name="context">The context in which this Google Drive wrapper will be created.</param>
    /// <returns>A new Google Drive wrapper, if successful, or <see langword="null"/> otherwise.</returns>
    public static GoogleDrive? TryCreateFrom(GoogleServiceContext context)
        => TryCreate<GoogleDrive>(context, DriveService.Scope.Drive);
    /// <summary>
    /// Creates a Google Drive wrapper in the specified <paramref name="context"/>, <b>throwing an
    /// Exception</b> if unsuccessful.
    /// </summary>
    /// <param name="context">The context in which this Google Drive wrapper will be created.</param>
    /// <returns>A new Google Drive wrapper.</returns>
    /// <exception cref="Exception">
    /// Thrown if creation of the wrapper was unsuccesful for any reason.
    /// </exception>
    public static GoogleDrive CreateFrom(GoogleServiceContext context)
        => Create<GoogleDrive>(context, DriveService.Scope.Drive);
    /// <summary>
    /// Attempts to download a file from a Drive URL to the <paramref name="filePath">specified
    /// path</paramref> and prints whether or not it was successful, as well as the response code.
    /// </summary>
    /// <remarks>
    /// The file must be shared, through the Sheets UI, with the <see
    /// cref="IGoogleAuthConfig.Email">email associated with the service account</see>.
    /// </remarks>
    /// <param name="fileId">The Drive ID of the file to download.</param>
    /// <param name="filePath">The path to the file when downloaded.</param>
    /// <param name="mimeType">
    /// The type of the file to download. Should be a valid <see
    /// href="https://developer.mozilla.org/en-US/docs/Web/HTTP/Basics_of_HTTP/MIME_types/Common_types">MIME type</see>.
    /// </param>
    /// <returns>
    /// The path to the downloaded file, if successfully downloaded, or <see langword="null"/> otherwise.
    /// </returns>
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
            Log?.LogError("Error when downloading file from Drive!\n\t{summary}", e.Summary());
            return null;
        }
    }
}