using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Beyova
{
    /// <summary>
    /// Class SmartSqlDataAccessController.
    /// </summary>
    /// <typeparam name="TEntity">The type of the t entity.</typeparam>
    /// <typeparam name="TCriteria">The type of the t criteria.</typeparam>
    public abstract class SmartSqlDataAccessController<TEntity, TCriteria> : SmartSqlDataAccessController<TEntity>
        where TEntity : class, new()
        where TCriteria : class
    {
        /// <summary>
        /// The _criteria entity converter
        /// </summary>
        protected SqlEntityConverter _criteriaEntityConverter;

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SmartSqlDataAccessController{TEntity, TCriteria}" /> class.
        /// </summary>
        /// <param name="sqlConnectionString">The SQL connection string.</param>
        /// <param name="fieldConverters">The field converters.</param>
        protected SmartSqlDataAccessController(string sqlConnectionString, params SqlFieldConverter[] fieldConverters)
                : base(sqlConnectionString, fieldConverters)
        {
            _criteriaEntityConverter = TryInitialize(typeof(TCriteria), fieldConverters);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SmartSqlDataAccessController{TEntity, TCriteria}" /> class.
        /// </summary>
        /// <param name="sqlConnection">The SQL connection.</param>
        /// <param name="fieldConverters">The field converters.</param>
        protected SmartSqlDataAccessController(SqlConnection sqlConnection, params SqlFieldConverter[] fieldConverters)
             : base(sqlConnection, fieldConverters)
        {
            _criteriaEntityConverter = TryInitialize(typeof(TCriteria), fieldConverters);
        }

        #endregion

        /// <summary>
        /// Generates the criteria SQL parameters.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="ignoredProperty">The ignored property.</param>
        /// <returns>List&lt;SqlParameter&gt;.</returns>
        protected List<SqlParameter> GenerateCriteriaSqlParameters(TCriteria criteria, params string[] ignoredProperty)
        {
            try
            {
                criteria.CheckNullObject("criteria");

                return GenerateSqlParameters<TCriteria>(criteria, ignoredProperty);
            }
            catch (Exception ex)
            {
                throw ex.Handle( new { entity = typeof(TCriteria).GetFullName(), ignoredProperty });
            }
        }

        /// <summary>
        /// Queries the specified criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="spName">Name of the sp.</param>
        /// <param name="ignoredProperty">The ignored property.</param>
        /// <returns>List&lt;TEntity&gt;.</returns>
        protected List<TEntity> Query(TCriteria criteria, string spName = null, params string[] ignoredProperty)
        {
            try
            {
                criteria.CheckNullObject("criteria");

                var parameters = GenerateCriteriaSqlParameters(criteria, ignoredProperty);

                return this.ExecuteReader(spName.SafeToString(string.Format("sp_Query{0}", typeof(TEntity).Name)), parameters);
            }
            catch (Exception ex)
            {
                throw ex.Handle( new { criteria, spName });
            }
        }
    }

    /// <summary>
    /// Class SmartSqlDataAccessController.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SmartSqlDataAccessController<T> : SqlDataAccessController
        where T : class, new()
    {
        /// <summary>
        /// The column_ operator key
        /// </summary>
        protected const string column_OperatorKey = "OperatorKey";

        #region  SqlFieldConverter

        /// <summary>
        /// The field converters
        /// </summary>
        protected static Dictionary<string, SqlEntityConverter> cacheFieldConverters = new Dictionary<string, SqlEntityConverter>();

        /// <summary>
        /// The locker
        /// </summary>
        protected static object locker = new object();

        /// <summary>
        /// Tries the initialize.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="fieldConverters">The field converters.</param>
        /// <returns>Beyova.SqlEntityConverter.</returns>
        protected static SqlEntityConverter TryInitialize(Type type, params SqlFieldConverter[] fieldConverters)
        {
            var entityFullName = type.GetFullName();

            if (!cacheFieldConverters.ContainsKey(entityFullName))
            {
                lock (locker)
                {
                    if (!cacheFieldConverters.ContainsKey(entityFullName))
                    {
                        List<SqlFieldConverter> tmpFieldConverters = new List<SqlFieldConverter>();

                        foreach (var one in type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
                        {
                            var matchedConveter = fieldConverters.Where((x) => { return x.PropertyName == one.Name; }).Select((x) => x).FirstOrDefault();

                            if (matchedConveter?.IsIgnored ?? false)
                            {
                                tmpFieldConverters.Add(matchedConveter ?? new SqlFieldConverter(one));
                            }
                        }
                        var converter = new SqlEntityConverter(type, tmpFieldConverters);
                        cacheFieldConverters.Add(entityFullName, converter);
                        return converter;
                    }
                }
            }

            return cacheFieldConverters[entityFullName];
        }

        #endregion

        /// <summary>
        /// The ignored property_ simple base object
        /// </summary>
        protected static readonly string[] ignoredProperty_SimpleBaseObject =
                            (from item in typeof(ISimpleBaseObject).GetProperties() where item.Name != "Key" select item.Name).ToArray();

        /// <summary>
        /// The ignored property_ base object
        /// </summary>
        protected static string[] ignoredProperty_BaseObject =
                            (from item in typeof(IBaseObject).GetProperties() where item.Name != "Key" select item.Name).ToArray();

        /// <summary>
        /// </summary>
        protected SqlEntityConverter _entityConverter;

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SmartSqlDataAccessController{T}" /> class.
        /// </summary>
        /// <param name="sqlConnectionString">The SQL connection string.</param>
        /// <param name="fieldConverters">The field converters.</param>
        protected SmartSqlDataAccessController(string sqlConnectionString, params SqlFieldConverter[] fieldConverters)
                : base(sqlConnectionString)
        {
            _entityConverter = TryInitialize(typeof(T), fieldConverters);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SmartSqlDataAccessController{T}" /> class.
        /// </summary>
        /// <param name="sqlConnection">The SQL connection.</param>
        /// <param name="fieldConverters">The field converters.</param>
        protected SmartSqlDataAccessController(SqlConnection sqlConnection, params SqlFieldConverter[] fieldConverters)
                : base(sqlConnection)
        {
            _entityConverter = TryInitialize(typeof(T), fieldConverters);
        }

        #endregion

        /// <summary>
        /// Generates the SQL parameters.
        /// </summary>
        /// <typeparam name="TObject">The type of the t object.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="ignoredProperty">The ignored property.</param>
        /// <returns>System.Collections.Generic.List&lt;System.Data.SqlClient.SqlParameter&gt;.</returns>
        protected List<SqlParameter> GenerateSqlParameters<TObject>(TObject entity, params string[] ignoredProperty)
        {
            try
            {
                entity.CheckNullObject("entity");

                List<SqlParameter> result = new List<SqlParameter>();

                foreach (var one in _entityConverter.FieldConverters)
                {
                    if (!ignoredProperty.Contains(one.PropertyName))
                    {
                        result.Add(new SqlParameter("@" + one.ColumnName.Trim(), one.ToDbParameterObject(entity)));
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex.Handle( new { entity = typeof(TObject).GetFullName(), ignoredProperty });
            }
        }

        /// <summary>
        /// Generates the entity SQL parameters.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="ignoredProperty">The ignored property.</param>
        /// <returns>System.Collections.Generic.List&lt;System.Data.SqlClient.SqlParameter&gt;.</returns>
        protected List<SqlParameter> GenerateEntitySqlParameters(T entity, params string[] ignoredProperty)
        {
            try
            {
                entity.CheckNullObject("entity");

                return GenerateSqlParameters<T>(entity, ignoredProperty);
            }
            catch (Exception ex)
            {
                throw ex.Handle( new { entity = typeof(T).GetFullName(), ignoredProperty });
            }
        }

        /// <summary>
        /// Converts the object.
        /// </summary>
        /// <param name="sqlDataReader">The SQL data reader.</param>
        /// <returns>List{`0}.</returns>
        protected List<T> ConvertObject(SqlDataReader sqlDataReader)
        {
            try
            {
                sqlDataReader.CheckNullObject("sqlDataReader");

                var result = new List<T>();
                // When enter this method, Read() has been called for detect exception already.
                result.Add(ConvertEntityObject(sqlDataReader, _entityConverter.FieldConverters));

                while (sqlDataReader.Read())
                {
                    result.Add(ConvertEntityObject(sqlDataReader, _entityConverter.FieldConverters));
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
        /// <param name="fieldConverter">The field converter.</param>
        /// <returns>Object instance in type {`0}.</returns>
        protected T ConvertEntityObject(SqlDataReader sqlDataReader, List<SqlFieldConverter> fieldConverter)
        {
            var result = new T();

            try
            {
                foreach (var one in fieldConverter)
                {
                    one.SetPropertyValueObject(result, sqlDataReader[one.ColumnName]);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex.Handle( new { Converters = fieldConverter.Select((x) => new { x.ColumnName, x.PropertyName }) });
            }
        }

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
                reader = this.Execute(spName, sqlParameters);
                return reader == null ? new List<T>() : ConvertObject(reader);
            }
            catch (Exception ex)
            {
                throw ex.Handle( new { SpName = spName, Parameters = SqlParameterToList(sqlParameters) });
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
        /// Creates the or update.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="operatorKey">The operator key.</param>
        /// <param name="spName">Name of the sp.</param>
        /// <param name="includeOperatorKey">The include operator key.</param>
        /// <param name="ignoredProperty">The ignored property.</param>
        /// <returns>System.Nullable&lt;System.Guid&gt;.</returns>
        protected Guid? CreateOrUpdate(T entity, Guid? operatorKey, string spName = null, bool includeOperatorKey = true, params string[] ignoredProperty)
        {
            try
            {
                entity.CheckNullObject("entity");

                var parameters = GenerateEntitySqlParameters(entity, ignoredProperty);
                if (includeOperatorKey)
                {
                    parameters.Add(this.GenerateSqlSpParameter(column_OperatorKey, operatorKey));
                };

                return this.ExecuteScalar(spName.SafeToString(string.Format("sp_CreateOrUpdate{0}", typeof(T).Name)), parameters).ObjectToGuid();
            }
            catch (Exception ex)
            {
                throw ex.Handle( new { entity, operatorKey, spName });
            }
        }
    }
}
