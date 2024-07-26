namespace d9.utl;
public static partial class CommandLineArgs
{
    /// <summary>
    /// Predefined <see cref="Parser{T}">parsers</see> for command-line args.
    /// </summary>
    public static partial class Parsers
    {
        /// <summary>
        /// Selects the first <see langword="string"/> among the values whose length is greater than 0.
        /// </summary>
        /// <remarks>
        /// Ignores the <c>flag</c> argument. Previously <c>NonNullOrEmpty</c>, but i realized that
        /// parsers will never get passed null values.
        /// </remarks>
        public static Parser<string?> FirstNonEmptyString => (values, _) =>
        {
            try
            {
                return values?.SkipWhile(x => string.IsNullOrEmpty(x)).First();
            }
            catch
            {
                return null;
            }
        };
        /// <summary>
        /// Returns the potentially <see langword="null"/><see
        /// cref="IEnumerable{T}">IEnumerable</see>&lt; <see langword="string"/>&gt; corresponding
        /// to the actual values passed when specifying the given variable.
        /// </summary>
        /// <remarks>Ignores the <c>flag</c> argument.</remarks>
        public static Parser<IEnumerable<string>?> Raw => (values, _) => values;
        /// <summary>
        /// Returns <inheritdoc cref="GetFlag(string, char?)" path="/returns"/>
        /// </summary>
        public static Parser<bool> Flag => (enumerable, flag) => enumerable is not null || flag;
    }
}