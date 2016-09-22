using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Beyova;

namespace Beyova.CommonService.DataAccessController
{
    /// <summary>
    /// Class BaseAuthenticationController.
    /// </summary>
    /// <typeparam name="TEntity">The type of the t entity.</typeparam>
    /// <typeparam name="TCriteria">The type of the t criteria.</typeparam>
    public abstract class BaseAuthenticationController<TEntity, TCriteria> : BaseCommonServiceController<TEntity, TCriteria>
        where TEntity : new()
    {
        #region Database Constants

        /// <summary>
        /// The column_ callback URL
        /// </summary>
        protected const string column_CallbackUrl = "CallbackUrl";

        /// <summary>
        /// The column_ token expiration
        /// </summary>
        protected const string column_TokenExpiration = "TokenExpiration";

        /// <summary>
        /// The column_ active only
        /// </summary>
        protected const string column_ActiveOnly = "ActiveOnly";

        /// <summary>
        /// The column_ Partner key
        /// </summary>
        protected const string column_PartnerKey = "PartnerKey";

        /// <summary>
        /// The column_ client request identifier
        /// </summary>
        protected const string column_ClientRequestId = "ClientRequestId";

        /// <summary>
        /// The column_ authorization token
        /// </summary>
        protected const string column_AuthorizationToken = "AuthorizationToken";

        /// <summary>
        /// The column_ used stamp
        /// </summary>
        protected const string column_UsedStamp = "UsedStamp";

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Beyova.CommonService.DataAccessController.BaseCommonServiceController`2" /> class.
        /// </summary>
        /// <param name="sqlConnection">The SQL connection.</param>
        public BaseAuthenticationController(string sqlConnection)
            : base(sqlConnection)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseAuthenticationController{TEntity, TCriteria}"/> class.
        /// </summary>
        public BaseAuthenticationController()
            : base()
        {
        }

        #endregion

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