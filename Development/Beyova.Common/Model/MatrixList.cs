using System;
using System.Collections.Generic;

namespace Beyova
{
    /// <summary>
    /// Class MatrixList.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MatrixList<T> : Dictionary<string, List<T>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MatrixList{T}" /> class.
        /// </summary>
        /// <param name="keyCaseSensitive">if set to <c>true</c> [key case sensitive].</param>
        public MatrixList(bool keyCaseSensitive = true)
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
                throw ex.Handle("Add", new { key, value });
            }
        }

        /// <summary>
        /// Gets the collection by key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="createIfNotExist">if set to <c>true</c> [create if not exist].</param>
        /// <returns>List&lt;T&gt;.</returns>
        protected List<T> GetCollectionByKey(string key, bool createIfNotExist)
        {
            List<T> result = null;

            if (!string.IsNullOrWhiteSpace(key))
            {
                result = this.ContainsKey(key) ? this[key] : null;

                if (result == null && createIfNotExist)
                {
                    this.Add(key, new List<T>());
                    result = this[key];
                }
            }

            return result;
        }
    }
}
