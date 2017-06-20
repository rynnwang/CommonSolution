using System;
using System.Collections.Generic;
using Beyova.ExceptionSystem;

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
        protected class CacheItem<T> : IExpirable
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

        #endregion Inner Container

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
        /// The handle exception. Return  bool value indicating whether needs to throw it.
        /// </summary>
        protected Func<BaseException, bool> handleException;

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; protected set; }

        /// <summary>
        /// Gets the failure expiration in second. If entity is failed to get, use this expiration if specified, otherwise use <see cref="ICacheParameter.ExpirationInSecond" />.
        /// </summary>
        /// <value>The failure expiration in second.</value>
        public long FailureExpirationInSecond { get; protected set; }

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheContainer{TKey, TEntity}" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="retrieveEntity">The retrieve entity.</param>
        /// <param name="capacity">The capacity.</param>
        /// <param name="expirationInSecond">The expiration in second.</param>
        /// <param name="failureExpirationInSecond">The failure expiration in second.</param>
        /// <param name="equalityComparer">The equality comparer.</param>
        /// <param name="handleException">The handle exception. Bool value indicating whether needs to throw it.</param>
        public CacheContainer(string name, Func<TKey, TEntity> retrieveEntity, int? capacity = null, long? expirationInSecond = null, long? failureExpirationInSecond = null, IEqualityComparer<TKey> equalityComparer = null, Func<BaseException, bool> handleException = null)
        {
            this.Name = name;
            this.Capacity = (capacity.HasValue && capacity.Value > 1) ? capacity : null;
            this.ExpirationInSecond = (expirationInSecond.HasValue && expirationInSecond.Value > 0) ? expirationInSecond : null;
            this.FailureExpirationInSecond = ((failureExpirationInSecond.HasValue && failureExpirationInSecond.Value > 0) ? failureExpirationInSecond : null) ?? this.ExpirationInSecond ?? 30;

            equalityComparer = equalityComparer ?? EqualityComparer<TKey>.Default;
            this.container = capacity == null ? new SequencedKeyDictionary<TKey, CacheItem<TEntity>>(equalityComparer) : new SequencedKeyDictionary<TKey, CacheItem<TEntity>>(capacity.Value, equalityComparer);
            this.retrieveEntity = retrieveEntity;
            this.handleException = handleException;

            CacheRealm.RegisterCacheContainer(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheContainer{TKey, TEntity}"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="retrieveEntity">The retrieve entity.</param>
        /// <param name="cacheParameter">The cache parameter.</param>
        /// <param name="capacity">The capacity.</param>
        /// <param name="equalityComparer">The equality comparer.</param>
        /// <param name="handleException">The handle exception.</param>
        public CacheContainer(string name, Func<TKey, TEntity> retrieveEntity, ICacheParameter cacheParameter, int? capacity = null, IEqualityComparer<TKey> equalityComparer = null, Func<BaseException, bool> handleException = null)
            : this(name, retrieveEntity, capacity, cacheParameter.ExpirationInSecond, cacheParameter.FailureExpirationInSecond, equalityComparer, handleException)
        {
        }

        #endregion Constructors

        #region protected methods

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

        /// <summary>
        /// Internals the update.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="ifNotExistsThenInsert">if set to <c>true</c> [if not exists then insert].</param>
        /// <returns>System.Nullable&lt;DateTime&gt;.</returns>
        protected DateTime? InternalUpdate(TKey key, TEntity entity, bool ifNotExistsThenInsert)
        {
            DateTime? result = null;

            if (key != null)
            {
                try
                {
                    lock (itemChangeLocker)
                    {
                        if (container.ContainsKey(key))
                        {
                            container.Remove(key);
                        }
                        else
                        {
                            if (!ifNotExistsThenInsert)
                            {
                                return result;
                            }
                        }

                        result = this.ExpirationInSecond.HasValue ? DateTime.UtcNow.AddSeconds(this.ExpirationInSecond.Value) as DateTime? : null;
                        container.Add(key, new CacheItem<TEntity> { Value = entity, ExpiredStamp = result });
                        InternalMaintainCapacity();
                    }
                }
                catch (Exception ex)
                {
                    throw ex.Handle(key);
                }
            }

            return result;
        }

        #endregion protected methods

        /// <summary>
        /// Updates the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>System.Nullable&lt;DateTime&gt;.</returns>
        public DateTime? Update(TKey key, TEntity entity)
        {
            return InternalUpdate(key, entity, false);
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
                                if (handleException != null)
                                {
                                    BaseException exception = ex.Handle(key);
                                    if (handleException(exception))
                                    {
                                        throw exception;
                                    }
                                }
                                else
                                {
                                    container.Merge(key, new CacheItem<TEntity> { Value = default(TEntity), ExpiredStamp = DateTime.UtcNow.AddSeconds(this.FailureExpirationInSecond) as DateTime? });
                                    InternalMaintainCapacity();
                                }
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