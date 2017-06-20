using System;

namespace Beyova.ApiTracking
{
    /// <summary>
    /// Interface ITimeFrame
    /// </summary>
    public interface ITimeFrame
    {
        /// <summary>
        /// Gets or sets from stamp.
        /// </summary>
        /// <value>From stamp.</value>
        DateTime? FromStamp { get; set; }

        /// <summary>
        /// Gets or sets to stamp.
        /// </summary>
        /// <value>To stamp.</value>
        DateTime? ToStamp { get; set; }

        /// <summary>
        /// Gets or sets the time zone. Unit: minute
        /// </summary>
        /// <value>The time zone.</value>
        int? TimeZone { get; set; }

        /// <summary>
        /// Gets or sets the frame interval.
        /// </summary>
        /// <value>The frame interval.</value>
        TimeScope? FrameInterval { get; set; }
    }
}