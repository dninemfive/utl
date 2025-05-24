using d9.utl.types;
using d9.utl.types.Dictionaries;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace d9.utl;
/// <summary>
/// Unnecessarily generalized class which lets you count arbitrary objects.
/// </summary>
/// <typeparam name="K">The object to count.</typeparam>
/// <typeparam name="V">A <see href="">number</see> type to use to count.</typeparam>
public class CountingDictionary<K, V> : BaseDictionary<K, V>, IDictionaryMathOperators<K, V>
    where K : notnull
    where V : INumber<V>
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
}