using System.Collections;
using System.Collections.Immutable;

namespace d9.utl;
/// <summary>
/// Represents the query portion of a url parsed from the standard <c>?key=value&amp;key2=value2</c> syntax.
/// </summary>
public readonly struct UrlQuery
    : IEnumerable<(string key, string value)>
{
    /// <summary>
    /// The individual parts of the query.
    /// </summary>
    /// <remarks><b>NOTE:</b> keys are not guaranteed to be unique.</remarks>
    public readonly IReadOnlyCollection<(string key, string value)> Items { get; }
    /// <summary>
    /// Creates a UrlQuery with the specified <paramref name="parameters"/>.
    /// </summary>
    /// <param name="parameters">The query parameters to include.</param>
    public UrlQuery(IEnumerable<(string key, string value)> parameters)
        => Items = parameters.ToImmutableList();
    /// <summary>
    /// Gets all the values responding to the instance(s) of the specified <paramref name="key"/>,
    /// if any.
    /// </summary>
    /// <param name="key">The key whose values to find.</param>
    /// <returns>
    /// All values corresponding to the specified <paramref name="key"/>, if any, or an empty
    /// enumerable if the key was not present.
    /// </returns>
    public IEnumerable<string> this[string key]
        => Items.Any() ? Items.Where(x => x.key == key)
                              .Select(x => x.value)
                       : Enumerable.Empty<string>();
    /// <summary>
    /// Gets the <paramref name="index"/>th item corresponding to the specified <paramref name="key"/>.
    /// </summary>
    /// <param name="key">The key whose corresponding item to get.</param>
    /// <param name="index">
    /// The index of the item of the corresponding <paramref name="key"/> to get.
    /// <br/><br/><b>Throws an exception</b> if <c>index &lt; 0</c>!
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <c>index &lt; 0</c>.</exception>
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
    /// <summary>
    /// Transforms the values corresponding to <paramref name="key"/> using the specified <paramref name="function"/>.
    /// </summary>
    /// <param name="key">The key whose values to transform.</param>
    /// <param name="function">The function which with to transform the appropriate values.</param>
    /// <returns></returns>
    public UrlQuery Set(string key, Func<string, string> function)
    {
        List<(string key, string value)> items = new();
        foreach ((string key, string value) item in Items)
        {
            if (item.key == key)
            {
                items.Add((key, function(item.value)));
            }
            else
            {
                items.Add(item);
            }
        }
        return new(items);
    }
    private UrlQuery KeepOrDrop(string[] tags, bool drop)
    {
        List<(string key, string value)> items = new();
        foreach ((string key, string value) item in Items)
        {
            if (tags.Contains(item.key) ^ drop)
                items.Add(item);
        }
        return new(items);
    }
    /// <param name="keys">The keys to keep.</param>
    /// <returns>
    /// A new <c>UrlQuery</c> with <b>only</b> the specified <paramref name="keys"/> or their
    /// corresponding values.
    /// </returns>
    public UrlQuery Keep(params string[] keys) => KeepOrDrop(keys, drop: false);
    /// <param name="keys">The keys to drop.</param>
    /// <returns>
    /// A new <c>UrlQuery</c><b>without</b> the specified <paramref name="keys"/> or their
    /// corresponding values.
    /// </returns>
    public UrlQuery Drop(params string[] keys) => KeepOrDrop(keys, drop: true);
    /// <summary>
    /// Parses the values corresponding to the specified <paramref name="key"/> using an <see
    /// cref="IParsable{TSelf}"/> type.
    /// </summary>
    /// <typeparam name="T">The type to change the values into.</typeparam>
    /// <param name="key">The key of the values to parse.</param>
    /// <param name="formatProvider">The format provider to provide to the parsing function.</param>
    /// <returns>
    /// The parsed values corresponding to the specified <paramref name="key"/>, <b>excluding</b>
    /// any unparsable values.
    /// </returns>
    /// <remarks>
    /// <b>NOTE:</b> If parsing fails on any value, it will be <b>skipped</b>. The returned
    /// enumerable may be empty if no values were successfully parsed or there were no corresponding
    /// values in the first place.
    /// </remarks>
    public IEnumerable<T> Parse<T>(string key, IFormatProvider? formatProvider = null)
        where T : IParsable<T>
    {
        foreach (string value in this[key])
            if (T.TryParse(value, formatProvider, out T? result))
                yield return result;
    }
    /// <summary>
    /// <inheritdoc cref="Parse{T}(string, IFormatProvider?)" path="/summary"/>
    /// </summary>
    /// <typeparam name="T"><inheritdoc cref="Parse{T}(string, IFormatProvider?)" path="/typeparam[@name='T']"/></typeparam>
    /// <param name="key"><inheritdoc cref="Parse{T}(string, IFormatProvider?)" path="/param[@name='key']"/></param>
    /// <param name="formatProvider"><inheritdoc cref="Parse{T}(string, IFormatProvider?)" path="/param[@name='formatProvider']"/></param>
    /// <returns>
    /// The parsed values corresponding to the specified <paramref name="key"/>, <b>including</b>
    /// any unparsable values.
    /// </returns>
    /// <remarks>
    /// <b>NOTE:</b> If parsing fails on any value, it will be <b>included</b> as a <see
    /// langword="null"/> value. The returned enumerable may still be empty if there were no
    /// corresponding values in the first place.
    /// </remarks>
    public IEnumerable<T?> TryParse<T>(string key, IFormatProvider? formatProvider = null)
        where T : IParsable<T>
    {
        foreach (string value in this[key])
        {
            T.TryParse(value, formatProvider, out T? result);
            yield return result;
        }
    }
    /// <summary>
    /// Parses the values corresponding to the specified <paramref name="key"/> using the specified
    /// <paramref name="parser"/>.
    /// </summary>
    /// <typeparam name="T"><inheritdoc cref="Parse{T}(string, IFormatProvider?)" path="/typeparam[@name='T']"/></typeparam>
    /// <param name="key"><inheritdoc cref="Parse{T}(string, IFormatProvider?)" path="/param[@name='key']"/></param>
    /// <param name="parser">
    /// A function which parses a <see langword="string"/> into an object of type <typeparamref name="T"/>.
    /// </param>
    /// <returns>The parsed values corresponding to the specified <paramref name="key"/>.</returns>
    /// <remarks>
    /// <b>NOTE:</b> The returned enumerable may be empty if there were no values in the first place.
    /// </remarks>
    public IEnumerable<T> Parse<T>(string key, Func<string, T> parser)
        => this[key].Select(parser);
    /// <summary>
    /// Reserializes this query into a valid URL query string.
    /// </summary>
    /// <returns>A valid URL query string representing this query.</returns>
    /// <remarks>TODO: properly encode characters which require escaping (and decode them when constructing UrlQueries)</remarks>
    public override string ToString() => $"?{Items.Select(x => $"{x.key}={x.value}").Aggregate((x, y) => $"{x}&{y}")}";
    /// <summary>
    /// Implements <see cref="IEnumerable{T}"/> over this query's keys and values.
    /// </summary>
    public IEnumerator<(string key, string value)> GetEnumerator()
        => Items.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator()
        => ((IEnumerable)Items).GetEnumerator();
    /// <summary>
    /// Implicitly converts a UrlQuery into its corresponding query string.
    /// </summary>
    /// <param name="query">The query to convert.</param>
    public static implicit operator string(UrlQuery query) => query.ToString();
    /// <summary>
    /// Implicitly converts the specified list of <paramref name="parameters"/> to a UrlQuery.
    /// </summary>
    /// <param name="parameters"><inheritdoc cref="UrlQuery(IEnumerable{ValueTuple{string, string}})" path="/param[@name='parameters']"/></param>
    public static implicit operator UrlQuery(List<(string key, string value)> parameters)
        => new(parameters);
}