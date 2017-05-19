using System;
using System.Collections.Generic;
using System.Threading;

namespace Beyova.Cache
{
    /// <summary>
    /// Enum ApiCacheStatus
    /// </summary>
    public enum ApiCacheStatus
    {
        /// <summary>
        /// The no cache
        /// </summary>
        NoCache = 0,
        /// <summary>
        /// The use cache
        /// </summary>
        UseCache = 1,
        /// <summary>
        /// The update cache
        /// </summary>
        UpdateCache = 2
    }
}