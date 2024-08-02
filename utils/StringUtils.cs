using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace d9.utl;
/// <summary>
/// Utilities which convert objects to strings or perform operations on strings.
/// </summary>
public static partial class StringUtils
{
    /// <summary>
    /// Truncates a string so that it is at most <paramref name="maxLength"/> characters long,
    /// including an optional <paramref name="truncationSuffix"/>.
    /// </summary>
    /// <param name="value">The string to truncate.</param>
    /// <param name="maxLength">The maximum length of the resulting string.</param>
    /// <param name="truncationSuffix">
    /// A string to append to the result in order to indicate that it was truncated. <br/><br/> This
    /// will <b>never</b> cause the result to be longer than <paramref name="maxLength"/>; an <see
    /// cref="ArgumentException"/> will be thrown if <paramref name="truncationSuffix"/><c>.Length &gt;
    /// </c><paramref name="maxLength"/>.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown if <paramref name="truncationSuffix"/><c>.Length &gt;</c><paramref name="maxLength"/>.
    /// </exception>
    /// <returns>
    /// The first <paramref name="maxLength"/> characters of <paramref name="value"/>, with
    /// <paramref name="truncationSuffix"/> at the end if appropriate.
    /// </returns>
    /// <remarks>
    /// Largely based on <see href="https://stackoverflow.com/a/2776689">this StackOverflow answer</see>.
    /// i chose not to allow <paramref name="value"/> to be <see langword="null"/> because i
    /// would prefer to use .<see cref="PrintNull(object?, string)">PrintNull</see>() in a chain
    /// before this instead.
    /// </remarks>
    public static string Truncate(this string value, int maxLength, string truncationSuffix = Default.TruncationSuffix)
    {
        if (truncationSuffix.Length > maxLength)
            throw new ArgumentException($"`{truncationSuffix}` (length {truncationSuffix.Length}) is longer than the specified maxLength ({maxLength})!", nameof(truncationSuffix));
        int targetLength = maxLength - truncationSuffix.Length;
        if (value.Length > targetLength)
            return value[..targetLength] + truncationSuffix;
        return value;
    }
    /// <summary>
    /// Join a set of characters to a string.
    /// </summary>
    /// <param name="chars">The characters to join.</param>
    /// <returns>The specified characters, joined into a string.</returns>
    public static string Join(this IEnumerable<char> chars)
        => chars.Select(x => $"{x}").Aggregate((x, y) => x + y);
    /// <summary>
    /// Concatenates a collection of <paramref name="strings"/>.
    /// </summary>
    /// <param name="strings">The strings to join.</param>
    /// <returns>The specified <paramref name="strings"/>, concatenated together.</returns>
    public static string Join(this IEnumerable<string> strings)
        => strings.Aggregate((x, y) => x + y);
    /// <summary>
    /// Concatenates a collection of <paramref name="strings"/> with a delimiter between them.
    /// </summary>
    /// <param name="strings"><inheritdoc cref="Join(IEnumerable{string})" path="/param[@name='strings']"/></param>
    /// <param name="delimiter">The string to delimit each pair of strings.</param>
    /// <returns>
    /// The specified <paramref name="strings"/>, concatenated, with <paramref name="delimiter"/>
    /// between each pair.
    /// </returns>
    public static string JoinWithDelimiter(this IEnumerable<string> strings, string delimiter)
        => strings.Aggregate((x, y) => $"{x}{delimiter}{y}");
    /// <summary>
    /// Represents an enumerable in human-readable format.
    /// </summary>
    /// <typeparam name="T">The type the <c>enumerable</c> contains.</typeparam>
    /// <param name="enumerable">The enumerable to print.</param>
    /// <param name="leftBracket">The string to prepend to the beginning of the result.</param>
    /// <param name="rightBracket">The string to append to the end of the result.</param>
    /// <param name="delimiter">The string to place in between each item.</param>
    /// <param name="nullString">The value to return if the enumerable is <see langword="null"/>.</param>
    /// <returns>
    /// A string of the format <c>[item1, item2, ... itemN]</c> representing the items in <c>enumerable</c>.
    /// </returns>
    public static string ListNotation<T>(this IEnumerable<T> enumerable,
                                              string leftBracket = "[",
                                              string rightBracket = "]",
                                              string delimiter = ", ",
                                              string nullString = Constants.NullString)
    {
        if (enumerable is null)
            return nullString;
        return $"{leftBracket}{enumerable.Count() switch
        {
            0 => string.Empty,
            1 => $"{enumerable.First()}",
            _ => enumerable.Select(x => x.PrintNull(nullString))
                                         .JoinWithDelimiter(delimiter)
        }}{rightBracket}";
    }
    /// <summary>
    /// <inheritdoc cref="ListNotation{T}(IEnumerable{T}, string, string, string, string)" path="/summary"/>
    /// </summary>
    /// <typeparam name="T">
    /// <inheritdoc cref="ListNotation{T}(IEnumerable{T}, string, string, string, string)" path="/typeparam[@name='T']"/>
    /// </typeparam>
    /// <param name="enumerable">
    /// <inheritdoc cref="ListNotation{T}(IEnumerable{T}, string, string, string, string)" path="/param[@name='enumerable']"/>
    /// </param>
    /// <param name="brackets">
    /// A tuple of the left and right brackets to print. If <see langword="null"/>, no brackets are printed.
    /// </param>
    /// <param name="delimiter">
    /// <inheritdoc cref="ListNotation{T}(IEnumerable{T}, string, string, string, string)" path="/param[@name='delimiter']"/>
    /// </param>
    /// <param name="nullString">
    /// <inheritdoc cref="ListNotation{T}(IEnumerable{T}, string, string, string, string)" path="/param[@name='nullString']"/>
    /// </param>
    /// <returns>
    /// <inheritdoc cref="ListNotation{T}(IEnumerable{T}, string, string, string, string)" path="/returns"/>
    /// </returns>
    public static string ListNotation<T>(this IEnumerable<T> enumerable,
                                              (string left, string right)? brackets,
                                               string delimiter = ", ",
                                               string nullString = Constants.NullString)
        => enumerable.ListNotation(brackets?.left ?? "", brackets?.right ?? "", delimiter, nullString);
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
    /// Wrapper for <see cref="string.IsNullOrEmpty(string?)"/>, because it reads better to me as an
    /// extension method.
    /// </summary>
    /// <param name="s">The string to check.</param>
    /// <returns><inheritdoc cref="string.IsNullOrEmpty(string?)" path="/returns"/></returns>
    public static bool IsNullOrEmpty(this string? s) => string.IsNullOrEmpty(s);
    private static readonly JsonSerializerOptions _prettyPrintOptions = new()
    {
        WriteIndented = true,
        NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals,
        Converters =
        {
            new JsonStringEnumConverter()
        }
    };
    /// <summary>
    /// Prints an object in its entirety in relatively readable JSON format.
    /// </summary>
    /// <param name="obj">The object to print.</param>
    /// <returns>A pretty-printed object.</returns>
    public static string PrettyPrint(this object? obj) => JsonSerializer.Serialize(obj, _prettyPrintOptions);
    /// <summary>
    /// Represents an object in human-readable format, even if it's <see langword="null"/>.
    /// </summary>
    /// <param name="obj">The object or <see langword="null"/> value to represent.</param>
    /// <param name="resultIfNull">The string to print if <paramref name="obj"/> is <see langword="null"/>.</param>
    /// <returns>
    /// A string which is either <c><paramref name="obj"/>.ToString()</c>, if <paramref name="obj"/>
    /// is not <see langword="null"/>, or <paramref name="resultIfNull"/> otherwise.
    /// </returns>
    public static string PrintNull(this object? obj, string resultIfNull = Constants.NullString)
        => obj?.ToString() ?? resultIfNull;
    /// <summary>
    /// Repeats a character a specified number of times.
    /// </summary>
    /// <param name="c">The character to repeat.</param>
    /// <param name="times">How many times <paramref name="c"/> should be repeated.</param>
    /// <returns>A string consisting of <c>times</c> instances of <c>c</c>.</returns>
    /// <remarks>
    /// Would be <c>Obsolete</c> because of <see langword="string"/>. <see langword="new"/>( <see
    /// langword="char"/>, <see langword="int"/>), but i'm keeping it for consistency since there's
    /// no equivalent for strings.
    /// </remarks>
    public static string Repeated(this char c, int times)
        => new(c, times);
    /// <summary>
    /// Repeats a string a specified number of times.
    /// </summary>
    /// <param name="s">The string to repeat.</param>
    /// <param name="times">How many times <paramref name="s"/> should be repeated.</param>
    /// <returns>A string consisting of <c>times</c> instances of <c>s</c>.</returns>
    public static string Repeated(this string s, int times)
    {
        string result = "";
        for (int i = 0; i < times; i++)
            result += s;
        return result;
    }
    /// <summary>
    /// Converts a character to lowercase. Wrapper for <see cref="char.ToLower(char)"/>, because it
    /// reads better to me as an extension method.
    /// </summary>
    /// <param name="c">The character to format.</param>
    /// <returns><inheritdoc cref="char.ToLower(char)"/></returns>
    public static char ToLower(this char c) => char.ToLower(c);
    /// <summary>
    /// Removes a set of characters from a string.
    /// </summary>
    /// <param name="s">The string from which the characters will be removed.</param>
    /// <param name="chars">The characters to be removed.</param>
    /// <returns>A copy of <c>s</c> without any instances of the specified characters.</returns>
    public static string Without(this string s, IEnumerable<char> chars)
    {
        string result = "";
        foreach (char c in s)
        {
            if (chars.Contains(c))
                continue;
            result += c;
        }
        return result;
    }
    /// <summary>
    /// Removes a set of substrings from a string.
    /// </summary>
    /// <param name="s">The string from which the substrings will be removed.</param>
    /// <param name="strs">The substrings to be removed.</param>
    /// <returns>A copy of <c>s</c> without any instances of the specified substrings.</returns>
    public static string Without(this string s, IEnumerable<string> strs)
    {
        foreach (string s2 in strs.Where(x => !x.IsNullOrEmpty()))
            s = s.Replace(s2, "");
        return s;
    }
    /// <summary>
    /// Converts a single <see langword="byte"/> to its hexadecimal equivalent.
    /// </summary>
    /// <param name="b">The <see langword="byte"/> to convert.</param>
    /// <returns>The <see langword="byte"/>, formatted to hexadecimal as with <see cref="ToHex(byte[])"/>.</returns>
    public static string ToHex(this byte b) => new[] { b }.ToHex();
    /// <summary>
    /// Writes <see langword="byte"/> s in hexadecimal without separating them with hyphens.
    /// </summary>
    /// <param name="bytes">The <see langword="byte"/> s to convert.</param>
    /// <returns>As described above.</returns>
    public static string ToHex(this byte[] bytes) => BitConverter.ToString(bytes).Without("-");
    /// <summary>
    /// Enumerates over the specified <paramref name="enumerable"/> and returns each element with
    /// its corresponding index.
    /// </summary>
    /// <typeparam name="T">The type of the elements to enumerate.</typeparam>
    /// <param name="enumerable">The enumerable containing the elements to enumerate.</param>
    /// <param name="log">
    /// A <see langword="void"/> function which takes a <see langword="string"/>, intended to allow
    /// logging progress of this method.
    /// </param>
    /// <param name="numberOfPrints">The total number of lines of progress which will be printed.</param>
    /// <returns>
    /// The elements of the specified <c><paramref name="enumerable"/></c>, with their respective indices.
    /// </returns>
    public static IEnumerable<(T element, int index)> WithIndices<T>(this IEnumerable<T> enumerable, Action<string>? log = null, int numberOfPrints = 10)
    {
        int total = enumerable.Count(), ct = 0, interval = total / numberOfPrints;
        foreach (T t in enumerable)
        {
            if (ct % interval == 0)
            {
                string msg = $"{ct,8}/{total,-8} ({ct / (float)total:P0})";
                if (log is not null)
                    log(msg);
            }
            yield return (t, ct);
            ct++;
        }
    }
    /// <summary>
    /// Prints the specified items as a list in natural English, with the specified conjunction.
    /// </summary>
    /// <param name="items">The items to print.</param>
    /// <param name="conjunction">
    /// The conjunction at the end of the string, just before the last item.
    /// </param>
    /// <returns>
    /// The items in the list separated by commas as appropriate, with a conjunction between the
    /// last two items.
    /// </returns>
    /// <remarks>Uses the Oxford comma, which is the correct way to write such lists.</remarks>
    public static string NaturalLanguageList<T>(this IEnumerable<T> items, string conjunction)
        => items.Count() switch
        {
            0 => "",
            1 => $"{items.First()}",
            2 => $"{items.First()} {conjunction} {items.Last()}",
            _ => $"{items.SkipLast(1).ListNotation(brackets: null)}, {conjunction} {items.Last()}"
        };
    /// <summary>
    /// Chooses the <paramref name="singular"/> or <paramref name="plural"/> form of a string based on the size of a <paramref name="number"/>.
    /// </summary>
    /// <typeparam name="T">The type of the number to compare.</typeparam>
    /// <param name="number">The number whose size determines the form of the string to use.</param>
    /// <param name="singular">The string to use if the <paramref name="number"/> is equal to 1.</param>
    /// <param name="plural">The string to use if the <paramref name="number"/> is <em>not</em> equal to 1.</param>
    /// <remarks>Not localized. Localizing this kind of thing would be way out of scope for this project.</remarks>
    /// <returns><paramref name="singular"/> if <paramref name="number"/> is 1 or -1, or <paramref name="plural"/> otherwise.</returns>
    public static string Plural<T>(this string singular, string plural, T number)
        where T : INumberBase<T>
        => T.Abs(number) == T.One ? singular : plural;
    /// <summary>
    /// Chooses whether to pluralize (append an s to) a string based on the size of a <paramref name="number"/>.
    /// </summary>
    /// <typeparam name="T"><inheritdoc cref="Plural{T}(string, string, T)" path="/typeparam[@name='T']"/></typeparam>
    /// <param name="number"><inheritdoc cref="Plural{T}(string, string, T)" path="/param[@name='number']"/></param>
    /// <param name="singular"><inheritdoc cref="Plural{T}(string, string, T)" path="/param[@name='singular']"/></param>
    /// <remarks><inheritdoc cref="Plural{T}(string, string, T)" path="/remarks"/></remarks>
    /// <returns><paramref name="singular"/> if <paramref name="number"/> is 1 or -1, <paramref name="singular"/> + "s" otherwise.</returns>
    public static string Plural<T>(this string singular, T number)
        where T : INumberBase<T>
        => singular.Plural($"{singular}s", number);
    /// <summary>
    /// Chooses the <paramref name="singular"/> or <paramref name="plural"/> form of a string based on the size of an <paramref name="enumerable"/>.
    /// </summary>
    /// <typeparam name="T"><inheritdoc cref="Plural{T}(string, string, T)" path="/typeparam[@name='T']"/></typeparam>
    /// <param name="enumerable">The enumerable whose size determines the form of the string to use.</param>
    /// <param name="singular">The string to use if the <paramref name="enumerable"/> has exactly 1 element.</param>
    /// <param name="plural">The string to use if the <paramref name="enumerable"/> has 0 or multiple elements.</param>
    /// <remarks><inheritdoc cref="Plural{T}(string, string, T)" path="/remarks"/></remarks>
    /// <returns><paramref name="singular"/> if <paramref name="enumerable"/> has exactly 1 item, or <paramref name="plural"/> otherwise.</returns>
    public static string Plural<T>(this string singular, string plural, IEnumerable<T> enumerable)
        => singular.Plural(plural, enumerable.Count());
    /// <summary>
    /// Chooses whether to pluralize (append an s to) a string based on the size of an <paramref name="enumerable"/>.
    /// </summary>
    /// <typeparam name="T"><inheritdoc cref="Plural{T}(string, string, T)" path="/typeparam[@name='T']"/></typeparam>
    /// <param name="enumerable"><inheritdoc cref="Plural{T}(string, string, IEnumerable{T})" path="/param[@name='enumerable']"/></param>
    /// <param name="singular"><inheritdoc cref="Plural{T}(string, string, IEnumerable{T})" path="/param[@name='singular']"/></param>
    /// <remarks><inheritdoc cref="Plural{T}(string, string, T)" path="/remarks"/></remarks>
    /// <returns><paramref name="singular"/> if <paramref name="enumerable"/> has exactly 1 item, or <paramref name="singular"/> + "s" otherwise.</returns>
    public static string Plural<T>(this string singular, IEnumerable<T> enumerable)
        => singular.Plural(enumerable.Count());
    /// <summary>
    /// Produces the ordinal form of a <paramref name="number"/>.
    /// </summary>
    /// <typeparam name="T">The type of the number to make an ordinal from.</typeparam>
    /// <param name="number">The number to make an ordinal from.</param>
    /// <remarks>Not localized. Localizing this kind of thing would be way out of scope for this project.</remarks>
    public static string Ordinal<T>(this int number)
        => number is 11 or 12 or 13 ? $"{number}th"
            : (number % 10) switch
            {
                1 => $"{number}st",
                2 => $"{number}nd",
                3 => $"{number}rd",
                _ => $"{number}th"
            };
}