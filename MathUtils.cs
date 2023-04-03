using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace d9.utils;
/// <summary>
/// Mathematical utilities.
/// </summary>
public static class MathUtils
{
    /// <summary>
    /// Gets the mean of an arbitrary set of numbers.
    /// </summary>
    /// <param name="numbers">An array of numbers to be averaged.</param>
    /// <returns>The <see href="https://en.wikipedia.org/wiki/Arithmetic_mean">arithmetic mean</see> of the given numbers.</returns>
    public static decimal Mean(params decimal[] numbers) => numbers.Aggregate((x, y) => x + y) / numbers.Length;
    /// <inheritdoc cref="Mean(decimal[])"/>
    public static decimal Mean(this IEnumerable<decimal> numbers) => Mean(numbers.ToArray());
}
