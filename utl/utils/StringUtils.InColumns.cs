namespace d9.utl;
public static partial class StringUtils
{
    /// <summary>
    /// Prints the specified <paramref name="values"/> so that they are in columns of the specified widths.
    /// </summary>
    /// <typeparam name="T">The type of the objects to print.</typeparam>
    /// <param name="values">
    /// An enumerable holding the objects to print paired with the width of their respective columns.
    /// </param>
    /// <param name="columnSeparator">A string which will separate each column.</param>
    /// <param name="truncationSuffix">
    /// If non- <see langword="null"/>, each column will be <see cref="Truncate(string, int,
    /// string)">truncated</see> if it is too wide, with this string appended to indicate the
    /// truncation occurred.
    /// </param>
    /// <param name="nullString"><inheritdoc cref="Constants.NullString" path="/summary"/></param>
    /// <returns>
    /// A string corresponding to the objects <c>t</c> in order, with columns padded to their <c>width</c>.
    /// </returns>
    public static string InColumns<T>(this IEnumerable<(T t, int width)> values,
                                           string columnSeparator = Default.ColumnSeparator,
                                           string? truncationSuffix = null,
                                           string nullString = Constants.NullString)
    {
        List<string> strs = new();
        foreach ((T t, int width) in values)
        {
            string str = t.PrintNull(nullString);
            if (truncationSuffix is not null)
                str = str.Truncate(width, truncationSuffix);
            strs.Add(str.PadRight(width));
        }
        return strs.Any() ? strs.JoinWithDelimiter(columnSeparator) : "";
    }
    /// <summary>
    /// <inheritdoc cref="InColumns{T}(IEnumerable{ValueTuple{T, int}}, string, string?, string)" path="/summary"/>
    /// </summary>
    /// <typeparam name="T">
    /// <inheritdoc cref="InColumns{T}(IEnumerable{ValueTuple{T, int}}, string, string?, string)" path="/typeparam[@name='T']"/>
    /// </typeparam>
    /// <param name="values">
    /// <inheritdoc cref="InColumns{T}(IEnumerable{ValueTuple{T, int}}, string, string?, string)" path="/param[@name='values']"/>
    /// </param>
    /// <param name="columnSeparator">
    /// <inheritdoc cref="InColumns{T}(IEnumerable{ValueTuple{T, int}}, string, string?, string)" path="/param[@name='columnSeparator']"/>
    /// </param>
    /// <param name="truncate">
    /// If <see langword="true"/>, each column will be <see cref="Truncate(string, int,
    /// string)">truncated</see> if it is too wide.
    /// </param>
    /// <param name="nullString"><inheritdoc cref="Constants.NullString" path="/summary"/></param>
    /// <returns>
    /// <inheritdoc cref="InColumns{T}(IEnumerable{ValueTuple{T, int}}, string, string?, string)" path="/returns"/>
    /// </returns>
    public static string InColumns<T>(this IEnumerable<(T t, int width)> values,
                                           string columnSeparator = Default.ColumnSeparator,
                                           bool truncate = false,
                                           string nullString = Constants.NullString)
        => values.InColumns(columnSeparator, truncate ? Default.TruncationSuffix : null, nullString);
    /// <summary>
    /// <inheritdoc cref="InColumns{T}(IEnumerable{ValueTuple{T, int}}, string, string?, string)" path="/summary"/>
    /// </summary>
    /// <typeparam name="T">
    /// <inheritdoc cref="InColumns{T}(IEnumerable{ValueTuple{T, int}}, string, string?, string)" path="/typeparam[@name='T']"/>
    /// </typeparam>
    /// <param name="values">
    /// <inheritdoc cref="InColumns{T}(IEnumerable{ValueTuple{T, int}}, string, string?, string)" path="/param[@name='values']"/>
    /// </param>
    /// <param name="widths">
    /// An enumerable of the same length as <paramref name="values"/> indicating the width the
    /// corresponding column should have.
    /// </param>
    /// <param name="columnSeparator">
    /// <inheritdoc cref="InColumns{T}(IEnumerable{ValueTuple{T, int}}, string, string?, string)" path="/param[@name='columnSeparator']"/>
    /// </param>
    /// <param name="truncate">
    /// <inheritdoc cref="InColumns{T}(IEnumerable{ValueTuple{T, int}}, string, bool, string)" path="/param[@name='truncate']"/>
    /// </param>
    /// <param name="nullString"><inheritdoc cref="Constants.NullString" path="/summary"/></param>
    /// <returns>
    /// A string corresponding to the specified <paramref name="values"/> in order, in columns of
    /// their corresponding <paramref name="widths"/>.
    /// </returns>
    public static string InColumns<T>(this IEnumerable<T> values,
                                           IEnumerable<int> widths,
                                           string columnSeparator = Default.ColumnSeparator,
                                           bool truncate = false,
                                           string nullString = Constants.NullString)
        => InColumns(values.Zip(widths), columnSeparator, truncate, nullString);
    /// <summary>
    /// Prints the specified <paramref name="values"/> so that they are in columns of the specified
    /// <paramref name="width"/>.
    /// </summary>
    /// <typeparam name="T">
    /// <inheritdoc cref="InColumns{T}(IEnumerable{ValueTuple{T, int}}, string, string?, string)" path="/typeparam[@name='T']"/>
    /// </typeparam>
    /// <param name="values">
    /// <inheritdoc cref="InColumns{T}(IEnumerable{ValueTuple{T, int}}, string, string?, string)" path="/param[@name='values']"/>
    /// </param>
    /// <param name="width">The width of each column.</param>
    /// <param name="columnSeparator">
    /// <inheritdoc cref="InColumns{T}(IEnumerable{ValueTuple{T, int}}, string, string?, string)" path="/param[@name='columnSeparator']"/>
    /// </param>
    /// <param name="truncate">
    /// <inheritdoc cref="InColumns{T}(IEnumerable{ValueTuple{T, int}}, string, bool, string)" path="/param[@name='truncate']"/>
    /// </param>
    /// <param name="nullString"><inheritdoc cref="Constants.NullString" path="/summary"/></param>
    /// <returns>
    /// A string corresponding to the specified <paramref name="values"/> in order, in columns of
    /// size <paramref name="width"/>.
    /// </returns>
    public static string InColumns<T>(this IEnumerable<T> values,
                                           int width,
                                           string columnSeparator = Default.ColumnSeparator,
                                           bool truncate = false,
                                           string nullString = Constants.NullString)
        => values.InColumns(Enumerable.Repeat(width, values.Count()), columnSeparator, truncate, nullString);
}