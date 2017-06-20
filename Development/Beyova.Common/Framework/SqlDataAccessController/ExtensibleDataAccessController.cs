using System.Data.SqlClient;

namespace Beyova
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ExtensibleDataAccessController<T> : BaseDataAccessController<T>
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtensibleDataAccessController{T}"/> class.
        /// </summary>
        protected ExtensibleDataAccessController()
            : this(Framework.PrimarySqlConnection)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseDataAccessController{T}" /> class.
        /// </summary>
        /// <param name="primarySqlConnectionString">The primary SQL connection string.</param>
        /// <param name="readOnlySqlConnectionString">The read only SQL connection string.</param>
        protected ExtensibleDataAccessController(string primarySqlConnectionString, string readOnlySqlConnectionString = null)
            : base(primarySqlConnectionString, readOnlySqlConnectionString)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Beyova.SqlDataAccessController`1" /> class.
        /// </summary>
        /// <param name="primarySqlConnection">The SQL connection.</param>
        /// <param name="readOnlySqlConnection">The read only SQL connection.</param>
        protected ExtensibleDataAccessController(SqlConnection primarySqlConnection, SqlConnection readOnlySqlConnection = null)
            : base(primarySqlConnection, readOnlySqlConnection)
        {
        }

        #endregion Constructor
    }
}