using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Beyova.CommonAdminService.DataAccessController
{
    /// <summary>
    /// Access controller for <see cref="AdminUserInfo" /> class instance.
    /// </summary>
    internal class AdminUserInfoAccessController : AdminDataAccessController<AdminUserInfo>
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminUserInfoAccessController" /> class.
        /// </summary>
        public AdminUserInfoAccessController()
            : base()
        {
        }

        #endregion

        /// <summary>
        /// Converts the entity object.
        /// </summary>
        /// <param name="sqlDataReader">The SQL data reader.</param>
        /// <returns>AdminUserInfo.</returns>
        protected override AdminUserInfo ConvertEntityObject(SqlDataReader sqlDataReader)
        {
            var result = new AdminUserInfo
            {
                Name = sqlDataReader[column_Name].ObjectToString(),
                LoginName = sqlDataReader[column_LoginName].ObjectToString(),
                ThirdPartyId = sqlDataReader[column_ThirdPartyId].ObjectToString(),
                Email = sqlDataReader[column_Email].ObjectToString()
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

        /// <summary>
        /// Gets the admin user information by token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>AdminUserInfo.</returns>
        public AdminUserInfo GetAdminUserInfoByToken(string token)
        {
            const string spName = "sp_GetAdminUserInfoByToken";

            try
            {
                token.CheckEmptyString("token");

                var parameters = new List<SqlParameter>
                {
                    this.GenerateSqlSpParameter(column_Token,token)
                };

                return this.ExecuteReader(spName, parameters).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex.Handle("GetAdminUserInfoByToken", token);
            }
        }

        /// <summary>
        /// Creates the or update admin user information.
        /// </summary>
        /// <param name="adminUserInfo">The admin user information.</param>
        /// <returns>AdminUserInfo.</returns>
        public AdminUserInfo CreateOrUpdateAdminUserInfo(AdminUserInfo adminUserInfo)
        {
            const string spName = "sp_CreateOrUpdateAdminUserInfo";

            try
            {
                adminUserInfo.CheckNullObject("adminUserInfo");

                var parameters = new List<SqlParameter>
                {
                    this.GenerateSqlSpParameter(column_Key, adminUserInfo.Key),
                    this.GenerateSqlSpParameter(column_LoginName, adminUserInfo.LoginName),
                    this.GenerateSqlSpParameter(column_Password, HashPassword(adminUserInfo.Password)),
                    this.GenerateSqlSpParameter(column_DisplayName, adminUserInfo.DisplayName),
                    this.GenerateSqlSpParameter(column_State, (int) adminUserInfo.State)
                };

                return this.ExecuteReader(spName, parameters).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex.Handle("CreateOrUpdateAdminUserInfo", adminUserInfo);
            }
        }

        /// <summary>
        /// Queries the admin user information.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;AdminUserInfo&gt;.</returns>
        public List<AdminUserInfo> QueryAdminUserInfo(AdminUserCriteria criteria)
        {
            const string spName = "sp_QueryAdminUserInfo";

            try
            {
                criteria.CheckNullObject("criteria");

                var parameters = new List<SqlParameter>
                {
                    this.GenerateSqlSpParameter(column_Key, criteria.Key),
                    this.GenerateSqlSpParameter(column_LoginName, criteria.LoginName),
                    this.GenerateSqlSpParameter(column_DisplayName, criteria.DisplayName)
                };

                return this.ExecuteReader(spName, parameters);
            }
            catch (Exception ex)
            {
                throw ex.Handle("QueryAdminUserInfo", criteria);
            }
        }
    }
}
