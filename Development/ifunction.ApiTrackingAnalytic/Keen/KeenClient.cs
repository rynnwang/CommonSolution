using System;
using System.Collections.Generic;
using ifunction.KeenSDK.Core.AddOns;
using ifunction.KeenSDK.Model;
using ifunction.Model;
using ifunction.RestfulApi;

namespace ifunction.ApiTrackingAnalytic.KeenAnalytics
{
    /// <summary>
    /// Class KeenClient.
    /// </summary>
    public class KeenClient : KeenSDK.Core.KeenClient, IApiTracking
    {
        /// <summary>
        /// The collection_ event
        /// </summary>
        const string collection_Event = "Event";

        /// <summary>
        /// The collection_ exception
        /// </summary>
        const string collection_Exception = "Exception";

        /// <summary>
        /// The collection_ trace
        /// </summary>
        const string collection_Trace = "Trace";

        /// <summary>
        /// The client
        /// </summary>
        public KeenSDK.Core.KeenClient OriginalClient { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeenClient" /> class.
        /// <remarks>
        /// It would read configuration in configuration/AppConfig.xml for following values:
        /// <list type="number">
        /// <item>KeenProjectId: string</item>
        /// <item>KeenMasterKey: string</item>
        /// <item>KeenReadKey: string</item>
        /// <item>KeenWriteKey: string</item>
        ///  <item>KeenUrl: string</item>
        /// </list>
        /// </remarks>
        /// </summary>
        public KeenClient()
            : this(
                Framework.GetConfiguration("KeenProjectId"),
                Framework.GetConfiguration("KeenMasterKey"),
                Framework.GetConfiguration("KeenReadKey"),
                Framework.GetConfiguration("KeenWriteKey"),
                Framework.GetConfiguration("KeenUrl")
                )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeenClient" /> class.
        /// </summary>
        /// <param name="projectId">The project identifier.</param>
        /// <param name="masterKey">The master key.</param>
        /// <param name="readKey">The read key.</param>
        /// <param name="writeKey">The write key.</param>
        /// <param name="keenUrl">The keen URL.</param>
        public KeenClient(string projectId, string masterKey = null, string readKey = null, string writeKey = null, string keenUrl = null)
            : base(projectId, masterKey, readKey, writeKey, string.IsNullOrWhiteSpace(keenUrl) ? null : new Uri(keenUrl))
        {
        }

        /// <summary>
        /// Logs the API event.
        /// </summary>
        /// <param name="eventLog">The event log.</param>
        public Guid? LogApiEvent(ApiEventLog eventLog)
        {
            this.AddEvent(collection_Event, eventLog, new IpParser("IpAddress", "Location"));
            return eventLog.Key;
        }

        /// <summary>
        /// Logs the exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="serviceIdentifier">The service identifier.</param>
        /// <param name="serverIdentifier">The server identifier.</param>
        /// <returns>System.Nullable&lt;Guid&gt;.</returns>
        public Guid? LogException(BaseException exception, string serviceIdentifier = null, string serverIdentifier = null)
        {
            if (exception != null)
            {
                var exceptionInfo = exception.ToExceptionInfo(serviceIdentifier, serverIdentifier);
                this.AddEvent(collection_Exception, exceptionInfo);
                return exceptionInfo.Key;
            }

            return null;
        }

        /// <summary>
        /// Logs the API trace log.
        /// </summary>
        /// <param name="traceLog">The trace log.</param>
        public void LogApiTraceLog(ApiTraceLog traceLog)
        {
            if (traceLog != null)
            {
                this.AddEvent(collection_Trace, traceLog);
            }
        }

        /// <summary>
        /// Queries the exception.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="count">The count.</param>
        /// <returns>IEnumerable&lt;ExceptionInfo&gt;.</returns>
        public IEnumerable<ExceptionInfo> QueryException(ExceptionInfoCriteria criteria, int count = 100)
        {
            return this.QueryObjectAs<ExceptionInfo>(collection_Exception, filters: ToQueryFilters(criteria), countByLatest: count);
        }

        /// <summary>
        /// Queries the API event log.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="count">The count.</param>
        /// <returns>IEnumerable&lt;ApiEventLog&gt;.</returns>
        public IEnumerable<ApiEventLog> QueryApiEventLog(ApiEventLogCriteria criteria, int count = 100)
        {
            return this.QueryObjectAs<ApiEventLog>(collection_Event, filters: ToQueryFilters(criteria), countByLatest: count);
        }

        /// <summary>
        /// Gets the API event group statistic.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="propertyMapping">The property mapping.</param>
        /// <returns>List&lt;ApiEventGroupStatistic&gt;.</returns>
        /// <exception cref="ifunction.InvalidObjectException">groupByNames</exception>
        public IList<ApiEventGroupStatistic> GetApiEventGroupStatistic(EventLogGroupingCriteria criteria, IDictionary<string, string> propertyMapping = null)
        {
            if (criteria != null)
            {
                var groupByNames = ToGroupByNames(criteria);

                var timeFrameInterval = criteria.TimeFrameInterval.DBToString();

                if (groupByNames.Count == 0)
                {
                    throw new InvalidObjectException("groupByNames");
                }
                else
                {
                    return this.CountingByGroup<ApiEventGroupStatistic>(collection_Event,
                        timeFrame: QueryTimeFrame.ThisNDays(criteria.TimeFrameDays > 0 ? criteria.TimeFrameDays : 7),
                        filters: ToQueryFilters(criteria.Filters),
                        groupByNames: groupByNames,
                        timezone: criteria.TimezoneOffset,
                        propertyMapping: propertyMapping);
                }
            }

            return null;
        }

        #region Protected methods

        /// <summary>
        /// To the query filters.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;QueryFilter&gt;.</returns>
        protected static List<QueryFilter> ToQueryFilters(ExceptionInfoCriteria criteria)
        {
            List<QueryFilter> result = new List<QueryFilter>();

            if (criteria != null)
            {
                if (criteria.Key != null)
                {
                    result.Add(new QueryFilter("Key", QueryFilter.FilterOperator.Equal, criteria.Key));
                }
                else
                {
                    if (criteria.Level != null)
                    {
                        result.Add(new QueryFilter("Level", QueryFilter.FilterOperator.Equal, criteria.Level));
                    }

                    if (criteria.Message != null)
                    {
                        result.Add(new QueryFilter("Message", QueryFilter.FilterOperator.Contains, criteria.Message));
                    }

                    if (criteria.OperatorKey != null)
                    {
                        result.Add(new QueryFilter("OperatorKey", QueryFilter.FilterOperator.Equal, criteria.OperatorKey));
                    }

                    if (criteria.ExceptionType != null)
                    {
                        result.Add(new QueryFilter("ExceptionType", QueryFilter.FilterOperator.Equal, criteria.ExceptionType));
                    }

                    if (criteria.AppIdentifier != null)
                    {
                        result.Add(new QueryFilter("AppIdentifier", QueryFilter.FilterOperator.Equal, criteria.AppIdentifier));
                    }

                    if (criteria.Code != null)
                    {
                        result.Add(new QueryFilter("Code.Major", QueryFilter.FilterOperator.Equal, (int)criteria.Code.Value));
                    }

                    if (criteria.FromStamp != null)
                    {
                        result.Add(new QueryFilter("CreatedStamp", QueryFilter.FilterOperator.GreaterThanOrEqual, criteria.FromStamp));
                    }

                    if (criteria.ToStamp != null)
                    {
                        result.Add(new QueryFilter("CreatedStamp", QueryFilter.FilterOperator.LessThan, criteria.ToStamp));
                    }

                    if (criteria.Source != null)
                    {
                        result.Add(new QueryFilter("Source", QueryFilter.FilterOperator.Contains, criteria.Source));
                    }

                    if (criteria.Data != null)
                    {
                        result.Add(new QueryFilter("Data", QueryFilter.FilterOperator.Contains, criteria.Data));
                    }

                    if (criteria.TargetSite != null)
                    {
                        result.Add(new QueryFilter("TargetSite", QueryFilter.FilterOperator.Contains, criteria.TargetSite));
                    }

                    if (criteria.StackTrace != null)
                    {
                        result.Add(new QueryFilter("StackTrace", QueryFilter.FilterOperator.Contains, criteria.StackTrace));
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// To the filters.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;QueryFilter&gt;.</returns>
        protected static List<QueryFilter> ToQueryFilters(ApiEventLogCriteria criteria)
        {
            List<QueryFilter> result = new List<QueryFilter>();

            if (criteria != null)
            {
                if (criteria.Key != null)
                {
                    result.Add(new QueryFilter("Key", QueryFilter.FilterOperator.Equal, criteria.Key));
                }
                else
                {
                    if (criteria.ApiFullName != null)
                    {
                        result.Add(new QueryFilter("ApiFullName", QueryFilter.FilterOperator.Equal, criteria.ApiFullName));
                    }

                    if (criteria.AppIdentifier != null)
                    {
                        result.Add(new QueryFilter("AppIdentifier", QueryFilter.FilterOperator.Equal, criteria.AppIdentifier));
                    }

                    if (criteria.Action != null)
                    {
                        result.Add(new QueryFilter("Action", QueryFilter.FilterOperator.Equal, criteria.Action));
                    }

                    if (criteria.ClientIdentifier != null)
                    {
                        result.Add(new QueryFilter("ClientIdentifier", QueryFilter.FilterOperator.Equal, criteria.ClientIdentifier));
                    }

                    if (criteria.ExceptionKey != null)
                    {
                        result.Add(new QueryFilter("ExceptionKey", QueryFilter.FilterOperator.Equal, criteria.ExceptionKey));
                    }

                    if (criteria.HttpMethod != null)
                    {
                        result.Add(new QueryFilter("HttpMethod", QueryFilter.FilterOperator.Equal, criteria.HttpMethod));
                    }

                    if (criteria.ResourceName != null)
                    {
                        result.Add(new QueryFilter("ResourceName", QueryFilter.FilterOperator.Equal, criteria.ResourceName));
                    }

                    if (criteria.ResourceKey != null)
                    {
                        result.Add(new QueryFilter("ResourceKey", QueryFilter.FilterOperator.Equal, criteria.ResourceKey));
                    }

                    if (criteria.ResponseCode != null)
                    {
                        result.Add(new QueryFilter("ResponseCode", QueryFilter.FilterOperator.Equal, criteria.ResponseCode));
                    }

                    if (criteria.ServerIdentifier != null)
                    {
                        result.Add(new QueryFilter("ServerIdentifier", QueryFilter.FilterOperator.Equal, criteria.ServerIdentifier));
                    }

                    if (criteria.ServiceIdentifier != null)
                    {
                        result.Add(new QueryFilter("ServiceIdentifier", QueryFilter.FilterOperator.Equal, criteria.ServiceIdentifier));
                    }

                    if (criteria.FromStamp != null)
                    {
                        result.Add(new QueryFilter("CreatedStamp", QueryFilter.FilterOperator.GreaterThanOrEqual, criteria.FromStamp));
                    }

                    if (criteria.ToStamp != null)
                    {
                        result.Add(new QueryFilter("CreatedStamp", QueryFilter.FilterOperator.LessThan, criteria.ToStamp));
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// To the group by names.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;System.String&gt;.</returns>
        protected static List<string> ToGroupByNames(ApiEventLogGroupingCriteria criteria)
        {
            List<string> groupByNames = new List<string>();

            if (criteria != null)
            {
                if (criteria.GroupByResourceName)
                {
                    groupByNames.Add("ResourceName");
                }

                if (criteria.GroupByAction)
                {
                    groupByNames.Add("Action");
                }

                if (criteria.GroupByHttpMethod)
                {
                    groupByNames.Add("HttpMethod");
                }

                if (criteria.GroupByResponseCode)
                {
                    groupByNames.Add("ResponseCode");
                }

                if (criteria.GroupByServiceIdentifier)
                {
                    groupByNames.Add("ServiceIdentifier");
                }

                if (criteria.GroupByServerIdentifier)
                {
                    groupByNames.Add("ServerIdentifier");
                }

                if (criteria.GroupByAppIdentifier)
                {
                    groupByNames.Add("AppIdentifier");
                }

                if (criteria.GroupByLocation)
                {
                    groupByNames.Add("Location.city");
                    groupByNames.Add("Location.country");
                }
            }

            return groupByNames;
        }

        #endregion
    }
}
