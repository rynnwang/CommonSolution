using System;
using System.Collections.Generic;

namespace Beyova.Cache
{
    /// <summary>
    /// Class CacheRealm.
    /// </summary>
    internal static class CacheRealm
    {
        /// <summary>
        /// The containers
        /// </summary>
        private static MatrixList<Type, ICacheContainer> containers = new MatrixList<Type, ICacheContainer>();

        /// <summary>
        /// Registers the cache container.
        /// </summary>
        /// <typeparam name="TKey">The type of the t key.</typeparam>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="cacheContainer">The cache container.</param>
        internal static void RegisterCacheContainer<TKey, TEntity>(CacheContainer<TKey, TEntity> cacheContainer)
        {
            if (cacheContainer != null)
            {
                containers.Add(typeof(TEntity), cacheContainer);
            }
        }

        /// <summary>
        /// Gets the containers.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>List&lt;ICacheContainer&gt;.</returns>
        public static List<ICacheContainer> GetContainers<T>()
        {
            return GetContainers(typeof(T));
        }

        /// <summary>
        /// Gets the containers.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>List&lt;ICacheContainer&gt;.</returns>
        public static List<ICacheContainer> GetContainers(Type type)
        {
            return type != null ? containers[type] : null;
        }

        /// <summary>
        /// Clears all.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public static int ClearAll()
        {
            int result = 0;
            foreach (var one in containers)
            {
                foreach (var item in one.Value)
                {
                    result += item.Count;
                    item.Clear();
                }
            }

            return result;
        }

        /// <summary>
        /// Clears the cache.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static int ClearCache<T>()
        {
            return ClearCache(typeof(T));
        }

        /// <summary>
        /// Clears the cache.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>System.Int32.</returns>
        public static int ClearCache(Type type)
        {
            List<ICacheContainer> cacheContainers;
            int result = 0;

            if (containers.TryGetValue(type, out cacheContainers))
            {
                foreach (var one in cacheContainers)
                {
                    result += one.Count;
                    one.Clear();
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the summary.
        /// </summary>
        /// <returns>MatrixList&lt;CacheContainerSummary&gt;.</returns>
        public static MatrixList<CacheContainerSummary> GetSummary()
        {
            var result = new MatrixList<CacheContainerSummary>();

            foreach (var item in containers)
            {
                result.Add(item.Key.FullName, item.Value.ToCacheContainerSummary());
            }

            return result;
        }

        /// <summary>
        /// Gets the summary.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>System.Collections.Generic.List&lt;Beyova.Cache.CacheContainerSummary&gt;.</returns>
        public static List<CacheContainerSummary> GetSummary<T>()
        {
            return ToCacheContainerSummary(GetContainers(typeof(T)));
        }

        /// <summary>
        /// To the cache container summary.
        /// </summary>
        /// <param name="containers">The containers.</param>
        /// <returns>System.Collections.Generic.List&lt;Beyova.Cache.CacheContainerSummary&gt;.</returns>
        private static List<CacheContainerSummary> ToCacheContainerSummary(this IList<ICacheContainer> containers)
        {
            var result = new List<CacheContainerSummary>();

            if (containers != null)
            {
                foreach (var one in containers)
                {
                    result.AddIfNotNull(one.ToCacheContainerSummary());
                }
            }

            return result;
        }

        /// <summary>
        /// To the cache container summary.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <returns>CacheContainerSummary.</returns>
        private static CacheContainerSummary ToCacheContainerSummary(this ICacheContainer container)
        {
            return container == null ? null : new CacheContainerSummary
            {
                Capacity = container.Capacity,
                Count = container.Count,
                ExpirationInSecond = container.ExpirationInSecond,
                Name = container.Name
            };
        }
    }
}