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
    /// Breaks a collection into <c><paramref name="n"/></c> parts of roughly equal size.<br/><br/>
    /// Specifically, the size of each part will either be <c>floor(<paramref name="original"/>.Count()</c> / <c><paramref name="n"/>)</c>
    /// or <c>floor(<paramref name="original"/>.Count()</c> / <c><paramref name="n"/>) + 1</c>, with the larger parts coming first.
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
    /// <summary>
    /// Randomly reorders a collection.
    /// </summary>
    /// <typeparam name="T">The type of the enumerable's elements.</typeparam>
    /// <param name="original">The original enumerable, which is not modified.</param>
    /// <returns>The elements of <c>original</c>, in a random order.</returns>
    public static IEnumerable<T> Shuffled<T>(this IEnumerable<T> original)
    {
        throw new NotImplementedException();
    }
}