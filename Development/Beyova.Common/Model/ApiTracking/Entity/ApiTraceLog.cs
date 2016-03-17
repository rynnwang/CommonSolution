using System;
using System.Collections.Generic;
using Beyova.ExceptionSystem;

namespace Beyova.ApiTracking
{
    /// <summary>
    /// Class ApiTraceLog.
    /// </summary>
    public class ApiTraceLog: ApiTraceLogPiece
    {
        /// <summary>
        /// Gets or sets the trace identifier.
        /// </summary>
        /// <value>The trace identifier.</value>
        public string TraceId { get; set; }

        /// <summary>
        /// Gets or sets the created stamp.
        /// </summary>
        /// <value>The created stamp.</value>
        public DateTime CreatedStamp { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiTraceLog"/> class.
        /// </summary>
        public ApiTraceLog()
        {
            this.CreatedStamp = DateTime.UtcNow;
            this.InnerTraces = new List<ApiTraceLogPiece>();
        }
    }
}
