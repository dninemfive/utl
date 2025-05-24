namespace d9.utl.types;
public abstract partial class BaseDictionary<K, V>()
    where K : notnull
{
    protected readonly Dictionary<K, V> _dictionary = new();
    protected abstract V GetDefaultValue(K key);
    public BaseDictionary(IEnumerable<KeyValuePair<K, V>> initial)
        : this()
        => _dictionary = initial.ToDictionary();
    public BaseDictionary(IEnumerable<(K key, V value)> initial)
        : this()
        => _dictionary = initial.ToDictionary();
    public override string ToString()
        => _dictionary.ListNotation(brackets: ("{", "}"));
}