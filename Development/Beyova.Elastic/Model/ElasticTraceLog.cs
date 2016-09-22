using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beyova;
using Beyova.ExceptionSystem;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Beyova.Elastic
{
    /// <summary>
    /// Class ElasticTraceLog.
    /// </summary>
    public class ElasticTraceLog
    {
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
        /// Gets or sets the inner traces.
        /// </summary>
        /// <value>The inner traces.</value>
        public string InnerTraces { get; set; }

        /// <summary>
        /// Gets or sets the created stamp.
        /// </summary>
        /// <value>The created stamp.</value>
        public DateTime CreatedStamp { get; set; }

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
        /// Gets or sets the name of the service.
        /// </summary>
        /// <value>The name of the service.</value>
        public string ServiceName { get; set; }
    }
}
