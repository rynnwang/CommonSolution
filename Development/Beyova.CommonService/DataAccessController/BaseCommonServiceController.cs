using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Beyova;
using Beyova.ExceptionSystem;

namespace Beyova.CommonService.DataAccessController
{
    /// <summary>
    /// Class BaseCommonServiceController.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseCommonServiceController<TEntity, TCriteria> : BaseDataAccessController<TEntity>
        where TEntity : new()
    {
        #region Database Constants

        /// <summary>
        /// The column_ token expired stamp
        /// </summary>
        protected const string column_TokenExpiredStamp = "TokenExpiredStamp";

        /// <summary>
        /// The column_ access identifier
        /// </summary>
        protected const string column_AccessIdentifier = "AccessIdentifier";

        /// <summary>
        /// The column_ functional role
        /// </summary>
        protected const string column_FunctionalRole = "FunctionalRole";

        /// <summary>
        /// The column_ permission
        /// </summary>
        protected const string column_Permission = "Permission";

        /// <summary>
        /// The column_ user name
        /// </summary>
        protected const string column_UserName = "UserName";

        /// <summary>
        /// The column_ email
        /// </summary>
        protected const string column_Email = "Email";

        /// <summary>
        /// The column_ domain
        /// </summary>
        protected const string column_Domain = "Domain";

        /// <summary>
        /// The column_ description
        /// </summary>
        protected const string column_Description = "Description";

        /// <summary>
        /// The column_ start index
        /// </summary>
        protected const string column_StartIndex = "StartIndex";

        /// <summary>
        /// The column_ expired stamp
        /// </summary>
        protected const string column_ExpiredStamp = "ExpiredStamp";

        /// <summary>
        /// The column_ token
        /// </summary>
        protected const string column_Token = "Token";

        /// <summary>
        /// The column_ user agent
        /// </summary>
        protected const string column_UserAgent = "UserAgent";

        /// <summary>
        /// The column_ ip address
        /// </summary>
        protected const string column_IpAddress = "IpAddress";

        /// <summary>
        /// The column_ platform
        /// </summary>
        protected const string column_Platform = "Platform";

        /// <summary>
        /// The column_ device type
        /// </summary>
        protected const string column_DeviceType = "DeviceType";

        /// <summary>
        /// The column_ expiration
        /// </summary>
        protected const string column_Expiration = "Expiration";

        /// <summary>
        /// The column_ time zone
        /// </summary>
        protected const string column_TimeZone = "TimeZone";

        /// <summary>
        /// The column_ language
        /// </summary>
        protected const string column_Language = "Language";

        /// <summary>
        /// The column_ avatar key
        /// </summary>
        protected const string column_AvatarKey = "AvatarKey";

        /// <summary>
        /// The column_ gender
        /// </summary>
        protected const string column_Gender = "Gender";

        /// <summary>
        /// The column_ name
        /// </summary>
        protected const string column_Name = "Name";

        /// <summary>
        /// The column_ avatar URL
        /// </summary>
        protected const string column_AvatarUrl = "AvatarUrl";

        /// <summary>
        /// The column_ container
        /// </summary>
        protected const string column_Container = "Container";

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCommonServiceController{TEntity,TCriteria}" /> class.
        /// </summary>
        public BaseCommonServiceController()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCommonServiceController{TEntity,TCriteria}" /> class.
        /// </summary>
        /// <param name="sqlConnection">The SQL connection.</param>
        public BaseCommonServiceController(string sqlConnection)
           : base(sqlConnection)
        {
        }

        #endregion

        /// <summary>
        /// Fills the additional field value.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="entity">The entity.</param>
        protected virtual void FillAdditionalFieldValue(List<SqlParameter> parameters, TEntity entity) { }

        /// <summary>
        /// Fills the additional field value.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="criteria">The criteria.</param>
        protected virtual void FillAdditionalFieldValue(List<SqlParameter> parameters, TCriteria criteria) { }

        /// <summary>
        /// Fills the additional field value.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="reader">The reader.</param>
        protected virtual void FillAdditionalFieldValue(TEntity entity, SqlDataReader reader) { }
    }
}