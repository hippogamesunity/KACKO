using System.IO;
using DocumentFormat.OpenXml.Packaging;

namespace Iteco.Autotests.Common.Utilities
{
    public static class OfficeHelper
    {
        public static string ExtractDocxText(string path)
        {
            string documentText;

            using (var wordDoc = WordprocessingDocument.Open(path, true))
            {
                using (var reader = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                {
                    documentText = reader.ReadToEnd();
                }
            }
            
            return documentText;
        }
    }
}