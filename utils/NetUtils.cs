using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace d9.utl;
public static class NetUtils
{
    /// <summary>
    /// Parses the query portion of a <see cref="Uri"/> and returns each item and its corresponding value.
    /// </summary>
    /// <remarks>Currently, if a query has duplicates of the same key, only the first one will be returned.</remarks>
    /// <returns>A dictionary</returns>
    public static Dictionary<string, string> ParseQuery(this Uri uri)
    {
        string query = uri.Query[1..^0]; // slice the question mark away
        Dictionary<string, string> result = new();
        foreach(string item in query.Split("&"))
        {
            string[] split = item.Split("=");
            if (result.ContainsKey(split[0])) continue;
            result[split[0]] = split[1..^0].Aggregate((x, y) => $"{x}={y}");
        }
        return result;
    }
    public static Dictionary<string, string> ParseQuery(this string url) => new Uri(url).ParseQuery();
    public static string ToQuery(this IEnumerable<(string key, string value)> items)
        => $"?{items.Select(x => $"{x.key}={x.value}")
                    .Aggregate((x, y) => $"{x}&{y}")}";
    public static string ToQuery(params (string key, string value)[] items) => items.ToQuery();
} 
