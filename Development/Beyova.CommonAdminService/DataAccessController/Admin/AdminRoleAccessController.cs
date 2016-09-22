using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Beyova.CommonAdminService.DataAccessController
{
    /// <summary>
    /// Access controller for <see cref="AdminRole" /> class instance.
    /// </summary>
    internal class AdminRoleAccessController : AdminDataAccessController<AdminRole>
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminRoleAccessController" /> class.
        /// </summary>
        public AdminRoleAccessController()
            : base()
        {
        }

        #endregion

        protected override AdminRole ConvertEntityObject(SqlDataReader sqlDataReader)
        {
            var result = new AdminRole
            {
                Name = sqlDataReader[column_Name].ObjectToString(),
                ParentKey = sqlDataReader[column_ParentKey].ObjectToGuid(),
                Description = sqlDataReader[column_Description].ObjectToString()
            };

            FillBaseObjectFields(result, sqlDataReader);

            return result;
        }

        /// <summary>
        /// Creates the or update admin role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <param name="operatorKey">The operator key.</param>
        /// <returns>System.Nullable&lt;Guid&gt;.</returns>
        public Guid? CreateOrUpdateAdminRole(AdminRole role, Guid? operatorKey)
        {
            const string spName = "sp_CreateOrUpdateAdminRole";

            try
            {
                role.CheckNullObject("role");

                var parameters = new List<SqlParameter>
                {
                    this.GenerateSqlSpParameter(column_Key, role.Key),
                    this.GenerateSqlSpParameter(column_Name, role.Name),
                    this.GenerateSqlSpParameter(column_ParentKey, role.ParentKey),
                    this.GenerateSqlSpParameter(column_Description, role.Description),
                    this.GenerateSqlSpParameter(column_State, role.State),
                    this.GenerateSqlSpParameter(column_OperatorKey, operatorKey),
                };

                return this.ExecuteScalar(spName, parameters).ObjectToGuid();
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { role, operatorKey });
            }
        }

        /// <summary>
        /// Queries the admin role.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;AdminRole&gt;.</returns>
        public List<AdminRole> QueryAdminRole(AdminRoleCriteria criteria)
        {
            const string spName = "sp_QueryAdminRole";

            try
            {
                criteria.CheckNullObject("criteria");

                var parameters = new List<SqlParameter>
                {
                    this.GenerateSqlSpParameter(column_Key, criteria.Key),
                    this.GenerateSqlSpParameter(column_Name, criteria.Name),
                    this.GenerateSqlSpParameter(column_ParentKey, criteria.ParentKey),
                    this.GenerateSqlSpParameter(column_UserKey, criteria.UserKey),
                    this.GenerateSqlSpParameter(column_PermissionKey, criteria.PermissionKey)
                };

                return this.ExecuteReader(spName, parameters);
            }
            catch (Exception ex)
            {
                throw ex.Handle(criteria);
            }
        }

        /// <summary>
        /// Binds the permission on role.
        /// </summary>
        /// <param name="binding">The binding.</param>
        /// <param name="operatorKey">The operator key.</param>
        /// <returns>System.Nullable&lt;Guid&gt;.</returns>
        public Guid? BindPermissionOnRole(AdminPermissionBinding binding, Guid? operatorKey)
        {
            const string spName = "sp_BindPermissionOnRole";

            try
            {
                binding.CheckNullObject("binding");

                var parameters = new List<SqlParameter>
                {
                    this.GenerateSqlSpParameter(column_OwnerKey, binding.OwnerKey),
                    this.GenerateSqlSpParameter(column_PermissionKey, binding.PermissionKey),
                    this.GenerateSqlSpParameter(column_OperatorKey,operatorKey)
                };

                return this.ExecuteScalar(spName, parameters).ObjectToGuid();
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { binding, operatorKey });
            }
        }

        /// <summary>
        /// Unbinds the permission on role.
        /// </summary>
        /// <param name="binding">The binding.</param>
        /// <param name="operatorKey">The operator key.</param>
        public void UnbindPermissionOnRole(AdminPermissionBinding binding, Guid? operatorKey)
        {
            const string spName = "sp_UnbindPermissionOnRole";

            try
            {
                binding.CheckNullObject("binding");

                var parameters = new List<SqlParameter>
                {
                    this.GenerateSqlSpParameter(column_OwnerKey, binding.OwnerKey),
                    this.GenerateSqlSpParameter(column_PermissionKey, binding.PermissionKey),
                    this.GenerateSqlSpParameter(column_OperatorKey,operatorKey)
                };

                this.ExecuteNonQuery(spName, parameters);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { binding, operatorKey });
            }
        }

    }
}
