using System.Numerics;

namespace d9.utl.types.Dictionaries;
public static partial class DictionaryExtensions
{
    public static IReadOnlyDictionary<K, V> Transform<K, V>(this IReadOnlyDictionary<K, V> a, 
                                                                 IReadOnlyDictionary<K, V> b, 
                                                                 Func<V, V, V> f,
                                                                 V vDefault)
        where K : notnull
        where V : INumber<V>
    {
        Dictionary<K, V> result = new();
        foreach(K key in a.Keys.Union(b.Keys))
        {
            a.TryGetValue(key, out V? aValue);
            b.TryGetValue(key, out V? bValue);
            result[key] = f(aValue ?? vDefault, bValue ?? vDefault);
        }
        return result.AsReadOnly();
    }
    public static IReadOnlyDictionary<K, V> Add<K, V>(this IReadOnlyDictionary<K, V> a, IReadOnlyDictionary<K, V> b)
        where K : notnull
        where V : INumber<V>
        => a.Transform(b, (av, bv) => av + bv, V.AdditiveIdentity);
    public static IReadOnlyDictionary<K, V> Subtract<K, V>(this IReadOnlyDictionary<K, V> a, IReadOnlyDictionary<K, V> b)
        where K : notnull
        where V : INumber<V>
        => a.Transform(b, (av, bv) => av - bv, V.AdditiveIdentity);
    public static IReadOnlyDictionary<K, V> Multiply<K, V>(this IReadOnlyDictionary<K, V> a, IReadOnlyDictionary<K, V> b)
        where K : notnull
        where V : INumber<V>
        => a.Transform(b, (av, bv) => av * bv, V.MultiplicativeIdentity);
    public static IReadOnlyDictionary<K, V> Divide<K, V>(this IReadOnlyDictionary<K, V> a, IReadOnlyDictionary<K, V> b)
        where K : notnull
        where V : INumber<V>
        => a.Transform(b, (av, bv) => av / bv, V.MultiplicativeIdentity);
}
