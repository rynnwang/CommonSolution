using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Beyova.ApiTracking;
using Beyova.ExceptionSystem;
using Beyova.RestApi;

namespace Beyova
{
    /// <summary>
    /// Class DiagnosticFileLogger. This class cannot be inherited.
    /// </summary>
    public sealed class DiagnosticFileLogger : IApiTracking
    {
        /// <summary>
        /// The diagnostic file loggers
        /// </summary>
        private static Dictionary<string, DiagnosticFileLogger> diagnosticFileLoggers = new Dictionary<string, DiagnosticFileLogger>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// The initialize locker
        /// </summary>
        private static object initializeLocker = new object();

        /// <summary>
        /// Creates the or update diagnostic file logger.
        /// </summary>
        /// <param name="applicationName">Name of the application.</param>
        /// <returns>DiagnosticFileLogger.</returns>
        public static DiagnosticFileLogger CreateOrUpdateDiagnosticFileLogger(string applicationName = null)
        {
            lock (initializeLocker)
            {
                applicationName = applicationName.SafeToString("default");
                DiagnosticFileLogger result;

                if (!diagnosticFileLoggers.TryGetValue(applicationName, out result))
                {
                    result = new DiagnosticFileLogger(applicationName);
                    diagnosticFileLoggers.Add(applicationName, result);
                }

                return result;
            }
        }

        private System.IO.StreamWriter writer;

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DiagnosticFileLogger"/> class.
        /// </summary>
        /// <param name="applicationName">Name of the application.</param>
        private DiagnosticFileLogger(string applicationName)
        {
            try
            {
                applicationName.CheckEmptyString("applicationName");
                writer = new StreamWriter(Path.Combine(EnvironmentCore.LogDirectory, applicationName));
            }
            catch (InvalidObjectException)
            {
                throw;
            }
            catch
            {
                try
                {
                    writer = new StreamWriter(Path.Combine(EnvironmentCore.LogDirectory, applicationName + "(1)"));
                }
                catch (Exception ex)
                {
                    throw ex.Handle(applicationName);
                }
            }
        }

        #endregion

        /// <summary>
        /// Logs the message with time stamp.
        /// </summary>
        /// <param name="content">The content.</param>
        private void InternalLogMessageWithTimeStamp(string content)
        {
            if (!string.IsNullOrWhiteSpace(content))
            {
                InternalWriteContent(string.Format(CultureInfo.InvariantCulture, "[{0}] {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), content));
            }
        }

        /// <summary>
        /// Logs the message.
        /// </summary>
        /// <param name="content">The content.</param>
        private void InternalLogMessage(string content)
        {
            InternalWriteContent(content.SafeToString());
        }

        /// <summary>
        /// Writes the content.
        /// </summary>
        /// <param name="content">The content.</param>
        private void InternalWriteContent(string content)
        {
            writer.WriteLine(content);
        }

        /// <summary>
        /// Logs the API event.
        /// </summary>
        /// <param name="eventLog">The event log.</param>
        /// <returns>System.Nullable&lt;Guid&gt;.</returns>
        private Guid? InternalLogApiEvent(ApiEventLog eventLog)
        {
            if (eventLog != null)
            {
                InternalWriteContent(eventLog.ApiEventLogToString());
                return eventLog.Key;
            }

            return null;
        }

        /// <summary>
        /// Logs the API trace log.
        /// </summary>
        /// <param name="traceLog">The trace log.</param>
        private void InternalLogApiTraceLog(ApiTraceLog traceLog)
        {
            if (traceLog != null)
            {
                InternalWriteContent(traceLog.ApiTraceLogToString());
            }
        }

        /// <summary>
        /// Logs the API event asynchronous.
        /// </summary>
        /// <param name="eventLog">The event log.</param>
        public void LogApiEvent(ApiEventLog eventLog)
        {
            if (eventLog != null)
            {
                Task.Factory.StartNew(() => InternalLogApiEvent(eventLog));
            }
        }

        /// <summary>
        /// Logs the event.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="eventLog">The event log.</param>
        private void InternalLogEvent<T>(T eventLog)
        {
            if (eventLog != null)
            {
                InternalWriteContent(eventLog.ToJson());
            }
        }

        /// <summary>
        /// Logs the event asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="container">The container.</param>
        /// <param name="eventLog">The event log.</param>
        public void LogEventAsync<T>(string container, T eventLog)
        {
            if (eventLog != null)
            {
                Task.Factory.StartNew(() => InternalLogEvent(eventLog));
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
                Task.Factory.StartNew(() => InternalLogApiTraceLog(traceLog));
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
                Task.Factory.StartNew(() => InternalWriteContent(exceptionInfo.ToJson()));
            }
        }

        /// <summary>
        /// Logs the message asynchronous.
        /// </summary>
        /// <param name="message">The message.</param>
        public void LogMessage(string message)
        {
            Task.Factory.StartNew(() => InternalLogMessage(message));
        }
    }
}
