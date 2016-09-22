using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Beyova.CommonAdminService.DataAccessController
{
    /// <summary>
    /// Access controller for <see cref="AdminSession" /> class instance.
    /// </summary>
    public class AdminSessionAccessController : AdminDataAccessController<AdminSession>
    {
        /// <summary>
        /// Gets or sets the admin session expiration.
        /// </summary>
        /// <value>The admin session expiration.</value>
        public int AdminSessionExpiration { get; protected set; }

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminSessionAccessController" /> class.
        /// </summary>
        /// <param name="adminSessionExpiration">The admin session expiration.</param>
        public AdminSessionAccessController(int adminSessionExpiration)
            : base()
        {
            this.AdminSessionExpiration = adminSessionExpiration;
        }

        #endregion

        /// <summary>
        /// Converts the entity object.
        /// </summary>
        /// <param name="sqlDataReader">The SQL data reader.</param>
        /// <returns>AdminSession.</returns>
        protected override AdminSession ConvertEntityObject(SqlDataReader sqlDataReader)
        {
            var result = new AdminSession
            {
                Token = sqlDataReader[column_Token].ObjectToString(),
                OwnerKey = sqlDataReader[column_UserKey].ObjectToGuid(),
                IpAddress = sqlDataReader[column_IpAddress].ObjectToString(),
                UserAgent = sqlDataReader[column_UserAgent].ObjectToString(),
                ExpiredStamp = sqlDataReader[column_ExpiredStamp].ObjectToDateTime(),
                CreatedStamp = sqlDataReader[column_CreatedStamp].ObjectToDateTime(),
                LastUpdatedStamp = sqlDataReader[column_LastUpdatedStamp].ObjectToDateTime(),
                State = (ObjectState)sqlDataReader[column_State].ObjectToInt32(),
            };

            return result;
        }

        /// <summary>
        /// Creates the admin session.
        /// </summary>
        /// <param name="adminSession">The admin session.</param>
        /// <param name="isUnique">if set to <c>true</c> [is unique].</param>
        /// <returns>AdminSession.</returns>
        public AdminSession CreateAdminSession(AdminSession adminSession, bool isUnique = false)
        {
            const string spName = "sp_CreateAdminSession";

            try
            {
                adminSession.CheckNullObject("adminSession");

                var parameters = new List<SqlParameter>
                {
                    this.GenerateSqlSpParameter(column_UserKey, adminSession.OwnerKey),
                    this.GenerateSqlSpParameter(column_IpAddress, adminSession.IpAddress),
                    this.GenerateSqlSpParameter(column_UserAgent, adminSession.UserAgent),
                    this.GenerateSqlSpParameter(column_ExpiredStamp, DateTime.UtcNow.AddMinutes(this.AdminSessionExpiration)),
                    this.GenerateSqlSpParameter(column_IsUnique,isUnique)
                };

                return this.ExecuteReader(spName, parameters).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { adminSession, isUnique });
            }
        }

        /// <summary>
        /// Deletes the admin session.
        /// </summary>
        /// <param name="token">The token.</param>
        public void DeleteAdminSession(string token)
        {
            const string spName = "sp_DeleteAdminSession";

            try
            {
                token.CheckEmptyString("token");

                var parameters = new List<SqlParameter>
                {
                    this.GenerateSqlSpParameter(column_Token,token)
                };

                this.ExecuteNonQuery(spName, parameters);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { token });
            }
        }

        /// <summary>
        /// Queries the admin session.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;AdminSession&gt;.</returns>
        public List<AdminSession> QueryAdminSession(AdminSessionCriteria criteria)
        {
            const string spName = "sp_QueryAdminSession";

            try
            {
                criteria.CheckNullObject("criteria");

                var parameters = new List<SqlParameter>
                {
                    this.GenerateSqlSpParameter(column_UserKey, criteria.OwnerKey),
                    this.GenerateSqlSpParameter(column_Token, criteria.Token),
                    this.GenerateSqlSpParameter(column_IpAddress, criteria.IpAddress),
                    this.GenerateSqlSpParameter(column_ActiveOnly, criteria.ActiveOnly),
                };

                return this.ExecuteReader(spName, parameters);
            }
            catch (Exception ex)
            {
                throw ex.Handle(criteria);
            }
        }
    }
}
