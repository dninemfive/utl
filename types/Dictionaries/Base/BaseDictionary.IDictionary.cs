using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace d9.utl.types;
public abstract partial class BaseDictionary<K, V>
    : IDictionary<K, V>, IEnumerable<KeyValuePair<K, V>>
{
    public V this[K key] 
    {
        get
        {
            if(!_dictionary.TryGetValue(key, out V? value))
            {
                value = GetDefaultValue(key);
                this[key] = value;
            }
            return value;
        }
        set => _dictionary[key] = value;
    }
    /// <summary>
    /// Enumerates over the keys in this dictionary.
    /// </summary>
    public ICollection<K> Keys 
        => _dictionary.Keys;
    /// <summary>
    /// Enumerates over the values in this dictionary.
    /// </summary>
    public ICollection<V> Values 
        => _dictionary.Values;
    /// <summary>
    /// The number of keys in this dictionary.
    /// </summary>
    public int Count 
        => _dictionary.Count;
    /// <summary>
    /// Whether this dictionary is read-only. It is not.
    /// </summary>
    public bool IsReadOnly 
        => ((ICollection<KeyValuePair<K, V>>)_dictionary).IsReadOnly;
    /// <summary>
    /// Adds a <paramref name="key"/> and corresponding <paramref name="value"/> to the dictionary.
    /// </summary>
    /// <param name="key">The key whose <paramref name="value"/> to set.</param>
    /// <param name="value">The value the <paramref name="key"/> will have.</param>
    public void Add(K key, V value)
        => _dictionary.Add(key, value);
    /// <summary>
    /// <see cref="Add(K, V?)">Adds</see> a specified key-value pair to this dictionary.
    /// </summary>
    /// <param name="item">The pair to add.</param>
    public void Add(KeyValuePair<K, V> item)
        => this.Add(item.Key, item.Value);
    /// <summary>
    /// Removes all items from this dictionary.
    /// </summary>
    public void Clear()
        => _dictionary.Clear();
    /// <summary>
    /// Checks whether a given key-value pair is present in the dictionary.
    /// </summary>
    /// <param name="item">The pair whose presence to check for.</param>
    /// <returns>
    /// <see langword="true"/> if the <paramref name="item"/> was present, or <see
    /// langword="false"/> otherwise.
    /// </returns>
    public bool Contains(KeyValuePair<K, V> item)
        => _dictionary.Contains(item);
    /// <summary>
    /// Whether this dictionary contains the specified <paramref name="key"/>.
    /// </summary>
    /// <param name="key">The key whose presence to check for.</param>
    public bool ContainsKey(K key)
        => _dictionary.ContainsKey(key);
    /// <summary>
    /// Copies the specified data to this dictionary.
    /// </summary>
    /// <param name="array">The data to copy.</param>
    /// <param name="arrayIndex">The index of the first item to copy.</param>
    public void CopyTo(KeyValuePair<K, V>[] array, int arrayIndex)
        => ((ICollection<KeyValuePair<K, V>>)_dictionary).CopyTo(array, arrayIndex);
    /// <summary>
    /// Implements <see cref="IEnumerator"/><c>.GetEnumerator()</c> for the dictionary's key-value pairs.
    /// </summary>
    /// <returns>The internal dictionary's enumerator.</returns>
    public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
        => _dictionary.GetEnumerator();
    /// <summary>
    /// Removes a specified <paramref name="key"/> from this dictionary.
    /// </summary>
    /// <param name="key">The key to remove.</param>
    /// <returns>
    /// <see langword="true"/> if the <paramref name="key"/> was successfully removed, i.e. it was
    /// present before this call, or <see langword="false"/> otherwise.
    /// </returns>
    public bool Remove(K key)
        => _dictionary.Remove(key);
    /// <summary>
    /// Removes a specified key-value pair from this dictionary.
    /// </summary>
    /// <param name="item">The pair to remove.</param>
    /// <returns>
    /// <see langword="true"/> if the <paramref name="item"/> was removed, or <see
    /// langword="false"/> otherwise.
    /// </returns>
    public bool Remove(KeyValuePair<K, V> item)
        => ((ICollection<KeyValuePair<K, V>>)_dictionary).Remove(item);
    /// <summary>
    /// Tries to get the <paramref name="value"/> corresponding to the specified <paramref name="key"/>.
    /// </summary>
    /// <param name="key">The key whose <paramref name="value"/> to get.</param>
    /// <param name="value">If the <paramref name="key"/> was present, the corresponding value.</param>
    /// <returns>
    /// <see langword="true"/> if the <paramref name="key"/> was present, or <see langword="false"/> otherwise.
    /// </returns>
    public bool TryGetValue(K key, [MaybeNullWhen(false)] out V value)
        => _dictionary.TryGetValue(key, out value);
    IEnumerator IEnumerable.GetEnumerator()
        => _dictionary.GetEnumerator();
}