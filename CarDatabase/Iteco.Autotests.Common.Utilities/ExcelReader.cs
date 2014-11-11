using System.Data;
using System.Data.OleDb;
using System.Diagnostics.Contracts;
using System.IO;

namespace Iteco.Autotests.Common.Utilities
{
    public static class ExcelReader
    {
        public static DataSet ReadDataSet(string path)
        {
            Contract.Requires(!string.IsNullOrEmpty(path));
            Contract.Requires(File.Exists(path));

            var connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Excel 12.0", path);
            
            using (var connection = new OleDbConnection(connectionString))
            {
                var query = string.Format("SELECT * FROM [{0}]", GetFirstSheetName(connection));
                var command = new OleDbCommand(query, connection);
                var dataSet = new DataSet();
                var adapter = new OleDbDataAdapter(command);

                adapter.Fill(dataSet);

                return dataSet;
            }
        }

        public static DataTable ReadTestData(string sheetName)
        {
            using (var oleDbConnection = new OleDbConnection(Constants.DataSourceConnectionString))
            {
                oleDbConnection.Open();

                var command = new OleDbDataAdapter(string.Format("select * from [{0}$]", sheetName), oleDbConnection);
                var dataSet = new DataSet();

                command.Fill(dataSet);

                var table = dataSet.Tables[0];

                oleDbConnection.Close();

                return table;
            }
        }

        private static string GetFirstSheetName(OleDbConnection connection)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            var schema = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });

            Contract.Assert(schema != null);

            return schema.Rows[0]["TABLE_NAME"].ToString();
        }
    }
}
