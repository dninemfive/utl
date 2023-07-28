using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace d9.utl;
public static class NetUtils
{
    public class ParsedQuery
    {
        public IEnumerable<(string key, string value)> Items { get; private set; }
        internal ParsedQuery(IEnumerable<(string key, string value)> items)
        {
            Items = items;
        }
        public IEnumerable<string> this[string key] => Items.Where(x => x.key == key).Select(x => x.value);
        public IEnumerable<T> Get<T>(string key) where T : class
            => this[key].Select(x => x as T).Where(x => x is not null)!;
        public string? First(string key) => this[key].Any() ? this[key].First() : null;
        public T? First<T>(string key) where T : class
            => First(key) as T;
    }
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
