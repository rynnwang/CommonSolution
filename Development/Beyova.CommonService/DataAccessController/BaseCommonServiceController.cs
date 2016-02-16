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
        /// The column_ permission
        /// </summary>
        protected const string column_Permission = "Permission";

        /// <summary>
        /// The column_ user name
        /// </summary>
        protected const string column_UserName = "UserName";

        /// <summary>
        /// The column_ gender
        /// </summary>
        protected const string column_Gender = "Gender";

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