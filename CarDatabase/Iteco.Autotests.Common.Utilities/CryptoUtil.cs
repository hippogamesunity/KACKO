using System.Security.Cryptography;
using System.Text;

namespace Iteco.Autotests.Common.Utilities
{
    public static class CryptoUtil
    {
        public static string Md5(string input)
        {
            var md5 = MD5.Create();
            var inputBytes = Encoding.ASCII.GetBytes(input);
            var hash = md5.ComputeHash(inputBytes);
            var stringBuilder = new StringBuilder();

            for (var i = 0; i < hash.Length; i++)
            {
                stringBuilder.Append(hash[i].ToString("X2"));
            }

            return stringBuilder.ToString();
        }
    }
}
