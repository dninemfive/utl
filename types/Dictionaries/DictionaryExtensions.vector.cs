using System.Numerics;

namespace d9.utl.types;
public static partial class DictionaryExtensions
{
    /// <summary>
    /// Merges two dictionaries by applying the specified <paramref name="function"/>.
    /// </summary>
    /// <typeparam name="K"><inheritdoc cref="Transform{K, V}(IReadOnlyDictionary{K, V}, Func{V, V})" path="/typeparam[@name='K']"/></typeparam>
    /// <typeparam name="V"><inheritdoc cref="Transform{K, V}(IReadOnlyDictionary{K, V}, Func{V, V})" path="/typeparam[@name='V']"/></typeparam>
    /// <param name="a">The first dictionary to be merged. Will <b>NOT</b> be modified.</param>
    /// <param name="b">The second dictionary to be merged. Will <b>NOT</b> be modified.</param>
    /// <param name="function">
    /// The function which merges the values from dictionaries <paramref name="a"/> and 
    /// <paramref name="b"/>.
    /// </param>
    /// <param name="defaultValue">
    /// The value a key will take before the <paramref name="function"/> is applied if it is not 
    /// present in one dictionary or the other.
    /// </param>
    /// <returns>
    /// A new read-only dictionary whose keys are the union of the keys of <paramref name="a"/> and
    /// <paramref name="b"/> and whose values are the result of applying <paramref name="function"/>
    /// to the key's corresponding values in <c>a</c> and <c>b</c>.
    /// </returns>
    public static IReadOnlyDictionary<K, V> Transform<K, V>(this IReadOnlyDictionary<K, V> a,
                                                                 IReadOnlyDictionary<K, V> b,
                                                                 Func<V, V, V> function,
                                                                 V defaultValue)
        where K : notnull
        where V : INumber<V>
    {
        Dictionary<K, V> result = new();
        foreach (K key in a.Keys.Union(b.Keys))
        {
            a.TryGetValue(key, out V? aValue);
            b.TryGetValue(key, out V? bValue);
            result[key] = function(aValue ?? defaultValue, bValue ?? defaultValue);
        }
        return result.AsReadOnly();
    }
    /// <summary>
    /// Adds the values of two dictionaries together, assuming keys missing from one dictionary 
    /// or the other are the additive identity (usually 0).
    /// </summary>
    /// <typeparam name="K"><inheritdoc cref="Transform{K, V}(IReadOnlyDictionary{K, V}, Func{V, V})" path="/typeparam[@name='K']"/></typeparam>
    /// <typeparam name="V"><inheritdoc cref="Transform{K, V}(IReadOnlyDictionary{K, V}, Func{V, V})" path="/typeparam[@name='V']"/></typeparam>
    /// <param name="left">The first dictionary to add.</param>
    /// <param name="right">The second dictionary to add.</param>
    /// <returns>
    /// A dictionary whose keys are the union of the keys of <paramref name="left"/> and
    /// <paramref name="right"/> and whose values are the result of <c>left + right</c>.
    /// </returns>
    public static IReadOnlyDictionary<K, V> Add<K, V>(this IReadOnlyDictionary<K, V> left, IReadOnlyDictionary<K, V> right)
        where K : notnull
        where V : INumber<V>
        => left.Transform(right, (av, bv) => av + bv, V.AdditiveIdentity);
    /// <summary>
    /// Subtracts the values of two dictionaries from one another, assuming keys missing from one dictionary 
    /// or the other are the additive identity (usually 0).
    /// </summary>
    /// <typeparam name="K"><inheritdoc cref="Transform{K, V}(IReadOnlyDictionary{K, V}, Func{V, V})" path="/typeparam[@name='K']"/></typeparam>
    /// <typeparam name="V"><inheritdoc cref="Transform{K, V}(IReadOnlyDictionary{K, V}, Func{V, V})" path="/typeparam[@name='V']"/></typeparam>
    /// <param name="left">The dictionary whose values are on the left-hand side when subtracting.</param>
    /// <param name="right">The dictionary whose values are on the right-hand side when subtracting.</param>
    /// <returns>
    /// A dictionary whose keys are the union of the keys of <paramref name="left"/> and
    /// <paramref name="right"/> and whose values are the result of <c>left - right</c>.
    /// </returns>
    public static IReadOnlyDictionary<K, V> Subtract<K, V>(this IReadOnlyDictionary<K, V> left, IReadOnlyDictionary<K, V> right)
        where K : notnull
        where V : INumber<V>
        => left.Transform(right, (av, bv) => av - bv, V.AdditiveIdentity);
    /// <summary>
    /// Multiplies the corresponding values of two dictionaries by one another, assuming keys
    /// missing from one dictionary or the other are the multiplicative identity (usually 1).
    /// </summary>
    /// <typeparam name="K"><inheritdoc cref="Transform{K, V}(IReadOnlyDictionary{K, V}, Func{V, V})" path="/typeparam[@name='K']"/></typeparam>
    /// <typeparam name="V"><inheritdoc cref="Transform{K, V}(IReadOnlyDictionary{K, V}, Func{V, V})" path="/typeparam[@name='V']"/></typeparam>
    /// <param name="a">The first dictionary to multiply.</param>
    /// <param name="b">The second dictionary to multiply.</param>
    /// <returns>
    /// A dictionary whose keys are the union of the keys of <paramref name="a"/> and
    /// <paramref name="b"/> and whose values are the result of <c>left * right</c>.
    /// </returns>
    public static IReadOnlyDictionary<K, V> Multiply<K, V>(this IReadOnlyDictionary<K, V> a, IReadOnlyDictionary<K, V> b)
        where K : notnull
        where V : INumber<V>
        => a.Transform(b, (av, bv) => av * bv, V.MultiplicativeIdentity);
    /// <summary>
    /// Divides the values of one dictionary by another, assuming keys missing from one dictionary 
    /// or the other are the multiplicative identity (usually 1).
    /// </summary>
    /// <typeparam name="K"><inheritdoc cref="Transform{K, V}(IReadOnlyDictionary{K, V}, Func{V, V})" path="/typeparam[@name='K']"/></typeparam>
    /// <typeparam name="V"><inheritdoc cref="Transform{K, V}(IReadOnlyDictionary{K, V}, Func{V, V})" path="/typeparam[@name='V']"/></typeparam>
    /// <param name="left">The dictionary whose values are on the left-hand side when subtracting.</param>
    /// <param name="right">The dictionary whose values are on the right-hand side when subtracting.</param>
    /// <returns>
    /// A dictionary whose keys are the union of the keys of <paramref name="left"/> and
    /// <paramref name="right"/> and whose values are the result of <c>left / right</c>.
    /// </returns>
    public static IReadOnlyDictionary<K, V> Divide<K, V>(this IReadOnlyDictionary<K, V> left, IReadOnlyDictionary<K, V> right)
        where K : notnull
        where V : INumber<V>
        => left.Transform(right, (av, bv) => av / bv, V.MultiplicativeIdentity);
}
