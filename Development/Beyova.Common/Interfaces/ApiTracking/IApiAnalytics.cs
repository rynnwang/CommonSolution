using System.Collections.Generic;
using Beyova.ApiTracking;
using Beyova.ExceptionSystem;

namespace Beyova
{
    /// <summary>
    /// Interface IApiAnalytics
    /// </summary>
    public interface IApiAnalytics
    {
        #region Query Entity

        /// <summary>
        /// Queries the API event.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;ApiEventLog&gt;.</returns>
        List<ApiEventLog> QueryApiEvent(ApiEventCriteria criteria);

        /// <summary>
        /// Queries the exception.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;ExceptionInfo&gt;.</returns>
        List<ExceptionInfo> QueryException(ExceptionCriteria criteria);

        /// <summary>
        /// Gets the API trace log by identifier.
        /// </summary>
        /// <param name="traceId">The trace identifier.</param>
        /// <returns>List&lt;ApiTraceLog&gt;.</returns>
        List<ApiTraceLog> GetApiTraceLogById(string traceId);

        /// <summary>
        /// Queries the API message.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;ApiMessage&gt;.</returns>
        List<ApiMessage> QueryApiMessage(ApiMessageCriteria criteria);

        #endregion Query Entity

        #region Time frame based query

        /// <summary>
        /// Gets the API event statistic.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>System.Collections.Generic.List&lt;Beyova.ApiTracking.GroupStatistic&gt;.</returns>
        List<GroupStatistic> GetApiEventStatistic(ApiEventStatisticCriteria criteria);

        /// <summary>
        /// Gets the API exception statistic.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>System.Collections.Generic.List&lt;Beyova.ApiTracking.GroupStatistic&gt;.</returns>
        List<GroupStatistic> GetApiExceptionStatistic(ExceptionStatisticCriteria criteria);

        /// <summary>
        /// Gets the API event group statistic.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>System.Collections.Generic.List&lt;Beyova.ApiTracking.ApiEventGroupStatistic&gt;.</returns>
        List<ApiEventGroupStatistic> GetApiEventGroupStatistic(ApiEventGroupingCriteria criteria);

        /// <summary>
        /// Gets the API exception grouping statistic.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>System.Collections.Generic.List&lt;Beyova.ApiTracking.ExceptionGroupStatistic&gt;.</returns>
        List<ExceptionGroupStatistic> GetApiExceptionGroupingStatistic(ExceptionGroupingCriteria criteria);

        #endregion Time frame based query
    }
}