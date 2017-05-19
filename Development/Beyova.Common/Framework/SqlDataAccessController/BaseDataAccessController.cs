using System.Collections.Generic;
using System.Data.SqlClient;

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
        /// The column_ culture code
        /// </summary>
        protected const string column_CultureCode = "CultureCode";

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
        /// The column_ name
        /// </summary>
        protected const string column_Name = "Name";

        /// <summary>
        /// The column_ token
        /// </summary>
        protected const string column_Token = "Token";

        /// <summary>
        /// The SQL connection
        /// </summary>
        private const string configurationKey_sqlConnection = "SqlConnection";

        /// <summary>
        /// The column_ ip address
        /// </summary>
        protected const string column_IpAddress = "IpAddress";

        /// <summary>
        /// The column_ user agent
        /// </summary>
        protected const string column_UserAgent = "UserAgent";

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
        /// The column_ platform
        /// </summary>
        protected const string column_Platform = "Platform";

        /// <summary>
        /// The column platform type
        /// </summary>
        protected const string column_PlatformType = "PlatformType";

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
        /// The column_ third party identifier
        /// </summary>
        protected const string column_ThirdPartyId = "ThirdPartyId";

        /// <summary>
        /// The column_ parent key
        /// </summary>
        protected const string column_ParentKey = "ParentKey";

        /// <summary>
        /// The column_ avatar key
        /// </summary>
        protected const string column_AvatarKey = "AvatarKey";

        /// <summary>
        /// The column_ is expired
        /// </summary>
        protected const string column_IsExpired = "IsExpired";

        /// <summary>
        /// The column_ sequence
        /// </summary>
        protected const string column_Sequence = "Sequence";

        /// <summary>
        /// The column_ stamp
        /// </summary>
        protected const string column_Stamp = "Stamp";

        /// <summary>
        /// The column_ content
        /// </summary>
        protected const string column_Content = "Content";

        /// <summary>
        /// The column_ content type
        /// </summary>
        protected const string column_ContentType = "ContentType";

        /// <summary>
        /// The column_ tags
        /// </summary>
        protected const string column_Tags = "Tags";

        /// <summary>
        /// The column_ public key
        /// </summary>
        protected const string column_PublicKey = "PublicKey";

        /// <summary>
        /// The column_ private key
        /// </summary>
        protected const string column_PrivateKey = "PrivateKey";

        /// <summary>
        /// The column_ snapshot key
        /// </summary>
        protected const string column_SnapshotKey = "SnapshotKey";

        /// <summary>
        /// The column_ project key
        /// </summary>
        protected const string column_ProjectKey = "ProjectKey";

        /// <summary>
        /// The column platform key
        /// </summary>
        protected const string column_PlatformKey = "PlatformKey";

        /// <summary>
        /// The column URL
        /// </summary>
        protected const string column_Url= "Url";

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseDataAccessController{T}"/> class.
        /// </summary>
        protected BaseDataAccessController()
            : this(Framework.GetConfiguration(configurationKey_sqlConnection))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseDataAccessController{T}" /> class.
        /// </summary>
        /// <param name="primarySqlConnectionString">The primary SQL connection string.</param>
        /// <param name="readOnlySqlConnectionString">The read only SQL connection string.</param>
        protected BaseDataAccessController(string primarySqlConnectionString, string readOnlySqlConnectionString = null)
            : base(primarySqlConnectionString, readOnlySqlConnectionString)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Beyova.SqlDataAccessController`1" /> class.
        /// </summary>
        /// <param name="primarySqlConnection">The SQL connection.</param>
        /// <param name="readOnlySqlConnection">The read only SQL connection.</param>
        protected BaseDataAccessController(SqlConnection primarySqlConnection, SqlConnection readOnlySqlConnection = null)
            : base(primarySqlConnection, readOnlySqlConnection)
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

        /// <summary>
        /// Executes the reader as base object.
        /// </summary>
        /// <param name="spName">Name of the sp.</param>
        /// <param name="sqlParameters">The SQL parameters.</param>
        /// <param name="preferReadOnlyOperator">if set to <c>true</c> [prefer read only operator].</param>
        /// <param name="ignoreOperator">if set to <c>true</c> [ignore operator].</param>
        /// <returns>List&lt;BaseObject&lt;T&gt;&gt;.</returns>
        protected List<BaseObject<T>> ExecuteReaderAsBaseObject(string spName, List<SqlParameter> sqlParameters = null, bool preferReadOnlyOperator = false, bool ignoreOperator = false)
        {
            return ExecuteReader<BaseObject<T>>(spName, sqlParameters, (reader) =>
            {
                var result = new BaseObject<T>(ConvertEntityObject(reader));
                FillBaseObjectFields(result, reader, ignoreOperator);
                return result;
            }, preferReadOnlyOperator);
        }

        /// <summary>
        /// Executes the reader as simple base object.
        /// </summary>
        /// <param name="spName">Name of the sp.</param>
        /// <param name="sqlParameters">The SQL parameters.</param>
        /// <param name="preferReadOnlyOperator">if set to <c>true</c> [prefer read only operator].</param>
        /// <returns>List&lt;SimpleBaseObject&lt;T&gt;&gt;.</returns>
        protected List<SimpleBaseObject<T>> ExecuteReaderAsSimpleBaseObject(string spName, List<SqlParameter> sqlParameters = null, bool preferReadOnlyOperator = false)
        {
            return ExecuteReader<SimpleBaseObject<T>>(spName, sqlParameters, (reader) =>
            {
                var result = new SimpleBaseObject<T>(ConvertEntityObject(reader));
                FillSimpleBaseObjectFields(result, reader);
                return result;
            }, preferReadOnlyOperator);
        }
    }
}
