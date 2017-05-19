using System;
using System.Collections.Generic;

namespace Beyova
{
    /// <summary>
    /// Class EntitySynchronizationResponse.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EntitySynchronizationResponse<T>
    {
        /// <summary>
        /// Gets or sets the last stamp.
        /// </summary>
        /// <value>The last stamp.</value>
        public DateTime? LastStamp { get; set; }

        /// <summary>
        /// Gets or sets the last key.
        /// </summary>
        /// <value>The last key.</value>
        public Guid? LastKey { get; set; }

        /// <summary>
        /// Gets or sets the upserts.
        /// </summary>
        /// <value>The upserts.</value>
        public List<T> Upserts { get; set; }

        /// <summary>
        /// Gets or sets the removals.
        /// </summary>
        /// <value>The removals.</value>
        public List<string> Removals { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntitySynchronizationResponse{T}" /> class.
        /// </summary>
        public EntitySynchronizationResponse()
        {
            Upserts = new List<T>();
            Removals = new List<string>();
        }
    }
}
