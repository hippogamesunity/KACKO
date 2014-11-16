using System.Diagnostics.Contracts;
using MySql.Data.MySqlClient;

namespace Iteco.Autotests.Common.Utilities
{
    public class MySqlHelper
    {
        private readonly string _connectionString;

        public MySqlHelper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void BackupDatabase(string path)
        {
            var mySqlBackup = new MySqlBackup(_connectionString)
            {
                ExportInfo =
                {
                    FileName = path
                }
            };

            mySqlBackup.Export();
        }

        public void RestoreDatabase(string path)
        {
            var mySqlBackup = new MySqlBackup(_connectionString)
            {
                ImportInfo =
                {
                    FileName = path
                }
            };

            mySqlBackup.Import();
        }

        public int ExecuteQuery(string query)
        {
            var mySqlConnection = new MySqlConnection(_connectionString);

            mySqlConnection.Open();

            var mySqlCommand = new MySqlCommand(query, mySqlConnection);
            var rowsAffected = mySqlCommand.ExecuteNonQuery();

            mySqlConnection.Close();

            return rowsAffected;
        }
    }
}