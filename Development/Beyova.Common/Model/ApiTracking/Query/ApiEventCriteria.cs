using System;

namespace Beyova.ApiTracking
{
    /// <summary>
    /// Class ApiEventCriteria.
    /// </summary>
    public class ApiEventCriteria : ApiEventLogBase, ICriteria
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public Guid? Key { get; set; }

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
        /// Gets or sets the API duration from.
        /// </summary>
        /// <value>The API duration from.</value>
        public double? ApiDurationFrom { get; set; }

        /// <summary>
        /// Gets or sets the API duration to.
        /// </summary>
        /// <value>The API duration to.</value>
        public double? ApiDurationTo { get; set; }

        /// <summary>
        /// Gets or sets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiEventCriteria" /> class.
        /// </summary>
        public ApiEventCriteria(ApiEventLogBase eventLogBase)
            : base(eventLogBase)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiEventCriteria"/> class.
        /// </summary>
        public ApiEventCriteria()
            : this(null)
        {
        }
    }
}
