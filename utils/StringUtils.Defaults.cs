namespace d9.utl;
public static partial class StringUtils
{
    /// <summary>
    /// Holds the default arguments for various StringUtils extension methods.
    /// </summary>
    public static class Default
    {
        /// <summary>
        /// The default string which indicates that a string was <see cref="Truncate(string, int, string)">truncated</see>.
        /// </summary>
        public const string TruncationSuffix = "…";
        /// <summary>
        /// The default string which separates objects printed <see
        /// cref="InColumns{T}(IEnumerable{ValueTuple{T, int}}, string, string?, string)">in columns</see>.
        /// </summary>
        public const string ColumnSeparator = " ";
    }
}