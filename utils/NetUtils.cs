using System.Collections.Immutable;
namespace d9.utl;
public static class NetUtils
{    
    /// <summary>
    /// Parses the query portion of a <see cref="Uri"/> and returns each item and its corresponding value.
    /// </summary>
    /// <remarks>Currently, if a query has duplicates of the same key, only the first one will be returned.</remarks>
    /// <returns>A dictionary</returns>
    public static ParsedQuery ParseQuery(this Uri uri)
    {
        string query = uri.Query[1..^0]; // slice the question mark away
        List<(string key, string value)> items = new();
        foreach(string item in query.Split("&"))
        {
            string[] split = item.Split("=");
            if (split.Length < 2) continue;
            items.Add((split[0], split[1..].Aggregate((x, y) => $"{x}{y}")));
        }
        return new(items);
    }
    public static ParsedQuery ParseQuery(this string url) => new Uri(url).ParseQuery();
    public static string ToQuery(this IEnumerable<(string key, string value)> items)
        => $"?{items.Select(x => $"{x.key}={x.value}")
                    .Aggregate((x, y) => $"{x}&{y}")}";
    public static string ToQuery(params (string key, string value)[] items) => items.ToQuery();
}
public readonly struct ParsedQuery
{
    public readonly IReadOnlyCollection<(string key, string value)> Items { get; }
    internal ParsedQuery(IEnumerable<(string key, string value)> items)
    {
        Items = items.ToImmutableList();
    }
    public IEnumerable<string> this[string key] => Items.Where(x => x.key == key).Select(x => x.value);
    public string? this[string key, int index]
    {
        get
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));
            IEnumerable<string> items = this[key];
            if (index >= items.Count())
                return null;
            return items.ElementAt(index);
        }
    }
    public ParsedQuery Set(string key, Func<string, string> func)
    {
        List<(string key, string value)> items = new();
        foreach((string key, string value) item in Items)
        {
            if(item.key == key)
            {
                items.Add((key, func(item.value)));
            }
            else
            {
                items.Add(item);
            } 
        }
        return new(items);
    }
    private ParsedQuery KeepOrDrop(string[] tags, bool drop)
    {
        List<(string key, string value)> items = new();
        foreach ((string key, string value) item in Items)
        {
            if (tags.Contains(item.key) ^ drop)
                items.Add(item);
        }
        return new(items);
    }
    public ParsedQuery Keep(params string[] tags) => KeepOrDrop(tags, drop: false);
    public ParsedQuery Drop(params string[] tags) => KeepOrDrop(tags, drop: true);
    public IEnumerable<T> Get<T>(string key) where T : class
        => this[key].OfType<T>();
    public string? First(string key) => this[key, 0];
    public T? First<T>(string key) where T : class
        => First(key) as T;
    public override string ToString() => $"?{Items.Select(x => $"{x.key}={x.value}").Aggregate((x, y) => $"{x}&{y}")}";
    public static implicit operator string(ParsedQuery query) => query.ToString();
}