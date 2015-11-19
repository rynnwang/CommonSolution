using ifunction.ApiTracking.Model;
using ifunction.ExceptionSystem;

namespace ifunction
{
    /// <summary>
    /// Interface IApiTracking
    /// </summary>
    public interface IApiTracking
    {
        /// <summary>
        /// Logs the API event asynchronous.
        /// </summary>
        /// <param name="eventLog">The event log.</param>
        void LogApiEventAsync(ApiEventLog eventLog);

        /// <summary>
        /// Logs the exception asynchronous.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="serviceIdentifier">The service identifier.</param>
        /// <param name="serverIdentifier">The server identifier.</param>
        void LogExceptionAsync(BaseException exception, string serviceIdentifier = null, string serverIdentifier = null);

        /// <summary>
        /// Logs the API trace log asynchronous.
        /// </summary>
        /// <param name="traceLog">The trace log.</param>
        void LogApiTraceLogAsync(ApiTraceLog traceLog);
    }
}
