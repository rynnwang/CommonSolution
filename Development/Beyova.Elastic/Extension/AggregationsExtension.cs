using System;
using System.Collections.Generic;
using System.Linq;
using Beyova;
using Beyova.ApiTracking;
using Newtonsoft.Json.Linq;

namespace Beyova.Elastic
{
    /// <summary>
    /// Class AggregationsExtension.
    /// </summary>
    internal static class AggregationsExtension
    {
        /// <summary>
        /// Creates the date aggregations criteria.
        /// </summary>
        /// <param name="timeFrame">The time frame.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>AggregationsCriteria.</returns>
        public static AggregationsCriteria CreateDateAggregationsCriteria(this ITimeFrame timeFrame, string fieldName)
        {
            return (timeFrame != null && !string.IsNullOrWhiteSpace(fieldName) && timeFrame.FrameInterval.HasValue) ? new AggregationsCriteria
            {
                DateHistogram = new DateHistogramCriteria
                {
                    Field = fieldName,
                    Interval = timeFrame.FrameInterval.ToDateHistogramInterval().Value.ToString().ToLower(),
                    TimeZone = timeFrame.TimeZone.TimeZoneMinuteOffsetToTimeZoneString()
                }
            } : null;
        }

        /// <summary>
        /// To the elastic criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>SearchCriteria.</returns>
        public static SearchCriteria ToElasticCriteria(this ExceptionStatisticCriteria criteria)
        {
            if (criteria == null)
            {
                return null;
            }

            var aggregationsCriteria = CreateDateAggregationsCriteria(criteria, Constants.CreatedStamp);
            var queryCriteria = new QueryCriteria
            {
                BooleanCriteria = CriteriaBuilder.CreateBooleanCriteria(criteria),
                Range = CriteriaBuilder.CreateTimeRange(criteria.FromStamp, criteria.ToStamp, criteria.LastNDays)
            };

            return new SearchCriteria
            {
                Count = 0,
                QueryCriteria = queryCriteria.ToValidQueryCriteria(),
                AggregationsCriteria = (new Dictionary<string, AggregationsCriteria> { { Constants.StampIdentifier, aggregationsCriteria } }).ToValidAggregationCriteria()
            };
        }

        /// <summary>
        /// To the elastic criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>SearchCriteria.</returns>
        public static SearchCriteria ToElasticCriteria(this ExceptionGroupingCriteria criteria)
        {
            if (criteria == null)
            {
                return null;
            }

            var searchCriteria = ToElasticCriteria(criteria as ExceptionStatisticCriteria);
            var aggregationCriteria = searchCriteria.AggregationsCriteria.SafeFirstOrDefault().Value;

            if (criteria.GroupByServiceIdentifier)
            {
                aggregationCriteria = aggregationCriteria.AppendSubAggregationsCriteria("ServiceIdentifier", new AggregationsCriteria { Terms = new Dictionary<string, string> { { Constants.AggregationField, "ServiceIdentifier" } } });
            }

            if (criteria.GroupByServerIdentifier)
            {
                aggregationCriteria = aggregationCriteria.AppendSubAggregationsCriteria("ServerIdentifier", new AggregationsCriteria { Terms = new Dictionary<string, string> { { Constants.AggregationField, "ServerIdentifier" } } });
            }

            if (criteria.GroupByExceptionCode)
            {
                aggregationCriteria = aggregationCriteria.AppendSubAggregationsCriteria("ExceptionCode", new AggregationsCriteria { Terms = new Dictionary<string, string> { { Constants.AggregationField, "Code.Major" } } });
            }

            var queryCriteria = new QueryCriteria
            {
                BooleanCriteria = CriteriaBuilder.CreateBooleanCriteria(criteria),
                Range = CriteriaBuilder.CreateTimeRange(criteria.FromStamp, criteria.ToStamp, criteria.LastNDays)
            };

            searchCriteria.QueryCriteria = queryCriteria.ToValidQueryCriteria();

            return searchCriteria;
        }

