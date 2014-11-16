using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;

namespace Iteco.Autotests.Common.Utilities
{
    public static class TableHelper
    {
        public static List<List<string>> ToList(string jQueryTableSelector)
        {
            Contract.Assert(!string.IsNullOrEmpty(jQueryTableSelector));

            Browser.WaitReadyState();
            Browser.WaitAjax();

            var rows = new List<List<string>>();

            int rowsCount;

            try
            {
                rowsCount = int.Parse(Browser.ExecuteJavaScript(string.Format("return $('{0} tr').length;", jQueryTableSelector)).ToString());
            }
            catch
            {
                Thread.Sleep(5000);

                rowsCount = int.Parse(Browser.ExecuteJavaScript(string.Format("return $('{0} tr').length;", jQueryTableSelector)).ToString());
            }

            for (var i = 0; i < rowsCount; i++)
            {
                var row = new List<string>();
                var cellsCount = int.Parse(Browser.ExecuteJavaScript(string.Format("return $('{0} tr:eq({1}) td').length;", jQueryTableSelector, i)).ToString());

                for (var j = 0; j < cellsCount; j++)
                {
                    var cellText = Browser.ExecuteJavaScript(string.Format("return $('{0} tr:eq({1}) td:eq({2})').text();", jQueryTableSelector, i, j)).ToString().Trim();

                    row.Add(cellText);
                }

                if (!row.All(string.IsNullOrEmpty))
                {
                    rows.Add(row);
                }
            }

            return rows;
        }
    }
}