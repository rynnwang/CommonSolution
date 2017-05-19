using System;
using System.Collections.Generic;
using Beyova.ExceptionSystem;

namespace Beyova.Cache
{
    /// <summary>
    /// Class CacheContainer.
    /// </summary>
    /// <typeparam name="TEntity">The type of the t entity.</typeparam>
    public class CollectionCacheContainer<TEntity> : CacheContainerBase<List<TEntity>>, ICacheContainer
    {
        /// <summary>
        /// The capacity
        /// </summary>
        /// <value>The capacity.</value>
        public override int? Capacity
        {
            get
            {
                return container.Capacity;
            }
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        public override int Count
        {
            get
            {
                return container.Count;
            }
        }

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Beyova.Cache.CacheContainer`2" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="retrieveEntity">The retrieve entity.</param>
        /// <param name="expirationInSecond">The expiration in second.</param>
        /// <param name="failureExpirationInSecond">The failure expiration in second.</param>
        /// <param name="handleException">The handle exception. Bool value indicating whether needs to throw it.</param>
        public CollectionCacheContainer(string name, Func<List<TEntity>> retrieveEntity, long? expirationInSecond = null, long? failureExpirationInSecond = null, Func<BaseException, bool> handleException = null)
            : base(name, retrieveEntity, expirationInSecond: expirationInSecond, failureExpirationInSecond: failureExpirationInSecond, handleException: handleException)
        {
            this.container = new List<TEntity>();
        }

        #endregion

        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns>System.Collections.Generic.IReadOnlyList&lt;TEntity&gt;.</returns>
        public IReadOnlyList<TEntity> GetAll()
        {
            return InternalGet().AsReadOnly();
        }

        /// <summary>
        /// Fors the each.
        /// </summary>
        /// <param name="stuff">The stuff.</param>
        public void ForEach(Action<TEntity> stuff)
        {
            if (stuff != null)
            {
                foreach (var one in container)
                {
                    stuff(one);
                }
            }
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public override void Clear()
        {
            lock (locker)
            {
                this.container.Clear();
                needRefresh = true;
            }
        }
    }
}