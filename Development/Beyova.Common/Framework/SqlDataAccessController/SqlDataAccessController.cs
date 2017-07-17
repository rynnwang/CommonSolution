using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Xml.Linq;
using Beyova.ExceptionSystem;
using Newtonsoft.Json.Linq;

namespace Beyova
{
    /// <summary>
    /// Abstract class for SQL data access controller, which refers an entity in {T} type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SqlDataAccessController<T> : SqlDataAccessController
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlDataAccessController{T}" /> class.
        /// </summary>
        /// <param name="primarySqlConnectionString">The primary SQL connection string.</param>
        /// <param name="readOnlySqlConnectionString">The read only SQL connection string.</param>
        protected SqlDataAccessController(string primarySqlConnectionString, string readOnlySqlConnectionString = null)
            : base(primarySqlConnectionString, readOnlySqlConnectionString)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlDataAccessController{T}" /> class.
        /// </summary>
        /// <param name="primarySqlConnection">The SQL connection.</param>
        /// <param name="readOnlySqlConnection">The read only SQL connection.</param>
        protected SqlDataAccessController(SqlConnection primarySqlConnection, SqlConnection readOnlySqlConnection = null)
            : base(primarySqlConnection, readOnlySqlConnection)
        {
        }

        #endregion Constructor

        /// <summary>
        /// Converts the object.
        /// </summary>
        /// <typeparam name="TOutput">The type of the t output.</typeparam>
        /// <param name="sqlDataReader">The SQL data reader.</param>
        /// <param name="converter">The converter.</param>
        /// <returns>List{`0}.</returns>
        protected List<TOutput> ConvertObject<TOutput>(SqlDataReader sqlDataReader, Func<SqlDataReader, TOutput> converter)
        {
            try
            {
                sqlDataReader.CheckNullObject(nameof(sqlDataReader));
                converter.CheckNullObject(nameof(converter));

                var result = new List<TOutput>();
                // When enter this method, Read() has been called for detect exception already.
                result.Add(converter(sqlDataReader));

                while (sqlDataReader.Read())
                {
                    result.Add(converter(sqlDataReader));
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { type = typeof(T).GetFullName() });
            }
            finally
            {
                if (sqlDataReader != null)
                {
                    sqlDataReader.Close();
                }

                if (_primaryDatabaseOperator != null)
                {
                    _primaryDatabaseOperator.Close();
                }
            }
        }

        /// <summary>
        /// Converts the entity object.
        /// </summary>
        /// <param name="sqlDataReader">The SQL data reader.</param>
        /// <returns>Object instance in type {`0}.</returns>
        protected abstract T ConvertEntityObject(SqlDataReader sqlDataReader);

