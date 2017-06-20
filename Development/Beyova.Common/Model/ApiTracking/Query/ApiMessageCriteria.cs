using System;

namespace Beyova.ApiTracking
{
    /// <summary>
    /// Class ApiMessageCriteria.
    /// </summary>
    public class ApiMessageCriteria : BaseCriteria
    {
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; set; }

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
        /// Initializes a new instance of the <see cref="ApiMessageCriteria"/> class.
        /// </summary>
        public ApiMessageCriteria()
        {
        }
    }
}