using System;

namespace Beyova.ApiTracking.Model
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
        /// Gets or sets the time zone.
        /// </summary>
        /// <value>The time zone.</value>
        string TimeZone { get; set; }
    }
}
