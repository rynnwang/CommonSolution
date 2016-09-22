using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Beyova;

namespace Beyova.CommonService.DataAccessController
{
    /// <summary>
    /// Access controller for <see cref="AccessCredentialInfo" /> class instance.
    /// </summary>
    public class AccessCredentialInfoAccessController : BaseCommonServiceController<AccessCredentialInfo, AccessCredentialCriteria>
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="AccessCredentialInfoAccessController" /> class.
        /// </summary>
        public AccessCredentialInfoAccessController()
            : base()
        {
        }

        #endregion

        /// <summary>
        /// Converts the entity object.
        /// </summary>
        /// <param name="sqlDataReader">The SQL data reader.</param>
        /// <returns>AccessCredentialInfo.</returns>
        protected override AccessCredentialInfo ConvertEntityObject(SqlDataReader sqlDataReader)
        {
            var result = new AccessCredentialInfo
            {
                UserKey = sqlDataReader[column_UserKey].ObjectToGuid(),
                AccessIdentifier = sqlDataReader[column_AccessIdentifier].ObjectToString(),
                Domain = sqlDataReader[column_Domain].ObjectToString(),
                Token = sqlDataReader[column_Token].ObjectToString(),
                TokenExpiredStamp = sqlDataReader[column_TokenExpiredStamp].ObjectToDateTime()
            };

            FillSimpleBaseObjectFields(result, sqlDataReader);

            return result;
        }

        ///// <summary>
        ///// Thirds the party login.
        ///// </summary>
        ///// <param name="accessCredential">The access credential.</param>
        ///// <returns>AccessCredentialInfo.</returns>
        //public AccessCredentialInfo ThirdPartyLogin(AccessCredentialInfo accessCredential)
        //{
        //    const string spName = "sp_ThirdPartyLogin";

        //    try
        //    {
        //        accessCredential.CheckNullObject("accessCredential");

        //        var parameters = new List<SqlParameter>
        //        {
        //            this.GenerateSqlSpParameter(column_AccessIdentifier, accessCredential.AccessIdentifier),
        //            this.GenerateSqlSpParameter(column_Domain, accessCredential.Domain),
        //            this.GenerateSqlSpParameter(column_Token, accessCredential.Token),
        //            this.GenerateSqlSpParameter(column_TokenExpiredStamp, accessCredential.TokenExpiredStamp),
        //            this.GenerateSqlSpParameter(column_UserKey, accessCredential.UserKey)
        //        };

        //        return this.ExecuteReader(spName, parameters).FirstOrDefault();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex.Handle( accessCredential);
        //    }
        //}

        /// <summary>
        /// Authenticates the access credential.
        /// </summary>
        /// <param name="accessCredential">The access credential.</param>
        public AccessCredentialInfo AuthenticateAccessCredential(AccessCredential accessCredential)
        {
            const string spName = "sp_AuthenticateAccessCredential";

            try
            {
                accessCredential.CheckNullObject("accessCredential");

                var parameters = new List<SqlParameter>
                {
                    this.GenerateSqlSpParameter(column_AccessIdentifier, accessCredential.AccessIdentifier),
                    this.GenerateSqlSpParameter(column_Domain, accessCredential.Domain),
                    this.GenerateSqlSpParameter(column_Token, HashPassword(accessCredential.Token))
                };

                return this.ExecuteReader(spName, parameters).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex.Handle( accessCredential);
            }
        }

        /// <summary>
        /// Queries the access credential.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;AccessCredentialInfo&gt;.</returns>
        public List<AccessCredentialInfo> QueryAccessCredential(AccessCredentialCriteria criteria)
        {
            const string spName = "sp_QueryAccessCredential";

            try
            {
                criteria.CheckNullObject("criteria");

                var parameters = new List<SqlParameter>
                {
                    this.GenerateSqlSpParameter(column_UserKey, criteria.UserKey),
                    this.GenerateSqlSpParameter(column_AccessIdentifier, criteria.AccessIdentifier),
                    this.GenerateSqlSpParameter(column_Domain, criteria.Domain),
                    this.GenerateSqlSpParameter(column_Token, criteria.Token),
                    this.GenerateSqlSpParameter(column_Count, 500)
                };

                return this.ExecuteReader(spName, parameters);
            }
            catch (Exception ex)
            {
                throw ex.Handle( criteria);
            }
        }

        ///// <summary>
        ///// Binds the access credential on account.
        ///// </summary>
        ///// <param name="accessCredential">The access credential.</param>
        ///// <param name="operatorKey">The operator key.</param>
        ///// <returns></returns>
        //public List<AccessCredentialInfo> BindThirdPartyAccessCredentialOnAccount(AccessCredential accessCredential, Guid? operatorKey)
        //{
        //    const string spName = "sp_BindAccessCredentialOnAccount";

        //    try
        //    {
        //        accessCredential.CheckNullObject("accessCredential");
        //        operatorKey.CheckNullObject("operatorKey");
        //        accessCredential.Domain.CheckEmptyString("accessCredential.Domain");

        //        var parameters = new List<SqlParameter>
        //        {
        //            this.GenerateSqlSpParameter(column_AccessIdentifier, accessCredential.AccessIdentifier),
        //            this.GenerateSqlSpParameter(column_Domain, accessCredential.Domain),
        //            this.GenerateSqlSpParameter(column_Token, accessCredential.Token),
        //            this.GenerateSqlSpParameter(column_TokenExpiredStamp, null),
        //            this.GenerateSqlSpParameter(column_OperatorKey, operatorKey),
        //        };

        //        return this.ExecuteReader(spName, parameters);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex.Handle( new { operatorKey, accessCredential });
        //    }
        //}

        /// <summary>
        /// Hashes the password.
        /// </summary>
        /// <param name="rawPassword">The raw password.</param>
        /// <returns>System.String.</returns>
        protected string HashPassword(string rawPassword)
        {
            return rawPassword.SafeToString().ToSHA1(Encoding.UTF8);
        }
    }
}