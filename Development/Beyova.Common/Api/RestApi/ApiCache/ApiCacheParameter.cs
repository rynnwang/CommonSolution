using System;
using System.Collections.Generic;
using System.Threading;

namespace Beyova.Cache
{
    /// <summary>
    /// Class ApiCacheParameter.
    /// </summary>
    public struct ApiCacheParameter : ICacheParameter
    {
        /// <summary>
        /// Gets or sets a value indicating whether [cached by parameterized identity].
        /// </summary>
        /// <value><c>true</c> if [cached by parameterized identity]; otherwise, <c>false</c>.</value>
        public bool CachedByParameterizedIdentity { get; set; }

        /// <summary>
        /// Gets or sets the capacity.
        /// </summary>
        /// <value>
        /// The capacity.
        /// </value>
        public int? Capacity { get; set; }

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