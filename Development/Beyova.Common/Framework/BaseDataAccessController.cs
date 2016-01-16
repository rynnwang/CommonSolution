using System.Data.SqlClient;
using Beyova;

namespace Beyova
{
    /// <summary>
    /// Abstract class for SQL data access controller, which refers an entity in {T} type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseDataAccessController<T> : SqlDataAccessController<T>
    {
        #region Constants

        /// <summary>
        /// The column_ state
        /// </summary>
        protected const string column_State = "State";

        /// <summary>
        /// The column_ key
        /// </summary>
        protected const string column_Key = "Key";

        /// <summary>
        /// The column_ user key
        /// </summary>
        protected const string column_UserKey = "UserKey";

        /// <summary>
        /// The column_ owner key
        /// </summary>
        protected const string column_OwnerKey = "OwnerKey";

        /// <summary>
        /// The column_ operator key
        /// </summary>
        protected const string column_OperatorKey = "OperatorKey";

        /// <summary>
        /// The column_ created stamp
        /// </summary>
        protected const string column_CreatedStamp = "CreatedStamp";

        /// <summary>
        /// The column_ last updated stamp
        /// </summary>
        protected const string column_LastUpdatedStamp = "LastUpdatedStamp";

        /// <summary>
        /// The column_ count
        /// </summary>
        protected const string column_Count = "Count";

        /// <summary>
        /// The column_ data cursor
        /// </summary>
        protected const string column_DataCursor = "DataCursor";

        /// <summary>
        /// The column_ identifier
        /// </summary>
        protected const string column_Identifier = "Identifier";

        /// <summary>
        /// The column_ hash
        /// </summary>
        protected const string column_Hash = "Hash";

        /// <summary>
        /// The column_ from stamp
        /// </summary>
        protected const string column_FromStamp = "FromStamp";

        /// <summary>
        /// The column_ to stamp
        /// </summary>
        protected const string column_ToStamp = "ToStamp";

        /// <summary>
        /// The column_ type
        /// </summary>
        protected const string column_Type = "Type";

        /// <summary>
        /// The column_ code
        /// </summary>
        protected const string column_Code = "Code";

        /// <summary>
        /// The column_ order descending
        /// </summary>
        protected const string column_OrderDescending = "OrderDescending";

        /// <summary>
        /// The column_ time forwarding
        /// </summary>
        protected const string column_TimeForwarding = "TimeForwarding";

        /// <summary>
        /// The column_ XML
        /// </summary>
        protected const string column_Xml = "Xml";

        /// <summary>
        /// The column_ created by
        /// </summary>
        protected const string column_CreatedBy = "CreatedBy";

        /// <summary>
        /// The column_ last updated by
        /// </summary>
        protected const string column_LastUpdatedBy = "LastUpdatedBy";

        /// <summary>
        /// The SQL connection
        /// </summary>
        private const string configurationKey_sqlConnection = "SqlConnection";

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseDataAccessController{T}"/> class.
        /// </summary>
        protected BaseDataAccessController()
            : base(Framework.GetConfiguration(configurationKey_sqlConnection))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseDataAccessController{T}"/> class.
        /// </summary>
        /// <param name="sqlConnection">The SQL connection.</param>
        protected BaseDataAccessController(string sqlConnection)
            : base(sqlConnection)
        {
        }

        #endregion

        #region Fill interface based object.

        /// <summary>
        /// Fills the base object fields.
        /// </summary>
        /// <param name="baseObj">The base obj.</param>
        /// <param name="sqlDataReader">The SQL data reader.</param>
        /// <param name="ignoreOperator">if set to <c>true</c> [ignore operator].</param>
        protected static void FillBaseObjectFields(IBaseObject baseObj, SqlDataReader sqlDataReader, bool ignoreOperator = false)
        {
            if (baseObj != null)
            {
                FillSimpleBaseObjectFields(baseObj, sqlDataReader);
                if (!ignoreOperator)
                {
                    baseObj.CreatedBy = sqlDataReader[column_CreatedBy].ObjectToString();
                    baseObj.LastUpdatedBy = sqlDataReader[column_LastUpdatedBy].ObjectToString();
                }
            }
        }

        /// <summary>
        /// Fills the base simple object fields.
        /// </summary>
        /// <param name="baseObj">The base object.</param>
        /// <param name="sqlDataReader">The SQL data reader.</param>
        protected static void FillSimpleBaseObjectFields(ISimpleBaseObject baseObj, SqlDataReader sqlDataReader)
        {
            if (baseObj != null)
            {
                baseObj.Key = sqlDataReader[column_Key].ObjectToGuid();
                baseObj.CreatedStamp = sqlDataReader[column_CreatedStamp].ObjectToDateTime();
                baseObj.LastUpdatedStamp = sqlDataReader[column_LastUpdatedStamp].ObjectToDateTime();
                baseObj.State = (ObjectState)(sqlDataReader[column_State].ObjectToInt32());
            }
        }

        #endregion
    }
}
