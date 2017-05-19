using System;

namespace Beyova
{
    /// <summary>
    /// Class EntitySynchronizationRequest.
    /// </summary>
    public class EntitySynchronizationRequest
    {
        /// <summary>
        /// Gets or sets the last synchronized stamp.
        /// </summary>
        /// <value>The last synchronized stamp.</value>
        public DateTime? LastSynchronizedStamp { get; set; }

        /// <summary>
        /// Gets or sets the last synchronized key.
        /// </summary>
        /// <value>The last synchronized key.</value>
        public Guid? LastSynchronizedKey { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [upserts only].
        /// </summary>
        /// <value><c>true</c> if [upserts only]; otherwise, <c>false</c>.</value>
        public bool UpsertsOnly { get; set; }

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        /// <value>The amount.</value>
        public int? Amount { get; set; }
    }
}
