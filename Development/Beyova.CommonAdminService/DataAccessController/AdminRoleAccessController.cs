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
                ParentKey = sqlDataReader[column_ParentKey].ObjectToGuid()
            };

            FillBaseObjectFields(result, sqlDataReader, true);

            return result;
        }

        /// <summary>
        /// Authenticates the admin user.
        /// </summary>
        /// <param name="accessCredential">The access credential.</param>
        /// <returns>AdminUserInfo.</returns>
        public AdminUserInfo AuthenticateAdminUser(AccessCredential accessCredential)
        {
            const string spName = "sp_AuthenticateAdminUserInfo";

            try
            {
                accessCredential.CheckNullObject("accessCredential");

                var parameters = new List<SqlParameter>
                {
                    this.GenerateSqlSpParameter(column_LoginName, accessCredential.AccessIdentifier),
                    this.GenerateSqlSpParameter(column_Password, HashPassword(accessCredential.Token)),
                };

                return this.ExecuteReader(spName, parameters).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex.Handle("AuthenticateAdminUser", accessCredential);
            }
        }


    }
}
