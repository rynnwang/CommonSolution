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
        /// Gets or sets the raw trace log.
        /// </summary>
        /// <value>The raw trace log.</value>
        internal ApiTraceLog RawTraceLog { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RuntimeApiTraceLog"/> class.
        /// </summary>
        internal RuntimeApiTraceLog()
        {
        }
    }
}