        /// <summary>
        /// To the elastic criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>SearchCriteria.</returns>
        public static SearchCriteria ToElasticCriteria(this ApiEventStatisticCriteria criteria)
        {
            if (criteria == null)
            {
                return null;
            }

            var aggregationsCriteria = CreateDateAggregationsCriteria(criteria, Constants.CreatedStamp);
            var queryCriteria = new QueryCriteria
            {
                BooleanCriteria = CriteriaBuilder.CreateBooleanCriteria(criteria),
                Range = CriteriaBuilder.CreateTimeRange(criteria.FromStamp, criteria.ToStamp, criteria.LastNDays)
            };

            return new SearchCriteria
            {
                Count = 0,
                QueryCriteria = queryCriteria.ToValidQueryCriteria(),
                AggregationsCriteria = (new Dictionary<string, AggregationsCriteria> { { Constants.StampIdentifier, aggregationsCriteria } }).ToValidAggregationCriteria()
            };
        }

        /// <summary>
        /// To the elastic criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>SearchCriteria.</returns>
        public static SearchCriteria ToElasticCriteria(this ApiEventGroupingCriteria criteria)
        {
            if (criteria == null)
            {
                return null;
            }

            var searchCriteria = ToElasticCriteria(criteria as ApiEventStatisticCriteria);
            var aggregationCriteria = searchCriteria.AggregationsCriteria.SafeFirstOrDefault().Value;

            if (criteria.GroupByServiceIdentifier)
            {
                aggregationCriteria = aggregationCriteria.AppendSubAggregationsCriteria("ServiceIdentifier", new AggregationsCriteria { Terms = new Dictionary<string, string> { { Constants.AggregationField, "ServiceIdentifier" } } });
            }

            if (criteria.GroupByServerIdentifier)
            {
                aggregationCriteria = aggregationCriteria.AppendSubAggregationsCriteria("ServerIdentifier", new AggregationsCriteria { Terms = new Dictionary<string, string> { { Constants.AggregationField, "ServerIdentifier" } } });
            }

            if (criteria.GroupByLocation)
            {
                aggregationCriteria = aggregationCriteria.AppendSubAggregationsCriteria("Location", new AggregationsCriteria { Terms = new Dictionary<string, string> { { Constants.AggregationField, "GeoInfo.CityName" } } });
            }

            var queryCriteria = new QueryCriteria
            {
                BooleanCriteria = CriteriaBuilder.CreateBooleanCriteria(criteria),
                Range = CriteriaBuilder.CreateTimeRange(criteria.FromStamp, criteria.ToStamp, criteria.LastNDays)
            };

            return searchCriteria;
        }

        /// <summary>
        /// Converts to aggregation bucket object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bucketJson">The bucket json.</param>
        /// <param name="identifierName">Name of the identifier.</param>
        /// <returns>AggregationGroupObject&lt;T&gt;.</returns>
        internal static List<AggregationGroupObject<T>> ConvertToAggregationBucketObject<T>(this JObject bucketJson, string identifierName)
        {
            List<AggregationGroupObject<T>> result = new List<AggregationGroupObject<T>>();

            if (bucketJson != null)
            {
                foreach (JObject item in bucketJson.SelectToken("buckets"))
                {
                    AggregationGroupObject<T> group = new AggregationGroupObject<T>
                    {
                        Name = identifierName
                    };

                    foreach (var one in item.Properties())
                    {
                        switch (one.Name)
                        {
                            case "doc_count":
                                group.Value = one.Value.ToObject<long>();
                                break;
                            case "key":
                                group.Identifier = one.Value.ToObject<T>();
                                break;
                            case "key_as_string":
                                group.IdentifierString = one.Value.ToObject<string>();
                                break;
                            default:
                                if (group.SubGroupObjects == null)
                                {
                                    group.SubGroupObjects = new MatrixList<AggregationGroupObject<T>>();
                                }

                                group.SubGroupObjects.Add(one.Name, ConvertToAggregationBucketObject<T>(one.Value as JObject, one.Name));
                                break;
                        }
                    }

                    result.Add(group);
                }
            }

            return result;
        }

        /// <summary>
        /// To the API event group statistic.
        /// </summary>
        /// <param name="groupObjects">The group objects.</param>
        /// <param name="frameInterval">The frame interval.</param>
        /// <returns>List&lt;ApiEventGroupStatistic&gt;.</returns>
        internal static List<ApiEventGroupStatistic> ToApiEventGroupStatistic(this MatrixList<AggregationGroupObject<JToken>> groupObjects, TimeScope? frameInterval)
        {
            Dictionary<string, JToken> passThroughValues = new Dictionary<string, JToken>();
            return ToFlat<ApiEventGroupStatistic>(groupObjects, passThroughValues);
        }

