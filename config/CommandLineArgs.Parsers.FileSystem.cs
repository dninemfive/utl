namespace d9.utl;
public static partial class CommandLineArgs
{
    /// <summary>
    /// Predefined <see cref="Parser{T}">parsers</see> for command-line args.
    /// </summary>
    public static partial class Parsers
    {
        /// <summary>
        /// <para>
        /// Checks that the <see cref="FirstNonEmptyString">first non-null-or-empty <see
        /// langword="string"/></see> is a path to a folder.
        /// </para>
        /// <list type="bullet">
        /// <item>If the folder exists, returns the path.</item>
        /// <item>
        /// If the folder does not exist but the path is valid, creates the folder and returns the path.
        /// </item>
        /// <item>Otherwise, returns <see langword="null"/>.</item>
        /// </list>
        /// <para>
        /// If the result is not <see langword="null"/>, it is guaranteed to be a path pointing to a
        /// folder which exists.
        /// </para>
        /// </summary>
        public static Parser<string?> FolderPath => delegate (IEnumerable<string>? enumerable, bool _)
        {
            string? possiblePath = FirstNonEmptyString(enumerable, false);
            if (possiblePath is null)
                return null;
            string path;
            try
            {
                path = Path.GetFullPath(possiblePath);
                // creating the directory throws an error if a file exists there
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
            }
            catch (Exception e)
            {
                LogExtensions.DebugLog(e);
                return null;
            }
            return path;
        };
        /// <summary>
        /// <para>
        /// Checks that the <see cref="FirstNonEmptyString">first non-null-or-empty <see
        /// langword="string"/></see> is a path to an existing file. If the file exists, returns the
        /// path; returns <see langword="null"/> otherwise.
        /// </para>
        /// <para>
        /// If the result is not <see langword="null"/>, it is guaranteed to be a path pointing to a
        /// folder which exists.
        /// </para>
        /// </summary>
        public static Parser<string?> FilePath => delegate (IEnumerable<string>? enumerable, bool _)
        {
            string? possiblePath = FirstNonEmptyString(enumerable, false);
            if (possiblePath is null)
                return null;
            string path;
            try
            {
                path = Path.GetFullPath(possiblePath);
            }
            catch (Exception e)
            {
                LogExtensions.DebugLog(e);
                return null;
            }
            if (!File.Exists(path))
                return null;
            return path;
        };
    }
}