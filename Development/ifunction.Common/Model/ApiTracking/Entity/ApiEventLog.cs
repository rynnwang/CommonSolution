﻿using System;

namespace ifunction.ApiTracking.Model
{
    /// <summary>
    /// Class ApiEventLog.
    /// </summary>
    public class ApiEventLog : ApiEventLogBase
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public Guid? Key { get; set; }

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
        /// Gets or sets the created stamp.
        /// </summary>
        /// <value>The created stamp.</value>
        public DateTime CreatedStamp { get; set; }

        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        /// <value>The length.</value>
        public int? ContentLength { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiEventLog"/> class.
        /// </summary>
        public ApiEventLog(ApiEventLogBase eventLogBase)
            : base(eventLogBase)
        {
            this.Key = Guid.NewGuid();
            this.CreatedStamp = DateTime.UtcNow;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiEventLog"/> class.
        /// </summary>
        public ApiEventLog()
            : this(null)
        {
        }
    }
}
