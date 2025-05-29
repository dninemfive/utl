using System.Numerics;

namespace d9.utl.types;
/// <summary>
/// Extension methods which allow doing math operations on entire dictionaries at once, if their
/// values implement <see cref="INumber{TSelf}"/>.
/// </summary>
/// <remarks>
/// Implemented in the following files:
/// <br/>- <c>DictionaryExtensions.scalar.cs</c> - operations between dictionaries and single values
/// <br/>- <c>DictionaryExtensions.vector.cs</c> - operations between dictionaries and dictionaries
/// </remarks>
public static partial class DictionaryExtensions
{
    /// <summary>
    /// Transforms a specified <paramref name="dictionary"/>, setting each key-value pair by
    /// applying the specified <paramref name="function"/>.
    /// </summary>
    /// <typeparam name="K">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="V">The type of values in the dictionary.</typeparam>
    /// <param name="dictionary">The dictionary to transform. Will <b>NOT</b> be modified.</param>
    /// <param name="function">The function which generates the output value for each input key-value pair.</param>
    /// <returns>A read-only copy of <paramref name="dictionary"/> transformed by <paramref name="function"/>.</returns>
    public static IReadOnlyDictionary<K, V> Transform<K, V>(this IReadOnlyDictionary<K, V> dictionary, Func<V, V> function)
        where K : notnull
        where V : INumber<V>
    {
        Dictionary<K, V> result = new();
        foreach((K key, V value) in dictionary)
        {
            result[key] = function(value);
        }
        return result.AsReadOnly();
    }
    /// <summary>
    /// Adds the specified <paramref name="addend"/> to all values in the <paramref name="dictionary"/>.
    /// </summary>
    /// <typeparam name="K"><inheritdoc cref="Transform{K, V}(IReadOnlyDictionary{K, V}, Func{V, V})" path="/typeparam[@name='K']"/></typeparam>
    /// <typeparam name="V"><inheritdoc cref="Transform{K, V}(IReadOnlyDictionary{K, V}, Func{V, V})" path="/typeparam[@name='V']"/></typeparam>
    /// <param name="dictionary">The input dictionary. Will <b>NOT</b> be modified.</param>
    /// <param name="addend">The constant which will be added to all values in the dictionary.</param>
    /// <returns>
    /// A read-only copy of <paramref name="dictionary"/> where each key's value has been increased
    /// by <paramref name="addend"/>.
    /// </returns>
    public static IReadOnlyDictionary<K, V> Add<K, V>(this IReadOnlyDictionary<K, V> dictionary, V addend)
        where K : notnull
        where V : INumber<V>
        => dictionary.Transform(v => v + addend);
    /// <summary>
    /// Subtracts the specified <paramref name="subtrahend"/> from all values in the <paramref name="dictionary"/>.
    /// </summary>
    /// <typeparam name="K"><inheritdoc cref="Transform{K, V}(IReadOnlyDictionary{K, V}, Func{V, V})" path="/typeparam[@name='K']"/></typeparam>
    /// <typeparam name="V"><inheritdoc cref="Transform{K, V}(IReadOnlyDictionary{K, V}, Func{V, V})" path="/typeparam[@name='V']"/></typeparam>
    /// <param name="dictionary">The input dictionary. Will <b>NOT</b> be modified.</param>
    /// <param name="subtrahend">The constant which will be subtracted from all values in the dictionary.</param>
    /// <returns>
    /// A read-only copy of <paramref name="dictionary"/> where each key's value has been decreased
    /// by <paramref name="subtrahend"/>.
    /// </returns>
    public static IReadOnlyDictionary<K, V> Subtract<K, V>(this IReadOnlyDictionary<K, V> dictionary, V subtrahend)
        where K : notnull
        where V : INumber<V>
        => dictionary.Transform(v => v - subtrahend);
    /// <summary>
    /// Multiplies all values in the <paramref name="dictionary"/> by the specified <paramref name="factor"/>.
    /// </summary>
    /// <typeparam name="K"><inheritdoc cref="Transform{K, V}(IReadOnlyDictionary{K, V}, Func{V, V})" path="/typeparam[@name='K']"/></typeparam>
    /// <typeparam name="V"><inheritdoc cref="Transform{K, V}(IReadOnlyDictionary{K, V}, Func{V, V})" path="/typeparam[@name='V']"/></typeparam>
    /// <param name="dictionary">The input dictionary. Will <b>NOT</b> be modified.</param>
    /// <param name="factor">The constant which will multiply all values in the dictionary.</param>
    /// <returns>
    /// A read-only copy of <paramref name="dictionary"/> where each key's value has been multiplied
    /// by <paramref name="factor"/>.
    /// </returns>
    public static IReadOnlyDictionary<K, V> Multiply<K, V>(this IReadOnlyDictionary<K, V> dictionary, V factor)
        where K : notnull
        where V : INumber<V>
        => dictionary.Transform(v => v * factor);
    /// <summary>
    /// Divides all values in the <paramref name="dictionary"/> by the specified <paramref name="divisor"/>.
    /// </summary>
    /// <typeparam name="K"><inheritdoc cref="Transform{K, V}(IReadOnlyDictionary{K, V}, Func{V, V})" path="/typeparam[@name='K']"/></typeparam>
    /// <typeparam name="V"><inheritdoc cref="Transform{K, V}(IReadOnlyDictionary{K, V}, Func{V, V})" path="/typeparam[@name='V']"/></typeparam>
    /// <param name="dictionary">The input dictionary. Will <b>NOT</b> be modified.</param>
    /// <param name="divisor">The constant which will divide all values in the dictionary.</param>
    /// <returns>
    /// A read-only copy of <paramref name="dictionary"/> where each key's value has been divided
    /// by <paramref name="divisor"/>.
    /// </returns>
    public static IReadOnlyDictionary<K, V> Divide<K, V>(this IReadOnlyDictionary<K, V> dictionary, V divisor)
        where K : notnull
        where V : INumber<V>
        => dictionary.Transform(v => v / divisor);
}
