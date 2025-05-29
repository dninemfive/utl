using System.Numerics;

namespace d9.utl;
/// <summary>
/// Mathematical utilities.
/// </summary>
public static class MathUtils
{
    /// <summary>
    /// Clamps an <see cref="IComparable"/> within a specified range.
    /// </summary>
    /// <typeparam name="T">The type of the arguments. Must implement <see cref="IComparable"/>.</typeparam>
    /// <param name="t">The value to clamp.</param>
    /// <param name="min">The smaller of the two values.</param>
    /// <param name="max">The larger of the two values.</param>
    /// <returns>
    /// <c>min</c> if it's larger than <c>t</c>, <c>max</c> if it's smaller than <c>t</c>, or
    /// <c>t</c> otherwise.
    /// </returns>
    public static T Clamp<T>(this T t, T min, T max) where T : IComparable
    {
        if (t.CompareTo(min) < 0)
            return min;
        else if (t.CompareTo(max) > 0)
            return max;
        return t;
    }
    /// <summary>
    /// Gets the mean of an arbitrary set of numbers.
    /// </summary>
    /// <typeparam name="T">
    /// A type which <see
    /// href="https://learn.microsoft.com/en-us/dotnet/api/system.numerics.iadditionoperators-3">can
    /// be added</see> to itself and <see
    /// href="https://learn.microsoft.com/en-us/dotnet/api/system.numerics.idivisionoperators-3">can
    /// be divided</see> by an <see langword="int"/>.
    /// </typeparam>
    /// <param name="numbers">An array of numbers to be averaged.</param>
    /// <returns>
    /// The <see href="https://en.wikipedia.org/wiki/Arithmetic_mean">arithmetic mean</see> of the
    /// given numbers.
    /// </returns>
    public static T Mean<T>(params T[] numbers) where T : INumber<T>
        => numbers.Aggregate((x, y) => x + y) / T.CreateChecked(numbers.Length);
    /// <summary>
    /// Gets the mean of an arbitrary set of numbers.
    /// </summary>
    /// <typeparam name="T">
    /// A type which <see
    /// href="https://learn.microsoft.com/en-us/dotnet/api/system.numerics.iadditionoperators-3">can
    /// be added</see> to itself and <see
    /// href="https://learn.microsoft.com/en-us/dotnet/api/system.numerics.idivisionoperators-3">can
    /// be divided</see> by an <see langword="int"/>.
    /// </typeparam>
    /// <param name="numbers">An array of numbers to be averaged.</param>
    /// <returns>
    /// The <see href="https://en.wikipedia.org/wiki/Arithmetic_mean">arithmetic mean</see> of the
    /// given numbers.
    /// </returns>
    public static T Mean<T>(this IEnumerable<T> numbers) where T : INumber<T>
        => numbers.Aggregate((x, y) => x + y) / T.CreateChecked(numbers.Count());
    /// <summary>
    /// Gets the median, i.e. middle value when ordered, of an arbitrary set of orderable objects.
    /// </summary>
    /// <typeparam name="T">A type which implements <see cref="IComparable"/>.</typeparam>
    /// <param name="orderables">The collection whose median to find.</param>
    /// <param name="ifEven">
    /// A function which breaks a tie when the collection is even. For example, the median of an
    /// even set of numbers is the mean of the two middle numbers.
    /// </param>
    /// <returns>The median as described above.</returns>
    public static T Median<T>(this IEnumerable<T> orderables, Func<T, T, T> ifEven) where T : IComparable
    {
        List<T> ordered = orderables.OrderBy(x => x).ToList();
        if (ordered.Count.IsOdd())
            return ordered[ordered.Count / 2];
        else
            return ifEven(ordered[ordered.Count / 2 - 1], ordered[ordered.Count / 2]);
    }
    /// <summary>
    /// Gets the median of an arbitrary set of numbers.
    /// </summary>
    /// <typeparam name="T">
    /// A type which is <see cref="IComparable">comparable</see> and implements <see
    /// href="https://learn.microsoft.com/en-us/dotnet/api/system.numerics.iadditionoperators-3">addition
    /// operators</see> with itself and <see
    /// href="https://learn.microsoft.com/en-us/dotnet/api/system.numerics.idivisionoperators-3">division
    /// operators</see> with itself as the dividend and <see langword="int"/> as the divisor.
    /// </typeparam>
    /// <param name="numbers">The numbers whose median to find.</param>
    /// <returns>
    /// The mathematical median, i.e. the middle number of the ordered collection if the collection
    /// has an odd number of elements, or the average of the two middle numbers if it has an even
    /// number of elements.
    /// </returns>
    public static T Median<T>(params T[] numbers) where T : IComparable, INumber<T>
        => numbers.Median();
    /// <inheritdoc cref="Median{T}(T[])"/>
    public static T Median<T>(this IEnumerable<T> numbers) where T : IComparable, INumber<T>
        => Median(numbers, (x, y) => Mean(x, y));
    /// <typeparam name="T">
    /// A <see
    /// href="https://learn.microsoft.com/en-us/dotnet/api/system.numerics.inumber-1">numeric</see> type.
    /// </typeparam>
    /// <param name="t">The number whose oddness to check.</param>
    /// <returns><see langword="true"/> if the number is odd, or <see langword="false"/> otherwise.</returns>
    public static bool IsOdd<T>(this T t) where T : INumber<T> => T.IsOddInteger(t);
    /// <typeparam name="T">
    /// A <see
    /// href="https://learn.microsoft.com/en-us/dotnet/api/system.numerics.inumber-1">numeric</see> type.
    /// </typeparam>
    /// <param name="t">The number whose evenness to check.</param>
    /// <returns><see langword="true"/> if the number is even, or <see langword="false"/> otherwise.</returns>
    public static bool IsEven<T>(this T t) where T : INumber<T> => T.IsEvenInteger(t);
}