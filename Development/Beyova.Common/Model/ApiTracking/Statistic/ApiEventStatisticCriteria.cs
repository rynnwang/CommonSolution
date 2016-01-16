using System;

namespace Beyova.ApiTracking
{
    /// <summary>
    /// Class ApiEventStatisticCriteria.
    /// </summary>
    public class ApiEventStatisticCriteria : ApiEventLogBase, ITimeFrame
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
        /// Gets or sets from stamp.
        /// </summary>
        /// <value>From stamp.</value>
        public DateTime? FromStamp { get; set; }

        /// <summary>
        /// Gets or sets to stamp.
        /// </summary>
        /// <value>To stamp.</value>
        public DateTime? ToStamp { get; set; }

        /// <summary>
        /// Gets or sets the time zone.
        /// </summary>
        /// <value>The time zone.</value>
        public string TimeZone { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiEventStatisticCriteria"/> class.
        /// </summary>
        public ApiEventStatisticCriteria()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiEventStatisticCriteria"/> class.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        public ApiEventStatisticCriteria(ApiEventLogBase criteria)
            : base(criteria)
        {
        }
    }
}
