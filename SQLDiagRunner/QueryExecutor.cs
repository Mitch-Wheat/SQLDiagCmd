
using System.Data.SqlClient;
using System.Data;


namespace SQLDiagRunner
{
    public static class QueryExecutor
    {
        private const int DefaultTimeout30Seconds = 30;

        public static DataTable Execute(string connectionString, string query)
        {
            return Execute(connectionString, query, DefaultTimeout30Seconds);
        }

        public static DataTable Execute(string connectionString, string query, int timeoutSeconds)
        {
            var dataTable = new DataTable();

            using (var cn = new SqlConnection(connectionString))
            {
                cn.Open();

                using (SqlCommand cmd = cn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;
                    cmd.CommandTimeout = timeoutSeconds;

                    SqlDataReader sqlDataReader = cmd.ExecuteReader();

                    dataTable.Load(sqlDataReader);
                }

                cn.Close();
            }

            return dataTable;
        }
    }
}
