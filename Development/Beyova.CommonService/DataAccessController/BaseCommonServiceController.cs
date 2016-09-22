using System.Collections.Generic;
using System.Data.SqlClient;

namespace Beyova.CommonService.DataAccessController
{
    /// <summary>
    /// Class BaseCommonServiceController.
    /// </summary>
    /// <typeparam name="TEntity">The type of the t entity.</typeparam>
    /// <typeparam name="TCriteria">The type of the t criteria.</typeparam>
    public abstract class BaseCommonServiceController<TEntity, TCriteria> : BaseDataAccessController<TEntity>
        where TEntity : new()
    {
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