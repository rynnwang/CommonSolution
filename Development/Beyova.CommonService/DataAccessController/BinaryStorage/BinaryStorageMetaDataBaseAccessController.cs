using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Beyova;

namespace Beyova.CommonService.DataAccessController
{
    /// <summary>
    /// Class BinaryStorageMetaDataBaseAccessController.
    /// </summary>
    public abstract class BinaryStorageMetaDataBaseAccessController<TBinaryStorageMetaEntity, TBinaryStorageMetaCriteria> : BaseCommonServiceController<TBinaryStorageMetaEntity, TBinaryStorageMetaCriteria>
        where TBinaryStorageMetaEntity : BinaryStorageMetaData, new()
        where TBinaryStorageMetaCriteria : BinaryStorageMetaDataCriteria, new()
    {
        #region Constants

        /// <summary>
        /// The column_ MIME
        /// </summary>
        protected const string column_Mime = "Mime";

        /// <summary>
        /// The column_ length
        /// </summary>
        protected const string column_Length = "Length";

        /// <summary>
        /// The column_ minimum length
        /// </summary>
        protected const string column_MinLength = "MinLength";

        /// <summary>
        /// The column_ maximum length
        /// </summary>
        protected const string column_MaxLength = "MaxLength";

        /// <summary>
        /// The column_ meta data
        /// </summary>
        protected const string column_MetaData = "MetaData";

        /// <summary>
        /// The column_ height
        /// </summary>
        protected const string column_Height = "Height";

        /// <summary>
        /// The column_ minimum height
        /// </summary>
        protected const string column_MinHeight = "MinHeight";

        /// <summary>
        /// The column_ maximum height
        /// </summary>
        protected const string column_MaxHeight = "MaxHeight";

        /// <summary>
        /// The column_ width
        /// </summary>
        protected const string column_Width = "Width";

        /// <summary>
        /// The column_ minimum width
        /// </summary>
        protected const string column_MinWidth = "MinWidth";

        /// <summary>
        /// The column_ maximum width
        /// </summary>
        protected const string column_MaxWidth = "MaxWidth";

        /// <summary>
        /// The column_ duration
        /// </summary>
        protected const string column_Duration = "Duration";

        /// <summary>
        /// The column_ minimum duration
        /// </summary>
        protected const string column_MinDuration = "MinDuration";

        /// <summary>
        /// The column_ maximum duration
        /// </summary>
        protected const string column_MaxDuration = "MaxDuration";

        /// <summary>
        /// The column_ identifiers
        /// </summary>
        protected const string column_Identifiers = "Identifiers";

        /// <summary>
        /// The column_ commit option
        /// </summary>
        protected const string column_CommitOption = "CommitOption";

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryStorageMetaDataBaseAccessController{TMetaEntity, TMetaCriteria}" /> class.
        /// </summary>
        public BinaryStorageMetaDataBaseAccessController()
            : base()
        {
        }

        #endregion

        /// <summary>
        /// Converts the entity object.
        /// </summary>
        /// <param name="sqlDataReader">The SQL data reader.</param>
        /// <returns>BinaryStorageMetaData.</returns>
        protected override TBinaryStorageMetaEntity ConvertEntityObject(SqlDataReader sqlDataReader)
        {
            var result = new TBinaryStorageMetaEntity
            {
                Container = sqlDataReader[column_Container].ObjectToString(),
                Identifier = sqlDataReader[column_Identifier].ObjectToString(),
                Name = sqlDataReader[column_Name].ObjectToString(),
                Mime = sqlDataReader[column_Mime].ObjectToString(),
                Hash = sqlDataReader[column_Hash].ObjectToString(),
                Length = sqlDataReader[column_Length].ObjectToNullableInt32(),
                Height = sqlDataReader[column_Height].ObjectToNullableInt32(),
                Width = sqlDataReader[column_Width].ObjectToNullableInt32(),
                Duration = sqlDataReader[column_Duration].ObjectToNullableInt32(),
                CreatedStamp = sqlDataReader[column_CreatedStamp].ObjectToDateTime(DateTime.UtcNow),
                LastUpdatedStamp = sqlDataReader[column_LastUpdatedStamp].ObjectToDateTime(DateTime.UtcNow),
                State = (BinaryStorageState)sqlDataReader[column_State].ObjectToInt32()
            };

            FillAdditionalFieldValue(result, sqlDataReader);

            return result;
        }

        /// <summary>
        /// Creates the binary storage meta data.
        /// </summary>
        /// <param name="metaData">The meta data.</param>
        /// <param name="operatorKey">The operator key.</param>
        /// <returns>TBinaryStorageMetaEntity.</returns>
        public TBinaryStorageMetaEntity CreateBinaryStorageMetaData(TBinaryStorageMetaEntity metaData, Guid? operatorKey)
        {
            const string spName = "sp_CreateBinaryStorageMetaData";

            try
            {
                metaData.CheckNullObject("metaData");
                operatorKey.CheckNullObject("operatorKey");

                var parameters = new List<SqlParameter>
                {
                    GenerateSqlSpParameter(column_Container, metaData.Container),
                    GenerateSqlSpParameter(column_Identifier, metaData.Identifier),
                    GenerateSqlSpParameter(column_Name, metaData.Name),
                    GenerateSqlSpParameter(column_Mime, metaData.Mime),
                    GenerateSqlSpParameter(column_Height, metaData.Height),
                    GenerateSqlSpParameter(column_Width, metaData.Width),
                    GenerateSqlSpParameter(column_Duration, metaData.Duration),
                    GenerateSqlSpParameter(column_OperatorKey, operatorKey),
                };

                FillAdditionalFieldValue(parameters, metaData);

                return this.ExecuteReader(spName, parameters).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { metaData, operatorKey });
            }
        }

