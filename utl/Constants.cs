namespace d9.utl;

/// <summary>
/// Useful global variables.
/// </summary>
public static class Constants
{
    /// <summary>
    /// Normal apostrophes plus the weird ones that iPhones automatically replace the normal ones with.
    /// </summary>
    public static readonly string Apostrophes = "'‘’";
    /// <summary>
    /// Normal quotation marks plus the weird ones that iPhones automatically replace the normal ones with.
    /// </summary>
    public static readonly string Quotes = new[] { '"', '“', '”' }.Join();
    /// <summary>
    /// Dashes of various lengths.
    /// </summary>
    public static readonly string Hyphens = "-–—";
    /// <summary>
    /// The null character, traditionally used for ending strings.
    /// </summary>
    public const char NullCharacter = (char)0;
    private static readonly Dictionary<string, string> _mimeTypes = new()
    {
        { "aac",    "audio/aac" },
        { "abw",    "application/x-abiword" },
        { "apng",   "image/apng" },
        { "arc",    "application/x-freearc" },
        { "avif",   "image/avif" },
        { "avi",    "video/x-msvideo" },
        { "azw",    "application/vnd.amazon.ebook" },
        { "bin",    "application/octet-stream" },
        { "bmp",    "image/bmp" },
        { "bz",     "application/x-bzip" },
        { "bz2",    "application/x-bzip2" },
        { "cda",    "application/x-cdf" },
        { "csh",    "application/x-csh" },
        { "css",    "text/css" },
        { "csv",    "text/csv" },
        { "doc",    "application/msword" },
        { "docx",   "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
        { "eot",    "application/vnd.ms-fontobject" },
        { "epub",   "application/epub+zip" },
        { "gz",     "application/gzip" },
        { "gif",    "image/gif" },
        { "htm",    "text/html" },
        { "html",   "text/html" },
        { "ico",    "image/vnd.microsoft.icon" },
        { "ics",    "text/calendar" },
        { "jar",    "application/java-archive" },
        { "jpeg",   "image/jpeg" },
        { "jpg",    "image/jpeg" },
        { "js",     "text/javascript" },
        { "json",   "application/json" },
        { "jsonld", "application/ld+json" },
        { "mid",    "audio/midi" },
        { "midi",   "audio/midi" },
        { "mjs",    "text/javascript" },
        { "mp3",    "audio/mpeg" },
        { "mp4",    "video/mp4" },
        { "mpeg",   "video/mpeg" },
        { "mpkg",   "application/vnd.apple.installer+xml" },
        { "odp",    "application/vnd.oasis.opendocument.presentation" },
        { "ods",    "application/vnd.oasis.opendocument.spreadsheet" },
        { "odt",    "application/vnd.oasis.opendocument.text" },
        { "oga",    "audio/ogg" },
        { "ogv",    "video/ogg" },
        { "ogx",    "application/ogg" },
        { "opus",   "audio/ogg" },
        { "otf",    "font/otf" },
        { "png",    "image/png" },
        { "pdf",    "application/pdf" },
        { "php",    "application/x-httpd-php" },
        { "ppt",    "application/vnd.ms-powerpoint" },
        { "pptx",   "application/vnd.openxmlformats-officedocument.presentationml.presentation" },
        { "rar",    "application/vnd.rar" },
        { "rtf",    "application/rtf" },
        { "sh",     "application/x-sh" },
        { "svg",    "image/svg+xml" },
        { "tar",    "application/x-tar" },
        { "tif",    "image/tiff" },
        { "tiff",   "image/tiff" },
        { "ts",     "video/mp2t" },
        { "tsv",    "text/tab-separated-values" },
        { "ttf",    "font/ttf" },
        { "txt",    "text/plain" },
        { "vsd",    "application/vnd.visio" },
        { "wav",    "audio/wav" },
        { "weba",   "audio/webm" },
        { "webm",   "video/webm" },
        { "webp",   "image/webp" },
        { "woff",   "font/woff" },
        { "woff2",  "font/woff2" },
        { "xhtml",  "application/xhtml+xml" },
        { "xls",    "application/vnd.ms-excel" },
        { "xlsx",   "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
        { "xml",    "application/xml" },
        { "xul",    "application/vnd.mozilla.xul+xml" },
        { "zip",    "application/zip" },
        { "7z",     "application/x-7z-compressed" }
    };
    /// <summary>
    /// All <see href="https://developer.mozilla.org/en-US/docs/Web/HTTP/Basics_of_HTTP/MIME_types/">MIME types</see> defined by MDN,
    /// indexed by their file extensions <b>WITHOUT</b> the leading period.
    /// </summary>
    /// <remarks>See <see href="https://developer.mozilla.org/en-US/docs/Web/HTTP/Basics_of_HTTP/MIME_types/Common_types">here</see>
    /// for more information.</remarks>
    public static IReadOnlyDictionary<string, string> MimeTypes => _mimeTypes;
    /// <summary>
    /// The <see href="https://developer.mozilla.org/en-US/docs/Web/HTTP/Basics_of_HTTP/MIME_types/">MIME type</see> corresponding
    /// to a given file extension. The file extension <b>MAY</b> contain the leading period; it is ignored.
    /// </summary>
    /// <param name="fileExtension">The file extension whose MIME type to get.</param>
    /// <returns>The corresponding MIME type, if a recognized file extension is passed in, or <c>application/octet-stream</c>
    /// if an unknown type is passed in.</returns>
    /// <remarks><inheritdoc cref="MimeTypes" path="/remarks"/></remarks>
    public static string MimeType(this string fileExtension)
    {
        if (fileExtension.Length > 1 && fileExtension.StartsWith('.'))
            fileExtension = fileExtension[1..];
        if (MimeTypes.TryGetValue(fileExtension, out string? value))
            return value;
        return "application/octet-stream";
    }
    /// <summary>
    /// The default string to indicate a value was <see langword="null"/>.
    /// </summary>
    public const string NullString = "(null)";
    /// <summary>
    /// The default tab character used by <see cref="StringUtils.Indent(string, string, string?)"/>.
    /// </summary>
    public const string DefaultTab = "  ";
}
