using System;
using System.Collections.Concurrent;

namespace Beyova.Concurrent
{
    /// <summary>
    /// Class ConcurrentMatrixList.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ConcurrentMatrixList<T> : ConcurrentDictionary<string, ConcurrentBag<T>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConcurrentMatrixList{T}" /> class.
        /// </summary>
        /// <param name="keyCaseSensitive">if set to <c>true</c> [key case sensitive].</param>
        public ConcurrentMatrixList(bool keyCaseSensitive = true)
            : base(keyCaseSensitive ? StringComparer.Ordinal : StringComparer.OrdinalIgnoreCase)
        {
        }

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void Add(string key, T value)
        {
            try
            {
                key.CheckEmptyString("key");

                var list = this.GetCollectionByKey(key, true);
                list.Add(value);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { key, value });
            }
        }

        /// <summary>
        /// Gets the collection by key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="createIfNotExist">if set to <c>true</c> [create if not exist].</param>
        /// <returns>List&lt;T&gt;.</returns>
        protected ConcurrentBag<T> GetCollectionByKey(string key, bool createIfNotExist)
        {
            ConcurrentBag<T> result = null;

            if (!string.IsNullOrWhiteSpace(key))
            {
                result = this.ContainsKey(key) ? this[key] : null;

                if (result == null && createIfNotExist)
                {
                    result = base.GetOrAdd(key, new ConcurrentBag<T>());
                }
            }

            return result;
        }
    }
}