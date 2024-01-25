using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
namespace d9.utl;
/// <summary>
/// Unnecessarily generalized class which lets you count arbitrary objects.
/// </summary>
/// <typeparam name="K">The object to count.</typeparam>
/// <typeparam name="V">A <see href="">number</see> type to use to count.</typeparam>
public class CountingDictionary<K, V> : IEnumerable<KeyValuePair<K,V>>, IDictionary<K, V>
    where K : notnull
    where V : INumberBase<V>
{
    private readonly Dictionary<K, V> _dict = new();
    /// <summary>
    /// Initializes an empty counting dictionary with 0 entries.
    /// </summary>
    public CountingDictionary() { }
    /// <summary>
    /// Initializes a counting dictionary with the specified keys and counts.
    /// </summary>
    /// <param name="dict">The dictionary with which to initialize the counting dictionary.</param>
    public CountingDictionary(Dictionary<K, V> dict)
    {
        _dict = dict;
    }
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
    /// <returns>The number of times that key has been <see cref="Increment(K)">inc</see> to the dictionary.</returns>
    public V this[K key] { get => _dict.TryGetValue(key, out V? value) ? value : V.Zero; set => _dict[key] = value; }
    IEnumerator<KeyValuePair<K,V>> IEnumerable<KeyValuePair<K,V>>.GetEnumerator() => _dict.GetEnumerator();
    /// <summary>
    /// Implements <see cref="IEnumerator"/><c>.GetEnumerator()</c> for the dictionary's key-value pairs.
    /// </summary>
    /// <returns>
    /// The internal dictionary's enumerator.
    /// </returns>
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

    public void Add(K key, V value) 
        => this[key] = value;
    public bool ContainsKey(K key) 
        => _dict.ContainsKey(key);
    public bool Remove(K key) 
        => _dict.Remove(key);
    public bool TryGetValue(K key, [MaybeNullWhen(false)] out V value)
        => _dict.TryGetValue(key, out value);
    public void Add(KeyValuePair<K, V> item)
        => Add(item.Key, item.Value);
    public void Clear()
        => _dict.Clear();
    public bool Contains(KeyValuePair<K, V> item)
        => _dict.Contains(item);
    public void CopyTo(KeyValuePair<K, V>[] array, int arrayIndex)
        => ((ICollection)_dict).CopyTo(array, arrayIndex);
    public bool Remove(KeyValuePair<K, V> item)
        => ((ICollection<KeyValuePair<K,V>>)_dict).Remove(item);
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
    public bool IsReadOnly => false;
    // todo: CountMany
}
