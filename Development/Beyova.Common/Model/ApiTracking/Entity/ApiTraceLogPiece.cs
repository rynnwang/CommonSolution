using System;
using System.Collections.Generic;
using Beyova.ExceptionSystem;

namespace Beyova.ApiTracking
{
    /// <summary>
    /// Class ApiTraceLogPiece.
    /// </summary>
    public class ApiTraceLogPiece
    {
        /// <summary>
        /// Gets or sets the full name of the method.
        /// </summary>
        /// <value>The full name of the method.</value>
        public string MethodFullName { get; set; }

        /// <summary>
        /// Gets or sets the method parameters.
        /// </summary>
        /// <value>The method parameters.</value>
        public Dictionary<string, object> MethodParameters { get; set; }

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
        /// Gets or sets the detail.
        /// </summary>
        /// <value>The detail.</value>
        public List<ApiTraceLogPiece> InnerTraces { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiTraceLog"/> class.
        /// </summary>
        public ApiTraceLogPiece()
        {
            this.InnerTraces = new List<ApiTraceLogPiece>();
        }
    }
}
