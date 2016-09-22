using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beyova.ApiTracking;
using Beyova.ExceptionSystem;

namespace Beyova.Elastic
{
    /// <summary>
    /// Class CriteriaBuilder.
    /// </summary>
    public static class CriteriaBuilder
    {
        #region Util

        /// <summary>
        /// To the valid search criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>SearchCriteria.</returns>
        public static QueryCriteria ToValidQueryCriteria(this QueryCriteria criteria)
        {
            return (criteria == null || (
                criteria.Terms == null && criteria.FromIndex == null && criteria.Matches == null
                && criteria.Range == null && criteria.PhraseMatches == null && criteria.BooleanCriteria == null
                )) ? null : criteria;
        }

        /// <summary>
        /// To the valid boolean criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>BooleanCriteria.</returns>
        public static BooleanCriteria ToValidBooleanCriteria(this BooleanCriteria criteria)
        {
            return (criteria == null || (!criteria.Must.HasItem() && !criteria.MustNot.HasItem() && !criteria.Should.HasItem() && !criteria.Filter.HasItem())) ? null : criteria;
        }

        /// <summary>
        /// To the valid aggregation criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Dictionary&lt;System.String, Dictionary&lt;System.String, System.Object&gt;&gt;.</returns>
        internal static AggregationsCriteria ToValidAggregationCriteria(this AggregationsCriteria criteria)
        {
            return (criteria == null || (
                criteria.Terms == null && criteria.SubAggregations.ToValidAggregationCriteria() == null && criteria.DateHistogram == null && criteria.Histogram == null
           )) ? null : criteria;
        }

        /// <summary>
        /// To the valid aggregation criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Dictionary&lt;System.String, AggregationsCriteria&gt;.</returns>
        internal static Dictionary<string, AggregationsCriteria> ToValidAggregationCriteria(this Dictionary<string, AggregationsCriteria> criteria)
        {
            return (criteria == null || !criteria.Any(x => x.Value.ToValidAggregationCriteria() != null)) ? null : criteria;
        }

        /// <summary>
        /// Appends the sub aggregations criteria.
        /// </summary>
        /// <param name="parentCriteria">The parent criteria.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns>AggregationsCriteria.</returns>
        internal static AggregationsCriteria AppendSubAggregationsCriteria(this AggregationsCriteria parentCriteria, string fieldName, AggregationsCriteria criteria)
        {
            if (parentCriteria != null && !string.IsNullOrWhiteSpace(fieldName) && criteria != null)
            {
                if (parentCriteria.SubAggregations == null)
                {
                    parentCriteria.SubAggregations = new Dictionary<string, AggregationsCriteria>();
                }
                parentCriteria.SubAggregations.Merge(fieldName, criteria);

                return parentCriteria.SubAggregations.SafeFirstOrDefault().Value;
            }

            return null;
        }

        /// <summary>
        /// Sets the criteria if not null or empty.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="fieldValue">The field value.</param>
        internal static void SetCriteriaIfNotNullOrEmpty(this Dictionary<string, object> container, string fieldName, string fieldValue)
        {
            if (!string.IsNullOrWhiteSpace(fieldValue) && !string.IsNullOrWhiteSpace(fieldName) && container != null)
            {
                container.Add(fieldName, fieldValue);
            }
        }

        /// <summary>
        /// Sets the criteria if has value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="container">The container.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="fieldValue">The field value.</param>
        internal static void SetCriteriaIfHasValue<T>(this Dictionary<string, object> container, string fieldName, T? fieldValue) where T : struct
        {
            if (fieldValue.HasValue && !string.IsNullOrWhiteSpace(fieldName) && container != null)
            {
                container.Add(fieldName, fieldValue);
            }
        }

