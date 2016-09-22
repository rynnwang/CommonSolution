using System;
using System.Collections.Generic;

namespace Beyova.Cache
{
    /// <summary>
    /// Class CacheContainer.
    /// </summary>
    /// <typeparam name="TKey">The type of the t key.</typeparam>
    /// <typeparam name="TEntity">The type of the t entity.</typeparam>
    public class CacheContainer<TKey, TEntity> : ICacheContainer<TKey, TEntity>
    {
        #region Inner Container

        /// <summary>
        /// Class CacheItem.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        protected class CacheItem<T>
        {
            /// <summary>
            /// Gets or sets the value.
            /// </summary>
            /// <value>The value.</value>
            public T Value { get; set; }

            /// <summary>
            /// Gets or sets the expired stamp.
            /// </summary>
            /// <value>The expired stamp.</value>
            public DateTime? ExpiredStamp { get; set; }

            /// <summary>
            /// Gets a value indicating whether this instance is expired.
            /// </summary>
            /// <value><c>true</c> if this instance is expired; otherwise, <c>false</c>.</value>
            public bool IsExpired
            {
                get { return ExpiredStamp.HasValue && ExpiredStamp < DateTime.UtcNow; }
            }
        }

        #endregion

        /// <summary>
        /// The container
        /// </summary>
        protected SequencedKeyDictionary<TKey, CacheItem<TEntity>> container;

        /// <summary>
        /// The item change locker
        /// </summary>
        protected object itemChangeLocker = new object();

        /// <summary>
        /// The capacity
        /// </summary>
        public int? Capacity { get; protected set; }

        /// <summary>
        /// The expiration in second
        /// </summary>
        public long? ExpirationInSecond { get; protected set; }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count { get { return container.Count; } }

        /// <summary>
        /// The retrieve entity
        /// </summary>
        protected Func<TKey, TEntity> retrieveEntity;

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; protected set; }

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheContainer{TKey, TEntity}" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="capacity">The capacity.</param>
        /// <param name="expirationInSecond">The expiration in second.</param>
        /// <param name="retrieveEntity">The retrieve entity.</param>
        /// <param name="equalityComparer">The equality comparer.</param>
        public CacheContainer(string name, int? capacity, long? expirationInSecond, Func<TKey, TEntity> retrieveEntity, IEqualityComparer<TKey> equalityComparer = null)
        {
            this.Name = name;
            this.Capacity = (capacity.HasValue && capacity.Value > 1) ? capacity : null;
            this.ExpirationInSecond = (expirationInSecond.HasValue && expirationInSecond.Value > 0) ? expirationInSecond : null;

            equalityComparer = equalityComparer ?? EqualityComparer<TKey>.Default;
            this.container = capacity == null ? new SequencedKeyDictionary<TKey, CacheItem<TEntity>>(equalityComparer) : new SequencedKeyDictionary<TKey, CacheItem<TEntity>>(capacity.Value, equalityComparer);
            this.retrieveEntity = retrieveEntity;

            CacheRealm.RegisterCacheContainer(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheContainer{TKey, TEntity}" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="expirationInSecond">The expiration in second.</param>
        /// <param name="retrieveEntity">The retrieve entity.</param>
        /// <param name="equalityComparer">The equality comparer.</param>
        public CacheContainer(string name, long? expirationInSecond, Func<TKey, TEntity> retrieveEntity, IEqualityComparer<TKey> equalityComparer = null)
            : this(name, null, expirationInSecond, retrieveEntity, equalityComparer = null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheContainer{TKey, TEntity}" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="capacity">The capacity.</param>
        /// <param name="retrieveEntity">The retrieve entity.</param>
        /// <param name="equalityComparer">The equality comparer.</param>
        public CacheContainer(string name, int? capacity, Func<TKey, TEntity> retrieveEntity, IEqualityComparer<TKey> equalityComparer = null)
            : this(name, capacity, null, retrieveEntity, equalityComparer)
        {
        }

        #endregion

        #region internal methods

        /// <summary>
        /// Internals the maintain capacity.
        /// </summary>
        protected void InternalMaintainCapacity()
        {
            if (this.Capacity.HasValue && this.container.Count > this.Capacity.Value)
            {
                this.container.RemoveAt(0);
            }
        }

        #endregion

        /// <summary>
        /// Internals the update.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>System.Nullable&lt;DateTime&gt;.</returns>
        protected DateTime? InternalUpdate(TKey key, TEntity entity)
        {
            try
            {
                var expiredStamp = this.ExpirationInSecond.HasValue ? DateTime.UtcNow.AddSeconds(this.ExpirationInSecond.Value) as DateTime? : null;
                container.Add(key, new CacheItem<TEntity> { Value = entity, ExpiredStamp = expiredStamp });
                InternalMaintainCapacity();

                return expiredStamp;
            }
            catch (Exception ex)
            {
                throw ex.Handle(key);
            }
        }

        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>TEntity.</returns>
        public TEntity Get(TKey key)
        {
            TEntity entity;

            if (!InternalGet(key, out entity))
            {
                lock (itemChangeLocker)
                {
                    if (!InternalGet(key, out entity))
                    {
                        if (retrieveEntity != null)
                        {
                            try
                            {
                                entity = retrieveEntity(key);
                                container.Merge(key, new CacheItem<TEntity> { Value = entity, ExpiredStamp = this.ExpirationInSecond.HasValue ? DateTime.UtcNow.AddSeconds(this.ExpirationInSecond.Value) as DateTime? : null });
                                InternalMaintainCapacity();
                            }
                            catch (Exception ex)
                            {
                                throw ex.Handle(key);
                            }
                        }
                        else
                        {
                            entity = default(TEntity);
                        }
                    }
                }
            }

            return entity;
        }

        /// <summary>
        /// Gets the expired stamp.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>System.Nullable&lt;System.DateTime&gt;.</returns>
        public DateTime? GetExpiredStamp(TKey key, out TEntity entity)
        {
            CacheItem<TEntity> cachedObject;
            if (container.TryGetValue(key, out cachedObject))
            {
                entity = cachedObject.Value;
                return cachedObject.ExpiredStamp;
            }
            else
            {
                entity = default(TEntity);
                return null;
            }
        }

        /// <summary>
        /// Internals the get.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>TEntity.</returns>
        protected bool InternalGet(TKey key, out TEntity entity)
        {
            CacheItem<TEntity> cachedObject;
            if ((container.TryGetValue(key, out cachedObject) && !cachedObject.IsExpired))
            {
                entity = cachedObject.Value;
                return true;
            }
            else
            {
                entity = default(TEntity);
                return false;
            }
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            lock (itemChangeLocker)
            {
                this.container.Clear();
            }
        }
    }
}