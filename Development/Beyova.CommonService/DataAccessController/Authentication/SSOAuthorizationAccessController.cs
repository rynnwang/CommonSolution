using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Beyova.CommonService.DataAccessController
{
    /// <summary>
    /// Class SSOAuthorizationPartnerAccessController.
    /// </summary>
    public class SSOAuthorizationAccessController : SSOAuthorizationAccessController<SSOAuthorization, SSOAuthorizationCriteria>
    {
    }

    /// <summary>
    /// Class SSOAuthorizationAccessController.
    /// </summary>
    /// <typeparam name="TSSOAuthorization">The type of the sso authorization.</typeparam>
    /// <typeparam name="TSSOAuthorizationCriteria">The type of the sso authorization criteria.</typeparam>
    public class SSOAuthorizationAccessController<TSSOAuthorization, TSSOAuthorizationCriteria> : BaseAuthenticationController<TSSOAuthorization, TSSOAuthorizationCriteria>
        where TSSOAuthorization : SSOAuthorization, new()
        where TSSOAuthorizationCriteria : SSOAuthorizationCriteria, new()
    {
        /// <summary>
        /// The column_ is used
        /// </summary>
        protected const string column_IsUsed = "IsUsed";

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SSOAuthorizationAccessController" /> class.
        /// </summary>
        public SSOAuthorizationAccessController()
            : base()
        {
        }

        #endregion

        /// <summary>
        /// Converts the entity object.
        /// </summary>
        /// <param name="sqlDataReader">The SQL data reader.</param>
        /// <returns>AccessCredentialInfo.</returns>
        protected override TSSOAuthorization ConvertEntityObject(SqlDataReader sqlDataReader)
        {
            var result = new TSSOAuthorization
            {
                Key = sqlDataReader[column_Key].ObjectToGuid(),
                PartnerKey = sqlDataReader[column_PartnerKey].ObjectToGuid(),
                ClientRequestId = sqlDataReader[column_ClientRequestId].ObjectToString(),
                AuthorizationToken = sqlDataReader[column_AuthorizationToken].ObjectToString(),
                CallbackUrl = sqlDataReader[column_CallbackUrl].ObjectToString(),
                ExpiredStamp = sqlDataReader[column_ExpiredStamp].ObjectToDateTime(),
                UsedStamp = sqlDataReader[column_UsedStamp].ObjectToDateTime(),
                UserKey = sqlDataReader[column_UserKey].ObjectToGuid()
            };

            this.FillAdditionalFieldValue(result, sqlDataReader);
            FillSimpleBaseObjectFields(result, sqlDataReader);

            return result;
        }

        /// <summary>
        /// Requests the sso token exchange.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>TSSOAuthorization.</returns>
        public TSSOAuthorization RequestSSOTokenExchange(SSOAuthorizationRequest request)
        {
            const string spName = "sp_RequestTokenExchange";

            try
            {
                request.CheckNullObject(nameof(request));
                request.ClientRequestId.CheckEmptyString("request.ClientRequestId");
                request.UserKey.CheckNullObject("request.UserKey");
                request.PartnerKey.CheckNullObject("request.PartnerKey");

                var parameters = new List<SqlParameter>
                {
                    this.GenerateSqlSpParameter(column_PartnerKey, request.PartnerKey),
                    this.GenerateSqlSpParameter(column_ClientRequestId, request.ClientRequestId),
                    this.GenerateSqlSpParameter(column_UserKey, request.UserKey)
                };

                return this.ExecuteReader(spName, parameters).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex.Handle(request);
            }
        }

        /// <summary>
        /// Requests the o authentication.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>TSSOAuthorization.</returns>
        public TSSOAuthorization RequestOAuth(SSOAuthorizationRequest request)
        {
            const string spName = "sp_RequestOAuth";

            try
            {
                request.CheckNullObject(nameof(request));
                request.ClientRequestId.CheckEmptyString("request.ClientRequestId");
                request.CallbackUrl.CheckEmptyString("request.CallbackUrl");

                var parameters = new List<SqlParameter>
                {
                    this.GenerateSqlSpParameter(column_PartnerKey, request.PartnerKey),
                    this.GenerateSqlSpParameter(column_ClientRequestId, request.ClientRequestId),
                    this.GenerateSqlSpParameter(column_CallbackUrl, request.CallbackUrl)
                };

                return this.ExecuteReader(spName, parameters).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex.Handle(request);
            }
        }

        /// <summary>
        /// Queries the sso authorization.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>TSSOAuthorization.</returns>
        public List<TSSOAuthorization> QuerySSOAuthorization(TSSOAuthorizationCriteria criteria)
        {
            const string spName = "sp_QuerySSOAuthorization";

            try
            {
                criteria.CheckNullObject(nameof(criteria));

                var parameters = new List<SqlParameter>
                {
                    this.GenerateSqlSpParameter(column_Key, criteria.Key),
                    this.GenerateSqlSpParameter(column_PartnerKey, criteria.PartnerKey),
                    this.GenerateSqlSpParameter(column_UserKey, criteria.UserKey),
                    this.GenerateSqlSpParameter(column_AuthorizationToken, criteria.AuthorizationToken),
                    this.GenerateSqlSpParameter(column_IsUsed, criteria.IsUsed),
                    this.GenerateSqlSpParameter(column_StartIndex, criteria.StartIndex),
                    this.GenerateSqlSpParameter(column_Count, criteria.Count)
                };

                FillAdditionalFieldValue(parameters, criteria);

                return this.ExecuteReader(spName, parameters);
            }
            catch (Exception ex)
            {
                throw ex.Handle(criteria);
            }
        }
    }
}