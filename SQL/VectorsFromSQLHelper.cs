using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ProjektWektor3D.SQL
{
    internal class VectorsFromSQLHelper
    {
        public bool CheckIfTableExists(MSSQLHelper sqlHelper, string tableName)
        {
            var insertParameters = new SqlParameter[1];
            insertParameters[0] = new SqlParameter("@TableName", tableName);

            var result = sqlHelper.ExecuteQuery(
                "SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @TableName;",
                insertParameters);

            return result != null && result.Rows.Count == 1;
        }

        public bool CreateSQLTable(MSSQLHelper sqlHelper, string tableName)
        {
            var builder = new SqlCommandBuilder();
            var escapedTableName = builder.QuoteIdentifier(tableName);

            var result = sqlHelper.ExecuteQuery(
                $"CREATE TABLE {escapedTableName} (Name varchar(255) NOT NULL, X float(53) NOT NULL, Y float(53) NOT NULL, Z float(53) NOT NULL)");

            return result != null;
        }


        public Dictionary<string, Vector3D> ImportFromSQL(MSSQLHelper sqlHelper, string tableName)
        {
            var vectors = new Dictionary<string, Vector3D>();

            if (!sqlHelper.IsConnected())
                throw new Exception("SQL Connection is closed!");

            var builder = new SqlCommandBuilder();
            var escapedTableName = builder.QuoteIdentifier(tableName);

            var result = sqlHelper.ExecuteQuery($@"SELECT Name, X, Y, Z FROM {escapedTableName}");
            if (result != null)
            {
                if (result.Columns.Count != 4)
                    throw new Exception("Invalid column count");

                foreach (DataRow row in result.Rows)
                {
                    var name = row[0];
                    var x = row[1];
                    var y = row[2];
                    var z = row[3];

                    if (name is string && x is double && y is double && z is double)
                        vectors[Convert.ToString(name)] = new Vector3D(Convert.ToDouble(x), Convert.ToDouble(y),
                            Convert.ToDouble(z));
                }
            }

            return vectors;
        }

        public void ExportToSQL(MSSQLHelper sqlHelper, string tableName, Dictionary<string, Vector3D> vectors)
        {
            if (!sqlHelper.IsConnected())
                throw new Exception("SQL Connection is closed!");

            var builder = new SqlCommandBuilder();
            var escapedTableName = builder.QuoteIdentifier(tableName);

            if (sqlHelper.ExecuteQuery($@"TRUNCATE TABLE {escapedTableName}") == null)
                throw new Exception("Failed to truncate table!");

            foreach (var vector in vectors)
            {
                var insertParameters = new SqlParameter[4];
                insertParameters[0] = new SqlParameter("@VectorName", vector.Key);
                insertParameters[1] = new SqlParameter("@VectorX", vector.Value.X);
                insertParameters[2] = new SqlParameter("@VectorY", vector.Value.Y);
                insertParameters[3] = new SqlParameter("@VectorZ", vector.Value.Z);

                if (sqlHelper.ExecuteQuery(
                        $@"INSERT INTO {escapedTableName}(Name, X, Y, Z) VALUES (@VectorName, @VectorX, @VectorY, @VectorZ);",
                        insertParameters) == null)
                    break;
            }
        }
    }
}