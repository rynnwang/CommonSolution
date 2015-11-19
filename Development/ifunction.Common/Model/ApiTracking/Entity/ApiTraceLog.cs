using System;
using ifunction.ExceptionSystem;

namespace ifunction.ApiTracking.Model
{
    /// <summary>
    /// Class ApiTraceLog.
    /// </summary>
    public class ApiTraceLog
    {
        /// <summary>
        /// Gets or sets the created stamp.
        /// </summary>
        /// <value>The created stamp.</value>
        public DateTime CreatedStamp { get; set; }

        /// <summary>
        /// Gets or sets the full name of the method.
        /// </summary>
        /// <value>The full name of the method.</value>
        public string MethodFullName { get; set; }

        /// <summary>
        /// Gets or sets the method parameter.
        /// </summary>
        /// <value>The method parameter.</value>
        public object MethodParameter { get; set; }

        /// <summary>
        /// Gets or sets the entry stamp.
        /// </summary>
        /// <value>The entry stamp.</value>
        public DateTime? EntryStamp { get; set; }

        /// <summary>
        /// Gets or sets the exit stamp.
        /// </summary>
        /// <value>The exit stamp.</value>
        public DateTime? ExitStamp { get; set; }

        /// <summary>
        /// Gets or sets the exception.
        /// </summary>
        /// <value>The exception.</value>
        public ExceptionInfo Exception { get; set; }

        /// <summary>
        /// Gets or sets the inner trace log.
        /// </summary>
        /// <value>The inner trace log.</value>
        public ApiTraceLog InnerTraceLog { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiTraceLog"/> class.
        /// </summary>
        public ApiTraceLog()
        {
            this.CreatedStamp = DateTime.UtcNow;
        }

        /// <summary>
        /// Appends the specified API trace log.
        /// </summary>
        /// <param name="apiTraceLog">The API trace log.</param>
        public void Append(ApiTraceLog apiTraceLog)
        {
            if (apiTraceLog != null)
            {
                if (this.InnerTraceLog == null)
                {
                    this.InnerTraceLog = apiTraceLog;
                }
                else
                {
                    this.InnerTraceLog.Append(apiTraceLog);
                }
            }
        }
    }
}
