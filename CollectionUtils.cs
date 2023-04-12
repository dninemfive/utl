using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace d9.utl;
/// <summary>
/// Utilities which perform operations on collections.
/// </summary>
public static class CollectionUtils
{
    /// <summary>
    /// Breaks a collection into <c>n</c> parts of roughly equal size.
    /// </summary>
    /// <remarks>Does not modify the original.</remarks>
    /// <typeparam name="T">The type the enumerable to break up holds.</typeparam>
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
    /* this is stupid
    /// <summary>
    /// Adds an element into the collection at a key in a dictionary of collections.
    /// </summary>
    /// <typeparam name="K">The type of the key.</typeparam>
    /// <typeparam name="C">The type of the collection stored in the dictionary.</typeparam>
    /// <typeparam name="V">The type of the elements of <c>C</c>.</typeparam>
    /// <param name="dict">The dictionary containing the collections to add to.</param>
    /// <param name="key">The key to add the item to.</param>
    /// <param name="value">The item to add to the collection in the dictionary at K.</param>
    public static void Add<K, C, V>(this Dictionary<K, C> dict, K key, V value) 
        where K : notnull 
        where C : ICollection<V>, new()
    {
        if (!dict.ContainsKey(key) || dict[key] is null) dict[key] = new();
        dict[key].Add(value);
    } */
}