using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Beyova;
using Beyova.BinaryStorage;

namespace Beyova.CommonService.DataAccessController
{
    /// <summary>
    /// Class BinaryStorageMetaDataBaseAccessController.
    /// </summary>
    public abstract class BinaryStorageMetaDataBaseAccessController<TMetaEntity, TMetaCriteria> : BaseCommonServiceController<TMetaEntity, TMetaCriteria>
        where TMetaEntity : BinaryStorageMetaData, new()
        where TMetaCriteria : BinaryStorageMetaDataCriteria, new()
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
        protected override TMetaEntity ConvertEntityObject(SqlDataReader sqlDataReader)
        {
            var result = new TMetaEntity
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
                OwnerKey = sqlDataReader[column_OwnerKey].ObjectToGuid(),
                CreatedStamp = sqlDataReader[column_CreatedStamp].ObjectToDateTime(DateTime.UtcNow),
                LastUpdatedStamp = sqlDataReader[column_LastUpdatedStamp].ObjectToDateTime(DateTime.UtcNow),
                State = (BinaryStorageState)sqlDataReader[column_State].ObjectToInt32()
            };

            FillAdditionalFieldValue(result, sqlDataReader);

            return result;
        }

        /// <summary>
        /// Creates the or update binary storage meta data.
        /// </summary>
        /// <param name="metaData">The meta data.</param>
        /// <param name="operatorKey">The operator key.</param>
        /// <returns>Beyova.BinaryStorage.BinaryStorageMetaData.</returns>
        public TMetaEntity CreateOrUpdateBinaryStorageMetaData(TMetaEntity metaData, Guid? operatorKey)
        {
            const string spName = "sp_CreateOrUpdateBinaryStorageMetaData";

            try
            {
                metaData.CheckNullObject("metaData");

                var parameters = new List<SqlParameter>
                {
                    GenerateSqlSpParameter(column_Container, metaData.Container),
                    GenerateSqlSpParameter(column_Identifier, metaData.Identifier),
                    GenerateSqlSpParameter(column_Name, metaData.Name),
                    GenerateSqlSpParameter(column_Mime, metaData.Mime),
                    GenerateSqlSpParameter(column_Hash, metaData.Hash),
                    GenerateSqlSpParameter(column_Length, metaData.Length),
                    GenerateSqlSpParameter(column_Height, metaData.Height),
                    GenerateSqlSpParameter(column_Width, metaData.Width),
                    GenerateSqlSpParameter(column_Duration, metaData.Duration),
                    GenerateSqlSpParameter(column_OwnerKey, metaData.OwnerKey),
                    GenerateSqlSpParameter(column_State, (int)metaData.State),
                    GenerateSqlSpParameter(column_OperatorKey, operatorKey),
                };

                FillAdditionalFieldValue(parameters, metaData);

                return this.ExecuteReader(spName, parameters).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex.Handle("CreateOrUpdateBinaryStorageMetaData", new { metaData, operatorKey });
            }
        }

        /// <summary>
        /// Queries the binary storage meta data.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;BinaryStorageMetaData&gt;.</returns>
        public List<TMetaEntity> QueryBinaryStorageMetaData(TMetaCriteria criteria)
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
                    GenerateSqlSpParameter(column_OwnerKey, criteria.OwnerKey),
                    GenerateSqlSpParameter(column_State, criteria.State.EnumToInt32())
                };

                FillAdditionalFieldValue(parameters, criteria);

                return this.ExecuteReader(spName, parameters);
            }
            catch (Exception ex)
            {
                throw ex.Handle("QueryBinaryStorageMetaData", criteria);
            }
        }

        /// <summary>
        /// Gets the binary storage meta data by identifiers.
        /// </summary>
        /// <param name="metaConnection">The meta connection.</param>
        /// <param name="removeInvalid">if set to <c>true</c> [remove invalid].</param>
        /// <returns>List&lt;BinaryStorageMetaData&gt;.</returns>
        public List<TMetaEntity> GetBinaryStorageMetaDataByIdentifiers<T>(List<T> metaConnection, bool removeInvalid = false) where T : BinaryStorageIdentifier
        {
            const string spName = "sp_GetBinaryStorageMetaDataByIdentifiers";

            try
            {
                metaConnection.CheckNullObject("metaConnection");

                var xml = "Identifier".CreateXml();
                foreach (var one in metaConnection)
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

                var validMetaDataCollection = this.ExecuteReader(spName, parameters);
                return removeInvalid
                    ? new List<TMetaEntity>(from item in validMetaDataCollection where item.State == BinaryStorageState.Committed select item)
                    : validMetaDataCollection;
            }
            catch (Exception ex)
            {
                throw ex.Handle("GetBinaryStorageMetaDataByIdentifiers", new { metaConnection, removeInvalid });
            }
        }
    }
}
