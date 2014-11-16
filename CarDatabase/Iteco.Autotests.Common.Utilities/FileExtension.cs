using System.Diagnostics.Contracts;
using System.IO;

namespace Iteco.Autotests.Common.Utilities
{
    public static class FileExtension
    {
        public static string GetUniquePath(string filePath)
        {
            Contract.Requires(!string.IsNullOrEmpty(filePath));
            Contract.Ensures(!File.Exists(Contract.Result<string>()));

            if (!File.Exists(filePath)) return filePath;

            var directoryName = Path.GetDirectoryName(filePath);

            Contract.Assume(Directory.Exists(directoryName));

            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
            var fileExtention = Path.GetExtension(filePath);
            var i = 1;
            var newFileName = string.Format("{0} ({1}){2}", fileNameWithoutExtension, i, fileExtention);
            var newFilePath = Path.Combine(directoryName, newFileName);

            while (File.Exists(newFilePath))
            {
                newFileName = string.Format("{0} ({1}){2}", fileNameWithoutExtension, i++, fileExtention);
                newFilePath = Path.Combine(directoryName, newFileName);
            }

            return newFilePath;
        }
    }
}
