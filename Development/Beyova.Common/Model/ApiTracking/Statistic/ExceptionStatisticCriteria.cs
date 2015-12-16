using System;
using Beyova.ExceptionSystem;

namespace Beyova.ApiTracking.Model
{
    /// <summary>
    /// Class ExceptionStatisticCriteria.
    /// </summary>
    public class ExceptionStatisticCriteria : ExceptionBase, ITimeFrame
    {
        /// <summary>
        /// Gets or sets the frame interval.
        /// </summary>
        /// <value>The frame interval.</value>
        public TimeInterval FrameInterval { get; set; }

        /// <summary>
        /// Gets or sets the last n days.
        /// </summary>
        /// <value>The last n days.</value>
        public int? LastNDays { get; set; }

        /// <summary>
        /// Gets or sets to stamp.
        /// </summary>
        /// <value>To stamp.</value>
        public DateTime? ToStamp { get; set; }

        /// <summary>
        /// Gets or sets from stamp.
        /// </summary>
        /// <value>From stamp.</value>
        public DateTime? FromStamp { get; set; }

        /// <summary>
        /// Gets or sets the time zone.
        /// </summary>
        /// <value>The time zone.</value>
        public string TimeZone { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionStatisticCriteria" /> class.
        /// </summary>
        /// <param name="exceptionBase">The exception base.</param>
        public ExceptionStatisticCriteria(ExceptionBase exceptionBase)
            : base(exceptionBase)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionStatisticCriteria"/> class.
        /// </summary>
        public ExceptionStatisticCriteria()
            : this(null)
        {
        }
    }
}
