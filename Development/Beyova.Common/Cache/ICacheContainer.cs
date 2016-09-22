using System;
using System.Collections.Generic;

namespace Beyova.Cache
{
    /// <summary>
    /// Interface ICacheContainer
    /// </summary>
    /// <typeparam name="TKey">The type of the t key.</typeparam>
    /// <typeparam name="TEntity">The type of the t entity.</typeparam>
    public interface ICacheContainer<TKey, TEntity> : ICacheContainer
    {
        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>TEntity.</returns>
        TEntity Get(TKey key);

        /// <summary>
        /// Gets the expired stamp.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>System.Nullable&lt;DateTime&gt;.</returns>
        DateTime? GetExpiredStamp(TKey key, out TEntity entity);
    }

    /// <summary>
    /// Interface ICacheContainer
    /// </summary>
    public interface ICacheContainer
    {
        /// <summary>
        /// The capacity
        /// </summary>
        int? Capacity { get; }

        /// <summary>
        /// The expiration in second
        /// </summary>
        long? ExpirationInSecond { get; }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        int Count { get; }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        void Clear();

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; }
    }
}