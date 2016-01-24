using System;
using System.Collections.Generic;
using System.IO;
using Beyova;
using Beyova.ApiTracking;
using Beyova.ExceptionSystem;

namespace Beyova.Elastic
{
    /// <summary>
    /// Class ElasticApiTracking.
    /// </summary>
    public class ElasticApiTracking : IApiTracking
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
                            CountryName = geoInfo.countryName,
                            CityName = geoInfo.city
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
            FillLocationInfo(eventLog);
            elasticClient.Index(apiEventType, eventLog);
        }

        /// <summary>
        /// Logs the API trace log asynchronous.
        /// </summary>
        /// <param name="traceLog">The trace log.</param>
        public void LogApiTraceLog(ApiTraceLog traceLog)
        {
            elasticClient.Index("TraceLog", traceLog);
        }

        /// <summary>
        /// Logs the exception asynchronous.
        /// </summary>
        /// <param name="exceptionInfo">The exception information.</param>
        public void LogException(ExceptionInfo exceptionInfo)
        {
            elasticClient.Index(exceptionType, exceptionInfo);
        }

        /// <summary>
        /// Logs the exception asynchronous.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="serviceIdentifier">The service identifier.</param>
        /// <param name="serverIdentifier">The server identifier.</param>
        public void LogException(BaseException exception, string serviceIdentifier = null, string serverIdentifier = null)
        {
            elasticClient.Index(exceptionType, exception.ToExceptionInfo(serviceIdentifier, serverIdentifier));
        }

        /// <summary>
        /// Logs the message asynchronous.
        /// </summary>
        /// <param name="message">The message.</param>
        public void LogMessage(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            elasticClient.Index(messageType, new { CreatedStamp = DateTime.UtcNow, Message = message.SafeToString() });
        }

        #endregion

        #region Get

        /// <summary>
        /// Queries the API event log.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>QueryResult&lt;ApiEventLog&gt;.</returns>
        public QueryResult<ApiEventLog> QueryApiEventLog(ApiEventCriteria criteria)
        {
            try
            {
                return elasticClient.Query<ApiEventLog>(apiEventType, new
                {
                    query = criteria.ToElasticCriteria(),
                    sort = new List<object> { new { CreatedStamp = "desc" } }
                });
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
        public QueryResult<ExceptionInfo> QueryException(ExceptionCriteria criteria)
        {
            try
            {
                return elasticClient.Query<ExceptionInfo>(apiEventType, new
                {
                    query = criteria.ToElasticCriteria(),
                    sort = new List<object> { new { CreatedStamp = "desc" } }
                });
            }
            catch (Exception ex)
            {
                throw ex.Handle("QueryException", criteria);
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
    }
}
