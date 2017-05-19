using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Beyova;
using Beyova.Api;
using Beyova.ApiTracking;
using Beyova.ExceptionSystem;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Beyova.Elastic
{
    /// <summary>
    /// Class ElasticApiTracking.
    /// </summary>
    public class ElasticApiTracking : IApiTracking, IApiAnalytics
    {
        /// <summary>
        /// The API event type
        /// </summary>
        protected const string apiEventType = "ApiEvent";

        /// <summary>
        /// The exception type
        /// </summary>
        protected const string exceptionType = "Exception";

        /// <summary>
        /// The trace log type
        /// </summary>
        protected const string traceLogType = "TraceLog";

        /// <summary>
        /// The message type
        /// </summary>
        protected const string messageType = "Message";

        /// <summary>
        /// The elastic client
        /// </summary>
        protected ElasticIndexClient elasticClient;

        /// <summary>
        /// The geo service
        /// </summary>
        protected LookupService geoService;

        /// <summary>
        /// Gets or sets the request timeout.
        /// </summary>
        /// <value>The request timeout.</value>
        public int? RequestTimeout { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ElasticApiTracking"/> class.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="indexName">Name of the index.</param>
        /// <param name="geoDbPath">The geo database path.</param>
        /// <param name="requestTimeout">The request timeout.</param>
        public ElasticApiTracking(ApiEndpoint endpoint, string indexName, string geoDbPath = null, int? requestTimeout = null) : this(endpoint?.ToUri(false)?.ToString(), indexName, geoDbPath, requestTimeout)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ElasticApiTracking"/> class.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="indexName">Name of the index.</param>
        /// <param name="geoDbPath">The geo database path.</param>
        /// <param name="requestTimeout">The request timeout.</param>
        public ElasticApiTracking(UriEndpoint endpoint, string indexName, string geoDbPath = null, int? requestTimeout = null) : this(endpoint?.ToString(), indexName, geoDbPath, requestTimeout)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ElasticApiTracking" /> class.
        /// </summary>
        /// <param name="baseUrl">The base URL. Example: http://elastic.co:9200</param>
        /// <param name="indexName">Name of the index. </param>
        /// <param name="geoDbPath">The geo database path. </param>
        /// <param name="requestTimeout">The request timeout.</param>
        public ElasticApiTracking(string baseUrl, string indexName, string geoDbPath = null, int? requestTimeout = null)
        {
            elasticClient = new ElasticIndexClient(baseUrl, indexName);
            geoService = TryInitializeGeoService(geoDbPath);
            this.RequestTimeout = requestTimeout;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ElasticIndexClient" /> class.
        /// This constructor would try to read configuration: <c>ElasticsearchUri</c>, <c>ElasticsearchIndex</c>, and <c>ElasticsearchGeoPath</c> (optional).
        /// </summary>
        public ElasticApiTracking() : this(Framework.GetConfiguration("ElasticsearchUri"),
            Framework.GetConfiguration("ElasticsearchIndex", "apitracking"),
            Framework.GetConfiguration("ElasticsearchGeoPath", string.Empty)
            )
        {
        }

        /// <summary>
        /// Tries the initialize geo service.
        /// </summary>
        /// <param name="geoDbPath">The geo database path.</param>
        /// <returns>LookupService.</returns>
        protected LookupService TryInitializeGeoService(string geoDbPath)
        {
            if (!string.IsNullOrWhiteSpace(geoDbPath) && File.Exists(geoDbPath))
            {
                return new LookupService(geoDbPath, LookupService.GEOIP_MEMORY_CACHE);
            }

            return null;
        }
        /// <summary>
        /// Fills the location information.
        /// </summary>
        /// <param name="eventLog">The event log.</param>
        protected void FillLocationInfo(ApiEventLog eventLog)
        {
            if (eventLog != null && geoService != null && !string.IsNullOrWhiteSpace(eventLog.IpAddress))
            {
                try
                {
                    var geoInfo = geoService.getLocation(eventLog.IpAddress);
                    if (geoInfo != null)
                    {
                        eventLog.GeoInfo = new Beyova.GeoInfoBase()
                        {
                            IsoCode = geoInfo.countryCode,
                            Latitude = (decimal)geoInfo.latitude,
                            Longitude = (decimal)geoInfo.longitude,
                            CityName = geoInfo.city,
                            CountryName = geoInfo.countryName
                        };
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="F"></typeparam>
        /// <param name="obj">The object.</param>
        protected void WorkAction<T, F>(object obj)
        {
            try
            {
                ElasticWorkObject<T, F> objectToWork = obj as ElasticWorkObject<T, F>;
                objectToWork.Preprocess();
                elasticClient.Index(objectToWork, objectToWork.Timeout);
            }
            catch { }
        }

        /// <summary>
        /// Works the item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="F"></typeparam>
        /// <param name="workObject">The work object.</param>
        protected void WorkItem<T, F>(ElasticWorkObject<T, F> workObject)
        {
            try
            {
                if (elasticClient != null)
                {
                    ThreadPool.QueueUserWorkItem(new WaitCallback(WorkAction<T, F>), workObject);
                    // Task.Factory.StartNew(new Action<object>(WorkAction<T, F>), workObject);
                }
            }
            catch { }
        }

        /// <summary>
        /// Works the item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="workObject">The work object.</param>
        protected void WorkItem<T>(ElasticWorkObject<T> workObject)
        {
            try
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(WorkAction<T, object>), workObject);
                //Task.Factory.StartNew(new Action<object>(WorkAction<T, object>), workObject);
            }
            catch { }
        }

        #region IApiTracking

        /// <summary>
        /// Logs the API event asynchronous.
        /// </summary>
        /// <param name="eventLog">The event log.</param>
        public void LogApiEvent(ApiEventLog eventLog)
        {
            WorkItem(new ElasticApiEventWorkObject
            {
                ProcessFactor = geoService,
                RawData = eventLog,
                Type = apiEventType,
                Timeout = this.RequestTimeout
            });
        }

        /// <summary>
        /// Logs the API trace log asynchronous.
        /// </summary>
        /// <param name="traceLog">The trace log.</param>
        public void LogApiTraceLog(ApiTraceLog traceLog)
        {
            WorkItem(new ElasticWorkObject<ElasticTraceLog>
            {
                RawData = ToElasticTraceLog(traceLog),
                Type = traceLogType,
                Timeout = this.RequestTimeout
            });
        }

        /// <summary>
        /// Logs the exception asynchronous.
        /// </summary>
        /// <param name="exceptionInfo">The exception information.</param>
        public void LogException(ExceptionInfo exceptionInfo)
        {
            WorkItem(new ElasticWorkObject<ElasticExceptionInfo>
            {
                RawData = ToExceptionInfo(exceptionInfo),
                Type = exceptionType,
                Timeout = this.RequestTimeout
            });
        }

        /// <summary>
        /// Logs the exception asynchronous.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public void LogException(BaseException exception)
        {
            LogException(exception.ToExceptionInfo());
        }

        /// <summary>
        /// Logs the message asynchronous.
        /// </summary>
        /// <param name="message">The message.</param>
        public void LogMessage(string message)
        {
            if (string.IsNullOrWhiteSpace(message)) return;

            WorkItem(new ElasticWorkObject<ElasticMessage>
            {
                RawData = new ElasticMessage { Message = message },
                Type = messageType,
                Timeout = this.RequestTimeout
            });
        }

        #endregion

        #region Get Http Request Raw

        /// <summary>
        /// Gets the API event query HTTP request raw.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        public string GetApiEventQueryHttpRequestRaw(ApiEventCriteria criteria)
        {
            try
            {
                return elasticClient.GetQueryHttpRequestRaw(apiEventType, criteria.ToElasticCriteria());
            }
            catch (Exception ex)
            {
                throw ex.Handle(criteria);
            }
        }

        /// <summary>
        /// Gets the exception query HTTP request raw.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        public string GetExceptionQueryHttpRequestRaw(ExceptionCriteria criteria)
        {
            try
            {
                return elasticClient.GetQueryHttpRequestRaw(exceptionType, criteria.ToElasticCriteria());
            }
            catch (Exception ex)
            {
                throw ex.Handle(criteria);
            }
        }

        #endregion

        #region Get

        /// <summary>
        /// Queries the API event log.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>QueryResult&lt;ApiEventLog&gt;.</returns>
        protected QueryResult<ApiEventLog> InternalQueryApiEventLog(ApiEventCriteria criteria)
        {
            try
            {
                return elasticClient.Query<ApiEventLog>(apiEventType, criteria.ToElasticCriteria());
            }
            catch (Exception ex)
            {
                throw ex.Handle(criteria);
            }
        }

        /// <summary>
        /// Queries the exception.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>QueryResult&lt;ExceptionInfo&gt;.</returns>
        protected QueryResult<ExceptionInfo> InternalQueryException(ExceptionCriteria criteria)
        {
            try
            {
                return ToExceptionInfo(elasticClient.Query<ElasticExceptionInfo>(exceptionType, criteria.ToElasticCriteria()));
            }
            catch (Exception ex)
            {
                throw ex.Handle(criteria);
            }
        }

        /// <summary>
        /// Internals the query API message.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>QueryResult&lt;ApiMessage&gt;.</returns>
        protected QueryResult<ApiMessage> InternalQueryApiMessage(ApiMessageCriteria criteria)
        {
            try
            {
                return elasticClient.Query<ApiMessage>(messageType, criteria.ToElasticCriteria());
            }
            catch (Exception ex)
            {
                throw ex.Handle(criteria);
            }
        }

        /// <summary>
        /// Internals the query trace log.
        /// </summary>
        /// <param name="traceId">The trace identifier.</param>
        /// <returns>QueryResult&lt;ApiTraceLog&gt;.</returns>
        protected QueryResult<ApiTraceLog> InternalQueryTraceLog(string traceId)
        {
            try
            {
                return ToApiTraceLog(elasticClient.Query<ElasticTraceLog>(traceLogType, new SearchCriteria
                {
                    Count = 200,
                    QueryCriteria = new QueryCriteria
                    {
                        PhraseMatches = new Dictionary<string, object> { { "TraceId", traceId } }
                    }
                }));
            }
            catch (Exception ex)
            {
                throw ex.Handle(traceId);
            }
        }

        /// <summary>
        /// Internals the aggregation query exception grouping.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>AggregationsQueryResult.</returns>
        protected AggregationsQueryResult<JToken> InternalAggregationQueryExceptionGrouping(ExceptionGroupingCriteria criteria)
        {
            try
            {
                return elasticClient.AggregationQuery<JToken>(exceptionType, criteria.ToElasticCriteria());
            }
            catch (Exception ex)
            {
                throw ex.Handle(criteria);
            }
        }

        /// <summary>
        /// Internals the aggregation query exception statistic.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>AggregationsQueryResult.</returns>
        protected AggregationsQueryResult<DateTime> InternalAggregationQueryExceptionStatistic(ExceptionStatisticCriteria criteria)
        {
            try
            {
                return elasticClient.AggregationQuery<DateTime>(exceptionType, criteria.ToElasticCriteria());
            }
            catch (Exception ex)
            {
                throw ex.Handle(criteria);
            }
        }

        /// <summary>
        /// Internals the aggregation query API event grouping.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>AggregationsQueryResult.</returns>
        protected AggregationsQueryResult<JToken> InternalAggregationQueryApiEventGrouping(ApiEventGroupingCriteria criteria)
        {
            try
            {
                return elasticClient.AggregationQuery<JToken>(apiEventType, criteria.ToElasticCriteria());
            }
            catch (Exception ex)
            {
                throw ex.Handle(criteria);
            }
        }

        /// <summary>
        /// Internals the aggregation query API event statistic.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>AggregationsQueryResult.</returns>
        protected AggregationsQueryResult<DateTime> InternalAggregationQueryApiEventStatistic(ApiEventStatisticCriteria criteria)
        {
            try
            {
                return elasticClient.AggregationQuery<DateTime>(apiEventType, criteria.ToElasticCriteria());
            }
            catch (Exception ex)
            {
                throw ex.Handle(criteria);
            }
        }

        /// <summary>
        /// Queries the API event.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;ApiEventLog&gt;.</returns>
        public List<ApiEventLog> QueryApiEvent(ApiEventCriteria criteria)
        {
            try
            {
                return InternalQueryApiEventLog(criteria).ToEntityList();
            }
            catch (Exception ex)
            {
                throw ex.Handle(criteria);
            }
        }

        /// <summary>
        /// Queries the exception.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;ExceptionInfo&gt;.</returns>
        public List<ExceptionInfo> QueryException(ExceptionCriteria criteria)
        {
            try
            {
                return InternalQueryException(criteria).ToEntityList();
            }
            catch (Exception ex)
            {
                throw ex.Handle(criteria);
            }
        }

        /// <summary>
        /// Queries the API message.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>System.Collections.Generic.List&lt;Beyova.ApiTracking.ApiMessage&gt;.</returns>
        public List<ApiMessage> QueryApiMessage(ApiMessageCriteria criteria)
        {
            try
            {
                return InternalQueryApiMessage(criteria).ToEntityList();
            }
            catch (Exception ex)
            {
                throw ex.Handle(criteria);
            }
        }

        /// <summary>
        /// Gets the API trace log by identifier.
        /// </summary>
        /// <param name="traceId">The trace identifier.</param>
        /// <returns>List&lt;ApiTraceLog&gt;.</returns>
        public List<ApiTraceLog> GetApiTraceLogById(string traceId)
        {
            try
            {
                traceId.CheckEmptyString("traceId");

                return InternalQueryTraceLog(traceId).ToEntityList();
            }
            catch (Exception ex)
            {
                throw ex.Handle(traceId);
            }
        }

        /// <summary>
        /// Gets the API event statistic.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>System.Collections.Generic.List&lt;Beyova.ApiTracking.GroupStatistic&gt;.</returns>
        public List<GroupStatistic> GetApiEventStatistic(ApiEventStatisticCriteria criteria)
        {
            try
            {
                criteria.CheckNullObject("criteria");
                criteria.FrameInterval.CheckNullObject("criteria.FrameInterval");

                return InternalAggregationQueryApiEventStatistic(criteria).Aggregations.ToGroupStatistic(criteria.FrameInterval.Value);
            }
            catch (Exception ex)
            {
                throw ex.Handle(criteria);
            }
        }

        /// <summary>
        /// Gets the API event group statistic.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>System.Collections.Generic.List&lt;Beyova.ApiTracking.ApiEventGroupStatistic&gt;.</returns>
        public List<ApiEventGroupStatistic> GetApiEventGroupStatistic(ApiEventGroupingCriteria criteria)
        {
            try
            {
                criteria.CheckNullObject("criteria");

                return InternalAggregationQueryApiEventGrouping(criteria).Aggregations.ToApiEventGroupStatistic(criteria.FrameInterval);
            }
            catch (Exception ex)
            {
                throw ex.Handle(criteria);
            }
        }

        /// <summary>
        /// Gets the API exception statistic.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>System.Collections.Generic.List&lt;Beyova.ApiTracking.GroupStatistic&gt;.</returns>
        public List<GroupStatistic> GetApiExceptionStatistic(ExceptionStatisticCriteria criteria)
        {
            try
            {
                criteria.CheckNullObject("criteria");
                criteria.FrameInterval.CheckNullObject("criteria.FrameInterval");

                return InternalAggregationQueryExceptionStatistic(criteria).Aggregations.ToGroupStatistic(criteria.FrameInterval.Value);
            }
            catch (Exception ex)
            {
                throw ex.Handle(criteria);
            }
        }

        /// <summary>
        /// Gets the API exception grouping statistic.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;GroupStatistic&gt;.</returns>
        public List<ExceptionGroupStatistic> GetApiExceptionGroupingStatistic(ExceptionGroupingCriteria criteria)
        {
            try
            {
                criteria.CheckNullObject("criteria");

                return InternalAggregationQueryExceptionGrouping(criteria).Aggregations.ToExceptionGroupStatistic(criteria.FrameInterval);
            }
            catch (Exception ex)
            {
                throw ex.Handle(criteria);
            }
        }

        /// <summary>
        /// Deletes the index.
        /// </summary>
        public void DeleteIndex()
        {
            try
            {
                elasticClient.DeleteIndex();
            }
            catch (Exception ex)
            {
                throw ex.Handle();
            }
        }

        #endregion

        #region Analytics

        //public List<ApiEventGroupStatistic> GetApiEventGroupStatistic(EventLogGroupingCriteria criteria, IDictionary<string, string> propertyMapping = null)
        //{
        //    if (criteria != null)
        //    {
        //        var groupByNames = ToGroupByNames(criteria);

        //        var timeFrameInterval = criteria.TimeFrameInterval.DBToString();

        //        if (groupByNames.Count == 0)
        //        {
        //            throw new InvalidObjectException("groupByNames");
        //        }
        //        else
        //        {
        //            return this.CountingByGroup<ApiEventGroupStatistic>(collection_Event,
        //                timeFrame: QueryTimeFrame.ThisNDays(criteria.TimeFrameDays > 0 ? criteria.TimeFrameDays : 7),
        //                filters: ToQueryFilters(criteria.Filters),
        //                groupByNames: groupByNames,
        //                timezone: criteria.TimezoneOffset,
        //                propertyMapping: propertyMapping);
        //        }
        //    }

        //    return null;
        //}

        #endregion

        /// <summary>
        /// Aggregations the search.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">The type.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns>AggregationsQueryResult&lt;T&gt;.</returns>
        public AggregationsQueryResult<T> AggregationSearch<T>(string type, SearchCriteria criteria)
        {
            return elasticClient.AggregationQuery<T>(type, criteria);
        }

        /// <summary>
        /// To the exception information.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>QueryResult&lt;ExceptionInfo&gt;.</returns>
        protected static QueryResult<ExceptionInfo> ToExceptionInfo(QueryResult<ElasticExceptionInfo> data)
        {
            return data != null ? new QueryResult<ExceptionInfo>
            {
                Shards = data.Shards,
                Total = data.Total,
                Hits = data.Hits.Select((x) => ToExceptionInfo(x)).ToList()
            } : null;
        }

        /// <summary>
        /// To the exception information.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>RawDataItem&lt;ExceptionInfo&gt;.</returns>
        protected static RawDataItem<ExceptionInfo> ToExceptionInfo(RawDataItem<ElasticExceptionInfo> data)
        {
            return data != null ? new RawDataItem<ExceptionInfo>
            {
                Id = data.Id,
                Index = data.Index,
                Type = data.Type,
                Source = ToExceptionInfo(data.Source)
            } : null;
        }

        /// <summary>
        /// To the exception information.
        /// </summary>
        /// <param name="exceptionInfo">The exception information.</param>
        /// <returns>ExceptionInfo.</returns>
        public static ExceptionInfo ToExceptionInfo(ElasticExceptionInfo exceptionInfo)
        {
            if (exceptionInfo != null)
            {
                return new ExceptionInfo
                {
                    Code = exceptionInfo.Code,
                    CreatedStamp = exceptionInfo.CreatedStamp,
                    Data = exceptionInfo.Data.TryParseToJToken(),
                    ExceptionType = exceptionInfo.ExceptionType,
                    Key = exceptionInfo.Key,
                    Level = exceptionInfo.Level,
                    Message = exceptionInfo.Message,
                    ServerIdentifier = exceptionInfo.ServerIdentifier,
                    ServiceIdentifier = exceptionInfo.ServiceIdentifier,
                    Source = exceptionInfo.Source,
                    StackTrace = exceptionInfo.StackTrace,
                    TargetSite = exceptionInfo.TargetSite,
                    InnerException = JsonConvert.DeserializeObject<ExceptionInfo>(exceptionInfo.InnerException)
                };
            }

            return null;
        }

        /// <summary>
        /// To the exception information.
        /// </summary>
        /// <param name="exceptionInfo">The exception information.</param>
        /// <returns>ElasticExceptionInfo.</returns>
        public static ElasticExceptionInfo ToExceptionInfo(ExceptionInfo exceptionInfo)
        {
            if (exceptionInfo != null)
            {
                return new ElasticExceptionInfo
                {
                    Code = exceptionInfo.Code,
                    CreatedStamp = exceptionInfo.CreatedStamp,
                    Data = exceptionInfo.Data?.ToString(),
                    ExceptionType = exceptionInfo.ExceptionType,
                    Key = exceptionInfo.Key,
                    Level = exceptionInfo.Level,
                    Message = exceptionInfo.Message,
                    ServerIdentifier = exceptionInfo.ServerIdentifier,
                    ServiceIdentifier = exceptionInfo.ServiceIdentifier,
                    Source = exceptionInfo.Source,
                    StackTrace = exceptionInfo.StackTrace,
                    TargetSite = exceptionInfo.TargetSite,
                    InnerException = JsonConvert.SerializeObject(exceptionInfo.InnerException)
                };
            }

            return null;
        }

        /// <summary>
        /// To the API trace log.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>QueryResult&lt;ApiTraceLog&gt;.</returns>
        protected static QueryResult<ApiTraceLog> ToApiTraceLog(QueryResult<ElasticTraceLog> data)
        {
            return data != null ? new QueryResult<ApiTraceLog>
            {
                Shards = data.Shards,
                Total = data.Total,
                Hits = data.Hits.Select((x) => ToApiTraceLog(x)).ToList()
            } : null;
        }

        /// <summary>
        /// To the API trace log.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>RawDataItem&lt;ApiTraceLog&gt;.</returns>
        protected static RawDataItem<ApiTraceLog> ToApiTraceLog(RawDataItem<ElasticTraceLog> data)
        {
            return data != null ? new RawDataItem<ApiTraceLog>
            {
                Id = data.Id,
                Index = data.Index,
                Type = data.Type,
                Source = ToApiTraceLog(data.Source)
            } : null;
        }

        /// <summary>
        /// To the API trace log.
        /// </summary>
        /// <param name="traceLog">The trace log.</param>
        /// <returns>ApiTraceLog.</returns>
        public static ApiTraceLog ToApiTraceLog(ElasticTraceLog traceLog)
        {
            return traceLog == null ? null : new ApiTraceLog
            {
                CreatedStamp = traceLog.CreatedStamp,
                ServiceName = traceLog.ServiceName,
                EntryStamp = traceLog.EntryStamp,
                ExceptionKey = traceLog.ExceptionKey,
                ExitStamp = traceLog.ExitStamp,
                MethodFullName = traceLog.MethodFullName,
                TraceId = traceLog.TraceId,
                TraceSequence = traceLog.TraceSequence,
                InnerTraces = JsonExtension.TryDeserializeAsObject<List<ApiTraceLogPiece>>(traceLog.InnerTraces)
            };
        }

        /// <summary>
        /// To the elastic trace log.
        /// </summary>
        /// <param name="traceLog">The trace log.</param>
        /// <returns>ElasticTraceLog.</returns>
        public static ElasticTraceLog ToElasticTraceLog(ApiTraceLog traceLog)
        {
            return traceLog == null ? null : new ElasticTraceLog
            {
                CreatedStamp = traceLog.CreatedStamp,
                ServiceName = traceLog.ServiceName,
                EntryStamp = traceLog.EntryStamp,
                ExceptionKey = traceLog.ExceptionKey,
                ExitStamp = traceLog.ExitStamp,
                MethodFullName = traceLog.MethodFullName,
                TraceId = traceLog.TraceId,
                TraceSequence = traceLog.TraceSequence,
                InnerTraces = JsonConvert.SerializeObject(traceLog.InnerTraces)
            };
        }
    }
}
