using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;

namespace Beyova.CommonService.DataAccessController
{
    /// <summary>
    /// Class ProvisioningAccessController.
    /// </summary>
    /// <typeparam name="TProvisioning">The type of the t provisioning.</typeparam>
    /// <typeparam name="TProvisioningCriteria">The type of the t provisioning criteria.</typeparam>
    /// <typeparam name="TApplication">The type of the t application.</typeparam>
    public abstract class ProvisioningAccessController<TProvisioning, TProvisioningCriteria, TApplication> : BaseCommonServiceController<TProvisioning, TProvisioningCriteria>
        where TProvisioning : IProvisioningObject<TApplication>, new()
        where TProvisioningCriteria : IProvisioningCriteria<TApplication>, new()
        where TApplication : struct, IConvertible
    {
        #region Column constants

        /// <summary>
        /// The column_ module
        /// </summary>
        protected const string column_Module = "Module";

        /// <summary>
        /// The column_ application
        /// </summary>
        protected const string column_Application = "Application";

        /// <summary>
        /// The column_ value
        /// </summary>
        protected const string column_Value = "Value";

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ProvisioningAccessController{TProvisioning, TProvisioningCriteria, TApplication}" /> class.
        /// </summary>
        public ProvisioningAccessController()
            : base()
        {
        }

        #endregion

        /// <summary>
        /// Converts the entity object.
        /// </summary>
        /// <param name="sqlDataReader">The SQL data reader.</param>
        /// <returns>AccessCredentialInfo.</returns>
        protected override TProvisioning ConvertEntityObject(SqlDataReader sqlDataReader)
        {
            var result = new TProvisioning
            {
                Key = sqlDataReader[column_Key].ObjectToGuid(),
                Module = sqlDataReader[column_Module].ObjectToString(),
                Value = JToken.Parse(sqlDataReader[column_Value].ObjectToString()),
                Name = sqlDataReader[column_Name].ObjectToString(),
                Application = sqlDataReader[column_Application].ObjectToInt32().Int32ToEnum<TApplication>(),
            };

            FillAdditionalFieldValue(result, sqlDataReader);
            FillBaseObjectFields(result, sqlDataReader);

            return result;
        }

        /// <summary>
        /// Queries the provisioning object.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;TProvisioning&gt;.</returns>
        public List<TProvisioning> QueryProvisioningObject(TProvisioningCriteria criteria)
        {
            const string spName = "sp_QueryProvisioning";

            try
            {
                criteria.CheckNullObject("criteria");

                var parameters = new List<SqlParameter>
                {
                    this.GenerateSqlSpParameter(column_Key, criteria.Key),
                    this.GenerateSqlSpParameter(column_Application, criteria.Application.EnumToInt32()),
                    this.GenerateSqlSpParameter(column_Module, criteria.Module),
                    this.GenerateSqlSpParameter(column_Name, criteria.Name),
                    this.GenerateSqlSpParameter(column_OwnerKey, criteria.OwnerKey)
                };

                FillAdditionalFieldValue(parameters, criteria);

                return this.ExecuteReader(spName, parameters);
            }
            catch (Exception ex)
            {
                throw ex.Handle("QueryProvisioning", criteria);
            }
        }

        /// <summary>
        /// Saves the provisioning object.
        /// </summary>
        /// <param name="provisioningObject">The provisioning object.</param>
        /// <param name="operatorKey">The operator key.</param>
        public void SaveProvisioningObject(TProvisioning provisioningObject, Guid? operatorKey)
        {
            const string spName = "sp_SaveProvisioningObject";

            try
            {
                provisioningObject.CheckNullObject("provisioningObject");

                var parameters = new List<SqlParameter>
                {
                    this.GenerateSqlSpParameter(column_Application, provisioningObject.Application.EnumToInt32()),
                    this.GenerateSqlSpParameter(column_Module, provisioningObject.Module),
                    this.GenerateSqlSpParameter(column_Name, provisioningObject.Name),
                    this.GenerateSqlSpParameter(column_OwnerKey, provisioningObject.OwnerKey),
                    this.GenerateSqlSpParameter(column_Value, provisioningObject.Value.ToString()),
                    this.GenerateSqlSpParameter(column_OperatorKey, operatorKey)
                };

                FillAdditionalFieldValue(parameters, provisioningObject);

                this.ExecuteScalar(spName, parameters);
            }
            catch (Exception ex)
            {
                throw ex.Handle("SaveProvisioningObject", new { provisioningObject, operatorKey });
            }
        }
    }
}