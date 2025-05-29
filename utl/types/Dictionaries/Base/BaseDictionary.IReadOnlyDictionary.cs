namespace d9.utl.types;
public abstract partial class BaseDictionary<K, V>
    : IReadOnlyDictionary<K, V>
{
    IEnumerable<K> IReadOnlyDictionary<K, V>.Keys => ((IReadOnlyDictionary<K, V>)_dictionary).Keys;
    IEnumerable<V> IReadOnlyDictionary<K, V>.Values => ((IReadOnlyDictionary<K, V>)_dictionary).Values;
}