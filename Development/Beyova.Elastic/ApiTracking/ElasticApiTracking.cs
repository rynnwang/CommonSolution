using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Beyova;
using Beyova.ApiTracking;
using Beyova.ExceptionSystem;
using Newtonsoft.Json;

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
        /// Initializes a new instance of the <see cref="ElasticApiTracking" /> class.
        /// </summary>
        /// <param name="baseUrl">The base URL.</param>
        /// <param name="indexName">Name of the index.</param>
        /// <param name="geoDbPath">The geo database path.</param>
        public ElasticApiTracking(string baseUrl, string indexName, string geoDbPath = null)
        {
            elasticClient = new ElasticIndexClient(baseUrl, indexName);
            geoService = TryInitializeGeoService(geoDbPath);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ElasticIndexClient" /> class.
        /// This constructor would try to read configuration: <c>ElasticsearchUri</c>, <c>ElasticsearchIndex</c>, and <c>ElasticsearchGeoPath</c> (optional).
        /// </summary>
        public ElasticApiTracking() : this(Framework.GetConfiguration("ElasticsearchUri"),
            Framework.GetConfiguration("ElasticsearchIndex", "apitracking"),
            Framework.GetConfiguration("ElasticsearchGeoPath", string.Empty)
            )
        { }

        /// <summary>
        /// Tries the initialize geo service.
        /// </summary>
        /// <param name="geoDbPath">The geo database path.</param>
        /// <returns>LookupService.</returns>
        protected LookupService TryInitializeGeoService(string geoDbPath)
        {
            if (!string.IsNullOrWhiteSpace(geoDbPath) && File.Exists(geoDbPath))
            {
                return new LookupService(geoDbPath, LookupService.GEOIP_STANDARD);
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
                            Latitude = geoInfo.latitude,
                            Longitude = geoInfo.longitude,
                            CityName = geoInfo.city,
                            CountryName = geoInfo.countryName
                        };
                    }
                }
                catch { }
            }
        }

        #region IApiTracking

        /// <summary>
        /// Logs the API event asynchronous.
        /// </summary>
        /// <param name="eventLog">The event log.</param>
        public void LogApiEvent(ApiEventLog eventLog)
        {
            ThreadPool.QueueUserWorkItem((x) =>
            {
                try
                {
                    FillLocationInfo(eventLog);
                    elasticClient.Index(apiEventType, eventLog);
                }
                catch (Exception ex)
                {
                    Framework.ApiTracking.LogException(ex.Handle("LogApiEvent", eventLog));
                }
            });
        }

        /// <summary>
        /// Logs the API trace log asynchronous.
        /// </summary>
        /// <param name="traceLog">The trace log.</param>
        public void LogApiTraceLog(ApiTraceLog traceLog)
        {
            ThreadPool.QueueUserWorkItem((x) =>
            {
                try
                {
                    elasticClient.Index(traceLogType, traceLog);
                }
                catch (Exception ex)
                {
                    Framework.ApiTracking.LogException(ex.Handle("LogApiTraceLog", traceLog));
                }
            });
        }

        /// <summary>
        /// Logs the exception asynchronous.
        /// </summary>
        /// <param name="exceptionInfo">The exception information.</param>
        public void LogException(ExceptionInfo exceptionInfo)
        {
            ThreadPool.QueueUserWorkItem((x) =>
            {
                try
                {
                    elasticClient.Index(exceptionType, ToExceptionInfo(exceptionInfo));
                }
                catch (Exception ex)
                {
                    Framework.ApiTracking.LogException(ex.Handle("LogException", exceptionInfo));
                }
            });
        }

        /// <summary>
        /// Logs the exception asynchronous.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="serviceIdentifier">The service identifier.</param>
        /// <param name="serverIdentifier">The server identifier.</param>
        public void LogException(BaseException exception, string serviceIdentifier = null, string serverIdentifier = null)
        {
            LogException(exception.ToExceptionInfo(serviceIdentifier, serverIdentifier));
        }

        /// <summary>
        /// Logs the message asynchronous.
        /// </summary>
        /// <param name="message">The message.</param>
        public void LogMessage(string message)
        {
            if (string.IsNullOrWhiteSpace(message)) return;
            ThreadPool.QueueUserWorkItem((x) =>
            {
                try
                {
                    elasticClient.Index(messageType, new { CreatedStamp = DateTime.UtcNow, Message = message.SafeToString() });
                }
                catch (Exception ex)
                {
                    Framework.ApiTracking.LogException(ex.Handle("LogMessage", message));
                }
            });
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
                throw ex.Handle("QueryApiEventLog", criteria);
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
                throw ex.Handle("QueryException", criteria);
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
                return InternalQueryApiEventLog(criteria).Hits.Select((x) => { return x.Source; }).ToList();
            }
            catch (Exception ex)
            {
                throw ex.Handle("QueryApiEvent", criteria);
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
                return InternalQueryException(criteria).Hits.Select((x) => { return x.Source; }).ToList();
            }
            catch (Exception ex)
            {
                throw ex.Handle("QueryLogApiEvent", criteria);
            }
        }

        public ApiTraceLog GetApiTraceLogByKey(Guid? key)
        {
            throw new NotImplementedException();
        }

        public List<ApiEventGroupStatistic> GetApiEventStatistic(ApiEventStatisticCriteria criteria)
        {
            throw new NotImplementedException();
        }

        public List<ApiEventGroupStatistic> GetApiEventGroupStatistic(ApiEventGroupingCriteria criteria)
        {
            throw new NotImplementedException();
        }

        public List<ExceptionGroupStatistic> GetApiExceptionStatistic(ExceptionStatisticCriteria criteria)
        {
            throw new NotImplementedException();
        }

        public List<ExceptionGroupStatistic> GetApiExceptionGroupingStatistic(ExceptionGroupingCriteria criteria)
        {
            throw new NotImplementedException();
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
                    Data = exceptionInfo.Data,
                    ExceptionType = exceptionInfo.ExceptionType,
                    Key = exceptionInfo.Key,
                    Level = exceptionInfo.Level,
                    Message = exceptionInfo.Message,
                    ServerIdentifier = exceptionInfo.ServerIdentifier,
                    ServiceIdentifier = exceptionInfo.ServiceIdentifier,
                    Source = exceptionInfo.Source,
                    StackTrace = exceptionInfo.StackTrace,
                    TargetSite = exceptionInfo.TargetSite,
                    UserIdentifier = exceptionInfo.UserIdentifier,
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
                    Data = exceptionInfo.Data,
                    ExceptionType = exceptionInfo.ExceptionType,
                    Key = exceptionInfo.Key,
                    Level = exceptionInfo.Level,
                    Message = exceptionInfo.Message,
                    ServerIdentifier = exceptionInfo.ServerIdentifier,
                    ServiceIdentifier = exceptionInfo.ServiceIdentifier,
                    Source = exceptionInfo.Source,
                    StackTrace = exceptionInfo.StackTrace,
                    TargetSite = exceptionInfo.TargetSite,
                    UserIdentifier = exceptionInfo.UserIdentifier,
                    InnerException = JsonConvert.SerializeObject(exceptionInfo.InnerException)
                };
            }

            return null;
        }
    }
}