        /// <summary>
        /// Commits the binary storage.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="mime">The MIME.</param>
        /// <param name="hash">The hash.</param>
        /// <param name="length">The length.</param>
        /// <param name="operatorKey">The operator key.</param>
        /// <returns>TBinaryStorageMetaEntity.</returns>
        public TBinaryStorageMetaEntity CommitBinaryStorage(BinaryStorageCommitRequest request, string mime, string hash, long length, Guid? operatorKey)
        {
            const string spName = "sp_CommitBinaryStorage";

            try
            {
                request.CheckNullObject("request");
                operatorKey.CheckNullObject("operatorKey");

                var parameters = new List<SqlParameter>
                {
                    GenerateSqlSpParameter(column_Container, request.Container),
                    GenerateSqlSpParameter(column_Identifier, request.Identifier),
                    GenerateSqlSpParameter(column_Mime,mime),
                    GenerateSqlSpParameter(column_Hash,hash),
                    GenerateSqlSpParameter(column_Length, length),
                    GenerateSqlSpParameter(column_CommitOption,request.CommitOption.EnumToInt32()),
                    GenerateSqlSpParameter(column_OperatorKey, operatorKey),
                };

                return this.ExecuteReader(spName, parameters).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { request, operatorKey });
            }
        }

        /// <summary>
        /// Queries the binary storage meta data.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;BinaryStorageMetaData&gt;.</returns>
        public List<TBinaryStorageMetaEntity> QueryBinaryStorageMetaData(TBinaryStorageMetaCriteria criteria)
        {
            const string spName = "sp_QueryBinaryStorageMetaData";

            try
            {
                var parameters = new List<SqlParameter>
                {
                    GenerateSqlSpParameter(column_Container, criteria.Container),
                    GenerateSqlSpParameter(column_Identifier, criteria.Identifier),
                    GenerateSqlSpParameter(column_Name, criteria.Name),
                    GenerateSqlSpParameter(column_Mime, criteria.Mime),
                    GenerateSqlSpParameter(column_Hash, criteria.Hash),
                    GenerateSqlSpParameter(column_MinLength, criteria.MinLength),
                    GenerateSqlSpParameter(column_MaxLength, criteria.MaxLength),
                    GenerateSqlSpParameter(column_MinHeight, criteria.MinHeight),
                    GenerateSqlSpParameter(column_MaxHeight, criteria.MaxHeight),
                    GenerateSqlSpParameter(column_MinWidth, criteria.MinWidth),
                    GenerateSqlSpParameter(column_MaxWidth, criteria.MaxWidth),
                    GenerateSqlSpParameter(column_MinDuration, criteria.MinDuration),
                    GenerateSqlSpParameter(column_MaxDuration, criteria.MaxDuration),
                    GenerateSqlSpParameter(column_FromStamp, criteria.FromStamp),
                    GenerateSqlSpParameter(column_ToStamp, criteria.ToStamp),
                    GenerateSqlSpParameter(column_Count, criteria.Count)
                };

                FillAdditionalFieldValue(parameters, criteria);

                return this.ExecuteReader(spName, parameters);
            }
            catch (Exception ex)
            {
                throw ex.Handle(criteria);
            }
        }

        /// <summary>
        /// Queries the user binary storage meta data.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;TBinaryStorageMetaEntity&gt;.</returns>
        public List<TBinaryStorageMetaEntity> QueryUserBinaryStorageMetaData(TBinaryStorageMetaCriteria criteria)
        {
            const string spName = "sp_QueryUserBinaryStorageMetaData";

            try
            {
                var parameters = new List<SqlParameter>
                {
                    GenerateSqlSpParameter(column_Container, criteria.Container),
                    GenerateSqlSpParameter(column_Identifier, criteria.Identifier),
                    GenerateSqlSpParameter(column_Name, criteria.Name),
                    GenerateSqlSpParameter(column_Mime, criteria.Mime),
                    GenerateSqlSpParameter(column_Hash, criteria.Hash),
                    GenerateSqlSpParameter(column_MinLength, criteria.MinLength),
                    GenerateSqlSpParameter(column_MaxLength, criteria.MaxLength),
                    GenerateSqlSpParameter(column_MinHeight, criteria.MinHeight),
                    GenerateSqlSpParameter(column_MaxHeight, criteria.MaxHeight),
                    GenerateSqlSpParameter(column_MinWidth, criteria.MinWidth),
                    GenerateSqlSpParameter(column_MaxWidth, criteria.MaxWidth),
                    GenerateSqlSpParameter(column_MinDuration, criteria.MinDuration),
                    GenerateSqlSpParameter(column_MaxDuration, criteria.MaxDuration),
                    GenerateSqlSpParameter(column_FromStamp, criteria.FromStamp),
                    GenerateSqlSpParameter(column_ToStamp, criteria.ToStamp),
                    GenerateSqlSpParameter(column_OwnerKey, criteria.OwnerKey),
                    GenerateSqlSpParameter(column_Count, criteria.Count)
                };

                FillAdditionalFieldValue(parameters, criteria);

                return this.ExecuteReader(spName, parameters);
            }
            catch (Exception ex)
            {
                throw ex.Handle(criteria);
            }
        }

        /// <summary>
        /// Deletes the binary storage.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="operatorKey">The operator key.</param>
        public void DeleteBinaryStorage(Guid? identifier, Guid? operatorKey)
        {
            const string spName = "sp_DeleteBinaryStorage";

            try
            {
                identifier.CheckNullObject("identifier");
                operatorKey.CheckNullObject("operatorKey");

                var parameters = new List<SqlParameter>
                {
                    GenerateSqlSpParameter(column_Identifier, identifier),
                    GenerateSqlSpParameter(column_OperatorKey, operatorKey),
                };

                this.ExecuteNonQuery(spName, parameters);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { identifier, operatorKey });
            }
        }

        /// <summary>
        /// Gets the binary storage meta data by identifiers.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="identifiers">The identifiers.</param>
        /// <returns>List&lt;BinaryStorageMetaData&gt;.</returns>
        public List<TBinaryStorageMetaEntity> GetBinaryStorageMetaDataByIdentifiers<T>(List<T> identifiers) where T : BinaryStorageIdentifier
        {
            const string spName = "sp_GetBinaryStorageMetaDataByIdentifiers";

            try
            {
                identifiers.CheckNullObject("identifiers");

                var xml = "Identifier".CreateXml();
                foreach (var one in identifiers)
                {
                    var item = "Item".CreateXml();
                    item.SetAttributeValue(column_Container, one.Container);
                    item.SetValue(one.Identifier.SafeToString());

                    xml.Add(item);
                }

                var parameters = new List<SqlParameter>
                {
                    GenerateSqlSpParameter(column_Xml, xml),
                };

                return this.ExecuteReader(spName, parameters);
            }
            catch (Exception ex)
            {
                throw ex.Handle(identifiers);
            }
        }
    }
}
