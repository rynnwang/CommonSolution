using System;
using System.Collections.Generic;
using System.Threading;

namespace Beyova.Cache
{
    /// <summary>
    /// Class ApiCacheContainer.
    /// </summary>
    public class ApiCacheContainer : CacheContainer<string, string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiCacheContainer" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="cacheParameter">The cache parameter.</param>
        internal ApiCacheContainer(string name, ApiCacheParameter cacheParameter)
            : base(name, null, cacheParameter, cacheParameter.Capacity)
        {
        }

        /// <summary>
        /// Gets the cache result.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        internal bool GetCacheResult(string key, out string result)
        {
            return InternalGet(key, out result);
        }
    }
}