using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ProjektWektor3D.SQL
{
    internal class MSSQLHelper
    {
        public MSSQLHelper(string dataSource = "(local)")
        {
            DataSource = dataSource;
        }

        public string DataSource { get; set; }
        public string Database { get; set; }
        public SqlConnection Connection { get; set; }

        public void Close()
        {
            if (Connection != null)
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();

                Connection.Dispose();
                Connection = null;
            }
        }

        public bool IsConnected()
        {
            return Connection != null && Connection.State == ConnectionState.Open;
        }

        public void UseDatabase(string databaseName)
        {
            Database = databaseName;
        }

        public void Connect(string login, string password)
        {
            Close();

            var connectionString = $"Data Source={DataSource};Database={Database};User Id={login};Password={password}";

            Connection = new SqlConnection(connectionString);
            Connection.Open();
        }

        public DataTable ExecuteQuery(string query, SqlParameter[] parameters = null)
        {
            var dt = new DataTable();

            try
            {
                using (var cmd = new SqlCommand(query, Connection))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddRange(parameters);
                    }

                    using (var reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($@"Error while executing query!: {e}");
                return null;
            }

            return dt;
        }
    }
}