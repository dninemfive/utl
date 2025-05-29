namespace d9.utl.types;
/// <summary>
/// Abstract base class for <see cref="CountingDictionary{K, V}"/> and
/// <see cref="DefaultDictionary{K, V}"/>, containing code shared between the two.
/// </summary>
/// <typeparam name="K">The type of the keys of this dictionary.</typeparam>
/// <typeparam name="V">The type of the values of this dictionary.</typeparam>
/// <remarks>
/// Implemented in the following files:
/// <br/>- <c>BaseDictionary.cs</c> - methods specific to <c>BaseDictionary</c>
/// <br/>- <c>BaseDictionary.IDictionary.cs</c> - implementations of 
/// <see cref="IDictionary{TKey, TValue}"/> and <see cref="IEnumerable{T}"/>
/// <br/>- <c>BaseDictionary.IReadOnlyDictionary.cs</c> - implementation of
/// <see cref="IReadOnlyDictionary{TKey, TValue}"/>
/// </remarks>
public abstract partial class BaseDictionary<K, V>()
    : IDictionary<K, V>, IEnumerable<KeyValuePair<K, V>>, IReadOnlyDictionary<K, V>
    where K : notnull
{
    /// <summary>
    /// The internal dictionary used to store data.
    /// </summary>
    protected readonly Dictionary<K, V> _dictionary = new();
    /// <summary>
    /// Gets the default value if a key is not found.
    /// </summary>
    /// <param name="key">The key whose value is missing.</param>
    /// <returns>The value the dictionary should assign to the key if it is not present.</returns>
    protected abstract V GetDefaultValue(K key);
    /// <summary>
    /// Constructs a new dictionary with the specified initial key-value pairs.
    /// </summary>
    /// <param name="initial">The initial key-value pairs with which this dictionary should start.</param>
    public BaseDictionary(IEnumerable<KeyValuePair<K, V>> initial)
        : this()
        => _dictionary = initial.ToDictionary();
    /// <inheritdoc cref="BaseDictionary{K, V}(IEnumerable{KeyValuePair{K,V}})"/>
    public BaseDictionary(IEnumerable<(K key, V value)> initial)
        : this()
        => _dictionary = initial.ToDictionary();
    /// <summary>
    /// Prints this dictionary prettily.
    /// </summary>
    /// <returns>The dictionary in key-value-pair notation, wrapped in braces.</returns>
    public override string ToString()
        => _dictionary.ListNotation(brackets: ("{", "}"));
}