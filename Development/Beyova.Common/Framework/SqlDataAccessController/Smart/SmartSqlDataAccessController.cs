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
    /// <typeparam name="TEntityBase">The type of the t entity base.</typeparam>
    /// <typeparam name="TEntity">The type of the t entity.</typeparam>
    /// <typeparam name="TCriteria">The type of the t criteria.</typeparam>
    public abstract class SmartSqlDataAccessController<TEntityBase, TEntity, TCriteria> : SmartSqlDataAccessController<TEntityBase, TEntity>
        where TEntity : TEntityBase, new()
        where TEntityBase : class, new()
        where TCriteria : class
    {
        /// <summary>
        /// The _criteria entity converter
        /// </summary>
        protected SqlEntityConverter _criteriaEntityConverter;

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Beyova.SmartSqlDataAccessController`2" /> class.
        /// </summary>
        /// <param name="primarySqlConnectionString">The primary SQL connection string.</param>
        /// <param name="readonlySqlConnectionString">The readonly SQL connection string.</param>
        /// <param name="fieldConverters">The field converters.</param>
        protected SmartSqlDataAccessController(string primarySqlConnectionString, string readonlySqlConnectionString, params SqlFieldConverter[] fieldConverters)
                : base(primarySqlConnectionString, readonlySqlConnectionString, fieldConverters)
        {
            _criteriaEntityConverter = TryInitialize(typeof(TCriteria), fieldConverters);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Beyova.SmartSqlDataAccessController`2" /> class.
        /// </summary>
        /// <param name="primarySqlConnection">The primary SQL connection.</param>
        /// <param name="readonlySqlConnection">The readonly SQL connection.</param>
        /// <param name="fieldConverters">The field converters.</param>
        protected SmartSqlDataAccessController(SqlConnection primarySqlConnection, SqlConnection readonlySqlConnection, params SqlFieldConverter[] fieldConverters)
             : base(primarySqlConnection, readonlySqlConnection, fieldConverters)
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
                criteria.CheckNullObject(nameof(criteria));

                return GenerateSqlParameters<TCriteria>(criteria, ignoredProperty);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { entity = typeof(TCriteria).GetFullName(), ignoredProperty });
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
                criteria.CheckNullObject(nameof(criteria));

                var parameters = GenerateCriteriaSqlParameters(criteria, ignoredProperty);

                return this.ExecuteReader(spName.SafeToString(string.Format("sp_Query{0}", typeof(TEntity).Name)), parameters, true);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { criteria, spName });
            }
        }
    }

    /// <summary>
    /// Class SmartSqlDataAccessController.
    /// </summary>
    /// <typeparam name="TEntityBase">The type of the t entity base.</typeparam>
    /// <typeparam name="TEntity">The type of the t entity.</typeparam>
    public abstract class SmartSqlDataAccessController<TEntityBase, TEntity> : SqlDataAccessController
        where TEntity : TEntityBase, new()
        where TEntityBase : class, new()
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
        /// </summary>
        protected SqlEntityConverter _entityConverter;

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SmartSqlDataAccessController{TEntityBase, TEntity}" /> class.
        /// </summary>
        /// <param name="primarySqlConnectionString">The primary SQL connection string.</param>
        /// <param name="readonlySqlConnectionString">The readonly SQL connection string.</param>
        /// <param name="fieldConverters">The field converters.</param>
        protected SmartSqlDataAccessController(string primarySqlConnectionString, string readonlySqlConnectionString, params SqlFieldConverter[] fieldConverters)
                        : base(primarySqlConnectionString, readonlySqlConnectionString)
        {
            _entityConverter = TryInitialize(typeof(TEntityBase), fieldConverters);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SmartSqlDataAccessController{TEntityBase, TEntity}" /> class.
        /// </summary>
        /// <param name="primarySqlConnection">The primary SQL connection.</param>
        /// <param name="readonlySqlConnection">The readonly SQL connection.</param>
        /// <param name="fieldConverters">The field converters.</param>
        protected SmartSqlDataAccessController(SqlConnection primarySqlConnection, SqlConnection readonlySqlConnection, params SqlFieldConverter[] fieldConverters)
                        : base(primarySqlConnection, readonlySqlConnection)
        {
            _entityConverter = TryInitialize(typeof(TEntityBase), fieldConverters);
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
                entity.CheckNullObject(nameof(entity));

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
                throw ex.Handle(new { entity = typeof(TObject).GetFullName(), ignoredProperty });
            }
        }

        /// <summary>
        /// Generates the entity SQL parameters.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="ignoredProperty">The ignored property.</param>
        /// <returns>System.Collections.Generic.List&lt;System.Data.SqlClient.SqlParameter&gt;.</returns>
        protected List<SqlParameter> GenerateEntitySqlParameters(TEntityBase entity, params string[] ignoredProperty)
        {
            try
            {
                entity.CheckNullObject(nameof(entity));
                return GenerateSqlParameters<TEntityBase>(entity, ignoredProperty);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { entity = typeof(TEntityBase).GetFullName(), ignoredProperty });
            }
        }

        /// <summary>
        /// Converts the object.
        /// </summary>
        /// <param name="sqlDataReader">The SQL data reader.</param>
        /// <returns>List{`0}.</returns>
        protected List<TEntity> ConvertObject(SqlDataReader sqlDataReader)
        {
            try
            {
                sqlDataReader.CheckNullObject(nameof(sqlDataReader));

                var result = new List<TEntity>();
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
                throw ex.Handle(new { type = typeof(TEntity).GetFullName() });
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
        /// <param name="fieldConverter">The field converter.</param>
        /// <returns>Object instance in type {`0}.</returns>
        protected TEntity ConvertEntityObject(SqlDataReader sqlDataReader, List<SqlFieldConverter> fieldConverter)
        {
            var result = new TEntity();

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
                throw ex.Handle(new { Converters = fieldConverter.Select((x) => new { x.ColumnName, x.PropertyName }) });
            }
        }

        /// <summary>
        /// Executes the reader.
        /// </summary>
        /// <param name="spName">Name of the sp.</param>
        /// <param name="sqlParameters">The SQL parameters.</param>
        /// <param name="preferReadOnlyOperator">The prefer read only operator.</param>
        /// <returns>List{`0}.</returns>
        protected List<TEntity> ExecuteReader(string spName, List<SqlParameter> sqlParameters, bool preferReadOnlyOperator)
        {
            SqlDataReader reader = null;
            DatabaseOperator databaseOperator = null;

            try
            {
                reader = this.Execute(spName, sqlParameters, preferReadOnlyOperator, out databaseOperator);
                return reader == null ? new List<TEntity>() : ConvertObject(reader);
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
                databaseOperator?.Close();
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
        protected Guid? CreateOrUpdate(TEntityBase entity, Guid? operatorKey, string spName = null, bool includeOperatorKey = true, params string[] ignoredProperty)
        {
            try
            {
                entity.CheckNullObject(nameof(entity));

                var parameters = GenerateEntitySqlParameters(entity, ignoredProperty);
                if (includeOperatorKey)
                {
                    parameters.Add(this.GenerateSqlSpParameter(column_OperatorKey, operatorKey));
                };

                return this.ExecuteScalar(spName.SafeToString(string.Format("sp_CreateOrUpdate{0}", typeof(TEntityBase).Name)), parameters).ObjectToGuid();
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { entity, operatorKey, spName });
            }
        }
    }
}
