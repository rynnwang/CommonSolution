using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ifunction
{
    /// <summary>
    /// Class for database operator.
    /// </summary>
    public sealed class DatabaseOperator : IDisposable
    {
        /// <summary>
        /// The timeout mill seconds
        /// </summary>
        private const int timeoutMillSeconds = 30000;

        /// <summary>
        /// The SQL connection
        /// </summary>
        private readonly SqlConnection sqlConnection;

        /// <summary>
        /// The SQL command
        /// </summary>
        private readonly SqlCommand sqlCommand;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="databaseConnectionString">The database connection string.</param>
        public DatabaseOperator(string databaseConnectionString)
            : this(new SqlConnection(databaseConnectionString))
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="sqlConnection">The database connection string.</param>
        public DatabaseOperator(SqlConnection sqlConnection)
        {
            try
            {
                this.sqlConnection = sqlConnection;
                this.sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandType = CommandType.StoredProcedure;
            }
            catch (Exception ex)
            {
                throw ex.Handle("DatabaseOperator", sqlConnection == null ? null : sqlConnection.ConnectionString);
            }
        }

        #region Execute procedures

        /// <summary>
        /// Execute reader.
        /// </summary>
        /// <param name="procedureName">The procedure name.</param>
        /// <param name="sqlParameters">The sql parameters.</param>
        /// <returns>The SqlDataReader contains data.</returns>
        public SqlDataReader ExecuteReader(string procedureName, List<SqlParameter> sqlParameters = null)
        {
            try
            {
                procedureName.CheckEmptyString("procedureName");

                sqlCommand.CommandText = procedureName;
                sqlCommand.Parameters.Clear();
                sqlCommand.CommandTimeout = timeoutMillSeconds;
                if (sqlParameters != null)
                    sqlCommand.Parameters.AddRange(sqlParameters.ToArray());
                if (sqlConnection.State != ConnectionState.Open)
                {
                    sqlConnection.Open();
                }
                return sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (SqlException sqlEx)
            {
                throw sqlEx;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Fail to execute procedure [{0}]", procedureName), ex);
            }
        }

        /// <summary>
        /// Execute scalar
        /// </summary>
        /// <param name="procedureName">The procedure name.</param>
        /// <param name="sqlParameters">The sql parameters.</param>
        /// <returns>The first column in first row of result data.</returns>
        public object ExecuteScalar(string procedureName, List<SqlParameter> sqlParameters = null)
        {
            object result = null;

            try
            {
                procedureName.CheckEmptyString("procedureName");

                sqlCommand.CommandText = procedureName;
                sqlCommand.Parameters.Clear();
                sqlCommand.CommandTimeout = timeoutMillSeconds;
                if (sqlParameters != null)
                    sqlCommand.Parameters.AddRange(sqlParameters.ToArray());
                if (sqlConnection.State != ConnectionState.Open)
                {
                    sqlConnection.Open();
                }
                result = sqlCommand.ExecuteScalar();
            }
            catch (SqlException sqlEx)
            {
                throw sqlEx;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Fail to execute procedure [{0}]", procedureName), ex);
            }
            finally
            {
                sqlConnection.Close();
            }

            return result;
        }

        /// <summary>
        /// Execute none query.
        /// </summary>
        /// <param name="procedureName">The procedure name.</param>
        /// <param name="sqlParameters">The sql parameters.</param>
        /// <returns>The count of row influenced.</returns>
        public int ExecuteNonQuery(string procedureName, List<SqlParameter> sqlParameters = null)
        {
            int count = 0;

            try
            {
                procedureName.CheckEmptyString("procedureName");

                sqlCommand.CommandText = procedureName;
                sqlCommand.Parameters.Clear();
                sqlCommand.CommandTimeout = timeoutMillSeconds;
                if (sqlParameters != null)
                    sqlCommand.Parameters.AddRange(sqlParameters.ToArray());
                if (sqlConnection.State != ConnectionState.Open)
                {
                    sqlConnection.Open();
                }
                count = sqlCommand.ExecuteNonQuery();
            }
            catch (SqlException sqlEx)
            {
                throw sqlEx;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Fail to execute procedure [{0}]", procedureName), ex);
            }
            finally
            {
                sqlConnection.Close();
            }

            return count;
        }

        /// <summary>
        /// Executes the data table.
        /// </summary>
        /// <param name="procedureName">Name of the procedure.</param>
        /// <param name="sqlParameters">The SQL parameters.</param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string procedureName, List<SqlParameter> sqlParameters = null)
        {
            DataTable table = new DataTable();
            ExecuteDataTable(procedureName, table, sqlParameters);
            return table;
        }

        /// <summary>
        /// Executes the data table.
        /// </summary>
        /// <param name="procedureName">Name of the procedure.</param>
        /// <param name="dataTableContainer">The data table container.</param>
        /// <param name="sqlParameters">The SQL parameters.</param>
        /// <exception cref="System.NullReferenceException">dataTableContainer</exception>
        public void ExecuteDataTable(string procedureName, DataTable dataTableContainer, List<SqlParameter> sqlParameters = null)
        {
            if (dataTableContainer == null)
            {
                throw new NullReferenceException("dataTableContainer");
            }

            using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
            {
                sqlCommand.CommandText = procedureName;
                sqlCommand.Parameters.Clear();
                sqlCommand.CommandTimeout = timeoutMillSeconds;
                if (sqlParameters != null)
                {
                    sqlCommand.Parameters.AddRange(sqlParameters.ToArray());
                }

                if (sqlConnection.State != ConnectionState.Open)
                {
                    sqlConnection.Open();
                }
                sqlDataAdapter.Fill(dataTableContainer);
            }
        }

        /// <summary>
        /// Executes the data set.
        /// </summary>
        /// <param name="procedureName">Name of the procedure.</param>
        /// <param name="sqlParameters">The SQL parameters.</param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string procedureName, List<SqlParameter> sqlParameters = null)
        {
            var dataSet = new DataSet();
            ExecuteDataSet(procedureName, dataSet, sqlParameters);
            return dataSet;
        }

        /// <summary>
        /// Executes the data set.
        /// </summary>
        /// <param name="procedureName">Name of the procedure.</param>
        /// <param name="dataSetContainer">The data set container.</param>
        /// <param name="sqlParameters">The SQL parameters.</param>
        /// <exception cref="System.NullReferenceException">dataSetContainer</exception>
        public void ExecuteDataSet(string procedureName, DataSet dataSetContainer, List<SqlParameter> sqlParameters = null)
        {
            if (dataSetContainer == null)
            {
                throw new NullReferenceException("dataSetContainer");
            }

            using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
            {
                sqlCommand.CommandText = procedureName;
                sqlCommand.Parameters.Clear();
                sqlCommand.CommandTimeout = timeoutMillSeconds;
                if (sqlParameters != null)
                {
                    sqlCommand.Parameters.AddRange(sqlParameters.ToArray());
                }

                if (sqlConnection.State != ConnectionState.Open)
                {
                    sqlConnection.Open();
                }
                sqlDataAdapter.Fill(dataSetContainer);
            }
        }

        #endregion

        /// <summary>
        /// Abouts the SQL server.
        /// </summary>
        /// <param name="sqlConnection">The SQL connection.</param>
        /// <returns>System.String.</returns>
        public static string AboutSqlServer(string sqlConnection)
        {
            if (string.IsNullOrWhiteSpace(sqlConnection))
            {
                return string.Empty;
            }

            SqlConnection tempSqlConnection = null;

            try
            {
                tempSqlConnection = new SqlConnection(sqlConnection);

                var tempSqlCommand = tempSqlConnection.CreateCommand();
                tempSqlCommand.Connection = tempSqlConnection;
                tempSqlCommand.CommandType = System.Data.CommandType.Text;
                tempSqlCommand.CommandText = "SELECT @@VERSION";
                tempSqlCommand.CommandTimeout = timeoutMillSeconds;

                if (tempSqlConnection.State != ConnectionState.Open)
                {
                    tempSqlConnection.Open();
                }

                return tempSqlCommand.ExecuteScalar().ObjectToString();
            }
            catch (Exception ex)
            {
                throw ex.Handle("AboutSQLServer");
            }
            finally
            {
                if (tempSqlConnection != null)
                {
                    tempSqlConnection.Close();
                }
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Close();
            sqlConnection.Dispose();
        }

        /// <summary>
        /// Closes this instance.
        /// </summary>
        public void Close()
        {
            if (sqlConnection != null)
            {
                try
                {
                    sqlConnection.Close();
                }
                catch
                {
                    // ignored
                }
            }
        }
    }
}
