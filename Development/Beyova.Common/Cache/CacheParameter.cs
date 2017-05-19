using System;
using System.Collections.Generic;

namespace Beyova.Cache
{
    /// <summary>
    /// Class CacheParameter.
    /// </summary>
    public struct CacheParameter : ICacheParameter
    {
        /// <summary>
        /// The expiration in second
        /// </summary>
        /// <value>The expiration in second.</value>
        public long? ExpirationInSecond { get; set; }

        /// <summary>
        /// Gets the failure expiration in second. If entity is failed to get, use this expiration if specified, otherwise use <see cref="ICacheParameter.ExpirationInSecond" />.
        /// </summary>
        /// <value>The failure expiration in second.</value>
        public long FailureExpirationInSecond { get; set; }
    }
}