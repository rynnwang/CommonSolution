using System;
using System.Collections.Generic;
using Beyova.ApiTracking;

namespace Beyova.Elastic
{
    /// <summary>
    /// Class Extension.
    /// </summary>
    internal static class CriteriaExtension
    {
        /// <summary>
        /// To the date histogram interval.
        /// </summary>
        /// <param name="timeScope">The time scope.</param>
        /// <returns>DateHistogramInterval.</returns>
        public static DateHistogramInterval? ToDateHistogramInterval(this TimeScope? timeScope)
        {
            if (!timeScope.HasValue)
            {
                return null;
            }

            switch (timeScope)
            {
                case TimeScope.Day:
                    return DateHistogramInterval.day;
                case TimeScope.Hour:
                    return DateHistogramInterval.hour;
                case TimeScope.Month:
                    return DateHistogramInterval.month;
                case TimeScope.Week:
                    return DateHistogramInterval.week;
                case TimeScope.Year:
                    return DateHistogramInterval.year;
                default:
                    return default(DateHistogramInterval);
            }
        }

        /// <summary>
        /// Exceptions the criteria to elastic criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>System.Object.</returns>
        public static SearchCriteria ToElasticCriteria(this ExceptionCriteria criteria)
        {
            if (criteria == null) { return null; }

            QueryCriteria queryCriteria = null;

            if (criteria.Key != null)
            {
                criteria.Count = 1;
                queryCriteria = new QueryCriteria
                {
                    PhraseMatches = new Dictionary<string, object> { { "Key", criteria.Key.Value.ToString() } }
                };
            }
            else
            {
                var booleanCriteria = CriteriaBuilder.CreateBooleanCriteria(criteria) ?? new BooleanCriteria { Must = new List<ElasticCriteriaObject>() };
                booleanCriteria.Must.AddIfNotNull(CriteriaBuilder.CreateTermCriteriaObject("Code.Major", (int?)criteria.MajorCode));
                booleanCriteria.Must.AddIfNotNull(CriteriaBuilder.CreateTermCriteriaObject("Code.Minor", criteria.MinorCode));
                booleanCriteria.Must.AddIfNotNull(CriteriaBuilder.CreateTermCriteriaObject("Scene.MethodName", criteria.Scene));

                queryCriteria = new QueryCriteria
                {
                    BooleanCriteria = booleanCriteria.ToValidBooleanCriteria(),
                    Range = CriteriaBuilder.CreateTimeRange(criteria.FromStamp, criteria.ToStamp)
                }.ToValidQueryCriteria();
            }

            return new SearchCriteria
            {
                QueryCriteria = queryCriteria,
                Count = criteria.Count < 1 ? 50 : criteria.Count,
                OrderBy = new Dictionary<string, string> { { Constants.CreatedStamp, Constants.Descending } }
            };
        }

        /// <summary>
        /// To the elastic criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>SearchCriteria.</returns>
        public static SearchCriteria ToElasticCriteria(this ApiMessageCriteria criteria)
        {
            if (criteria == null) { return null; }

            QueryCriteria queryCriteria = null;

            if (criteria.Key != null)
            {
                criteria.Count = 1;
                queryCriteria = new QueryCriteria
                {
                    PhraseMatches = new Dictionary<string, object> { { "Key", criteria.Key.Value.ToString() } }
                };
            }
            else
            {
                var booleanCriteria = CriteriaBuilder.CreateBooleanCriteria(criteria) ?? new BooleanCriteria { Must = new List<ElasticCriteriaObject>() };

                queryCriteria = new QueryCriteria
                {
                    BooleanCriteria = booleanCriteria.ToValidBooleanCriteria(),
                    Range = CriteriaBuilder.CreateTimeRange(criteria.FromStamp, criteria.ToStamp)
                }.ToValidQueryCriteria();
            }

            return new SearchCriteria
            {
                QueryCriteria = queryCriteria,
                Count = criteria.Count < 1 ? 50 : criteria.Count,
                OrderBy = new Dictionary<string, string> { { Constants.CreatedStamp, Constants.Descending } }
            };
        }

        /// <summary>
        /// APIs the event criteria to elastic criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>SearchCriteria.</returns>
        public static SearchCriteria ToElasticCriteria(this ApiEventCriteria criteria)
        {
            if (criteria == null)
            {
                return null;
            }

            QueryCriteria queryCriteria = null;
            var rangeList = new Dictionary<string, Dictionary<string, object>>();

            if (criteria.Key != null)
            {
                criteria.Count = 1;
                queryCriteria = new QueryCriteria
                {
                    PhraseMatches = new Dictionary<string, object> { { "Key", criteria.Key.Value.ToString() } }
                };
            }
            else
            {
                queryCriteria = new QueryCriteria
                {
                    BooleanCriteria = CriteriaBuilder.CreateBooleanCriteria(criteria)
                };

                // Only when ExceptionKey is not specified, HasException has chance to use.
                if (criteria.ExceptionKey == null && criteria.HasException != null)
                {
                    queryCriteria.Filters = new FilterCriteria();
                    queryCriteria.Filters.Items = criteria.HasException.Value ? new { exists = new { field = Constants.ExceptionKey } } : new { missing = new { field = Constants.ExceptionKey } } as object;
                }

                CriteriaBuilder.SetTimeRange(rangeList, criteria.FromStamp, criteria.ToStamp);

                var durationRangeList = new Dictionary<string, object>();

                if (criteria.ApiDurationFrom != null)
                {
                    durationRangeList.Add(Constants.GreaterThanOrEquals, criteria.ApiDurationFrom.Value);
                }

                if (criteria.ApiDurationTo != null)
                {
                    durationRangeList.Add(Constants.LessThanOrEquals, criteria.ApiDurationTo.Value);
                }

                if (durationRangeList.Count > 0)
                {
                    rangeList.Add("Duration", durationRangeList);
                }

                if (rangeList.Count > 0)
                {
                    queryCriteria.Range = rangeList;
                }

                queryCriteria = queryCriteria.ToValidQueryCriteria();
            }

            return new SearchCriteria
            {
                QueryCriteria = queryCriteria,
                Count = criteria.Count < 1 ? 50 : criteria.Count,
                OrderBy = new Dictionary<string, string> { { Constants.CreatedStamp, Constants.Descending } }
            };
        }
    }
}
