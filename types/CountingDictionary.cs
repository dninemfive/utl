using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Runtime.InteropServices;

namespace d9.utl;
/// <summary>
/// Unnecessarily generalized class which lets you count arbitrary objects.
/// </summary>
/// <typeparam name="K">The object to count.</typeparam>
/// <typeparam name="V">A <see href="">number</see> type to use to count.</typeparam>
public class CountingDictionary<K, V> : IEnumerable<KeyValuePair<K,V>>
    where K : notnull
    where V : INumberBase<V>
{
    private readonly Dictionary<K, V> _dict = new();
    /// <summary>
    /// Initializes an empty counting dictionary with 0 entries.
    /// </summary>
    public CountingDictionary() { }
    /// <summary>
    /// Adds to a specific key.
    /// </summary>
    /// <param name="key">The object instance to count.</param>
    public void Add(K key)
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
    /// <returns>The number of times that key has been <see cref="Add(K)">added</see> to the dictionary.</returns>
    public V this[K key] => _dict.TryGetValue(key, out V? value) ? value : V.Zero;
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
    /// <summary>
    /// The sum of the values in this dictionary.
    /// </summary>
    public V Total => _dict.Count > 0 ? _dict.Values.Aggregate((x, y) => x + y) : V.Zero;
    /// <summary>
    /// The number of keys in this dictionary.
    /// </summary>
    public int Count => _dict.Count;
}
