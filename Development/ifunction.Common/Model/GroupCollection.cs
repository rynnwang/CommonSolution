using System;
using System.Collections.Generic;
using System.Linq;

namespace ifunction.Model
{
    /// <summary>
    /// Class GroupCollection.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GroupCollection<T>
    {
        /// <summary>
        /// The data
        /// </summary>
        public Dictionary<string, List<T>> Data { get; protected set; }

        /// <summary>
        /// Gets the group count.
        /// </summary>
        /// <value>The group count.</value>
        public int GroupCount
        {
            get
            {
                return this.Data.Keys.Count;
            }
        }

        /// <summary>
        /// Gets the total count.
        /// </summary>
        /// <value>The total count.</value>
        public int TotalCount
        {
            get
            {
                return this.Data.Sum(one => one.Value.Count);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupCollection{T}" /> class.
        /// </summary>
        /// <param name="keyCaseSensitive">if set to <c>true</c> [key case sensitive]. Default is true.</param>
        public GroupCollection(bool keyCaseSensitive = true)
        {
            Data = keyCaseSensitive ? new Dictionary<string, List<T>>() : new Dictionary<string, List<T>>(StringComparer.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Adds the specified group key.
        /// </summary>
        /// <param name="groupKey">The group key.</param>
        /// <param name="item">The item.</param>
        public void Add(string groupKey, T item)
        {
            var list = this.GetCollectionByKey(groupKey, true);
            list.Add(item);
        }

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="groupKey">The group key.</param>
        /// <param name="items">The items.</param>
        public void AddRange(string groupKey, IEnumerable<T> items)
        {
            var list = this.GetCollectionByKey(groupKey, true);
            list.AddRange(items);
        }

        /// <summary>
        /// Removes the group.
        /// </summary>
        /// <param name="groupKey">The group key.</param>
        /// <returns><c>true</c> if succeed to remove, <c>false</c> otherwise.</returns>
        public bool RemoveGroup(string groupKey)
        {
            return this.Data.ContainsKey(groupKey) && this.Data.Remove(groupKey);
        }

        /// <summary>
        /// Removes the specified group key.
        /// </summary>
        /// <param name="groupKey">The group key.</param>
        /// <param name="index">The index.</param>
        public void Remove(string groupKey, int index)
        {
            var list = this.GetCollectionByKey(groupKey);
            if (list != null && list.Count > index && index > -1)
            {
                list.RemoveAt(index);
            }
        }

        /// <summary>
        /// Gets the <see cref="List{T}" /> with the specified group key.
        /// </summary>
        /// <param name="groupKey">The group key.</param>
        /// <returns>List{.</returns>
        public List<T> this[string groupKey]
        {
            get
            {
                return GetCollectionByKey(groupKey);
            }
        }

        /// <summary>
        /// Gets the collection by key.
        /// </summary>
        /// <param name="groupKey">The group key.</param>
        /// <returns>List{`0}.</returns>
        public List<T> GetCollectionByKey(string groupKey)
        {
            return GetCollectionByKey(groupKey, false);
        }

        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <param name="groupKey">The group key.</param>
        /// <param name="index">The index.</param>
        /// <returns>`0.</returns>
        public T GetItem(string groupKey, int index)
        {
            var list = GetCollectionByKey(groupKey);
            return (list != null && list.Count > index && index >= 0) ? list[index] : default(T);
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <param name="groupKey">The group key.</param>
        /// <returns>System.Nullable{System.Int32}.</returns>
        public int? GetCount(string groupKey)
        {
            int? result = null;

            var list = GetCollectionByKey(groupKey);

            if (list != null)
            {
                result = list.Count;
            }

            return result;
        }

        /// <summary>
        /// Gets the collection by key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="createIfNotExist">if set to <c>true</c> [create if not exist].</param>
        /// <returns>List{`0}.</returns>
        protected List<T> GetCollectionByKey(string key, bool createIfNotExist)
        {
            List<T> result = null;

            if (!string.IsNullOrWhiteSpace(key))
            {
                result = this.Data.ContainsKey(key) ? this.Data[key] : null;

                if (result == null && createIfNotExist)
                {
                    this.Data.Add(key, new List<T>());
                    result = this.Data[key];
                }
            }

            return result;
        }
    }
}
