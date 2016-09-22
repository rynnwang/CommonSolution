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
        /// <param name="sqlConnectionString">The SQL connection string.</param>
        protected SqlDataAccessController(string sqlConnectionString)
            : base(sqlConnectionString)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlDataAccessController{T}" /> class.
        /// </summary>
        /// <param name="sqlConnection">The SQL connection.</param>
        protected SqlDataAccessController(SqlConnection sqlConnection)
            : base(sqlConnection)
        {
        }

        #endregion

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

                if (databaseOperator != null)
                {
                    databaseOperator.Close();
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
        /// <returns>List&lt;TOutput&gt;.</returns>
        protected List<TOutput> ExecuteReader<TOutput>(string spName, List<SqlParameter> sqlParameters, Func<SqlDataReader, TOutput> converter)
        {
            SqlDataReader reader = null;

            try
            {
                converter.CheckNullObject(nameof(converter));

                reader = this.Execute(spName, sqlParameters);
                return reader == null ? new List<TOutput>() : ConvertObject(reader, converter);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { SpName = spName, Parameters = SqlParameterToList(sqlParameters) });
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

                // use Close instead of Dispose so that operator can be reuse without re-initialize.
                databaseOperator.Close();
            }
        }

        /// <summary>
        /// Executes the reader. If parameter <c>converter</c> is specified, use it. Otherwise, use <c>ConvertEntityObject</c>.
        /// </summary>
        /// <param name="spName">Name of the sp.</param>
        /// <param name="sqlParameters">The SQL parameters.</param>
        /// <returns>List{`0}.</returns>
        protected List<T> ExecuteReader(string spName, List<SqlParameter> sqlParameters = null)
        {
            return ExecuteReader<T>(spName, sqlParameters, ConvertEntityObject);
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
        /// The database operator
        /// </summary>
        protected DatabaseOperator databaseOperator = null;

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlDataAccessController{T}" /> class.
        /// </summary>
        /// <param name="sqlConnectionString">The SQL connection string.</param>
        protected SqlDataAccessController(string sqlConnectionString)
            : base()
        {
            this.databaseOperator = new DatabaseOperator(sqlConnectionString);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlDataAccessController{T}" /> class.
        /// </summary>
        /// <param name="sqlConnection">The SQL connection.</param>
        protected SqlDataAccessController(SqlConnection sqlConnection)
            : base()
        {
            this.databaseOperator = new DatabaseOperator(sqlConnection);
        }

        #endregion

        #region Transanction

        /// <summary>
        /// Begins the transaction.
        /// </summary>
        /// <param name="iso">The iso.</param>
        /// <param name="transactionName">Name of the transaction.</param>
        /// <returns>SqlTransactionScope.</returns>
        internal SqlTransactionScope BeginTransaction(IsolationLevel iso = IsolationLevel.Unspecified, string transactionName = null)
        {
            return this.databaseOperator.BeginTransaction(iso, transactionName);
        }

        #endregion

        /// <summary>
        /// Executes the scalar.
        /// </summary>
        /// <param name="spName">Name of the sp.</param>
        /// <param name="sqlParameters">The SQL parameters.</param>
        /// <returns>System.Object.</returns>
        protected object ExecuteScalar(string spName, List<SqlParameter> sqlParameters = null)
        {
            SqlDataReader reader = null;

            try
            {
                reader = this.Execute(spName, sqlParameters);
                return reader == null ? DBNull.Value : reader[0];
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { SpName = spName, Parameters = SqlParameterToList(sqlParameters) });
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

                // use Close instead of Dispose so that operator can be reuse without re-initialize.
                databaseOperator.Close();
            }
        }

        /// <summary>
        /// Executes the non query.
        /// </summary>
        /// <param name="spName">Name of the sp.</param>
        /// <param name="sqlParameters">The SQL parameters.</param>
        /// <returns>System.Int32.</returns>
        protected void ExecuteNonQuery(string spName, List<SqlParameter> sqlParameters = null)
        {
            SqlDataReader reader = null;

            try
            {
                reader = this.Execute(spName, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { SpName = spName, Parameters = SqlParameterToList(sqlParameters) });
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

                // use Close instead of Dispose so that operator can be reuse without re-initialize.
                databaseOperator.Close();
            }
        }

        /// <summary>
        /// Executes the specified sp name.
        /// </summary>
        /// <param name="spName">Name of the sp.</param>
        /// <param name="sqlParameters">The SQL parameters.</param>
        /// <returns>SqlDataReader.</returns>
        protected SqlDataReader Execute(string spName, List<SqlParameter> sqlParameters = null)
        {
            try
            {
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
                reader.CheckNullObject("reader");
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

        #region InitializeCustomizedSqlErrorStoredProcedure

        /// <summary>
        /// Initializes the customized SQL error stored procedure.
        /// </summary>
        /// <param name="sqlConnection">The SQL connection.</param>
        public static void InitializeCustomizedSqlErrorStoredProcedure(string sqlConnection)
        {
            if (string.IsNullOrWhiteSpace(sqlConnection))
            {
                return;
            }

            try
            {
                using (var databaseOperator = new DatabaseOperator(sqlConnection))
                {
                    InitializeCustomizedSqlErrorStoredProcedure(databaseOperator);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(sqlConnection);
            }
        }

        /// <summary>
        /// Initializes the customized SQL error stored procedure.
        /// </summary>
        /// <param name="databaseOperator">The database operator.</param>
        protected static void InitializeCustomizedSqlErrorStoredProcedure(DatabaseOperator databaseOperator)
        {
            const string storedProcedureDrop = @"
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_ThrowException]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_ThrowException];
";
            const string storedProcedureCreate = @"
CREATE PROCEDURE [dbo].[sp_ThrowException](
    @Name [NVARCHAR](256),
    @Code INT,
    @Reason [NVARCHAR](256),
    @Message [NVARCHAR](512)
)
AS
BEGIN
    SELECT 
        @Name AS [SqlStoredProcedureName],
        ISNULL(@Code, 500) AS [SqlErrorCode],
        @Reason AS [SqlErrorReason],
        @Message AS [SqlErrorMessage];
END

";
            databaseOperator.ExecuteSqlTextNonQuery(storedProcedureDrop);
            databaseOperator.ExecuteSqlTextNonQuery(storedProcedureCreate);
        }

        #endregion

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
                    if (boolParameterObject != null)
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

        #endregion

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (this.databaseOperator != null)
            {
                this.databaseOperator.Dispose();
            }
        }
    }
}
