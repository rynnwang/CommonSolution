using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Beyova
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
                if (SqlTransactionScope.SqlCommand != null)
                {
                    this.sqlCommand = SqlTransactionScope.SqlCommand;
                    this.sqlConnection = SqlTransactionScope.SqlCommand.Connection;
                }
                else
                {
                    this.sqlConnection = sqlConnection;
                    this.sqlCommand = sqlConnection.CreateCommand();
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(sqlConnection == null ? null : sqlConnection.ConnectionString);
            }
        }

        #region Transanction

        /// <summary>
        /// Begins the transaction.
        /// </summary>
        /// <param name="iso">The iso.</param>
        /// <param name="transactionName">Name of the transaction.</param>
        /// <returns>
        /// SqlTransactionScope.
        /// </returns>
        internal SqlTransactionScope BeginTransaction(IsolationLevel iso = IsolationLevel.Unspecified, string transactionName = null)
        {
            //Default value follows Microsoft: http://referencesource.microsoft.com/#System.Data/System/Data/SqlClient/SqlConnection.cs

            if (SqlTransactionScope.SqlTransaction == null)
            {
                var sqlTransaction = this.sqlConnection.BeginTransaction(iso, transactionName);
                return new SqlTransactionScope(sqlTransaction, this.sqlCommand);
            }
            else
            {
                return SqlTransactionScope.Current;
            }
        }

        /// <summary>
        /// Commits this instance.
        /// </summary>
        internal void Commit()
        {
            try
            {
                var sqlTransaction = SqlTransactionScope.SqlTransaction;
                sqlTransaction.CheckNullObject(nameof(sqlTransaction));
                sqlTransaction.Commit();
            }
            catch (Exception ex)
            {
                throw ex.Handle();
            }
        }

        /// <summary>
        /// Rollbacks this instance.
        /// </summary>
        internal void Rollback()
        {
            try
            {
                var sqlTransaction = SqlTransactionScope.SqlTransaction;
                sqlTransaction.CheckNullObject(nameof(sqlTransaction));
                sqlTransaction.Rollback();
            }
            catch (Exception ex)
            {
                throw ex.Handle();
            }
        }

        #endregion Transanction

        /// <summary>
        /// Gets the output parameter.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>SqlParameter.</returns>
        public SqlParameter GetOutputParameter(string name)
        {
            try
            {
                name.CheckEmptyString(nameof(name));

                return sqlCommand.Parameters[name];
            }
            catch (Exception ex)
            {
                throw ex.Handle(name);
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
                procedureName.CheckEmptyString(nameof(procedureName));

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
                throw ex.Handle(new { procedureName, sqlParameters });
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
                procedureName.CheckEmptyString(nameof(procedureName));

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
                throw ex.Handle(new { procedureName, sqlParameters });
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
            try
            {
                procedureName.CheckEmptyString(nameof(procedureName));

                sqlCommand.CommandText = procedureName;
                sqlCommand.Parameters.Clear();
                sqlCommand.CommandTimeout = timeoutMillSeconds;
                if (sqlParameters != null)
                    sqlCommand.Parameters.AddRange(sqlParameters.ToArray());
                if (sqlConnection.State != ConnectionState.Open)
                {
                    sqlConnection.Open();
                }
                return sqlCommand.ExecuteNonQuery();
            }
            catch (SqlException sqlEx)
            {
                throw sqlEx;
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { procedureName, sqlParameters });
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        /// <summary>
        /// Executes the SQL text.
        /// </summary>
        /// <param name="sqlText">The SQL text.</param>
        /// <returns>System.Int32.</returns>
        /// <exception cref="System.Exception"></exception>
        internal int ExecuteSqlTextNonQuery(string sqlText)
        {
            try
            {
                sqlText.CheckEmptyString(nameof(sqlText));
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText = sqlText;
                sqlCommand.Parameters.Clear();
                sqlCommand.CommandTimeout = timeoutMillSeconds;

                if (sqlConnection.State != ConnectionState.Open)
                {
                    sqlConnection.Open();
                }
                return sqlCommand.ExecuteNonQuery();
            }
            catch (SqlException sqlEx)
            {
                throw sqlEx;
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { sqlText });
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        /// <summary>
        /// Executes the SQL text scalar.
        /// </summary>
        /// <param name="sqlText">The SQL text.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        internal object ExecuteSqlTextScalar(string sqlText)
        {
            try
            {
                sqlText.CheckEmptyString(nameof(sqlText));
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText = sqlText;
                sqlCommand.Parameters.Clear();
                sqlCommand.CommandTimeout = timeoutMillSeconds;

                if (sqlConnection.State != ConnectionState.Open)
                {
                    sqlConnection.Open();
                }
                return sqlCommand.ExecuteScalar();
            }
            catch (SqlException sqlEx)
            {
                throw sqlEx;
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { sqlText });
            }
            finally
            {
                sqlConnection.Close();
            }
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
            dataTableContainer.CheckNullObject(nameof(dataTableContainer));

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
            dataSetContainer.CheckNullObject(nameof(dataSetContainer));

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

        #endregion Execute procedures

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
                throw ex.Handle(sqlConnection);
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