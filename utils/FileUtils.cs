using System.IO;
using System.Security.Cryptography;

namespace d9.utl;
/// <summary>
/// Utilities for file, path, and directory manipulation.
/// </summary>
/// <remarks>Ensure that <see cref="AbsoluteOrInBaseFolder(string)"/> is deterministic (and therefore can be used for comparisons)</remarks>
public static class FileUtils
{
    /// <summary>
    /// If the specified <c><paramref name="path"/></c> is an absolute path, returns it unmodified; otherwise, creates an absolute path
    /// treating it as a subdirectory of <see cref="Config.BaseFolderPath"/>.
    /// </summary>
    /// <param name="path">The path to make into an absolute path.</param>
    /// <returns>A <see langword="string"/> containing an absolute path, as specified above.</returns>
    public static string AbsoluteOrInBaseFolder(this string path) => Path.IsPathFullyQualified(path) ? path : Path.Join(Config.BaseFolderPath, path);
    /// <summary>
    /// Copies a file from <c><paramref name="oldPath"/></c> to <c><paramref name="newPath"/></c>.
    /// </summary>
    /// <param name="oldPath">The path to the source file.</param>
    /// <param name="newPath">The path to the destination file.</param>
    /// <param name="overwrite">If <see langword="false"/>, an exception will be thrown by <see cref="File.Copy(string, string, bool)"/> if a file exists
    /// at the <c><paramref name="newPath"/></c>.</param>
    /// <exception cref="Exception">Thrown if the directory path is <see langword="null"/>.</exception>
    public static void CopyFileTo(this string oldPath, string newPath, bool overwrite = false)
    {
        string dirPath = Path.GetDirectoryName(newPath) ?? throw new Exception($"{newPath} has a null directory path.");
        if (!Directory.Exists(dirPath)) _ = Directory.CreateDirectory(dirPath);
        if (overwrite && File.Exists(newPath)) File.Delete(newPath);
        File.Copy(oldPath, newPath, overwrite);
    }
    /// <summary>
    /// Deletes any empty subfolders of the specified <c><paramref name="folder"/></c>, then, if the folder is empty, deletes it.
    /// </summary>
    /// <param name="folder">The folder to delete.</param>
    /// <param name="suppressWarnings">If <see langword="false"/>, a warning will be printed if this method attempts to delete a non-empty folder.</param>
    /// <exception cref="ArgumentException"></exception>
    public static void DeleteFolderRecursive(this string folder, bool suppressWarnings = true)
    {
        if (!Directory.Exists(folder))
            throw new ArgumentException($"Attempted to delete directory `{folder}`, but it either does not exist or is not a directory.");
        foreach (string path2 in Directory.EnumerateDirectories(folder)) DeleteFolderRecursive(path2);
        if (folder.FolderIsEmpty())
        {
            Directory.Delete(folder);
        }
        else
        {
            if(!suppressWarnings) Utils.Log($"Can't delete directory `{folder}` because it isn't empty.");
        }
    }
    /// <summary>
    /// Deletes any empty folders in the specified <c><paramref name="folder"/></c>.
    /// </summary>
    /// <param name="folder">The folder whose empty subfolders to delete.</param>
    /// <param name="pathsToIgnore">The absolute paths to folders whose empty subfolders should not be deleted.</param>
    /// <remarks>TODO: update to support relative paths in <c><paramref name="pathsToIgnore"/></c> as well.</remarks>
    public static void DeleteEmptyFolders(this string folder, params string[] pathsToIgnore)
    {
        foreach (string path in Directory.EnumerateDirectories(folder))
        {
            if (!pathsToIgnore.Any(folder.IsSubfolderOf))
                DeleteFolderRecursive(folder, true);
        }
    }
    /// <summary>
    /// Generates a unique hash of the given file for fast comparison purposes.
    /// </summary>
    /// <param name="stream">A stream of the file to hash.</param>
    /// <returns>A SHA512 hash of the file's bytes.</returns>
    /// <remarks>Based on <see href="https://stackoverflow.com/a/51966515">this StackOverflow answer</see>.</remarks>
    public static string FileHash(this Stream stream)
    {
        using SHA512 sha512 = SHA512.Create();
        return BitConverter.ToString(sha512.ComputeHash(stream)).Replace("-", "");
    }
    /// <summary>
    /// Generates a unique hash of the given file for fast comparison purposes.
    /// </summary>
    /// <param name="path">The path to the file to hash.</param>
    /// <returns>A SHA512 hash of the file's bytes.</returns>
    /// <remarks>Based on <see href="https://stackoverflow.com/a/51966515">this StackOverflow answer</see>.</remarks>
    public static string FileHash(this string path)
    {
        using FileStream fs = File.OpenRead(path);
        return fs.FileHash();
    }
    /// <summary>
    /// Whether the specified <c><paramref name="folder"/></c> is empty.
    /// </summary>
    /// <param name="folder">The folder whose emptiness to check.</param>
    /// <returns><see langword="true"/> if the <c><paramref name="folder"/></c> contains neither files nor directories, or <see langword="false"/> otherwise.</returns>
    public static bool FolderIsEmpty(this string folder) => !Directory.GetFiles(folder).Any() && !Directory.GetDirectories(folder).Any();
    /// <summary>
    /// Determines whether the specified <c><paramref name="path"/></c> is in the specified <c><paramref name="folder"/></c>, or a
    /// <see cref="IsSubfolderOf(string, string)">subfolder thereof</see>.
    /// </summary>
    /// <param name="path">The path whose membership in <c><paramref name="folder"/></c> to determine.</param>
    /// <param name="folder">The potential parent folder to the specified <c><paramref name="path"/></c>.</param>
    /// <returns><see langword="true"/> if the specified <c><paramref name="path"/></c> is in <c><paramref name="folder"/></c> as specified above,
    /// or <see langword="false"/> otherwise.</returns>
    /// <exception cref="Exception">Thrown if the specified <c><paramref name="path"/></c> is not a valid directory.</exception>
    public static bool IsInFolder(this string path, string folder)
    {
        string? directoryName = Path.GetDirectoryName(path) ?? throw new Exception($"{path} can't be in {folder} because it has no valid directory name!");
        return directoryName.IsSubfolderOf(folder);
    }
    /// <summary>
    /// Determines whether the specified <c><paramref name="folder"/></c> is or is a subfolder of the specified <c><paramref name="possibleParent"/></c>.
    /// </summary>
    /// <param name="folder">The path whose subfolder status to determine.</param>
    /// <param name="possibleParent">The potential parent of the folder.</param>
    /// <returns><see langword="true"/> if the specified <c><paramref name="folder"/></c> is identical to or a subfolder of <c><paramref name="possibleParent"/></c>,
    /// or <see langword="false"/> otherwise.
    /// </returns>
    /// <exception cref="Exception"></exception>
    public static bool IsSubfolderOf(this string folder, string possibleParent)
    {
        if (!Directory.Exists(folder) || !Directory.Exists(possibleParent))
            return false;
        string relPath = Path.GetRelativePath(possibleParent, folder);
        return relPath.Length < 2 || !(Path.GetRelativePath(possibleParent, folder)[0..2] == "..");
    }
    /// <summary>
    /// Moves a file from <paramref name="oldPath"/> to <paramref name="newPath"/>, creating any missing directories as appropriate.
    /// </summary>
    /// <param name="oldPath"><inheritdoc cref="CopyFileTo(string, string, bool)" path="/param[@name='oldPath']"/></param>
    /// <param name="newPath"><inheritdoc cref="CopyFileTo(string, string, bool)" path="/param[@name='newPath']"/></param>
    /// <param name="overwrite"><inheritdoc cref="CopyFileTo(string, string, bool)" path="/param[@name='overwrite']"/></param>
    public static void MoveFileTo(this string oldPath, string newPath, bool overwrite = false)
    {
        // todo: safety checks for Path.GetDirectoryName
        Directory.CreateDirectory($"{Path.GetDirectoryName(newPath)}");
        File.Move(oldPath, newPath, overwrite);
    }
    // https://stackoverflow.com/a/23182807
    /// <summary>
    /// Replaces any characters in <c><paramref name="s"/></c> which are not permitted in valid folder or file names with the specified 
    /// <c><paramref name="replacement"/></c>.
    /// </summary>
    /// <param name="s">The string to make into a safe folder or file name.</param>
    /// <param name="replacement">The string to replace invalid characters with.</param>
    /// <returns>A safe folder or file name, as described above.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static string PathSafe(this string s, string replacement = "_")
    {
        if (s is null) throw new ArgumentNullException(nameof(s));
        return string.Join(replacement, s.Split(Path.GetInvalidFileNameChars())).Trim();
    }
    public static IEnumerable<string> EnumerateFilesRecursive(this string folder)
    {
        foreach (string s in Directory.EnumerateFiles(folder))
            yield return s;
        foreach (string s in Directory.EnumerateDirectories(folder))
            foreach (string t in s.EnumerateFilesRecursive())
                yield return t;
    }
}
