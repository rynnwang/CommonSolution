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
        /// <summary>
        /// Queries the log API event.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;ApiEventLog&gt;.</returns>
        List<ApiEventLog> QueryLogApiEvent(ApiEventCriteria criteria);

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
    }
}