        /// <summary>
        /// Sets the criteria if not null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="container">The container.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="fieldValue">The field value.</param>
        internal static void SetCriteriaIfNotNull<T>(this Dictionary<string, object> container, string fieldName, T fieldValue) where T : class
        {
            if (fieldValue != null && !string.IsNullOrWhiteSpace(fieldName) && container != null)
            {
                container.Add(fieldName, fieldValue);
            }
        }

        #endregion

        /// <summary>
        /// Gets the time range.
        /// </summary>
        /// <param name="fromStamp">From stamp.</param>
        /// <param name="toStamp">To stamp.</param>
        /// <param name="lastNDays">The last n days.</param>
        /// <returns>Dictionary&lt;System.String, Dictionary&lt;System.String, System.Object&gt;&gt;.</returns>
        public static Dictionary<string, Dictionary<string, object>> CreateTimeRange(DateTime? fromStamp, DateTime? toStamp, int? lastNDays = null)
        {
            if (fromStamp != null || toStamp != null)
            {
                var result = new Dictionary<string, Dictionary<string, object>>();
                SetTimeRange(result, fromStamp, toStamp, lastNDays);

                return result;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Sets the time range.
        /// </summary>
        /// <param name="dateTimeRange">The date time range.</param>
        /// <param name="fromStamp">From stamp.</param>
        /// <param name="toStamp">To stamp.</param>
        /// <param name="lastNDays">The last n days.</param>
        public static void SetTimeRange(Dictionary<string, Dictionary<string, object>> dateTimeRange, DateTime? fromStamp, DateTime? toStamp, int? lastNDays = null)
        {
            SetRangeValue(dateTimeRange, Constants.CreatedStamp, fromStamp, toStamp, lastNDays);
        }

        /// <summary>
        /// Sets the range value.
        /// </summary>
        /// <param name="rangeContainer">The range container.</param>
        /// <param name="criteriaFieldName">Name of the criteria field.</param>
        /// <param name="fromStamp">From stamp.</param>
        /// <param name="toStamp">To stamp.</param>
        /// <param name="lastNDays">The last n days.</param>
        public static void SetRangeValue(Dictionary<string, Dictionary<string, object>> rangeContainer, string criteriaFieldName, DateTime? fromStamp, DateTime? toStamp, int? lastNDays = null)
        {
            if (rangeContainer != null && !string.IsNullOrWhiteSpace(criteriaFieldName))
            {
                var rangeValue = new Dictionary<string, object>();

                if (lastNDays.HasValue)
                {
                    toStamp = DateTime.UtcNow;
                    fromStamp = toStamp.Value.AddDays(-1 * lastNDays.Value);
                }

                if (fromStamp != null)
                {
                    rangeValue.Add(Constants.GreaterThanOrEquals, fromStamp);
                }

                if (toStamp != null)
                {
                    rangeValue.Add(Constants.LessThan, toStamp);
                }

                if (rangeValue.Count > 0)
                {
                    rangeContainer.Merge(criteriaFieldName, rangeValue);
                }
            }
        }

        /// <summary>
        /// Creates the boolean criteria.
        /// </summary>
        /// <param name="exceptionBase">The exception base.</param>
        /// <returns>BooleanCriteria.</returns>
        public static BooleanCriteria CreateBooleanCriteria(ExceptionBase exceptionBase)
        {
            BooleanCriteria booleanCriteria = new BooleanCriteria
            {
                Must = new List<ElasticCriteriaObject>()
            };

            if (exceptionBase != null)
            {
                booleanCriteria.Must.AddIfNotNull(CreateMatchPhraseCriteriaObject("ServerIdentifier", exceptionBase.ServerIdentifier));
                booleanCriteria.Must.AddIfNotNull(CreateMatchPhraseCriteriaObject("ServiceIdentifier", exceptionBase.ServiceIdentifier));

                if (exceptionBase.OperatorCredential != null)
                {
                    booleanCriteria.Must.AddIfNotNull(CreateMatchPhraseCriteriaObject("OperatorCredential.Key", exceptionBase.OperatorCredential.Key.ToString()));
                    booleanCriteria.Must.AddIfNotNull(CreateMatchPhraseCriteriaObject("OperatorCredential.Name", exceptionBase.OperatorCredential.Name));
                }
                booleanCriteria.Must.AddIfNotNull(CreateTermCriteriaObject("Message", exceptionBase.Message));
                booleanCriteria.Must.AddIfNotNull(CreateTermCriteriaObject("TargetSite", exceptionBase.TargetSite));
                booleanCriteria.Must.AddIfNotNull(CreateTermCriteriaObject("StackTrace", exceptionBase.StackTrace));
                booleanCriteria.Must.AddIfNotNull(CreateMatchPhraseCriteriaObject("ExceptionType", exceptionBase.ExceptionType));
                booleanCriteria.Must.AddIfNotNull(CreateTermCriteriaObject("Source", exceptionBase.Source));

                if (exceptionBase.Level.HasValue)
                {
                    booleanCriteria.Must.AddIfNotNull(CreateTermCriteriaObject("Level", (int)exceptionBase.Level.Value));
                }
            }

            return booleanCriteria.ToValidBooleanCriteria();
        }

        /// <summary>
        /// Creates the query criteria.
        /// </summary>
        /// <param name="logBase">The log base.</param>
        /// <param name="validate">if set to <c>true</c> [validate].</param>
        /// <returns>QueryCriteria.</returns>
        public static BooleanCriteria CreateBooleanCriteria(ApiEventLogBase logBase)
        {
            BooleanCriteria booleanCriteria = new BooleanCriteria
            {
                Must = new List<ElasticCriteriaObject>()
            };

            if (logBase != null)
            {
                booleanCriteria.Must.AddIfNotNull(CreateMatchPhraseCriteriaObject("ServerIdentifier", logBase.ServerIdentifier));
                booleanCriteria.Must.AddIfNotNull(CreateMatchPhraseCriteriaObject("ServiceIdentifier", logBase.ServiceIdentifier));

                if (logBase.OperatorCredential != null)
                {
                    booleanCriteria.Must.AddIfNotNull(CreateMatchPhraseCriteriaObject("OperatorCredential.Key", logBase.OperatorCredential.Key.ToString()));
                    booleanCriteria.Must.AddIfNotNull(CreateMatchPhraseCriteriaObject("OperatorCredential.Name", logBase.OperatorCredential.Name));
                }
                booleanCriteria.Must.AddIfNotNull(CreateTermCriteriaObject("ResourceName", logBase.ResourceName));
                booleanCriteria.Must.AddIfNotNull(CreateTermCriteriaObject("ModuleName", logBase.ModuleName));
                booleanCriteria.Must.AddIfNotNull(CreateTermCriteriaObject("ResourceEntityKey", logBase.ResourceEntityKey));
                booleanCriteria.Must.AddIfNotNull(CreateMatchPhraseCriteriaObject("ApiFullName", logBase.ApiFullName));
                booleanCriteria.Must.AddIfNotNull(CreateMatchPhraseCriteriaObject("ExceptionKey", logBase.ExceptionKey));
                booleanCriteria.Must.AddIfNotNull(CreateMatchPhraseCriteriaObject("CultureCode", logBase.CultureCode));
                booleanCriteria.Must.AddIfNotNull(CreateMatchPhraseCriteriaObject("ClientIdentifier", logBase.ClientIdentifier));
                booleanCriteria.Must.AddIfNotNull(CreateMatchPhraseCriteriaObject("IpAddress", logBase.IpAddress));
                booleanCriteria.Must.AddIfNotNull(CreateTermCriteriaObject("UserAgent", logBase.UserAgent));
                booleanCriteria.Must.AddIfNotNull(CreateTermCriteriaObject("RawUrl", logBase.RawUrl));
                booleanCriteria.Must.AddIfNotNull(CreateTermCriteriaObject("ReferrerUrl", logBase.ReferrerUrl));

                booleanCriteria.Must.AddIfNotNull(CreateMatchPhraseCriteriaObject("TraceId", logBase.TraceId));
                booleanCriteria.Must.AddIfNotNull(CreateTermCriteriaObject("Content", logBase.Content));
                booleanCriteria.Must.AddIfNotNull(CreateMatchPhraseCriteriaObject("Protocol", logBase.Protocol));

                if (logBase.Platform.HasValue)
                {
                    booleanCriteria.Must.AddIfNotNull(CreateTermCriteriaObject("Platform", (int)(logBase.Platform.Value)));
                }
                if (logBase.DeviceType != null)
                {
                    booleanCriteria.Must.AddIfNotNull(CreateTermCriteriaObject("DeviceType", (int)(logBase.DeviceType.Value)));
                }
            }

            return booleanCriteria.ToValidBooleanCriteria();
        }

        /// <summary>
        /// Creates the boolean criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>BooleanCriteria.</returns>
        public static BooleanCriteria CreateBooleanCriteria(ApiMessageCriteria criteria)
        {
            BooleanCriteria booleanCriteria = new BooleanCriteria
            {
                Must = new List<ElasticCriteriaObject>()
            };

            if (criteria != null)
            {
                booleanCriteria.Must.AddIfNotNull(CreateTermCriteriaObject("Message", criteria.Message));
            }

            return booleanCriteria.ToValidBooleanCriteria();
        }

        /// <summary>
        /// Creates the criteria object.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="criteriaValue">The criteria value.</param>
        /// <returns>ElasticCriteriaObject.</returns>
        private static ElasticCriteriaObject CreateCriteriaObject(string type, string fieldName, object criteriaValue)
        {
            return (!string.IsNullOrWhiteSpace(fieldName) && criteriaValue != null) ? new ElasticCriteriaObject
            {
                CriteriaType = type,
                CriteriaValue = criteriaValue,
                FieldName = fieldName
            } : null;
        }

        /// <summary>
        /// Creates the term criteria object.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="criteriaValue">The criteria value.</param>
        /// <returns>ElasticCriteriaObject.</returns>
        public static ElasticCriteriaObject CreateTermCriteriaObject(string fieldName, object criteriaValue)
        {
            return CreateCriteriaObject("term", fieldName, criteriaValue);
        }

        /// <summary>
        /// Creates the match criteria object.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="criteriaValue">The criteria value.</param>
        /// <returns>ElasticCriteriaObject.</returns>
        public static ElasticCriteriaObject CreateMatchCriteriaObject(string fieldName, object criteriaValue)
        {
            return CreateCriteriaObject("match", fieldName, criteriaValue);
        }

        /// <summary>
        /// Creates the match phrase criteria object.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="criteriaValue">The criteria value.</param>
        /// <returns>ElasticCriteriaObject.</returns>
        public static ElasticCriteriaObject CreateMatchPhraseCriteriaObject(string fieldName, object criteriaValue)
        {
            return CreateCriteriaObject("match_phrase", fieldName, criteriaValue);
        }

        /// <summary>
        /// Creates the range criteria object.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="criteriaValue">The criteria value.</param>
        /// <returns>ElasticCriteriaObject.</returns>
        public static ElasticCriteriaObject CreateRangeCriteriaObject(string fieldName, object criteriaValue)
        {
            return CreateCriteriaObject("range", fieldName, criteriaValue);
        }

        /// <summary>
        /// Creates the filtered criteria object.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="criteriaValue">The criteria value.</param>
        /// <returns>ElasticCriteriaObject.</returns>
        public static ElasticCriteriaObject CreateFilteredCriteriaObject(string fieldName, object criteriaValue)
        {
            return CreateCriteriaObject("filtered", fieldName, criteriaValue);
        }
    }
}
