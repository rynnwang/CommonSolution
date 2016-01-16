using System;
using System.Collections.Generic;
using Beyova.ExceptionSystem;

namespace Beyova
{
    /// <summary>
    /// Class MaintanenceJobLog.
    /// </summary>
    public class MaintanenceJobLog : SimpleBaseObject
    {
        /// <summary>
        /// Gets or sets the maintanence key.
        /// </summary>
        /// <value>The maintanence key.</value>
        public Guid? MaintanenceKey { get; set; }

        /// <summary>
        /// Gets or sets the audits.
        /// </summary>
        /// <value>The audits.</value>
        public Dictionary<string, string> Audits { get; set; }

        /// <summary>
        /// Gets or sets the exception.
        /// </summary>
        /// <value>The exception.</value>
        public ExceptionInfo Exception { get; set; }

        /// <summary>
        /// Gets or sets the start stamp.
        /// </summary>
        /// <value>The start stamp.</value>
        public DateTime? StartStamp { get; set; }

        /// <summary>
        /// Gets or sets the end stamp.
        /// </summary>
        /// <value>The end stamp.</value>
        public DateTime? EndStamp { get; set; }
    }
}