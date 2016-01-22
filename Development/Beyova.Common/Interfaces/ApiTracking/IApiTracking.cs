using Beyova.ApiTracking;
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
        void LogApiEvent(ApiEventLog eventLog);

        /// <summary>
        /// Logs the exception asynchronous.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="serviceIdentifier">The service identifier.</param>
        /// <param name="serverIdentifier">The server identifier.</param>
        void LogException(BaseException exception, string serviceIdentifier = null, string serverIdentifier = null);

        /// <summary>
        /// Logs the exception asynchronous.
        /// </summary>
        /// <param name="exceptionInfo">The exception information.</param>
        void LogException(ExceptionInfo exceptionInfo);

        /// <summary>
        /// Logs the API trace log asynchronous.
        /// </summary>
        /// <param name="traceLog">The trace log.</param>
        void LogApiTraceLog(ApiTraceLog traceLog);

        /// <summary>
        /// Logs the message asynchronous.
        /// </summary>
        /// <param name="message">The message.</param>
        void LogMessage(string message);
    }
}
