using System;
using System.Net;

namespace Iteco.Autotests.Common.Utilities
{
    public static class Constants
    {
        public static readonly Uri TfsCollectionUri = new Uri("http://saturn:8080/tfs/VegaCollection");
        public static readonly NetworkCredential TfsAdminCredentials = new NetworkCredential("TfsAdmin", "qwerty1234$", "orion");
        public const string InternetExplorerProcess = "iexplore";
        public const string ProviderInvariantName = "System.Data.OleDb";
        public const string DataSourceConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\\TestData.xls;Extended Properties='Excel 12.0 Xml;HDR=YES';";
    }
}