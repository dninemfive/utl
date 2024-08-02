namespace d9.utl;
public partial class CommandLineArgs
{
    /// <summary>
    /// Predefined <see cref="Parser{T}">parsers</see> for command-line args.
    /// </summary>
    public partial class Parsers
    {
        public Parser<string> FolderPaths => delegate (IntermediateArgs args, string name)
        {
            foreach(string path in args[name])
            {
                if (Directory.Exists(path))
                    yield return path;
                try
                {
                    
                } 
                catch(Exception e)
                {

                }
            }
            string? possiblePath = FirstNonEmptyString(enumerable, false);
            if (possiblePath is null)
                yield return null;
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
                DebugUtils.IfDebug(Console.WriteLine, e);
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
                DebugUtils.IfDebug(Console.WriteLine, e.Summary());
                return null;
            }
            if (!File.Exists(path))
                return null;
            return path;
        };
    }
}