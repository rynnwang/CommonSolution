using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Beyova;

namespace Beyova.CommonService.DataAccessController
{
    /// <summary>
    /// Access controller for <see cref="SessionInfo" /> class instance.
    /// </summary>
    public class SessionInfoAccessController : BaseCommonServiceController<SessionInfo, SessionCriteria>
    {
        #region Database Constants

        /// <summary>
        /// The column_ active only
        /// </summary>
        protected const string column_ActiveOnly = "ActiveOnly";

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionInfoAccessController" /> class.
        /// </summary>
        public SessionInfoAccessController()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionInfoAccessController"/> class.
        /// </summary>
        /// <param name="sqlConnection">The SQL connection.</param>
        public SessionInfoAccessController(string sqlConnection)
          : base(sqlConnection)
        {
        }

        #endregion

        /// <summary>
        /// Converts the entity object.
        /// </summary>
        /// <param name="sqlDataReader">The SQL data reader.</param>
        /// <returns>SessionInfo.</returns>
        protected override SessionInfo ConvertEntityObject(SqlDataReader sqlDataReader)
        {
            return new SessionInfo
            {
                UserKey = sqlDataReader[column_UserKey].ObjectToGuid(),
                Token = sqlDataReader[column_Token].ObjectToString(),
                UserAgent = sqlDataReader[column_UserAgent].ObjectToString(),
                IpAddress = sqlDataReader[column_IpAddress].ObjectToString(),
                Platform = (PlatformType)sqlDataReader[column_Platform].ObjectToInt32(),
                DeviceType = (DeviceType)sqlDataReader[column_DeviceType].ObjectToInt32(),
                CreatedStamp = sqlDataReader[column_CreatedStamp].ObjectToDateTime(),
                ExpiredStamp = sqlDataReader[column_ExpiredStamp].ObjectToDateTime()
            };
        }

        /// <summary>
        /// Disposes the session information.
        /// </summary>
        /// <param name="token">The token.</param>
        public void DisposeSessionInfo(string token)
        {
            const string spName = "sp_DisposeSessionInfo";

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
                throw ex.Handle("DisposeSessionInfo", new { token });
            }
        }

        /// <summary>
        /// Creates the session information.
        /// </summary>
        /// <param name="sessionInfo">The session information.</param>
        /// <returns>SessionInfo.</returns>
        public SessionInfo CreateSessionInfo(SessionInfo sessionInfo)
        {
            const string spName = "sp_CreateSessionInfo";

            try
            {
                sessionInfo.CheckNullObject("sessionInfo");

                var parameters = new List<SqlParameter>
                {
                    this.GenerateSqlSpParameter(column_UserKey, sessionInfo.UserKey),
                    this.GenerateSqlSpParameter(column_UserAgent,sessionInfo.UserAgent),
                    this.GenerateSqlSpParameter(column_Platform,(int)sessionInfo.Platform),
                    this.GenerateSqlSpParameter(column_DeviceType,(int)sessionInfo.DeviceType),
                    this.GenerateSqlSpParameter(column_IpAddress,sessionInfo.IpAddress),
                    this.GenerateSqlSpParameter(column_ExpiredStamp,sessionInfo.ExpiredStamp)
                };

                return this.ExecuteReader(spName, parameters).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex.Handle("CreateSessionInfo", new { sessionInfo });
            }
        }

        /// <summary>
        /// Queries the session information.
        /// </summary>
        /// <param name="userKey">The user key.</param>
        /// <param name="activeOnly">if set to <c>true</c> [active only].</param>
        /// <returns>List&lt;SessionInfo&gt;.</returns>
        public List<SessionInfo> QuerySessionInfo(SessionCriteria criteria)
        {
            const string spName = "sp_QuerySessionInfo";

            try
            {
                criteria.CheckNullObject("criteria");

                var parameters = new List<SqlParameter>
                {
                    this.GenerateSqlSpParameter(column_UserKey, criteria.UserKey)
                };

                return this.ExecuteReader(spName, parameters);
            }
            catch (Exception ex)
            {
                throw ex.Handle("QuerySessionInfo", criteria);
            }
        }

        /// <summary>
        /// Gets the session information by token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>SessionInfo.</returns>
        public SessionInfo GetSessionInfoByToken(string token)
        {
            const string spName = "sp_GetSessionInfoByToken";

            try
            {
                token.CheckEmptyString("token");

                var parameters = new List<SqlParameter>
                {
                    this.GenerateSqlSpParameter(column_Token, token)
                };

                return this.ExecuteReader(spName, parameters).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex.Handle("GetSessionInfoByToken", token);
            }
        }

        /// <summary>
        /// Cleans the session information.
        /// <remarks>
        /// Records whose <c>CreatedStamp</c> and <c>ExpiredStamp</c> are all earlier than specific <c>stamp</c> would be deleted physically.
        /// </remarks>
        /// </summary>
        /// <param name="stamp">The stamp.</param>
        /// <returns>System.Int32.</returns>
        public void CleanSessionInfo(DateTime? stamp)
        {
            const string spName = "sp_CleanSessionInfo";

            try
            {
                stamp.CheckNullObject("stamp");

                var parameters = new List<SqlParameter>
                {
                    this.GenerateSqlSpParameter("Stamp",stamp)
                };

                this.ExecuteNonQuery(spName, parameters);
            }
            catch (Exception ex)
            {
                throw ex.Handle("CleanSessionInfo");
            }
        }
    }
}
