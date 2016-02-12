using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Beyova.CommonAdminService.DataAccessController
{
    /// <summary>
    /// Access controller for <see cref="AdminPermission" /> class instance.
    /// </summary>
    internal class AdminPermissionAccessController : AdminDataAccessController<AdminPermission>
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminPermissionAccessController" /> class.
        /// </summary>
        public AdminPermissionAccessController()
            : base()
        {
        }

        #endregion

        /// <summary>
        /// Converts the entity object.
        /// </summary>
        /// <param name="sqlDataReader">The SQL data reader.</param>
        /// <returns>Object instance in type {`0}.</returns>
        protected override AdminPermission ConvertEntityObject(SqlDataReader sqlDataReader)
        {
            var result = new AdminPermission
            {
                Name = sqlDataReader[column_Name].ObjectToString(),
                Identifier = sqlDataReader[column_Identifier].ObjectToString(),
                Description = sqlDataReader[column_Description].ObjectToString()
            };

            FillBaseObjectFields(result, sqlDataReader);

            return result;
        }

        public Guid? CreateOrUpdateAdminPermission(AdminPermission permission, Guid? operatorKey)
        {
            const string spName = "sp_CreateOrUpdateAdminPermission";

            try
            {
                permission.CheckNullObject("permission");

                var parameters = new List<SqlParameter>
                {
                    this.GenerateSqlSpParameter(column_Key, permission.Key),
                    this.GenerateSqlSpParameter(column_Name, permission.Name),
                    this.GenerateSqlSpParameter(column_Identifier, permission.Identifier),
                    this.GenerateSqlSpParameter(column_Description, permission.Description),
                    this.GenerateSqlSpParameter(column_State, permission.State),
                    this.GenerateSqlSpParameter(column_OperatorKey, operatorKey),
                };

                return this.ExecuteScalar(spName, parameters).ObjectToGuid();
            }
            catch (Exception ex)
            {
                throw ex.Handle("CreateOrUpdateAdminPermission", new { permission, operatorKey });
            }
        }

        /// <summary>
        /// Queries the admin permission.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;AdminPermission&gt;.</returns>
        public List<AdminPermission> QueryAdminPermission(AdminPermissionCriteria criteria)
        {
            const string spName = "sp_QueryAdminPermission";

            try
            {
                criteria.CheckNullObject("criteria");

                var parameters = new List<SqlParameter>
                {
                  this.GenerateSqlSpParameter(column_Key, criteria.Key),
                    this.GenerateSqlSpParameter(column_Name, criteria.Name),
                    this.GenerateSqlSpParameter(column_Identifier, criteria.Identifier),
                    this.GenerateSqlSpParameter(column_Description, criteria.Description),
                    this.GenerateSqlSpParameter(column_UserKey, criteria.UserKey),
                    this.GenerateSqlSpParameter(column_RoleKey, criteria.RoleKey)
                };

                return this.ExecuteReader(spName, parameters);
            }
            catch (Exception ex)
            {
                throw ex.Handle("QueryAdminPermission", criteria);
            }
        }
    }
}
