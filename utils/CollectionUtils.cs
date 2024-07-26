using System.Numerics;

namespace d9.utl;
/// <summary>
/// If Linq is so good, why isn't there a-
/// </summary>
public static class Linq2
{
    /// <summary>
    /// Breaks a collection into <c><paramref name="n"/></c> parts of roughly equal size. <br/><br/>
    /// Specifically, the size of each part will either be <c>floor( <paramref
    /// name="original"/>.Count()</c> / <c><paramref name="n"/>)</c> or <c>floor( <paramref
    /// name="original"/>.Count()</c> / <c><paramref name="n"/>) + 1</c>, with the larger parts
    /// coming first.
    /// </summary>
    /// <remarks>
    /// Does not modify the original. Unlike <see
    /// cref="Enumerable.Chunk{TSource}(IEnumerable{TSource}, int)"/>, this specifies a number of
    /// chunks, rather than the size of each chunk.
    /// </remarks>
    /// <typeparam name="T">The type of the elements of the enumerable.</typeparam>
    /// <param name="original">The enumerable to be broken up.</param>
    /// <param name="n">The number of parts to break the enumerable into.</param>
    /// <returns>An enumerable of enumerables, broken up as described above.</returns>
    public static IEnumerable<IEnumerable<T>> BreakInto<T>(this IEnumerable<T> original, int n)
    {
        int partSize = original.Count() / n;
        int remainder = original.Count() - (n * partSize);
        int ct = 0;
        for (int i = 0; i < n; i++)
        {
            int thisSize = partSize + (remainder-- > 0 ? 1 : 0);
            int endSize = original.Count() - thisSize - ct;
            yield return original.Skip(ct).SkipLast(endSize);
            ct += thisSize;
        }
    }
    /// <summary>
    /// Randomly reorders a collection.
    /// </summary>
    /// <typeparam name="T">The type of the elements of the enumerable.</typeparam>
    /// <param name="original">The original enumerable, which is not modified.</param>
    /// <param name="random">
    /// A <see cref="Random"/> instance to use for number generation. If not specified, an internal
    /// instance will be used.
    /// </param>
    /// <returns>The elements of <c>original</c>, in a random order.</returns>
    public static IEnumerable<T> Shuffled<T>(this IEnumerable<T> original, Random? random = null)
    {
        List<T> items = original.ToList();
        while (items.Any())
        {
            T item = items.RandomElement(random);
            _ = items.Remove(item);
            yield return item;
        }
    }
    /// <summary>
    /// Selects an element from the given <paramref name="enumerable"/> randomly.
    /// </summary>
    /// <typeparam name="T">The type of the elements of the given <paramref name="enumerable"/>.</typeparam>
    /// <param name="enumerable">The enumerable from which to select a random element.</param>
    /// <param name="random"><inheritdoc cref="Shuffled{T}(IEnumerable{T}, Random?)" path="/param[@name='random']"/></param>
    /// <returns>An element from a random index in the specified <paramref name="enumerable"/>.</returns>
    public static T RandomElement<T>(this IEnumerable<T> enumerable, Random? random = null)
    {
        random ??= _cachedRandom;
        return enumerable.ElementAt(random.Next(enumerable.Count()));
    }
    private static readonly Random _cachedRandom = new();
    /// <summary>
    /// Selects a random element from the specified <paramref name="enumerable"/> based on a
    /// supplied <paramref name="weight">weight function</paramref>.
    /// </summary>
    /// <typeparam name="T">The type of the elements from which to select a random element.</typeparam>
    /// <param name="enumerable">The enumerable from which to select a random element.</param>
    /// <param name="weight">A function which provides the weight of any given element.</param>
    /// <param name="targetWeight">
    /// If specified, the first element which exceeds this value will be returned.
    /// <br/><br/><b>NOTE:</b> If the target weight is too large, the last item will always be returned!
    /// </param>
    /// <param name="random"><inheritdoc cref="Shuffled{T}(IEnumerable{T}, Random?)" path="/param[@name='random']"/></param>
    /// <returns>
    /// A random element from <paramref name="enumerable"/> such that, on average, any given element
    /// will have <paramref name="weight"/>( <c>element</c>) / (total weight) probability of occuring.
    /// </returns>
    public static T WeightedRandomElement<T>(this IEnumerable<T> enumerable,
                                                  Func<T, double> weight,
                                                  double targetWeight = -1,
                                                  Random? random = null)
    {
        if (!enumerable.Any())
            throw new ArgumentException("WeightedRandomElement must be called with a collection containing at least one item.");
        random ??= new();
        if (targetWeight < 0)
        {
            double totalWeight = enumerable.Select(x => weight(x)).Sum();
            targetWeight = random.NextDouble() * totalWeight;
        }
        foreach (T item in enumerable)
        {
            targetWeight -= weight(item);
            if (targetWeight <= 0)
                return item;
        }
        return enumerable.Last();
    }
    /// <summary>
    /// Selects a random key from the specified <paramref name="dict"/> ionary based on the relative
    /// size of its corresponding value.
    /// </summary>
    /// <typeparam name="K">The type of the key to return.</typeparam>
    /// <param name="dict">
    /// The dictionary of keys and their corresponding weights from which to select a random key.
    /// </param>
    /// <param name="targetWeight">
    /// <inheritdoc cref="WeightedRandomElement{T}(IEnumerable{T}, Func{T, double}, double,
    /// Random?)" path="/param[@name='targetWeight']"/>
    /// </param>
    /// <param name="random">
    /// <inheritdoc cref="WeightedRandomElement{T}(IEnumerable{T}, Func{T, double}, double,
    /// Random?)" path="/param[@name='random']"/>
    /// </param>
    /// <returns>
    /// <inheritdoc cref="WeightedRandomElement{T}(IEnumerable{T}, Func{T, double}, double,
    /// Random?)" path="/returns"/>
    /// </returns>
    public static K WeightedRandomElement<K>(this IReadOnlyDictionary<K, double> dict, double targetWeight = -1, Random? random = null)
        where K : notnull
        => dict.WeightedRandomElement(x => x.Value, targetWeight, random).Key;
    /// <summary>
    /// Adds an arbitrary set of <paramref name="dictionaries"/> with numerical values together.
    /// </summary>
    /// <typeparam name="K">The key type of the dictionary.</typeparam>
    /// <typeparam name="V">The value type of the dictionary.</typeparam>
    /// <param name="dictionaries">The dictionaries to add together.</param>
    /// <returns>
    /// A new dictionary with all keys found in any of the <paramref name="dictionaries"/>, with
    /// their corresponding values being the sum of the corresponding values in any dictionaries
    /// which contain them.
    /// </returns>
    public static IReadOnlyDictionary<K, V> Sum<K, V>(this IEnumerable<IReadOnlyDictionary<K, V>> dictionaries)
        where K : notnull
        where V : INumberBase<V>
    {
        Dictionary<K, V> result = new();
        IEnumerable<K> allKeys = dictionaries.SelectMany(x => x.Keys);
        foreach (K key in allKeys)
        {
            V sum = V.Zero;
            foreach (IReadOnlyDictionary<K, V> dictionary in dictionaries)
                if (dictionary.TryGetValue(key, out V? value))
                    sum += value;
            result[key] = sum;
        }
        return result;
    }
    /// <summary>
    /// Creates a new dictionary from the specified <paramref name="tuples"/>.
    /// </summary>
    /// <typeparam name="K">The key type of the specified <paramref name="tuples"/>.</typeparam>
    /// <typeparam name="V">The value type of the specified <paramref name="tuples"/>.</typeparam>
    /// <param name="tuples">The tuples from which to create a dictionary.</param>
    /// <returns>
    /// A new dictionary whose items are all the unique first items of the specified <paramref
    /// name="tuples"/> and their values are the second item from each corresponding tuple.
    /// </returns>
    public static Dictionary<K, V> ToDictionary<K, V>(this IEnumerable<(K key, V value)> tuples)
        where K : notnull
        => new(tuples.Select(x => new KeyValuePair<K, V>(x.key, x.value)));
    /// <summary>
    /// Creates a new dictionary from the specified <paramref name="items"/>.
    /// </summary>
    /// <typeparam name="K">The key type of the specified <paramref name="items"/>.</typeparam>
    /// <typeparam name="V">The value type of the specified <paramref name="items"/>.</typeparam>
    /// <param name="items">The items from which to create a dictionary.</param>
    /// <returns>A new dictionary containing the specified key-value pairs.</returns>
    public static Dictionary<K, V> ToDictionary<K, V>(this IEnumerable<KeyValuePair<K, V>> items)
        where K : notnull
        => new(items);
}