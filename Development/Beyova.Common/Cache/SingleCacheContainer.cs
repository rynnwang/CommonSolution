using System;
using System.Collections.Generic;
using Beyova.ExceptionSystem;

namespace Beyova.Cache
{
    /// <summary>
    /// Class SingleCacheContainer.
    /// </summary>
    public class SingleCacheContainer<T> : CacheContainerBase<T>
        where T : class
    {
        /// <summary>
        /// Gets a value indicating whether this instance is expired.
        /// </summary>
        /// <value><c>true</c> if this instance is expired; otherwise, <c>false</c>.</value>
        public bool IsExpired
        {
            get { return ExpiredStamp.HasValue && ExpiredStamp < DateTime.UtcNow; }
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        public override int Count { get { return this.container == null ? 0 : 1; } }

        /// <summary>
        /// The capacity
        /// </summary>
        /// <value>The capacity.</value>
        public override int? Capacity { get { return null; } }

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheContainer{TKey, TEntity}" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="retrieveEntity">The retrieve entity.</param>
        /// <param name="expirationInSecond">The expiration in second.</param>
        /// <param name="failureExpirationInSecond">The failure expiration in second.</param>
        /// <param name="handleException">The handle exception. Bool value indicating whether needs to throw it.</param>
        public SingleCacheContainer(string name, Func<T> retrieveEntity, long? expirationInSecond = null, long? failureExpirationInSecond = null, Func<BaseException, bool> handleException = null)
            : base(name, retrieveEntity, expirationInSecond: expirationInSecond, failureExpirationInSecond: failureExpirationInSecond, handleException: handleException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SingleCacheContainer{T}" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="retrieveEntity">The retrieve entity.</param>
        /// <param name="cacheParameter">The cache parameter.</param>
        /// <param name="handleException">The handle exception. Bool value indicating whether needs to throw it.</param>
        public SingleCacheContainer(string name, Func<T> retrieveEntity, CacheParameter cacheParameter, Func<BaseException, bool> handleException = null)
            : base(name, retrieveEntity, 
                    expirationInSecond: cacheParameter.ExpirationInSecond, 
                    failureExpirationInSecond: cacheParameter.FailureExpirationInSecond, 
                    handleException: handleException)
        {
        }

        #endregion

        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns>System.Collections.Generic.IReadOnlyList&lt;TEntity&gt;.</returns>
        public T Get()
        {
            return InternalGet();
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public override void Clear()
        {
            lock (locker)
            {
                this.container = null;
                needRefresh = true;
            }
        }
    }
}