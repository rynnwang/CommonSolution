using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ifunction.ApiTracking.Model;
using ifunction.ExceptionSystem;
using ifunction.RestApi;

namespace ifunction
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
        /// The instance
        /// </summary>
        public Microsoft.VisualBasic.Logging.FileLogTraceListener traceListener;

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
                applicationName = applicationName.SafeToString("Default");
                DiagnosticFileLogger result;

                if (!diagnosticFileLoggers.TryGetValue(applicationName, out result))
                {
                    result = new DiagnosticFileLogger(applicationName);
                    diagnosticFileLoggers.Add(applicationName, result);
                }

                return result;
            }
        }

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DiagnosticFileLogger"/> class.
        /// </summary>
        /// <param name="applicationName">Name of the application.</param>
        private DiagnosticFileLogger(string applicationName)
        {
            applicationName.CheckEmptyString("applicationName");

            traceListener = new Microsoft.VisualBasic.Logging.FileLogTraceListener
            {
                AutoFlush = true,
                Append = true,
                BaseFileName = applicationName,
                Encoding = Encoding.UTF8,
                Location = Microsoft.VisualBasic.Logging.LogFileLocation.Custom,
                CustomLocation = EnvironmentCore.LogDirectory,
                LogFileCreationSchedule = Microsoft.VisualBasic.Logging.LogFileCreationScheduleOption.Daily
            };
        }

        #endregion

        /// <summary>
        /// Logs the message with time stamp.
        /// </summary>
        /// <param name="content">The content.</param>
        public void LogMessageWithTimeStamp(string content)
        {
            if (!string.IsNullOrWhiteSpace(content))
            {
                WriteContent(string.Format(CultureInfo.InvariantCulture, "[{0}] {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), content));
            }
        }

        /// <summary>
        /// Logs the exception.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="data">The data.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public void LogException(Exception ex, object data = null)
        {
            try
            {
                string errorInfo = ex.FormatToString(data);
                WriteContent(errorInfo);
            }
            catch { }
        }

        /// <summary>
        /// Writes the content.
        /// </summary>
        /// <param name="content">The content.</param>
        private void WriteContent(string content)
        {
            traceListener.WriteLine(content);
        }

        /// <summary>
        /// Logs the API event.
        /// </summary>
        /// <param name="eventLog">The event log.</param>
        /// <returns>System.Nullable&lt;Guid&gt;.</returns>
        public Guid? LogApiEvent(ApiEventLog eventLog)
        {
            if (eventLog != null)
            {
                WriteContent(eventLog.ApiEventLogToString());
                return eventLog.Key;
            }

            return null;
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
                WriteContent(exception.FormatToString());
                return exception.Key;
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
                WriteContent(traceLog.ApiTraceLogToString());
            }
        }

        /// <summary>
        /// Logs the API event asynchronous.
        /// </summary>
        /// <param name="eventLog">The event log.</param>
        public void LogApiEventAsync(ApiEventLog eventLog)
        {
            if (eventLog != null)
            {
                Task.Factory.StartNew(() => LogApiEvent(eventLog));
            }
        }

        /// <summary>
        /// Logs the exception asynchronous.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="serviceIdentifier">The service identifier.</param>
        /// <param name="serverIdentifier">The server identifier.</param>
        public void LogExceptionAsync(BaseException exception, string serviceIdentifier = null, string serverIdentifier = null)
        {
            if (exception != null)
            {
                Task.Factory.StartNew(() => LogException(exception, serviceIdentifier, serverIdentifier));
            }
        }

        /// <summary>
        /// Logs the event.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="eventLog">The event log.</param>
        public void LogEvent<T>(T eventLog)
        {
            if (eventLog != null)
            {
                WriteContent(eventLog.ToJson());
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
                Task.Factory.StartNew(() => LogEvent(eventLog));
            }
        }

        /// <summary>
        /// Logs the API trace log asynchronous.
        /// </summary>
        /// <param name="traceLog">The trace log.</param>
        public void LogApiTraceLogAsync(ApiTraceLog traceLog)
        {
            if (traceLog != null)
            {
                Task.Factory.StartNew(() => LogEvent(traceLog));
            }
        }
    }
}
