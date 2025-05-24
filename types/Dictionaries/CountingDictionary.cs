using d9.utl.types;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace d9.utl;
/// <summary>
/// Unnecessarily generalized class which lets you count arbitrary objects.
/// </summary>
/// <typeparam name="K">The object to count.</typeparam>
/// <typeparam name="V">A <see href="">number</see> type to use to count.</typeparam>
public class CountingDictionary<K, V> : BaseDictionary<K, V>
    where K : notnull
    where V : INumberBase<V>
{
    public CountingDictionary() : base() { }
    public CountingDictionary(IEnumerable<KeyValuePair<K, V>> initial) : base(initial) { }
    public CountingDictionary(IEnumerable<(K key, V value)> initial) : base(initial) { }
    /// <summary>
    /// Initializes a counting dictionary by counting the specified <paramref name="items"/>.
    /// </summary>
    /// <param name="items">The initial items to count.</param>
    public CountingDictionary(IEnumerable<K> items) : base()
    {
        Add(items);
    }
    protected override V GetDefaultValue(K key)
        => V.Zero;
    /// <summary>
    /// Increments the count of the specified <paramref name="key"/> by one. If the value of the
    /// <c>key</c> is unset, it is initialized to zero before being incremented.
    /// </summary>
    /// <param name="key">The key to increment.</param>
    public void Add(K key)
        => this[key] += V.One;
    /// <summary>
    /// <see cref="Add(K)">Adds</see> many items to the count at once.
    /// </summary>
    /// <param name="keys">The items to add.</param>
    public void Add(IEnumerable<K> keys)
    {
        foreach (K k in keys)
            Add(k);
    }
    /// <summary>
    /// The sum of the values in this dictionary.
    /// </summary>
    public V Total => _dictionary.Count > 0 
                    ? _dictionary.Values.Aggregate((x, y) => x + y) 
                    : V.Zero;
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
        => [..dict.Select(x => new KeyValuePair<K, V>(x.Key, x.Value * factor))];
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
    public static CountingDictionary<K, V> operator /(CountingDictionary<K, V> dict, V factor)
        => dict * (V.One / factor);
    public static CountingDictionary<K, V> operator -(CountingDictionary<K, V> dict)
        => dict * (V.Zero - V.One);
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
    public static CountingDictionary<K, V> operator +(IReadOnlyDictionary<K, V> a, CountingDictionary<K, V> b)
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
    // todo: implement all of these as extensions IReadOnlyDictionary, IReadOnlyDictionary => IReadOnlyDictionary
    public static CountingDictionary<K, V> operator -(IReadOnlyDictionary<K, V> a, CountingDictionary<K, V> b)
        => a + -b;
}