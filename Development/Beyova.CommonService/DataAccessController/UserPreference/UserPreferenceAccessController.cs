using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;

namespace Beyova.CommonService.DataAccessController
{
    /// <summary>
    /// Class UserPreferenceAccessController.
    /// </summary>
    public abstract class UserPreferenceAccessController: BaseCommonServiceController<UserPreference, UserPreferenceCriteria>
    {
        #region Column constants

        /// <summary>
        /// The column_ realm
        /// </summary>
        protected const string column_Realm = "Realm";

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="UserPreferenceAccessController" /> class.
        /// </summary>
        public UserPreferenceAccessController()
            : base()
        {
        }

        #endregion

        /// <summary>
        /// Converts the entity object.
        /// </summary>
        /// <param name="sqlDataReader">The SQL data reader.</param>
        /// <returns>AccessCredentialInfo.</returns>
        protected override UserPreference ConvertEntityObject(SqlDataReader sqlDataReader)
        {
            var result = new UserPreference
            {
                //Key = sqlDataReader[column_Key].ObjectToGuid(),
                //Module = sqlDataReader[column_Module].ObjectToString(),
                //Value = JToken.Parse(sqlDataReader[column_Value].ObjectToString()),
                //Name = sqlDataReader[column_Name].ObjectToString(),
                //Application = sqlDataReader[column_Application].ObjectToInt32().Int32ToEnum<TApplication>(),
            };

            return result;
        }

        ///// <summary>
        ///// Queries the provisioning object.
        ///// </summary>
        ///// <param name="criteria">The criteria.</param>
        ///// <returns>List&lt;TProvisioning&gt;.</returns>
        //public List<TUserPreference> QueryProvisioningObject(TUserPreferenceCriteria criteria)
        //{
        //    const string spName = "sp_QueryProvisioning";

        //    try
        //    {
        //        criteria.CheckNullObject("criteria");

        //        var parameters = new List<SqlParameter>
        //        {
        //            this.GenerateSqlSpParameter(column_Key, criteria.Key),
        //            this.GenerateSqlSpParameter(column_Application, criteria.Application.EnumToInt32()),
        //            this.GenerateSqlSpParameter(column_Module, criteria.Module),
        //            this.GenerateSqlSpParameter(column_Name, criteria.Name),
        //            this.GenerateSqlSpParameter(column_OwnerKey, criteria.OwnerKey)
        //        };

        //        FillAdditionalFieldValue(parameters, criteria);

        //        return this.ExecuteReader(spName, parameters);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex.Handle( criteria);
        //    }
        //}

        ///// <summary>
        ///// Saves the provisioning object.
        ///// </summary>
        ///// <param name="provisioningObject">The provisioning object.</param>
        ///// <param name="operatorKey">The operator key.</param>
        //public void SaveProvisioningObject(TUserPreference provisioningObject, Guid? operatorKey)
        //{
        //    const string spName = "sp_SaveProvisioningObject";

        //    try
        //    {
        //        provisioningObject.CheckNullObject("provisioningObject");

        //        var parameters = new List<SqlParameter>
        //        {
        //            this.GenerateSqlSpParameter(column_Application, provisioningObject.Application.EnumToInt32()),
        //            this.GenerateSqlSpParameter(column_Module, provisioningObject.Module),
        //            this.GenerateSqlSpParameter(column_Name, provisioningObject.Name),
        //            this.GenerateSqlSpParameter(column_OwnerKey, provisioningObject.OwnerKey),
        //            this.GenerateSqlSpParameter(column_Value, provisioningObject.Value.ToString()),
        //            this.GenerateSqlSpParameter(column_OperatorKey, operatorKey)
        //        };

        //        FillAdditionalFieldValue(parameters, provisioningObject);

        //        this.ExecuteScalar(spName, parameters);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex.Handle( new { provisioningObject, operatorKey });
        //    }
        //}
    }
}