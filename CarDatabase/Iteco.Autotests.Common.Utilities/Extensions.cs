using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web.UI;
using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

        public static string FormatWith(this string format, object source)
        {
            Contract.Requires(!string.IsNullOrEmpty(format));

            var regex = new Regex(@"(?<start>\{)+(?<property>[\w\.\[\]]+)(?<format>:[^}]+)?(?<end>\})+",
                RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
            var values = new List<object>();
            var rewrittenFormat = regex.Replace(format, delegate(Match m)
                {
                    var startGroup = m.Groups["start"];
                    var propertyGroup = m.Groups["property"];
                    var formatGroup = m.Groups["format"];
                    var endGroup = m.Groups["end"];

                    values.Add((propertyGroup.Value == "0") ? source : DataBinder.Eval(source, propertyGroup.Value));

                    return new string('{', startGroup.Captures.Count) + (values.Count - 1) + formatGroup.Value + new string('}', endGroup.Captures.Count);
                });

            return string.Format(rewrittenFormat, values.ToArray());
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

        public static string GetClassName(this TestContext testContext)
        {
            return testContext.FullyQualifiedTestClassName.Split(Convert.ToChar(".")).Last();
        }

        public static bool Alone<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            return source.Count(predicate) == 1;
        }

        public static bool Exists(this UITestControl control, int timeoutSeconds)
        {
            var savedTimeout = Playback.PlaybackSettings.SearchTimeout;

            Playback.PlaybackSettings.SearchTimeout = 1000;

            var result = Executor.SpinWait(() => control.Exists, TimeSpan.FromSeconds(timeoutSeconds));

            Playback.PlaybackSettings.SearchTimeout = savedTimeout;

            return result;
        }

        public static void Click(this UITestControl control)
        {
            Mouse.Click(control);
        }

        public static string GetOuterHtml(this UITestControl control)
        {
            return control.GetProperty("OuterHtml").ToString();
        }

        public static void WaitAjax(this BrowserWindow browserWindow, int timeoutSeconds = 60)
        {
            Contract.Assume(browserWindow != null);

            const string javascript = "return (typeof($) === 'undefined') ? true : !$.active;";
            var ready = new Func<bool>(() => (bool)browserWindow.ExecuteScript(javascript));

            Contract.Assert(Executor.SpinWait(ready, TimeSpan.FromSeconds(timeoutSeconds), TimeSpan.FromMilliseconds(100)));
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
