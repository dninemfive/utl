namespace d9.utl.types;
/// <summary>
/// A dictionary which generates default values if keys are not found, based on a lambda specified
/// when it's initialized.
/// </summary>
/// <typeparam name="K"><inheritdoc cref="BaseDictionary{K, V}" path="/typeparam[@name='K']"/></typeparam>
/// <typeparam name="V"><inheritdoc cref="BaseDictionary{K, V}" path="/typeparam[@name='V']"/></typeparam>
public class DefaultDictionary<K, V>
    : BaseDictionary<K, V>
    where K : notnull
{
    private readonly Func<K, V> _defaultFrom;
    /// <summary>
    /// Creates an empty <c>DefaultDictionary</c> with the specified <paramref name="defaultFunction"/>.
    /// </summary>
    /// <param name="defaultFunction">
    /// A function which generates the values for keys which are not already present in the dictionary.
    /// </param>
    public DefaultDictionary(Func<K, V> defaultFunction) 
        : base()
        => _defaultFrom = defaultFunction;
    /// <summary>
    /// Creates a <c>DefaultDictionary</c> with the specified starting <paramref name="values"/> and
    /// the specified <paramref name="defaultFunction"/>.
    /// </summary>
    /// <param name="values">The initial values this dictionary should have.</param>
    /// <param name="defaultFunction"><inheritdoc cref="DefaultDictionary{K, V}(Func{K,V})" path="/param[@name='defaultFunction']"/></param>
    public DefaultDictionary(IEnumerable<KeyValuePair<K, V>> values, Func<K, V> defaultFunction)
        : base(values)
        => _defaultFrom = defaultFunction;
    /// <inheritdoc cref="DefaultDictionary{K, V}(IEnumerable{KeyValuePair{K,V}}, Func{K,V})"/>
    public DefaultDictionary(IEnumerable<(K key, V value)> values, Func<K, V> defaultFunction)
        : base(values)
        => _defaultFrom = defaultFunction;
    /// <summary>
    /// Gets the default for the specified <paramref name="key"/>, which for a <c>DefaultDictionary</c>
    /// is the <c>defaultFunction</c> passed in to the constructor.
    /// </summary>
    /// <param name="key">The key whose value to get.</param>
    /// <returns>The default value for the specified key.</returns>
    protected override V GetDefaultValue(K key)
        => _defaultFrom(key);
}
