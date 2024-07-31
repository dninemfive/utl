namespace d9.utl;
/// <summary>
/// Extensions for (de)serializing URL query strings.
/// </summary>
public static class ParsedQueryExtensions
{
    /// <summary>
    /// Parses the query portion of a <see cref="Uri"/> and returns each item and its corresponding value.
    /// </summary>
    /// <returns>A <see cref="UrlQuery"/> representing the query portion of the specified URL.</returns>
    public static UrlQuery ParseQuery(this Uri uri)
    {
        string query = uri.Query[1..^0]; // slice the question mark away
        List<(string key, string value)> items = new();
        foreach (string item in query.Split("&"))
        {
            string[] split = item.Split("=");
            if (split.Length < 2)
                continue;
            items.Add((split[0], split[1..].Aggregate((x, y) => $"{x}={y}")));
        }
        return new(items);
    }
    /// <summary>
    /// Parses a string into a <see cref="UrlQuery"/>.
    /// </summary>
    /// <param name="url">The string to parse.</param>
    public static UrlQuery ParseQuery(this string url) => new Uri(url).ParseQuery();
}