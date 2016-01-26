using System;
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
        /// Gets the API trace log by key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>ApiTraceLog.</returns>
        ApiTraceLog GetApiTraceLogByKey(Guid? key);

        #endregion

        #region Time frame based query

        /// <summary>
        /// Gets the API event statistic.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;ApiEventGroupStatistic&gt;.</returns>
        List<ApiEventGroupStatistic> GetApiEventStatistic(ApiEventStatisticCriteria criteria);

        /// <summary>
        /// Gets the API event group statistic.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;ApiEventGroupStatistic&gt;.</returns>
        List<ApiEventGroupStatistic> GetApiEventGroupStatistic(ApiEventGroupingCriteria criteria);

        /// <summary>
        /// Gets the API exception statistic.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;ExceptionGroupStatistic&gt;.</returns>
        List<ExceptionGroupStatistic> GetApiExceptionStatistic(ExceptionStatisticCriteria criteria);

        /// <summary>
        /// Gets the API exception grouping statistic.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;ExceptionGroupStatistic&gt;.</returns>
        List<ExceptionGroupStatistic> GetApiExceptionGroupingStatistic(ExceptionGroupingCriteria criteria);

        #endregion
    }
}
