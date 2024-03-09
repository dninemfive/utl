using System.Text.Json;
using System.Text.Json.Serialization;
namespace d9.utl;
/// <summary>
/// Utilities which convert objects to strings or perform operations on strings.
/// </summary>
public static class StringUtils
{
    /// <summary>
    /// Formats a <see cref="DateTime"/> into a sortable and filesystem-safe string which can be used to name files.
    /// </summary>
    /// <param name="datetime">The <see cref="DateTime"/> to format.</param>
    /// <returns>The specified <c><paramref name="datetime"/></c> formatted as described above.</returns>
    public static string FileNameFormat(this DateTime datetime) => $"{datetime:yyyy-MM-dd-HHmmss}";
    /// <summary><inheritdoc cref="InColumns{T}(IEnumerable{T}, IEnumerable{int})" path="/summary"/></summary>
    /// <typeparam name="T">The type of the objects to print.</typeparam>
    /// <param name="values">An enumerable holding the objects to print paired with the width of their respective columns.</param>
    /// <returns>A string corresponding to the objects <c>t</c> in order, with columns padded to <c>width</c>.</returns>
    public static string InColumns<T>(this IEnumerable<(T t, int width)> values)
    {
        string result = "";
        foreach ((T t, int width) in values)
        {
            result += t.PrintNull().PadRight(width);
        }
        return result;
    }
    /// <summary>Prints the specified objects in columns with specified widths.</summary>
    /// <typeparam name="T">The type of the objects to print.</typeparam>
    /// <param name="values">An enumerable holding the objects to print.</param>
    /// <param name="widths">An enumerable holding the widths of the columns, which will be applied in the same order as the objects.</param>
    /// <returns>A string corresponding to the <c>values</c> in order, in columns padded to their respective <c>widths</c>.</returns>
    public static string InColumns<T>(this IEnumerable<T> values, IEnumerable<int> widths) => InColumns(values.Zip(widths));
    /// <summary>
    /// Join a set of characters to a string.
    /// </summary>
    /// <param name="chars">The characters to join.</param>
    /// <returns>The specified characters, joined to a string.</returns>
    public static string Join(this IEnumerable<char> chars) => chars.Select(x => $"{x}").Aggregate((x, y) => x + y);
    /// <summary>
    /// Concatenates a collection of strings.
    /// </summary>
    /// <param name="strings">The strings to join.</param>
    /// <returns>The specified strings, concatenated together.</returns>
    public static string Join(this IEnumerable<string> strings) => strings.Aggregate((x, y) => x + y);
    /// <summary>
    /// Represents an enumerable in human-readable format.
    /// </summary>
    /// <typeparam name="T">The type the <c>enumerable</c> contains.</typeparam>
    /// <param name="enumerable">The enumerable to print.</param>
    /// <returns>A string of the format <c>[item1, item2, ... itemN]</c> representing the items in <c>enumerable</c>.</returns>
    public static string ListNotation<T>(this IEnumerable<T> enumerable,
                                              string nullString = Constants.NullString,
                                              string leftBracket = "[",
                                              string rightBracket = "]")
    {
        if (enumerable is null) return nullString;
        return $"{leftBracket}{enumerable.Count() switch
        {
            0 => string.Empty,
            1 => $"{enumerable.First()}",
            _ => enumerable.Select(x => x.PrintNull(nullString)).Aggregate((a, b) => $"{a}, {b}")
        }}{rightBracket}";
    }
    public static string ListNotation<T>(this IEnumerable<T> enumerable, (string left, string right)? brackets)
        => enumerable.ListNotation(brackets?.left ?? "", brackets?.right ?? "");
    /// <summary>
    /// Changes the first character of the specified string, if applicable, to lowercase.
    /// </summary>
    /// <param name="s">The string to format.</param>
    /// <returns>The string specified, with its first letter guaranteed to be in lower case.</returns>
    public static string LowerFirst(this string s) => s.Length switch
    {
        0 => s,
        1 => s.ToLower(),
        _ => $"{s[0].ToLower()}{s[1..]}"
    };
    /// <summary>
    /// Wrapper for <see cref="string.IsNullOrEmpty(string?)"/>, because it reads better to me as an extension method.
    /// </summary>
    /// <param name="s">The string to check.</param>
    /// <returns><inheritdoc cref="string.IsNullOrEmpty(string?)" path="/returns"/></returns>
    public static bool NullOrEmpty(this string? s) => string.IsNullOrEmpty(s);
    /// <summary>
    /// Prints an object in its entirety in relatively readable JSON format.
    /// </summary>
    /// <param name="obj">The object to print.</param>
    /// <returns>A pretty-printed object.</returns>
    public static string PrettyPrint(this object? obj) => JsonSerializer.Serialize(obj, new JsonSerializerOptions()
    {
        WriteIndented = true,
        NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals,
        Converters =
        {
            new JsonStringEnumConverter()
        }
    });
    /// <summary>
    /// Represents an object in human-readable format, even if it's <see langword="null"/>.
    /// </summary>
    /// <param name="obj">The object or <see langword="null"/> value to represent.</param>
    /// <param name="resultIfNull">The string to print if <c>obj</c> is null.</param>
    /// <returns>A string which is either <c>obj.ToString()</c>, if <c>obj</c> is not <see langword="null"/>, or <c>ifNull</c> otherwise.</returns>
    public static string PrintNull(this object? obj, string resultIfNull = Constants.NullString)
        => obj?.ToString() ?? resultIfNull;
    /// <summary>
    /// Repeats a character a specified number of times.
    /// </summary>
    /// <param name="c">The character to repeat.</param>
    /// <param name="times">How many of the character should be produced.</param>
    /// <returns>A string which is <c>times</c> instances of <c>c</c>.</returns>
    public static string Repeated(this char c, int times)
    {
        string result = "";
        for (int i = 0; i < times; i++) result += c;
        return result;
    }
    /// <summary>
    /// Converts a character to lowercase. Wrapper for <see cref="char.ToLower(char)"/>, because it reads better to me as an extension method.
    /// </summary>
    /// <param name="c">The character to format.</param>
    /// <returns><inheritdoc cref="char.ToLower(char)"/></returns>
    public static char ToLower(this char c) => char.ToLower(c);
    /// <summary>Removes a set of characters from a string.</summary>
    /// <param name="s">The string from which the characters will be removed.</param>
    /// <param name="chars">The characters to be removed.</param>
    /// <returns>A copy of <c>s</c> without any instances of the specified characters..</returns>
    public static string Without(this string s, IEnumerable<char> chars)
    {
        foreach (char c in chars) s = s.Replace("" + c, "");
        return s;
    }
    public static string Remove(this string s, params string[] strs)
    {
        string result = s;
        foreach (string s2 in strs.Where(x => !x.NullOrEmpty())) result = result.Replace(s2, "");
        return result;
    }
    /// <summary>
    /// Converts a single <see langword="byte"/> to its hexadecimal equivalent.
    /// </summary>
    /// <param name="b">The <see langword="byte"/> to convert.</param>
    /// <returns>The <see langword="byte"/>, formatted to hexadecimal as with <see cref="ToHex(byte[])"/>.</returns>
    public static string ToHex(this byte b) => new[] { b }.ToHex();
    /// <summary>
    /// Writes <see langword="byte"/>s in hexadecimal without separating them with hyphens.
    /// </summary>
    /// <param name="bytes">The <see langword="byte"/>s to convert.</param>
    /// <returns>As described above.</returns>
    public static string ToHex(this byte[] bytes) => BitConverter.ToString(bytes).Without("-");
    /// <summary>
    /// Enumerates over the specified enumerable and returns each element with its corresponding index.
    /// </summary>
    /// <typeparam name="T">The type of the elements to enumerate.</typeparam>
    /// <param name="enumerable">The enumerable containing the elements to enumerate.</param>
    /// <param name="log">The <see cref="Log"/> to write to. If not specified, uses <see cref="Utils.Log(object?)"/> instead.</param>
    /// <param name="numberOfPrints">The total number of lines of progress which will be printed.</param>
    /// <returns>The elements of the specified <c><paramref name="enumerable"/></c>, with their respective indices.</returns>
    public static IEnumerable<(T val, int i)> WithProgress<T>(this IEnumerable<T> enumerable, Log? log = null, int numberOfPrints = 10)
    {
        int total = enumerable.Count(), ct = 0, interval = total / numberOfPrints;
        foreach(T t in enumerable)
        {
            if (ct % interval == 0)
            {
                string msg = $"{ct,8}/{total,-8} ({ct / (float)total:P0})";
                if (log is not null) log.WriteLine(msg);
                else Utils.Log(msg);
            }
            yield return (t, ct);
            ct++;
        }
    }
}
