using System.Globalization;

namespace d9.utl;
public static partial class CommandLineArgs
{
    /// <summary>
    /// Predefined <see cref="Parser{T}">parsers</see> for command-line args.
    /// </summary>
    public static partial class Parsers
    {
        /// <summary>
        /// Parses a reference type which implements <see cref="IParsable{TSelf}"/> from the first
        /// parsable string in the given values.
        /// </summary>
        /// <typeparam name="T">The type to return.</typeparam>
        /// <param name="formatProvider">
        /// An <see cref="IFormatProvider"/> which is passed to <see
        /// cref="IParsable{TSelf}.TryParse(string?, IFormatProvider?, out TSelf)"/> in order to try
        /// to parse each string.
        /// </param>
        /// <returns>
        /// An instance of <typeparamref name="T"/>, if any of the <c>values</c> can be successfully
        /// parsed, or <see langword="null"/> otherwise.
        /// </returns>
        public static Parser<T?> Class<T>(IFormatProvider? formatProvider = null)
            where T : class, IParsable<T>
            => (values, _) =>
            {
                if (values is null)
                    return null;
                foreach (string s in values)
                    if (T.TryParse(s, formatProvider ?? CultureInfo.InvariantCulture, out T? result))
                        return result;
                return null;
            };
        /// <summary>
        /// Parses a value type which implements <see cref="IParsable{TSelf}"/> from the first
        /// parsable string in the given values.
        /// </summary>
        /// <typeparam name="T">The type to return.</typeparam>
        /// <param name="formatProvider">
        /// An <see cref="IFormatProvider"/> which is passed to <see
        /// cref="IParsable{TSelf}.TryParse(string?, IFormatProvider?, out TSelf)"/> in order to try
        /// to parse each string.
        /// </param>
        /// <returns>
        /// An instance of <typeparamref name="T"/>, if any of the <c>values</c> can be successfully
        /// parsed, or <see langword="null"/> otherwise.
        /// </returns>
        public static Parser<T?> Struct<T>(IFormatProvider? formatProvider = null)
            where T : struct, IParsable<T>
            => (values, _) =>
            {
                if (values is null)
                    return null;
                foreach (string s in values)
                    if (T.TryParse(s, formatProvider ?? CultureInfo.InvariantCulture, out T result))
                        return result;
                return null;
            };
    }
}