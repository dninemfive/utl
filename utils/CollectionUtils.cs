using System.Numerics;

namespace d9.utl;
/// <summary>
/// If Linq is so good, why isn't there a-
/// </summary>
public static class Linq2
{
    /// <summary>
    /// Breaks a collection into <c><paramref name="n"/></c> parts of roughly equal size.<br/><br/>
    /// Specifically, the size of each part will either be <c>floor(<paramref name="original"/>.Count()</c> / <c><paramref name="n"/>)</c>
    /// or <c>floor(<paramref name="original"/>.Count()</c> / <c><paramref name="n"/>) + 1</c>, with the larger parts coming first.
    /// </summary>
    /// <remarks>Does not modify the original.</remarks>
    /// <typeparam name="T">The type of the elements of the enumerable.</typeparam>
    /// <param name="original">The enumerable to be broken up.</param>
    /// <param name="n">The number of parts to break the enumerable into.</param>
    /// <returns>An enumerable of enumerables, broken up as described above.</returns>
    [Obsolete("This is literally the same as Linq's .Chunk() i think. Will remove pending testing")]
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
    /// <returns>The elements of <c>original</c>, in a random order.</returns>
    public static IEnumerable<T> Shuffled<T>(this IEnumerable<T> original, Random? random = null)
    {
        List<T> items = original.ToList();
        while(items.Any())
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
    /// <param name="enumerable">The input enumerable.</param>
    /// <param name="random">
    ///     A <see cref="Random"/> object which, if provided, will be used to generate the index of the element to return.
    ///     <br/><br/>
    ///     If <see langword="null"/>, a new <see cref="Random"/> will be created for use in this function.
    /// </param>
    /// <returns>A random element from <paramref name="enumerable"/>.</returns>
    public static T RandomElement<T>(this IEnumerable<T> enumerable, Random? random = null)
    {
        random ??= _cachedRandom;
        return enumerable.ElementAt(random.Next(enumerable.Count()));
    }
    private static readonly Random _cachedRandom = new();
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="enumerable"></param>
    /// <param name="weight"></param>
    /// <param name="targetWeight"></param>
    /// <param name="random"></param>
    /// <returns></returns>
    /// <remarks>Implements <see href="https://xlinux.nist.gov/dads//HTML/reservoirSampling.html">this algorithm</see>.</remarks>
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
    public static K WeightedRandomElement<K>(this IReadOnlyDictionary<K, double> dict, double targetWeight = -1, Random? random = null)
        where K : notnull
        => dict.WeightedRandomElement(x => x.Value, targetWeight, random).Key;
    /// <summary>
    /// Adds an arbitrary set of <paramref name="dictionaries"/> with numerical values together.
    /// </summary>
    /// <typeparam name="K">The key type of the dictionary.</typeparam>
    /// <typeparam name="V">The value type of the dictionary.</typeparam>
    /// <param name="dictionaries">The dictionaries to add together.</param>
    /// <returns>A new dictionary with all keys found in any of the <paramref name="dictionaries"/>, with their corresponding values being the sum of the corresponding values in any dictionaries which contain them.</returns>
    public static IReadOnlyDictionary<K, V> Sum<K, V>(this IEnumerable<IReadOnlyDictionary<K, V>> dictionaries)
        where K : notnull
        where V : INumberBase<V>
    {
        Dictionary<K, V> result = new();
        IEnumerable<K> allKeys = dictionaries.SelectMany(x => x.Keys);
        foreach(K key in allKeys)
        {
            V sum = V.Zero;
            foreach(IReadOnlyDictionary<K, V> dictionary in dictionaries)
                if (dictionary.TryGetValue(key, out V? value))
                    sum += value;
            result[key] = sum;
        }
        return result;
    }
}