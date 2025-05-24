using System.Numerics;

namespace d9.utl.types.Dictionaries;
public static partial class DictionaryExtensions
{
    public static IReadOnlyDictionary<K, V> Transform<K, V>(this IReadOnlyDictionary<K, V> dict, Func<V, V> f)
        where K : notnull
        where V : INumber<V>
    {
        Dictionary<K, V> result = new();
        foreach((K key, V value) in dict)
        {
            result[key] = f(value);
        }
        return result.AsReadOnly();
    }
    public static IReadOnlyDictionary<K, V> Add<K, V>(this IReadOnlyDictionary<K, V> dict, V c)
        where K : notnull
        where V : INumber<V>
        => dict.Transform(v => v + c);
    public static IReadOnlyDictionary<K, V> Subtract<K, V>(this IReadOnlyDictionary<K, V> dict, V c)
        where K : notnull
        where V : INumber<V>
        => dict.Transform(v => v - c);
    public static IReadOnlyDictionary<K, V> Multiply<K, V>(this IReadOnlyDictionary<K, V> dict, V c)
        where K : notnull
        where V : INumber<V>
        => dict.Transform(v => v * c);
    public static IReadOnlyDictionary<K, V> Divide<K, V>(this IReadOnlyDictionary<K, V> dict, V c)
        where K : notnull
        where V : INumber<V>
        => dict.Transform(v => v / c);
}
