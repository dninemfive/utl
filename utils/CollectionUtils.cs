using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    public static IEnumerable<T> WeightedShuffled<T>(this IEnumerable<T> original, Func<T, double> weight, Random? random = null)
    {
        List<T> items = original.ToList();
        while (items.Any())
        {
            T item = items.WeightedRandomElement(weight, random: random);
            _ = items.Remove(item);
            yield return item;
        }
    }
}