using System;
using System.Collections.Generic;
using Beyova.ExceptionSystem;

namespace Beyova.Cache
{
    /// <summary>
    /// Class CacheContainerBase.
    /// </summary>
    public abstract class CacheContainerBase<T> : ICacheContainer
    {
        /// <summary>
        /// The container
        /// </summary>
        protected T container = default(T);

        /// <summary>
        /// The locker
        /// </summary>
        protected object locker = new object();

        /// <summary>
        /// The expiration in second
        /// </summary>
        public long? ExpirationInSecond { get; protected set; }

        /// <summary>
        /// The retrieve entity
        /// </summary>
        protected Func<T> retrieveEntity;

        /// <summary>
        /// The handle exception. Return  bool value indicating whether needs to throw it.
        /// </summary>
        protected Func<BaseException, bool> handleException;

        /// <summary>
        /// Gets the failure expiration in second. If entity is failed to get, use this expiration if specified, otherwise use <see cref="ICacheParameter.ExpirationInSecond" />.
        /// </summary>
        /// <value>The failure expiration in second.</value>
        public long FailureExpirationInSecond { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether [need refresh].
        /// </summary>
        /// <value><c>true</c> if [need refresh]; otherwise, <c>false</c>.</value>
        protected bool needRefresh;

        /// <summary>
        /// Gets or sets the expired stamp.
        /// </summary>
        /// <value>The expired stamp.</value>
        public DateTime? ExpiredStamp { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; protected set; }

        /// <summary>
        /// The capacity
        /// </summary>
        /// <value>The capacity.</value>
        public abstract int? Capacity { get; }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        public abstract int Count { get; }

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheContainer{TKey, TEntity}" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="retrieveEntity">The retrieve entity.</param>
        /// <param name="expirationInSecond">The expiration in second.</param>
        /// <param name="failureExpirationInSecond">The failure expiration in second.</param>
        /// <param name="handleException">The handle exception. Bool value indicating whether needs to throw it.</param>
        public CacheContainerBase(string name, Func<T> retrieveEntity, long? expirationInSecond = null, long? failureExpirationInSecond = null, Func<BaseException, bool> handleException = null)
        {
            this.ExpirationInSecond = (expirationInSecond.HasValue && expirationInSecond.Value > 0) ? expirationInSecond : null;
            this.FailureExpirationInSecond = ((failureExpirationInSecond.HasValue && failureExpirationInSecond.Value > 0) ? failureExpirationInSecond : null) ?? this.ExpirationInSecond ?? 30;

            this.retrieveEntity = retrieveEntity;
            this.handleException = handleException;
            this.Name = name;
        }

        #endregion

        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns>System.Collections.Generic.IReadOnlyList&lt;TEntity&gt;.</returns>
        protected T InternalGet()
        {
            if (retrieveEntity != null
                && (needRefresh || !ExpiredStamp.HasValue || ExpiredStamp.Value < DateTime.UtcNow))
            {
                lock (locker)
                {
                    if (needRefresh || !ExpiredStamp.HasValue || ExpiredStamp.Value < DateTime.UtcNow)
                    {
                        try
                        {
                            var tmp = retrieveEntity();
                            container = tmp;

                            if (ExpirationInSecond.HasValue)
                            {
                                ExpiredStamp = DateTime.UtcNow.AddSeconds(this.ExpirationInSecond.Value);
                                needRefresh = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            if (handleException == null || handleException(ex.Handle()))
                            {
                                ExpiredStamp = DateTime.UtcNow.AddSeconds(this.FailureExpirationInSecond);
                                needRefresh = false;
                                throw ex;
                            }
                        }
                    }
                }
            }

            return container;
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public void Reset()
        {
            lock (locker)
            {
                needRefresh = true;
            }
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public abstract void Clear();
    }
}