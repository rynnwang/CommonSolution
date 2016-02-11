using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Beyova;
using Beyova.BinaryStorage;

namespace Beyova.CommonService.DataAccessController
{
    /// <summary>
    /// Class BinaryCapacitySummaryDataAccessController.
    /// </summary>
    public class BinaryCapacitySummaryDataAccessController : BaseCommonServiceController<BinaryCapacitySummary, BinaryCapacityCriteria>
    {
        #region Constants

        /// <summary>
        /// The column_ size
        /// </summary>
        protected const string column_Size = "Size";

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryCapacitySummaryDataAccessController" /> class.
        /// </summary>
        public BinaryCapacitySummaryDataAccessController()
            : base()
        {
        }

        #endregion

        /// <summary>
        /// Converts the entity object.
        /// </summary>
        /// <param name="sqlDataReader">The SQL data reader.</param>
        /// <returns>BinaryCapacitySummary.</returns>
        protected override BinaryCapacitySummary ConvertEntityObject(SqlDataReader sqlDataReader)
        {
            return new BinaryCapacitySummary
            {
                Container = sqlDataReader[column_Container].ObjectToString(),
                TotalSize = sqlDataReader[column_Size].ObjectToInt64(),
                TotalCount = sqlDataReader[column_Count].ObjectToInt32()
            };
        }

        /// <summary>
        /// Gets the binary capacity summary.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>BinaryCapacitySummary.</returns>
        public BinaryCapacitySummary GetBinaryCapacitySummary(BinaryCapacityCriteria criteria)
        {
            const string spName = "sp_GetBinaryCapacitySummary";

            try
            {
                criteria.CheckNullObject("criteria");

                var parameters = new List<SqlParameter>
                {
                    GenerateSqlSpParameter(column_Container, criteria.Container),
                    GenerateSqlSpParameter(column_OwnerKey, criteria.OwnerKey)
                };

                return this.ExecuteReader(spName, parameters).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex.Handle("GetBinaryCapacitySummary", criteria);
            }
        }
    }
}
