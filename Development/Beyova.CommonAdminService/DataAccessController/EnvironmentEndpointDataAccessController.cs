using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beyova;

namespace Beyova.CommonAdminService.DataAccessController
{
    /// <summary>
    /// Class EnvironmentEndpointDataAccessController.
    /// </summary>
    public class EnvironmentEndpointDataAccessController : AdminDataAccessController<EnvironmentEndpoint>
    {
        #region Column constants

        /// <summary>
        /// The column_ environment
        /// </summary>
        protected const string column_Environment = "Environment";

        /// <summary>
        /// The column_ host
        /// </summary>
        protected const string column_Host = "Host";

        /// <summary>
        /// The column_ port
        /// </summary>
        protected const string column_Port = "Port";

        /// <summary>
        /// The column_ protocol
        /// </summary>
        protected const string column_Protocol = "Protocol";

        /// <summary>
        /// The column_ version
        /// </summary>
        protected const string column_Version = "Version";

        /// <summary>
        /// The column_ connection strings
        /// </summary>
        protected const string column_ConnectionStrings = "ConnectionStrings";

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="EnvironmentEndpointDataAccessController"/> class.
        /// </summary>
        public EnvironmentEndpointDataAccessController() : base()
        {
        }

        /// <summary>
        /// Converts the entity object.
        /// </summary>
        /// <param name="sqlDataReader">The SQL data reader.</param>
        /// <returns>Object instance in type {`0}.</returns>
        protected override EnvironmentEndpoint ConvertEntityObject(SqlDataReader sqlDataReader)
        {
            var result = new EnvironmentEndpoint
            {
                Name = sqlDataReader[column_Name].ObjectToString(),
                Code = sqlDataReader[column_Code].ObjectToString(),
                Environment = sqlDataReader[column_Environment].ObjectToString(),
                Host = sqlDataReader[column_Host].ObjectToString(),
                Port = sqlDataReader[column_Port].ObjectToNullableInt32(),
                Protocol = sqlDataReader[column_Protocol].ObjectToString(),
                Token = sqlDataReader[column_Token].ObjectToString(),
                Version = sqlDataReader[column_Version].ObjectToString(),
                ConnectionStrings = sqlDataReader[column_ConnectionStrings].ObjectToXml().XmlToDictionary(x => x.Value)
            };

            FillBaseObjectFields(result, sqlDataReader);

            return result;
        }

        /// <summary>
        /// Creates the or update environment endpoint.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="operatorKey">The operator key.</param>
        /// <returns>System.Nullable&lt;Guid&gt;.</returns>
        public Guid? CreateOrUpdateEnvironmentEndpoint(EnvironmentEndpoint endpoint, Guid? operatorKey)
        {
            const string spName = "sp_CreateOrUpdateEnvironmentEndpoint";

            try
            {
                endpoint.CheckNullObject("endpoint");

                var parameters = new List<SqlParameter>
                {
                    this.GenerateSqlSpParameter(column_Key, endpoint.Key),
                    this.GenerateSqlSpParameter(column_Name, endpoint.Name),
                    this.GenerateSqlSpParameter(column_Code,endpoint.Code),
                    this.GenerateSqlSpParameter(column_Environment,endpoint.Environment),
                    this.GenerateSqlSpParameter(column_Protocol,endpoint.Protocol),
                    this.GenerateSqlSpParameter(column_Host,endpoint.Host),
                    this.GenerateSqlSpParameter(column_Port,endpoint.Port),
                    this.GenerateSqlSpParameter(column_Version,endpoint.Version),
                    this.GenerateSqlSpParameter(column_Token,endpoint.Token),
                    this.GenerateSqlSpParameter(column_ConnectionStrings,endpoint.ConnectionStrings.DictionaryToXml()),
                    this.GenerateSqlSpParameter(column_OperatorKey,operatorKey)
                };

                return this.ExecuteScalar(spName, parameters).ObjectToGuid();
            }
            catch (Exception ex)
            {
                throw ex.Handle("CreateOrUpdateEnvironmentEndpoint", new { endpoint, operatorKey });
            }
        }

        /// <summary>
        /// Queries the environment endpoint.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="code">The code.</param>
        /// <param name="environment">The environment.</param>
        /// <returns>List&lt;EnvironmentEndpoint&gt;.</returns>
        public List<EnvironmentEndpoint> QueryEnvironmentEndpoint(Guid? key, string code, string environment)
        {
            const string spName = "sp_QueryEnvironmentEndpoint";

            try
            {
                var parameters = new List<SqlParameter>
                {
                    this.GenerateSqlSpParameter(column_Key,key),
                    this.GenerateSqlSpParameter(column_Code,code),
                    this.GenerateSqlSpParameter(column_Environment,environment),
                };

                return this.ExecuteReader(spName, parameters);
            }
            catch (Exception ex)
            {
                throw ex.Handle("QueryEnvironmentEndpoint", new { key, code, environment });
            }
        }
    }
}
