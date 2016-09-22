using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Beyova.Cache
{
    /// <summary>
    /// Class CacheContainerSummary.
    /// </summary>
    public class CacheContainerSummary : ICacheContainer
    {
        /// <summary>
        /// The capacity
        /// </summary>
        /// <value>The capacity.</value>
        public int? Capacity { get; set; }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count { get; set; }

        /// <summary>
        /// The expiration in second
        /// </summary>
        /// <value>The expiration in second.</value>
        public long? ExpirationInSecond { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Clear()
        {
            //Do nothing.
        }
    }
}