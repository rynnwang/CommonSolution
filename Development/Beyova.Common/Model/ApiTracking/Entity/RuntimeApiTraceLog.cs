using System;
using System.Collections.Generic;
using Beyova.ExceptionSystem;

namespace Beyova.ApiTracking
{
    /// <summary>
    /// Class RuntimeApiTraceLog.
    /// </summary>
    internal class RuntimeApiTraceLog
    {
        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        /// <value>The parent.</value>
        internal RuntimeApiTraceLog Parent { get; set; }

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
        /// Gets or sets the children.
        /// </summary>
        /// <value>The children.</value>
        internal List<RuntimeApiTraceLog> Children { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RuntimeApiTraceLog"/> class.
        /// </summary>
        internal RuntimeApiTraceLog(RuntimeApiTraceLog parent, string methodFullName, Dictionary<string, object> methodParameters, DateTime? entryStamp)
        {
            this.Parent = parent;
            this.MethodFullName = methodFullName;
            this.MethodParameters = methodParameters;
            this.EntryStamp = entryStamp;
            this.Children = new List<RuntimeApiTraceLog>();
        }

        /// <summary>
        /// Fills the exit information.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="exitStamp">The exit stamp.</param>
        /// <param name="appIdentifier">The application identifier.</param>
        /// <param name="serverIdentifier">The server identifier.</param>
        /// <param name="level">The level.</param>
        /// <param name="operatorIdentifier">The operator identifier.</param>
        internal void FillExitInfo(BaseException exception = null, DateTime? exitStamp = null, string appIdentifier = null, string serverIdentifier = null, ExceptionInfo.ExceptionCriticality level = ExceptionInfo.ExceptionCriticality.Error, string operatorIdentifier = null)
        {
            this.ExitStamp = exitStamp ?? DateTime.UtcNow;
            this.Exception = exception.ToExceptionInfo(appIdentifier, serverIdentifier, level, operatorIdentifier);
        }

        /// <summary>
        /// Fills the exit information.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="exitStamp">The exit stamp.</param>
        internal void FillExitInfo(ExceptionInfo exception = null, DateTime? exitStamp = null)
        {
            this.ExitStamp = exitStamp ?? DateTime.UtcNow;
            this.Exception = exception;
        }
    }
}
