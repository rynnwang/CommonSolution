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
    public class SessionInfoAccessController : BaseAuthenticationController<SessionInfo, SessionCriteria>
    {
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
                throw ex.Handle(new { token });
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
                throw ex.Handle(new { sessionInfo });
            }
        }

        /// <summary>
        /// Exchanges the sso authorization.
        /// </summary>
        /// <param name="authorization">The authorization.</param>
        /// <returns>SessionInfo.</returns>
        public SessionInfo ExchangeSSOAuthorization(SSOAuthorizationBase authorization)
        {
            const string spName = "sp_ExchangeToken";

            try
            {
                authorization.CheckNullObject(nameof(authorization));

                var parameters = new List<SqlParameter>
                {
                    this.GenerateSqlSpParameter(column_ClientRequestId,authorization.ClientRequestId),
                    this.GenerateSqlSpParameter(column_AuthorizationToken,authorization.AuthorizationToken)
                };

                return this.ExecuteReader(spName, parameters).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { authorization });
            }
        }

        /// <summary>
        /// Queries the session information.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;SessionInfo&gt;.</returns>
        public List<SessionInfo> QuerySessionInfo(SessionCriteria criteria)
        {
            const string spName = "sp_QuerySessionInfo";

            try
            {
                criteria.CheckNullObject("criteria");

                var parameters = new List<SqlParameter>
                {
                    this.GenerateSqlSpParameter(column_UserKey, criteria.UserKey),
                    this.GenerateSqlSpParameter(column_Token, criteria.Token),
                    this.GenerateSqlSpParameter(column_UserAgent, criteria.UserAgent),
                    this.GenerateSqlSpParameter(column_IpAddress, criteria.IpAddress),
                    this.GenerateSqlSpParameter(column_Platform, criteria.Platform.EnumToInt32()),
                    this.GenerateSqlSpParameter(column_DeviceType, criteria.DeviceType.EnumToInt32()),
                    this.GenerateSqlSpParameter(column_IsExpired, criteria.IsExpired),
                    this.GenerateSqlSpParameter(column_Count, criteria.Count)
                };

                return this.ExecuteReader(spName, parameters);
            }
            catch (Exception ex)
            {
                throw ex.Handle(criteria);
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
                token.CheckEmptyString(nameof(token));

                var parameters = new List<SqlParameter>
                {
                    this.GenerateSqlSpParameter(column_Token, token)
                };

                return this.ExecuteReader(spName, parameters).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex.Handle(token);
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
                stamp.CheckNullObject(nameof(stamp));

                var parameters = new List<SqlParameter>
                {
                    this.GenerateSqlSpParameter("Stamp",stamp)
                };

                this.ExecuteNonQuery(spName, parameters);
            }
            catch (Exception ex)
            {
                throw ex.Handle(stamp);
            }
        }
    }
}
