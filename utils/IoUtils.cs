using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace d9.utl
{
    /// <summary>
    /// Utilities for file, path, and directory manipulation.
    /// </summary>
    public static class IoUtils
    {
        public static string AbsolutePath(this string path) => Path.IsPathFullyQualified(path) ? Path.Join(Config.BaseFolderPath, path) : path;
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
            if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);
            if (overwrite && File.Exists(newPath)) File.Delete(newPath);
            File.Copy(oldPath, newPath, overwrite);
        }
        /// <summary>
        /// Deletes any empty subfolders of the specified <c><paramref name="folder"/></c>, then, if the folder is empty, deletes it.
        /// </summary>
        /// <param name="folder">The folder to delete.</param>
        /// <exception cref="ArgumentException"></exception>
        public static void DeleteFolderRecursive(this string folder)
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
                Console.WriteLine($"Can't delete directory `{folder}` because it isn't empty.");
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
            foreach (string path in Directory.EnumerateDirectories(folder)) if (!pathsToIgnore.Contains(path)) DeleteFolderRecursive(path);
        }
        /// <summary>
        /// Whether the specified <c><paramref name="folder"/></c> is empty.
        /// </summary>
        /// <param name="folder">The folder whose emptiness to check.</param>
        /// <returns><see langword="true"/> if the <c><paramref name="folder"/></c> contains neither files nor directories, or <see langword="false"/> otherwise.</returns>
        public static bool FolderIsEmpty(this string folder) => !Directory.GetFiles(folder).Any() && !Directory.GetDirectories(folder).Any();
        /// <summary>
        /// <see cref="CopyFileTo(string, string, bool)">Copies</see> a file from <c><paramref name="oldPath"/></c> to 
        /// <c><paramref name="newPath"/></c>, then deletes the original, as an atomic operation.
        /// </summary>
        /// <param name="oldPath"><inheritdoc cref="CopyFileTo(string, string, bool)" path="/param[@name='oldPath']"/></param>
        /// <param name="newPath"><inheritdoc cref="CopyFileTo(string, string, bool)" path="/param[@name='newPath']"/></param>
        /// <param name="overwrite"><inheritdoc cref="CopyFileTo(string, string, bool)" path="/param[@name='overwrite']"/></param>
        public static void MoveFileTo(this string oldPath, string newPath, bool overwrite = false)
        {
            oldPath.CopyFileTo(newPath, overwrite);
            File.Delete(oldPath);
        }
    }
}
