using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;

namespace Iteco.Autotests.Common.Utilities
{
    public static class DirectoryExtension
    {
        public static void Copy(string sourceFolder, string targetFolder)
        {
            Contract.Requires(!string.IsNullOrEmpty(sourceFolder));
            Contract.Requires(!string.IsNullOrEmpty(targetFolder));

            var source = new DirectoryInfo(sourceFolder);
            var target = new DirectoryInfo(targetFolder);

            if (Directory.Exists(target.FullName) == false)
            {
                Directory.CreateDirectory(target.FullName);
            }

            foreach (var file in source.GetFiles())
            {
                file.CopyTo(Path.Combine(target.ToString(), file.Name), true);
            }

            foreach (var dir in source.GetDirectories())
            {
                var nextTargetSubDir = target.CreateSubdirectory(dir.Name);
                Copy(dir.FullName, nextTargetSubDir.FullName);
            }
        }

        public static void ForceDelete(string path)
        {
            Contract.Requires(!string.IsNullOrEmpty(path));

            if (!Directory.Exists(path))
                return;

            var baseFolder = new DirectoryInfo(path);
            
            foreach (var item in baseFolder.EnumerateDirectories("*", SearchOption.AllDirectories))
                item.Attributes = ResetAttributes(item.Attributes);
            
            foreach (var item in baseFolder.EnumerateFiles("*", SearchOption.AllDirectories))
                item.Attributes = ResetAttributes(item.Attributes);

            baseFolder.Delete(true);
        }

        public static bool TryClean(string path)
        {
            var result = Executor.Try(() => ForceDelete(path));

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return result;
        }

        public static bool TryClean(string path, DateTime dateTime)
        {
            var result = true;

            if (Directory.Exists(path))
            {
                foreach (var f in Directory.GetFiles(path, "*", SearchOption.AllDirectories).Where(file => File.GetLastWriteTime(file) < dateTime))
                {
                    var filePath = f;

                    result &= Executor.Try(() => File.Delete(filePath));
                }

                foreach (var d in Directory.GetDirectories(path).Where(dir => Directory.GetLastAccessTime(dir) < dateTime))
                {
                    var directoryPath = d;

                    result &= Executor.Try(() => Directory.Delete(directoryPath, recursive: true));
                }
            }
            else
            {
                Directory.CreateDirectory(path);
            }

            return result;
        }

        #region Helpers

        private static FileAttributes ResetAttributes(FileAttributes attributes)
        {
            return attributes & ~(FileAttributes.Archive | FileAttributes.ReadOnly | FileAttributes.Hidden);
        }

        #endregion
    }
}
