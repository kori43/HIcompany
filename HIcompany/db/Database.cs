using System.Data;
using System.Data.SqlClient;

namespace HIcompany.db
{
    public class Database
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=;Initial Catalog=HIcompany;Integrated Security=True");
        public void OpenConnection()
        {
            if (sqlConnection.State == ConnectionState.Closed)
            {
                sqlConnection.Open();
            }
        }
        public void CloseConnection()
        {
            if (sqlConnection.State == ConnectionState.Open)
            {
                sqlConnection.Close();
            }
        }
        public SqlConnection GetConnection()
        {
            return sqlConnection;
        }
    }
}
