using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace d9.utl;
/// <summary>
/// Mathematical utilities.
/// </summary>
public static class MathUtils
{
    /// <summary>
    /// Gets the mean of an arbitrary set of numbers.
    /// </summary>
    /// <typeparam name="T">A type which implements 
    /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.numerics.iadditionoperators-3">addition operators</see> with itself
    /// and <see href="https://learn.microsoft.com/en-us/dotnet/api/system.numerics.idivisionoperators-3">division operators</see>
    /// with itself as the dividend and <see langword="int"/> as the divisor.
    /// </typeparam>
    /// <param name="numbers">An array of numbers to be averaged.</param>
    /// <returns>The <see href="https://en.wikipedia.org/wiki/Arithmetic_mean">arithmetic mean</see> of the given numbers.</returns>
    public static T Mean<T>(params T[] numbers) where T : IAdditionOperators<T,T,T>, IDivisionOperators<T,int,T>
        => numbers.Aggregate((x, y) => x + y) / numbers.Length;
    /// <summary>
    /// Gets the median, i.e. middle value when ordered, of an arbitrary set of orderable objects.
    /// </summary>
    /// <typeparam name="T">A type which implements <see cref="IComparable"/>.</typeparam>
    /// <param name="orderables">The collection whose median to find.</param>
    /// <param name="ifEven">A function which breaks a tie when the collection is even. For example, the median of an even set of numbers
    /// is the mean of the two middle numbers.</param>
    /// <returns>The median as described above.</returns>
    public static T Median<T>(IEnumerable<T> orderables, Func<T, T, T> ifEven) where T : IComparable
    {
        List<T> ordered = orderables.OrderBy(x => x).ToList();
        if (ordered.Count.IsOdd()) return ordered[ordered.Count / 2];
        else return ifEven(ordered[ordered.Count / 2 - 1], ordered[ordered.Count / 2]);
    }
    /// <summary>
    /// Gets the median of an arbitrary set of numbers.
    /// </summary>
    /// <typeparam name="T">A type which is <see cref="IComparable">comparable</see> and implements 
    /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.numerics.iadditionoperators-3">addition operators</see> with itself
    /// and <see href="https://learn.microsoft.com/en-us/dotnet/api/system.numerics.idivisionoperators-3">division operators</see>
    /// with itself as the dividend and <see langword="int"/> as the divisor.
    /// </typeparam>
    /// <param name="numbers">The numbers whose median to find.</param>
    /// <returns>The mathematical median, i.e. the middle number of the ordered collection if the collection has an odd number of elements, or the average
    /// of the two middle numbers if it has an even number of elements.</returns>
    public static T Median<T>(params T[] numbers) where T : IComparable, IAdditionOperators<T, T, T>, IDivisionOperators<T, int, T>
        => Median(numbers, (x, y) => Mean(x, y));
    public static bool IsOdd<T>(this T t) where T : INumberBase<T> => T.IsOddInteger(t);
    public static bool IsEven<T>(this T t) where T : INumberBase<T> => T.IsEvenInteger(t);
}
