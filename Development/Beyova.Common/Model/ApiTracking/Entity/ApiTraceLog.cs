using System;
using System.Collections.Generic;

namespace Beyova.ApiTracking
{
    /// <summary>
    /// Class ApiTraceLog.
    /// </summary>
    public class ApiTraceLog : ApiTraceLogPiece
    {
        /// <summary>
        /// Gets or sets the trace identifier.
        /// </summary>
        /// <value>The trace identifier.</value>
        public string TraceId { get; set; }

        /// <summary>
        /// Gets or sets the trace sequence.
        /// </summary>
        /// <value>The trace sequence.</value>
        public int? TraceSequence { get; set; }

        /// <summary>
        /// Gets or sets the created stamp.
        /// </summary>
        /// <value>The created stamp.</value>
        public DateTime CreatedStamp { get; set; }

        /// <summary>
        /// Gets or sets the name of the service.
        /// </summary>
        /// <value>The name of the service.</value>
        public string ServiceName { get; set; }

        /// <summary>
        /// Gets or sets the name of the server.
        /// </summary>
        /// <value>The name of the server.</value>
        public string ServerName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiTraceLog" /> class.
        /// </summary>
        /// <param name="serverName">Name of the server.</param>
        /// <param name="serviceName">Name of the service.</param>
        /// <param name="entryStamp">The entry stamp.</param>
        internal ApiTraceLog(string serverName = null, string serviceName = null, DateTime? entryStamp = null) : this()
        {
            this.ServerName = serverName.SafeToString(EnvironmentCore.ServerName);
            this.ServiceName = serviceName.SafeToString(EnvironmentCore.ProjectName);
            this.EntryStamp = entryStamp ?? DateTime.UtcNow;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiTraceLog" /> class.
        /// </summary>
        public ApiTraceLog()
        {
            this.CreatedStamp = DateTime.UtcNow;
            this.InnerTraces = new List<ApiTraceLogPiece>();
        }
    }
}
