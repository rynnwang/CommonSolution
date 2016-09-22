using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Beyova;

namespace Beyova.CommonService.DataAccessController
{
    /// <summary>
    /// Class CountryInfoDataAccessController.
    /// </summary>
    public class CountryInfoDataAccessController : BaseDataAccessController<CountryInfo>
    {
        #region Constants

        /// <summary>
        /// The column_ currency code
        /// </summary>
        protected const string column_CurrencyCode = "CurrencyCode";

        /// <summary>
        /// The column_ iso2 code
        /// </summary>
        protected const string column_Iso2Code = "Iso2Code";

        /// <summary>
        /// The column_ iso3 code
        /// </summary>
        protected const string column_Iso3Code = "Iso3Code";

        /// <summary>
        /// The column_ tel code
        /// </summary>
        protected const string column_TelCode = "TelCode";

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CountryInfoDataAccessController" /> class.
        /// </summary>
        public CountryInfoDataAccessController()
            : base()
        {
        }

        #endregion

        /// <summary>
        /// Converts the entity object.
        /// </summary>
        /// <param name="sqlDataReader">The SQL data reader.</param>
        /// <returns>Object instance in type {`0}.</returns>
        protected override CountryInfo ConvertEntityObject(SqlDataReader sqlDataReader)
        {
            return new CountryInfo
            {
                Key = sqlDataReader[column_Key].ObjectToGuid(),
                Name = sqlDataReader[column_Name].ObjectToString(),
                CurrencyCode = sqlDataReader[column_CurrencyCode].ObjectToString(),
                Iso2Code = sqlDataReader[column_Iso2Code].ObjectToString(),
                Iso3Code = sqlDataReader[column_Iso3Code].ObjectToString(),
                TelCode = sqlDataReader[column_TelCode].ObjectToString(),
                TimeZone = sqlDataReader[column_TimeZone].ObjectToNullableInt32(),
                Sequence = sqlDataReader[column_Sequence].ObjectToInt32(),
                CultureCode = sqlDataReader[column_CultureCode].ObjectToString()
            };
        }

        /// <summary>
        /// Queries the country information.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;CountryInfo&gt;.</returns>
        public List<CountryInfo> QueryCountryInfo(CountryInfoCriteria criteria)
        {
            const string spName = "sp_QueryCountryInfo";

            try
            {
                criteria.CheckNullObject("criteria");

                var parameters = new List<SqlParameter>
                {
                    GenerateSqlSpParameter(column_Key, criteria.Key),
                    GenerateSqlSpParameter(column_Code, criteria.Code),
                    GenerateSqlSpParameter(column_Name, criteria.Name),
                    GenerateSqlSpParameter(column_CultureCode, criteria.CultureCode)
                };

                return this.ExecuteReader(spName, parameters);
            }
            catch (Exception ex)
            {
                throw ex.Handle(criteria);
            }
        }
    }
}
