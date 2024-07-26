using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace d9.utl;
/// <summary>
/// Unnecessarily generalized class which lets you count arbitrary objects.
/// </summary>
/// <typeparam name="K">The object to count.</typeparam>
/// <typeparam name="V">A <see href="">number</see> type to use to count.</typeparam>
public class CountingDictionary<K, V> : IEnumerable<KeyValuePair<K, V>>, IDictionary<K, V>, IReadOnlyDictionary<K, V>
    where K : notnull
    where V : INumberBase<V>
{
    private readonly Dictionary<K, V> _dict = new();
    /// <summary>
    /// Initializes an empty counting dictionary.
    /// </summary>
    public CountingDictionary()
    { }
    /// <summary>
    /// Initializes a counting dictionary with the specified keys and counts.
    /// </summary>
    /// <param name="dict">The dictionary with which to initialize the counting dictionary.</param>
    public CountingDictionary(Dictionary<K, V> dict)
    {
        _dict = dict;
    }
    /// <summary>
    /// <inheritdoc cref="CountingDictionary{K, V}.CountingDictionary(Dictionary{K, V})" path="/summary"/>
    /// </summary>
    /// <param name="data">
    /// The data, in key-value pair format, to initialize the counting dictionary with.
    /// </param>
    public CountingDictionary(IEnumerable<KeyValuePair<K, V>> data) : this(data.ToDictionary(x => x.Key, x => x.Value)) { }
    /// <inheritdoc cref="CountingDictionary{K, V}.CountingDictionary(IEnumerable{KeyValuePair{K, V}})"/>
    public CountingDictionary(IEnumerable<(K key, V value)> data) : this(data.Select(x => new KeyValuePair<K, V>(x.key, x.value))) { }
    /// <summary>
    /// Adds to a specific key.
    /// </summary>
    /// <param name="key">The object instance to count.</param>
    public void Increment(K key)
    {
#pragma warning disable CA1854
        // double lookup is unavoidable because i need ref access to the variable
        if (_dict.ContainsKey(key))
#pragma warning restore CA1854
        {
            _dict[key]++;
        }
        else
        {
            _dict[key] = V.One;
        }
    }
    /// <param name="key">The key whose count to retrieve.</param>
    /// <returns>
    /// The number of times that <paramref name="key"/> has been <see
    /// cref="Increment(K)">counted</see> by the dictionary.
    /// </returns>
    public V this[K key] { get => _dict.TryGetValue(key, out V? value) ? value : V.Zero; set => _dict[key] = value; }
    IEnumerator<KeyValuePair<K, V>> IEnumerable<KeyValuePair<K, V>>.GetEnumerator() => _dict.GetEnumerator();
    /// <summary>
    /// Implements <see cref="IEnumerator"/><c>.GetEnumerator()</c> for the dictionary's key-value pairs.
    /// </summary>
    /// <returns>The internal dictionary's enumerator.</returns>
    public IEnumerator GetEnumerator() => _dict.GetEnumerator();
    /// <summary>
    /// Gets this dictionary's items ordered by their values from greatest to least.
    /// </summary>
    /// <returns>This dictionary's key-value pairs in descending order by value.</returns>
    public IEnumerable<KeyValuePair<K, V>> Descending() => _dict.OrderByDescending(x => x.Value);

    /// <summary>
    /// Gets this dictionary's items ordered by their values from least to greatest.
    /// </summary>
    /// <returns>This dictionary's key-value pairs in ascending order by value.</returns>
    public IEnumerable<KeyValuePair<K, V>> Ascending() => _dict.OrderBy(x => x.Value);
    /// <summary>
    /// Adds a <paramref name="key"/> to the dictionary with a specified starting <paramref name="value"/>.
    /// </summary>
    /// <param name="key">The key whose value to set.</param>
    /// <param name="value"></param>
    public void Add(K key, V? value = default)
        => _dict.Add(key, value ?? V.One);
    /// <summary>
    /// Whether this dictionary contains the specified <paramref name="key"/>.
    /// </summary>
    /// <param name="key">The key whose presence to check for.</param>
    public bool ContainsKey(K key)
        => _dict.ContainsKey(key);
    /// <summary>
    /// Removes a specified <paramref name="key"/> from this dictionary.
    /// </summary>
    /// <param name="key">The key to remove.</param>
    /// <returns>
    /// <see langword="true"/> if the <paramref name="key"/> was successfully removed, i.e. it was
    /// present before this call, or <see langword="false"/> otherwise.
    /// </returns>
    public bool Remove(K key)
        => _dict.Remove(key);
    /// <summary>
    /// Tries to get the <paramref name="value"/> corresponding to the specified <paramref name="key"/>.
    /// </summary>
    /// <param name="key">The key whose <paramref name="value"/> to get.</param>
    /// <param name="value">If the <paramref name="key"/> was present, the corresponding value.</param>
    /// <returns>
    /// <see langword="true"/> if the <paramref name="key"/> was present, or <see langword="false"/> otherwise.
    /// </returns>
    public bool TryGetValue(K key, [MaybeNullWhen(false)] out V value)
        => _dict.TryGetValue(key, out value);
    /// <summary>
    /// <see cref="Add(K, V?)">Adds</see> a specified key-value pair to this dictionary.
    /// </summary>
    /// <param name="item">The pair to add.</param>
    public void Add(KeyValuePair<K, V> item)
        => Add(item.Key, item.Value);
    /// <summary>
    /// Removes all items from this dictionary.
    /// </summary>
    public void Clear()
        => _dict.Clear();
    /// <summary>
    /// Checks whether a given key-value pair is present in the dictionary.
    /// </summary>
    /// <param name="item">The pair whose presence to check for.</param>
    /// <returns>
    /// <see langword="true"/> if the <paramref name="item"/> was present, or <see
    /// langword="false"/> otherwise.
    /// </returns>
    public bool Contains(KeyValuePair<K, V> item)
        => _dict.Contains(item);
    /// <summary>
    /// Copies the specified data to this dictionary.
    /// </summary>
    /// <param name="array">The data to copy.</param>
    /// <param name="arrayIndex">The index of the first item to copy.</param>
    public void CopyTo(KeyValuePair<K, V>[] array, int arrayIndex)
        => ((ICollection)_dict).CopyTo(array, arrayIndex);
    /// <summary>
    /// Removes a specified key-value pair from this dictionary.
    /// </summary>
    /// <param name="item">The pair to remove.</param>
    /// <returns>
    /// <see langword="true"/> if the <paramref name="item"/> was removed, or <see
    /// langword="false"/> otherwise.
    /// </returns>
    public bool Remove(KeyValuePair<K, V> item)
        => ((ICollection<KeyValuePair<K, V>>)_dict).Remove(item);
    /// <summary>
    /// The sum of the values in this dictionary.
    /// </summary>
    public V Total => _dict.Count > 0 ? _dict.Values.Aggregate((x, y) => x + y) : V.Zero;
    /// <summary>
    /// The number of keys in this dictionary.
    /// </summary>
    public int Count => _dict.Count;
    /// <summary>
    /// Enumerates over the keys in this dictionary.
    /// </summary>
    public IEnumerable<K> Keys => _dict.Keys;
    /// <summary>
    /// Enumerates over the values in this dictionary.
    /// </summary>
    public IEnumerable<V> Values => _dict.Values;
    ICollection<K> IDictionary<K, V>.Keys => Keys.ToList();
    ICollection<V> IDictionary<K, V>.Values => Values.ToList();
    /// <summary>
    /// Whether this dictionary is read-only. All <c>CountingDictionary</c> s are <b>NOT</b> read-only.
    /// </summary>
    public bool IsReadOnly => false;
    /// <param name="keys">The keys whose values to sum.</param>
    /// <returns>The sum of the counts of the specified <paramref name="keys"/>.</returns>
    public V CountMany(IEnumerable<K> keys)
    {
        V result = V.Zero;
        foreach (K key in keys)
            result += this[key];
        return result;
    }
    /// <inheritdoc cref="CountMany(IEnumerable{K})"/>
    public V CountMany(params K[] keys)
        => CountMany(keys.AsEnumerable());
    private static V SumItems(IEnumerable<KeyValuePair<K, V>> items)
    {
        V result = V.Zero;
        foreach ((K _, V v) in items)
            result += v;
        return result;
    }
    /// <param name="predicate">
    /// A function which selects whether any given key-value pair should be counted.
    /// </param>
    /// <returns>The sum of the values of the items which match the specified <paramref name="predicate"/>.</returns>
    public V CountWhere(Func<KeyValuePair<K, V>, bool> predicate)
        => SumItems(this.Where(predicate));
    /// <param name="predicate">
    /// A function which selects whether any given key's value should be counted.
    /// </param>
    /// <returns>The sum of the values of the keys which match the specified <paramref name="predicate"/>.</returns>
    public V CountWhere(Func<K, bool> predicate)
        => SumItems(this.Where(x => predicate(x.Key)));
    /// <param name="predicate">A function which selects whether any given value should be counted.</param>
    /// <returns>The sum of the values which match the specified <paramref name="predicate"/>.</returns>
    public V CountWhere(Func<V, bool> predicate)
        => SumItems(this.Where(x => predicate(x.Value)));
    /// <summary>
    /// Multiplies all items in the specified dictionary by a constant <paramref name="factor"/>.
    /// </summary>
    /// <param name="dict">The dictionary to multiply.</param>
    /// <param name="factor">The factor by which to multiply each key.</param>
    /// <returns>
    /// A new <see cref="CountingDictionary{K, V}"/> where each value has been multiplied by a
    /// constant <paramref name="factor"/>.
    /// </returns>
    /// <remarks>Does <b>NOT</b> modify the original dictionary.</remarks>
    public static CountingDictionary<K, V> operator *(CountingDictionary<K, V> dict, V factor)
        => new(dict.Select(x => new KeyValuePair<K, V>(x.Key, x.Value * factor)));
    /// <summary>
    /// Multiplies all items in the specified dictionary by a constant <paramref name="factor"/>.
    /// </summary>
    /// <param name="dict">The dictionary to multiply.</param>
    /// <param name="factor">The factor by which to multiply each key.</param>
    /// <returns>
    /// A new <see cref="CountingDictionary{K, V}"/> where each value has been multiplied by a
    /// constant <paramref name="factor"/>.
    /// </returns>
    /// <remarks>Does <b>NOT</b> modify the original dictionary.</remarks>
    public static CountingDictionary<K, V> operator *(V factor, CountingDictionary<K, V> dict)
        => dict * factor;
    /// <summary>
    /// Adds the corresponding items from two dictionaries together.
    /// </summary>
    /// <param name="a">The first CountingDictionary to add.</param>
    /// <param name="b">The second dictionary to add.</param>
    /// <returns>
    /// A new <see cref="CountingDictionary{K, V}"/> whose keys are the union of the keys in both
    /// dictionaries, and whose values are as follows: <br/>- If the key was <b>only</b> in either
    /// dictionary <paramref name="a"/> or <paramref name="b"/>, the value in that dictionary;
    /// <br/>- If the key was in both dictionaries, the sum of its corresponding value in each.
    /// </returns>
    /// <remarks>Does <b>NOT</b> modify the original dictionary.</remarks>
    public static CountingDictionary<K, V> operator +(CountingDictionary<K, V> a, IReadOnlyDictionary<K, V> b)
    {
        Dictionary<K, V> result = new();
        foreach (K key in a.Keys.Union(b.Keys))
        {
            a.TryGetValue(key, out V? aValue);
            b.TryGetValue(key, out V? bValue);
            result[key] = (aValue ?? V.Zero) + (bValue ?? V.Zero);
        }
        return new(result);
    }
}