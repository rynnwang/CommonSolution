using System;
using Beyova.ApiTracking;
using Beyova.ExceptionSystem;

namespace Beyova
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Beyova.IApiTracking" />
    public class ConsoleLogger : IApiTracking
    {
        /// <summary>
        /// The format
        /// </summary>
        private const string format = "{0}\r\n{1}";

        /// <summary>
        /// Logs the API event asynchronous.
        /// </summary>
        /// <param name="eventLog">The event log.</param>
        public void LogApiEvent(ApiEventLog eventLog)
        {
            if (eventLog != null)
            {
                Console.WriteLine(format, DateTime.Now.ToFullDateTimeString(), eventLog.ToJson());
            }
        }

        /// <summary>
        /// Logs the API trace log asynchronous.
        /// </summary>
        /// <param name="traceLog">The trace log.</param>
        public void LogApiTraceLog(ApiTraceLog traceLog)
        {
            if (traceLog != null)
            {
                Console.WriteLine(format, DateTime.Now.ToFullDateTimeString(), traceLog.ToJson());
            }
        }

        /// <summary>
        /// Logs the exception asynchronous.
        /// </summary>
        /// <param name="exceptionInfo">The exception information.</param>
        public void LogException(ExceptionInfo exceptionInfo)
        {
            if (exceptionInfo != null)
            {
                Console.WriteLine(format, DateTime.Now.ToFullDateTimeString(), exceptionInfo.ToJson());
            }
        }

        /// <summary>
        /// Logs the message asynchronous.
        /// </summary>
        /// <param name="message">The message.</param>
        public void LogMessage(string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                Console.WriteLine(format, DateTime.Now.ToFullDateTimeString(), message);
            }
        }
    }
}