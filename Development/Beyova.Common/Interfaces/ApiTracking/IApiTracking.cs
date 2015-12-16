using Beyova.ApiTracking.Model;
using Beyova.ExceptionSystem;

namespace Beyova
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
        /// Logs the exception asynchronous.
        /// </summary>
        /// <param name="exceptionInfo">The exception information.</param>
        void LogExceptionAsync(ExceptionInfo exceptionInfo);

        /// <summary>
        /// Logs the API trace log asynchronous.
        /// </summary>
        /// <param name="traceLog">The trace log.</param>
        void LogApiTraceLogAsync(ApiTraceLog traceLog);

        /// <summary>
        /// Logs the message asynchronous.
        /// </summary>
        /// <param name="message">The message.</param>
        void LogMessageAsync(string message);
    }
}
