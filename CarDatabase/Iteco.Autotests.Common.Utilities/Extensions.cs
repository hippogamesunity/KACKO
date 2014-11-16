using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Iteco.Autotests.Common.Utilities
{
    public static class Extensions
    {
        public static bool Contains(this string source, string target, StringComparison stringComparison)
        {
            return source.IndexOf(target, stringComparison) >= 0;
        }

        public static bool Contains(this Uri source, Uri target)
        {
            return source.ToString().Contains(target.ToString());
        }

        public static bool Contains(this Uri source, string target)
        {
            return source.ToString().Contains(target);
        }

        public static int ToInt(this string source)
        {
            return string.IsNullOrEmpty(source) ? 0 : int.Parse(source);
        }

        public static float ToFloat(this string source)
        {
            return string.IsNullOrEmpty(source) ? 0 : float.Parse(source.Replace(".", ","));
        }

        public static Int64 ToInt64(this string source)
        {
            return string.IsNullOrEmpty(source) ? 0 : Int64.Parse(source);
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            var provider = new RNGCryptoServiceProvider();
            var n = list.Count;

            while (n > 1)
            {
                var box = new byte[1];

                do provider.GetBytes(box);

                while (!(box[0] < n * (Byte.MaxValue / n)));

                var k = (box[0] % n);
                
                n--;

                var value = list[k];

                list[k] = list[n];
                list[n] = value;
            }
        }

        public static Func<TArgument, TResult> Memoize<TArgument, TResult>(this Func<TArgument, TResult> func)
        {
            var map = new Dictionary<TArgument, TResult>();

            return argument =>
            {
                TResult value;

                if (!map.TryGetValue(argument, out value))
                {
                    value = map[argument] = func(argument);
                }

                return value;
            };
        }

        public static bool Equivalent<T>(this List<T> x, List<T> y)
        {
            return !x.Except(y).Union(y.Except(x)).Any();
        }

        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value);
        }
    }
}
