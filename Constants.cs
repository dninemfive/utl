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
    /// <summary>
    /// The <see href="https://developer.mozilla.org/en-US/docs/Web/HTTP/Basics_of_HTTP/MIME_types/">MIME type</see> corresponding
    /// to a given file extension.
    /// </summary>
    /// <param name="fileExtension">The file extension whose MIME type to get.</param>
    /// <returns>The corresponding MIME type, if a recognized file extension is passed in, or <c>application/octet-stream</c>
    /// if an invalid type is passed in.</returns>
    /// <remarks>See <see href="https://developer.mozilla.org/en-US/docs/Web/HTTP/Basics_of_HTTP/MIME_types/Common_types">here</see>
    /// for more information.</remarks>
    public static string MimeType(this string fileExtension) => fileExtension switch
    {
        "tsv" => "text/tab-separated-values",
        // https://developer.mozilla.org/en-US/docs/Web/HTTP/Basics_of_HTTP/MIME_types/Common_types
        // > An unknown file type should use this type.
        _ => "application/octet-stream"
    };        
}
