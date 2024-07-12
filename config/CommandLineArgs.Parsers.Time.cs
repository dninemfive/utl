namespace d9.utl;
public static partial class CommandLineArgs
{
    /// <summary>
    /// Predefined <see cref="Parser{T}">parsers</see> for command-line args.
    /// </summary>
    public static partial class Parsers
    {
        /// <summary>
        /// Parses the <see cref="FirstNonNullOrEmptyString">first non-null-or-empty <see
        /// langword="string"/></see> to a <see cref="DateTime"/> using the type's <see
        /// cref="DateTime.TryParse(string?, out System.DateTime)">default parser</see>.
        /// </summary>
        /// <remarks>Ignores the <c>flag</c> argument.</remarks>
        [Obsolete("Use CommandLineArgs.Parsers.Struct<T> instead")]
        public static Parser<DateTime?> DateTime
            => (values, _) => System.DateTime.TryParse(FirstNonNullOrEmptyString(values, false), out DateTime result) ? result : null;
        /// <summary>
        /// Parses the <see cref="FirstNonNullOrEmptyString">first non-null-or-empty <see
        /// langword="string"/></see> to a <see cref="TimeSpan"/> using the type's <see
        /// cref="TimeSpan.TryParse(string?, out System.TimeSpan)">default parser</see>.
        /// </summary>
        /// <remarks>Ignores the <c>flag</c> argument.</remarks>
        [Obsolete("Use CommandLineArgs.Parsers.Struct<T> instead")]
        public static Parser<TimeSpan?> TimeSpan
            => (values, _) => System.TimeSpan.TryParse(FirstNonNullOrEmptyString(values, false), out TimeSpan result) ? result : null;
        /// <summary>
        /// Parses the <see cref="FirstNonNullOrEmptyString">first non-null-or-empty <see
        /// langword="string"/></see> to a <see langword="double"/>, then converts that into a <see
        /// cref="TimeSpan"/> using the specified delegate.
        /// </summary>
        /// <remarks>
        /// Intended for use with <see cref="TimeSpan"/>'s methods which parse from <see
        /// langword="double"/> s, such as <see cref="TimeSpan.FromMinutes(double)"/>. <br/> Ignores
        /// the <c>flag</c> argument.
        /// </remarks>
        /// <param name="parser">The parser to use.</param>
        /// <returns>A <see cref="TimeSpan"/> as produced by the <c><paramref name="parser"/></c>.</returns>
        [Obsolete("Use a custom CommandLineArgs.Parser instead")]
        public static Parser<TimeSpan?> UsingParser(Func<double, TimeSpan> parser)
            => delegate (IEnumerable<string>? values, bool _)
            {
                double? d = Double(values, false);
                if (d is not null)
                    return parser(d.Value);
                return null;
            };
    }
}