        /// <summary>
        /// To the exception group statistic.
        /// </summary>
        /// <param name="groupObjects">The group objects.</param>
        /// <param name="frameInterval">The frame interval.</param>
        /// <returns>List&lt;ExceptionGroupStatistic&gt;.</returns>
        internal static List<ExceptionGroupStatistic> ToExceptionGroupStatistic(this MatrixList<AggregationGroupObject<JToken>> groupObjects, TimeScope? frameInterval)
        {
            Dictionary<string, JToken> passThroughValues = new Dictionary<string, JToken>();
            return ToFlat<ExceptionGroupStatistic>(groupObjects, passThroughValues);
        }

        /// <summary>
        /// To the flat.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="groupObjects">The group objects.</param>
        /// <param name="passThroughValues">The pass through values.</param>
        /// <returns>List&lt;T&gt;.</returns>
        private static List<T> ToFlat<T>(this MatrixList<AggregationGroupObject<JToken>> groupObjects, Dictionary<string, JToken> passThroughValues)
            where T : IGroupByResult, new()
        {
            List<T> result = new List<T>();

            if (groupObjects.HasItem() && passThroughValues != null)
            {
                var groupToHandle = groupObjects.FirstOrDefault();
                if (groupToHandle.Value.HasItem())
                {
                    foreach (var one in groupToHandle.Value)
                    {
                        if (one.SubGroupObjects != null)
                        {
                            Dictionary<string, JToken> nextPassThroughValues = new Dictionary<string, JToken>(passThroughValues);
                            nextPassThroughValues.Add(groupToHandle.Key, one.IdentifierString);

                            result.AddRange(ToFlat<T>(one.SubGroupObjects, nextPassThroughValues));
                        }
                        else
                        {
                            T t = new T();
                            t.Count = (int)one.Value;
                            t.Identifier = one.IdentifierString;

                            if (!string.IsNullOrWhiteSpace(one.Name))
                            {
                                passThroughValues.Merge(one.Name, one.IdentifierString.SafeToString(one.Identifier?.ToString()));
                            }

                            foreach (var passThroughValue in passThroughValues)
                            {
                                var property = typeof(T).GetProperty(passThroughValue.Key);
                                if (property != null)
                                {
                                    property.SetValue(t, passThroughValue.Value.ToObject(property.PropertyType));
                                }
                            }

                            result.Add(t);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// To the group statistic.
        /// </summary>
        /// <param name="groupObjects">The group objects.</param>
        /// <param name="timeFrameInterval">The time frame interval.</param>
        /// <returns>List&lt;GroupStatistic&gt;.</returns>
        internal static List<GroupStatistic> ToGroupStatistic(this MatrixList<AggregationGroupObject<DateTime>> groupObjects, TimeScope timeFrameInterval)
        {
            List<GroupStatistic> result = new List<GroupStatistic>();

            if (groupObjects.HasItem())
            {
                var format = timeFrameInterval.ToDateTimeFormat();

                //should have only one matrix item.
                foreach (var groupObject in groupObjects.First().Value)
                {
                    result.Add(new GroupStatistic
                    {
                        Count = (long)groupObject.Value,
                        Identifier = groupObject.Identifier.ToString(format)
                    });
                }
            }

            return result;
        }

        /// <summary>
        /// To the date time format.
        /// </summary>
        /// <param name="frameInterval">The frame interval.</param>
        /// <returns>System.String.</returns>
        private static string ToDateTimeFormat(this TimeScope? frameInterval)
        {
            return frameInterval.HasValue ? ToDateTimeFormat(frameInterval.Value) : string.Empty;
        }

        /// <summary>
        /// To the date time format.
        /// </summary>
        /// <param name="frameInterval">The frame interval.</param>
        /// <returns>System.String.</returns>
        private static string ToDateTimeFormat(this TimeScope frameInterval)
        {
            switch (frameInterval)
            {
                case TimeScope.Month:
                    return "yyyy-MM";
                case TimeScope.Hour:
                    return "yyyy-MM-dd HH:00";
                case TimeScope.Year:
                    return "yyyy";
                case TimeScope.Day:
                case TimeScope.Week:
                default:
                    return "yyyy-MM-dd";
            }
        }
    }
}
