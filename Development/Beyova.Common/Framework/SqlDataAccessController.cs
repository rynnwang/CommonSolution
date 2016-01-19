using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Xml.Linq;
using Beyova.AOP;

namespace Beyova
{
    /// <summary>
    /// Abstract class for SQL data access controller, which refers an entity in {T} type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SqlDataAccessController<T> : IDisposable
    {
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

        /// <summary>
        /// Converts the object.
        /// </summary>
        /// <param name="sqlDataReader">The SQL data reader.</param>
        /// <returns>List{`0}.</returns>
        public List<T> ConvertObject(SqlDataReader sqlDataReader)
        {
            try
            {
                sqlDataReader.CheckNullObject("sqlDataReader");

                var result = new List<T>();

                while (sqlDataReader.Read())
                {
                    result.Add(ConvertEntityObject(sqlDataReader));
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex.Handle("ConvertObject: " + typeof(T).ToString());
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
        /// <param name="spName">Name of the sp.</param>
        /// <param name="sqlParameters">The SQL parameters.</param>
        /// <returns>List{`0}.</returns>
        protected List<T> ExecuteReader(string spName, List<SqlParameter> sqlParameters)
        {
            SqlDataReader reader = null;

            try
            {
                reader = databaseOperator.ExecuteReader(spName, sqlParameters);
                return ConvertObject(reader);
            }
            catch (Exception ex)
            {
                throw ex.Handle("ExecuteReader", new { SpName = spName, Parameters = SqlParameterToList(sqlParameters) });
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
        /// Executes the scalar.
        /// </summary>
        /// <param name="spName">Name of the sp.</param>
        /// <param name="sqlParameters">The SQL parameters.</param>
        /// <returns>System.Object.</returns>
        protected object ExecuteScalar(string spName, List<SqlParameter> sqlParameters)
        {
            try
            {
                return databaseOperator.ExecuteScalar(spName, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex.Handle("ExecuteScalar", new { SpName = spName, Parameters = SqlParameterToList(sqlParameters) });
            }
            finally
            {
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
        protected int ExecuteNonQuery(string spName, List<SqlParameter> sqlParameters)
        {
            try
            {
                return databaseOperator.ExecuteNonQuery(spName, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex.Handle("ExecuteNonQuery", new { SpName = spName, Parameters = SqlParameterToList(sqlParameters) });
            }
            finally
            {
                // use Close instead of Dispose so that operator can be reuse without re-initialize.
                databaseOperator.Close();
            }
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
                result.AddRange(sqlParameters.Select(one => string.Format("Name: [{0}], Type: [{1}], Value: [{2}]\n\r", one.ParameterName, one.TypeName, one.Value.ToString())));
            }

            return result;
        }

        /// <summary>
        /// Generates the name of the SQL sp parameter.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="parameterObject">The parameter object.</param>
        /// <param name="direction">The direction.</param>
        /// <returns>SqlParameter.</returns>
        protected SqlParameter GenerateSqlSpParameter(string columnName, object parameterObject, ParameterDirection direction = ParameterDirection.Input)
        {
            return new SqlParameter("@" + columnName.Trim(), parameterObject ?? Convert.DBNull) { Direction = direction };
        }

        /// <summary>
        /// Generates the name of the SQL sp parameter.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="parameterObject">The parameter object.</param>
        /// <param name="direction">The direction.</param>
        /// <returns>SqlParameter.</returns>
        protected SqlParameter GenerateSqlSpParameter(string columnName, bool parameterObject, ParameterDirection direction = ParameterDirection.Input)
        {
            return GenerateSqlSpParameter(columnName, parameterObject ? 1 : 0, direction);
        }

        /// <summary>
        /// Generates the SQL sp parameter.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="parameterObject">The parameter object.</param>
        /// <param name="direction">The direction.</param>
        /// <returns>SqlParameter.</returns>
        protected SqlParameter GenerateSqlSpParameter(string columnName, XElement parameterObject, ParameterDirection direction = ParameterDirection.Input)
        {
            return GenerateSqlSpParameter(columnName, parameterObject == null ? Convert.DBNull : parameterObject.ToString(), direction);
        }

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

        /// <summary>
        /// Abouts the SQL server.
        /// </summary>
        /// <param name="sqlConnection">The SQL connection.</param>
        /// <returns>System.String.</returns>
        public static string AboutSqlServer(string sqlConnection)
        {
            return DatabaseOperator.AboutSqlServer(sqlConnection);
        }
    }
}