        /// <summary>
        /// Executes the reader.
        /// </summary>
        /// <typeparam name="TOutput">The type of the t output.</typeparam>
        /// <param name="spName">Name of the sp.</param>
        /// <param name="sqlParameters">The SQL parameters.</param>
        /// <param name="converter">The converter.</param>
        /// <param name="preferReadOnlyOperator">The prefer read only operator.</param>
        /// <returns>List&lt;TOutput&gt;.</returns>
        protected List<TOutput> ExecuteReader<TOutput>(string spName, List<SqlParameter> sqlParameters, Func<SqlDataReader, TOutput> converter, bool preferReadOnlyOperator = false)
        {
            SqlDataReader reader = null;
            DatabaseOperator databaseOperator = null;

            try
            {
                converter.CheckNullObject(nameof(converter));

                reader = this.Execute(spName, sqlParameters, preferReadOnlyOperator, out databaseOperator);
                return reader == null ? new List<TOutput>() : ConvertObject(reader, converter);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { SpName = spName, Parameters = SqlParameterToList(sqlParameters), PreferReadOnlyOperator = preferReadOnlyOperator });
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

                // use Close instead of Dispose so that operator can be reuse without re-initialize.
                databaseOperator?.Close();
            }
        }

        /// <summary>
        /// Executes the reader. If parameter <c>converter</c> is specified, use it. Otherwise, use <c>ConvertEntityObject</c>.
        /// </summary>
        /// <param name="spName">Name of the sp.</param>
        /// <param name="sqlParameters">The SQL parameters.</param>
        /// <param name="preferReadOnlyOperator">The prefer read only operator.</param>
        /// <returns>List{`0}.</returns>
        protected List<T> ExecuteReader(string spName, List<SqlParameter> sqlParameters = null, bool preferReadOnlyOperator = false)
        {
            return ExecuteReader<T>(spName, sqlParameters, ConvertEntityObject, preferReadOnlyOperator);
        }
    }

    /// <summary>
    /// Class SqlDataAccessController.
    /// </summary>
    public abstract class SqlDataAccessController : IDisposable
    {
        /// <summary>
        /// The column_ SQL error code
        /// </summary>
        protected const string column_SqlErrorCode = "SqlErrorCode";

        /// <summary>
        /// The column_ SQL error reason
        /// </summary>
        protected const string column_SqlErrorReason = "SqlErrorReason";

        /// <summary>
        /// The column_ SQL error message
        /// </summary>
        protected const string column_SqlErrorMessage = "SqlErrorMessage";

        /// <summary>
        /// The column_ SQL stored procedure name
        /// </summary>
        protected const string column_SqlStoredProcedureName = "SqlStoredProcedureName";

        /// <summary>
        /// The primary database operator
        /// </summary>
        protected DatabaseOperator _primaryDatabaseOperator = null;

        /// <summary>
        /// The read only database operator
        /// </summary>
        protected DatabaseOperator _readOnlyDatabaseOperator = null;

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlDataAccessController" /> class.
        /// </summary>
        /// <param name="primarySqlConnectionString">The primary SQL connection string.</param>
        /// <param name="readOnlySqlConnectionString">The read only SQL connection string.</param>
        protected SqlDataAccessController(string primarySqlConnectionString, string readOnlySqlConnectionString = null)
            : base()
        {
            this._primaryDatabaseOperator = new DatabaseOperator(primarySqlConnectionString);
            this._readOnlyDatabaseOperator = string.IsNullOrWhiteSpace(readOnlySqlConnectionString) ? null : new DatabaseOperator(readOnlySqlConnectionString);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlDataAccessController{T}" /> class.
        /// </summary>
        /// <param name="primarySqlConnection">The SQL connection.</param>
        /// <param name="readOnlySqlConnection">The read only SQL connection.</param>
        protected SqlDataAccessController(SqlConnection primarySqlConnection, SqlConnection readOnlySqlConnection = null)
            : base()
        {
            this._primaryDatabaseOperator = new DatabaseOperator(primarySqlConnection);
            this._readOnlyDatabaseOperator = readOnlySqlConnection == null ? null : new DatabaseOperator(readOnlySqlConnection);
        }

        #endregion Constructor

        #region Transanction

        /// <summary>
        /// Begins the transaction.
        /// </summary>
        /// <param name="iso">The iso.</param>
        /// <param name="transactionName">Name of the transaction.</param>
        /// <returns>SqlTransactionScope.</returns>
        internal SqlTransactionScope BeginTransaction(IsolationLevel iso = IsolationLevel.Unspecified, string transactionName = null)
        {
            return this._primaryDatabaseOperator.BeginTransaction(iso, transactionName);
        }

        #endregion Transanction

        /// <summary>
        /// Gets the database operator.
        /// </summary>
        /// <param name="preferReadOnlyOperator">if set to <c>true</c> [prefer read only operator].</param>
        /// <returns>DatabaseOperator.</returns>
        protected DatabaseOperator GetDatabaseOperator(bool preferReadOnlyOperator)
        {
            return (preferReadOnlyOperator && _readOnlyDatabaseOperator != null) ? _readOnlyDatabaseOperator : _primaryDatabaseOperator;
        }

        /// <summary>
        /// Executes the scalar.
        /// </summary>
        /// <param name="spName">Name of the sp.</param>
        /// <param name="sqlParameters">The SQL parameters.</param>
        /// <param name="preferReadOnlyOperator">The prefer read only operator.</param>
        /// <returns>System.Object.</returns>
        protected object ExecuteScalar(string spName, List<SqlParameter> sqlParameters = null, bool preferReadOnlyOperator = false)
        {
            SqlDataReader reader = null;
            DatabaseOperator databaseOperator = null;

            try
            {
                reader = this.Execute(spName, sqlParameters, preferReadOnlyOperator, out databaseOperator);
                return reader == null ? DBNull.Value : reader[0];
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { SpName = spName, Parameters = SqlParameterToList(sqlParameters), PreferReadOnlyOperator = preferReadOnlyOperator });
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

                // use Close instead of Dispose so that operator can be reuse without re-initialize.
                databaseOperator?.Close();
            }
        }

        /// <summary>
        /// Executes the non query.
        /// </summary>
        /// <param name="spName">Name of the sp.</param>
        /// <param name="sqlParameters">The SQL parameters.</param>
        /// <param name="preferReadOnlyOperator">The prefer read only operator.</param>
        /// <returns>System.Int32.</returns>
        protected void ExecuteNonQuery(string spName, List<SqlParameter> sqlParameters = null, bool preferReadOnlyOperator = false)
        {
            DatabaseOperator databaseOperator = null;
            SqlDataReader reader = null;

            try
            {
                reader = this.Execute(spName, sqlParameters, preferReadOnlyOperator, out databaseOperator);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { SpName = spName, Parameters = SqlParameterToList(sqlParameters), PreferReadOnlyOperator = preferReadOnlyOperator });
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

                // use Close instead of Dispose so that operator can be reuse without re-initialize.
                databaseOperator?.Close();
            }
        }

        /// <summary>
        /// Executes the specified sp name.
        /// </summary>
        /// <param name="spName">Name of the sp.</param>
        /// <param name="sqlParameters">The SQL parameters.</param>
        /// <param name="preferReadOnlyOperator">if set to <c>true</c> [prefer read only operator].</param>
        /// <param name="databaseOperator">The database operator.</param>
        /// <returns>SqlDataReader.</returns>
        protected SqlDataReader Execute(string spName, List<SqlParameter> sqlParameters, bool preferReadOnlyOperator, out DatabaseOperator databaseOperator)
        {
            try
            {
                databaseOperator = GetDatabaseOperator(preferReadOnlyOperator);
                databaseOperator.CheckNullObject(nameof(databaseOperator));

                var reader = databaseOperator.ExecuteReader(spName, sqlParameters);
                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        var exception = TryGetSqlException(reader);

                        if (exception != null)
                        {
                            throw exception;
                        }
                    }

                    return reader;
                }
                else
                {
                    return null;
                }
            }
            catch (SqlStoredProcedureException sqlEx)
            {
                throw sqlEx;
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { SpName = spName, Parameters = SqlParameterToList(sqlParameters) });
            }
        }

        /// <summary>
        /// Abouts the SQL server.
        /// </summary>
        /// <param name="sqlConnection">The SQL connection.</param>
        /// <returns>System.String.</returns>
        public static string AboutSqlServer(string sqlConnection)
        {
            return DatabaseOperator.AboutSqlServer(sqlConnection);
        }

        /// <summary>
        /// SQLs the parameter to list.
        /// </summary>
        /// <param name="sqlParameters">The SQL parameters.</param>
        /// <returns>List{System.String}.</returns>
        protected static List<string> SqlParameterToList(List<SqlParameter> sqlParameters)
        {
            List<string> result = new List<string>();

            if (sqlParameters != null)
            {
                result.AddRange(sqlParameters.Select(one => string.Format("Name: [{0}], Type: [{1}], Value: [{2}]\n\r", one.ParameterName, one.TypeName, one.Value)));
            }

            return result;
        }

        /// <summary>
        /// Tries the get SQL exception.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>SqlStoredProcedureException.</returns>
        protected static SqlStoredProcedureException TryGetSqlException(SqlDataReader reader)
        {
            try
            {
                reader.CheckNullObject(nameof(reader));

                var storedProcedureName = reader.HasColumn(column_SqlStoredProcedureName) ? reader[column_SqlStoredProcedureName].ObjectToString() : null;
                var errorCode = reader.HasColumn(column_SqlErrorCode) ? reader[column_SqlErrorCode].ObjectToNullableInt32() : null;
                var errorReason = reader.HasColumn(column_SqlErrorReason) ? reader[column_SqlErrorReason].ObjectToString() : null;
                var errorMessage = reader.HasColumn(column_SqlErrorMessage) ? reader[column_SqlErrorMessage].ObjectToString() : null;

                if (!string.IsNullOrWhiteSpace(storedProcedureName) && errorCode != null)
                {
                    return new SqlStoredProcedureException(storedProcedureName, errorMessage, errorCode.Value, errorReason);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw ex.Handle();
            }
        }

        #region GenerateSqlSpParameter

        /// <summary>
        /// Generates the name of the SQL sp parameter.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="parameterObject">The parameter object.</param>
        /// <param name="direction">The direction.</param>
        /// <returns>SqlParameter.</returns>
        protected SqlParameter GenerateSqlSpParameter(string columnName, object parameterObject, ParameterDirection direction = ParameterDirection.Input)
        {
            if (parameterObject != null)
            {
                if (parameterObject is Enum)
                {
                    parameterObject = (int)parameterObject;
                }
                else if (parameterObject is JToken || parameterObject is XElement)
                {
                    parameterObject = parameterObject.ToString();
                }
                else
                {
                    var boolParameterObject = parameterObject as bool?;
                    if (boolParameterObject.HasValue)
                    {
                        parameterObject = boolParameterObject.Value ? 1 : 0;
                    }
                }
            }

            return InternalGenerateSqlSpParameter(columnName, parameterObject ?? Convert.DBNull, direction);
        }

        /// <summary>
        /// Internals the generate SQL sp parameter.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="parameterObject">The parameter object.</param>
        /// <param name="direction">The direction.</param>
        /// <returns>SqlParameter.</returns>
        protected internal SqlParameter InternalGenerateSqlSpParameter(string columnName, object parameterObject, ParameterDirection direction = ParameterDirection.Input)
        {
            return new SqlParameter("@" + columnName.Trim(), parameterObject ?? Convert.DBNull) { Direction = direction };
        }

        #endregion GenerateSqlSpParameter

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this._primaryDatabaseOperator?.Dispose();
            this._readOnlyDatabaseOperator?.Dispose();
        }
    }
}