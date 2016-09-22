using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Beyova.ApiTracking
{
    /// <summary>
    /// Class ApiTraceLogPiece.
    /// </summary>
    public class ApiTraceLogPiece
    {
        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        /// <value>The parent.</value>
        [JsonIgnore]
        internal ApiTraceLogPiece Parent { get; set; }

        /// <summary>
        /// Gets or sets the full name of the method.
        /// </summary>
        /// <value>The full name of the method.</value>
        public string MethodFullName { get; set; }

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
        /// Gets or sets the exception key.
        /// </summary>
        /// <value>The exception key.</value>
        public Guid? ExceptionKey { get; set; }

        /// <summary>
        /// Gets or sets the detail.
        /// </summary>
        /// <value>The detail.</value>
        public List<ApiTraceLogPiece> InnerTraces { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiTraceLogPiece"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="methodFullName">Full name of the method.</param>
        /// <param name="entryStamp">The entry stamp.</param>
        internal ApiTraceLogPiece(ApiTraceLogPiece parent, string methodFullName = null, DateTime? entryStamp = null)
            : this()
        {
            this.Parent = parent;
            this.MethodFullName = methodFullName;
            this.EntryStamp = entryStamp ?? DateTime.UtcNow;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiTraceLog"/> class.
        /// </summary>
        public ApiTraceLogPiece()
        {
            this.InnerTraces = new List<ApiTraceLogPiece>();
        }
    }
}
