using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace d9.utl
{
    public static class IoUtils
    {
        public static void CopyFileTo(this string oldPath, string newPath, bool overwrite = false)
        {
            string dirPath = Path.GetDirectoryName(newPath) ?? throw new Exception($"{newPath} has a null directory path.");
            if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);
            if (overwrite && File.Exists(newPath)) File.Delete(newPath);
            File.Copy(oldPath, newPath);
        }
        public static void DeleteFolderRecursive(string path)
        {
            if (!Directory.Exists(path))
                throw new ArgumentException($"Attempted to delete directory `{path}`, but it either does not exist or is not a directory.");
            foreach (string path2 in Directory.EnumerateDirectories(path)) DeleteFolderRecursive(path2);
            if (path.FolderIsEmpty())
            {
                Directory.Delete(path);
            }
            else
            {
                Console.WriteLine($"Can't delete directory `{path}` because it isn't empty.");
            }
        }
        public static void DeleteEmptyFoldersIn(string basePath, params string[] pathsToIgnore)
        {
            foreach (string path in Directory.EnumerateDirectories(basePath)) if (!pathsToIgnore.Contains(path)) DeleteFolderRecursive(path);
        }
        public static bool FolderIsEmpty(this string dirPath) => !Directory.GetFiles(dirPath).Any() && !Directory.GetDirectories(dirPath).Any();
        public static void MoveFileTo(this string oldPath, string newPath, bool overwrite = false)
        {
            oldPath.CopyFileTo(newPath, overwrite);
            File.Delete(oldPath);
        }
    }
}
