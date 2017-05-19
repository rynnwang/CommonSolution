using System;
using System.Collections;
using System.Collections.Generic;

namespace Beyova
{
    /// <summary>
    /// Class SequencedKeyDictionary.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public class SequencedKeyDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        /// <summary>
        /// The keys
        /// </summary>
        protected List<TKey> keys;

        /// <summary>
        /// The dictionary
        /// </summary>
        protected Dictionary<TKey, TValue> dictionary;

        /// <summary>
        /// Gets an <see cref="T:System.Collections.Generic.ICollection`1" /> containing the keys of the <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
        /// <value>The keys.</value>
        public ICollection<TKey> Keys { get { return keys; } }

        /// <summary>
        /// Gets an <see cref="T:System.Collections.Generic.ICollection`1" /> containing the values in the <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
        /// <value>The values.</value>
        public ICollection<TValue> Values { get { return dictionary.Values; } }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <value>The count.</value>
        public int Count { get { return keys.Count; } }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.
        /// </summary>
        /// <value><c>true</c> if this instance is read only; otherwise, <c>false</c>.</value>
        public bool IsReadOnly { get { return false; } }

        /// <summary>
        /// Gets or sets with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>TValue.</returns>
        public TValue this[TKey key]
        {
            get
            {
                return dictionary[key];
            }
            set
            {
                dictionary[key] = value;
            }
        }

        /// <summary>
        /// Gets the index of.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>System.Int32.</returns>
        public int GetIndexOf(TKey key)
        {
            return key == null ? -1 : keys.IndexOf(key);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SequencedKeyDictionary{TKey, TValue}" /> class.
        /// </summary>
        /// <param name="baseDictionary">The base dictionary.</param>
        /// <param name="comparer">The comparer.</param>
        public SequencedKeyDictionary(IDictionary<TKey, TValue> baseDictionary, IEqualityComparer<TKey> comparer)
        {
            this.dictionary = baseDictionary == null ? new Dictionary<TKey, TValue>(comparer ?? EqualityComparer<TKey>.Default)
                : new Dictionary<TKey, TValue>(dictionary, comparer ?? EqualityComparer<TKey>.Default);
            keys = new List<TKey>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SequencedKeyDictionary{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="capacity">The capacity.</param>
        /// <param name="comparer">The comparer.</param>
        public SequencedKeyDictionary(int capacity, IEqualityComparer<TKey> comparer)
        {
            this.dictionary = new Dictionary<TKey, TValue>(capacity, comparer ?? EqualityComparer<TKey>.Default);
            keys = new List<TKey>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SequencedKeyDictionary{TKey, TValue}"/> class.
        /// </summary>
        public SequencedKeyDictionary()
        {
            this.dictionary = new Dictionary<TKey, TValue>();
            this.keys = new List<TKey>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SequencedKeyDictionary{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        public SequencedKeyDictionary(IDictionary<TKey, TValue> dictionary)
            : this(dictionary, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SequencedKeyDictionary{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the <see cref="T:System.Collections.Generic.Dictionary`2" /> can contain.</param>
        public SequencedKeyDictionary(int capacity)
            : this(capacity, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SequencedKeyDictionary{TKey, TValue}" /> class.
        /// </summary>
        /// <param name="comparer">The comparer.</param>
        public SequencedKeyDictionary(IEqualityComparer<TKey> comparer)
            : this(null, comparer)
        {
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.IDictionary`2" /> contains an element with the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the <see cref="T:System.Collections.Generic.IDictionary`2" />.</param>
        /// <returns>true if the <see cref="T:System.Collections.Generic.IDictionary`2" /> contains an element with the key; otherwise, false.</returns>
        public bool ContainsKey(TKey key)
        {
            return key != null && keys.Contains(key);
        }

        /// <summary>
        /// Adds an element with the provided key and value to the <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
        /// <param name="key">The object to use as the key of the element to add.</param>
        /// <param name="value">The object to use as the value of the element to add.</param>
        public void Add(TKey key, TValue value)
        {
            keys.Add(key);
            dictionary.Add(key, value);
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key whose value to get.</param>
        /// <param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the <paramref name="value" /> parameter. This parameter is passed uninitialized.</param>
        /// <returns>true if the object that implements <see cref="T:System.Collections.Generic.IDictionary`2" /> contains an element with the specified key; otherwise, false.</returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            return dictionary.TryGetValue(key, out value);
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            keys.Add(item.Key);
            dictionary.Add(item.Key, item.Value);
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        public void Clear()
        {
            keys.Clear();
            dictionary.Clear();
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1" /> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns>true if <paramref name="item" /> is found in the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, false.</returns>
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return (dictionary as ICollection<KeyValuePair<TKey, TValue>>).Contains(item);
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1" /> to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            (dictionary as ICollection<KeyValuePair<TKey, TValue>>).CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes the element with the specified key from the <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns>true if the element is successfully removed; otherwise, false.  This method also returns false if <paramref name="key" /> was not found in the original <see cref="T:System.Collections.Generic.IDictionary`2" />.</returns>
        public bool Remove(TKey key)
        {
            return keys.Remove(key) && dictionary.Remove(key);
        }

        /// <summary>
        /// Removes at.
        /// </summary>
        /// <param name="index">The index.</param>
        public void RemoveAt(int index)
        {
            var key = keys[index];
            keys.RemoveAt(index);
            dictionary.Remove(key);
        }

        /// <summary>
        /// Sorts the keys.
        /// </summary>
        /// <param name="comparer">The comparer.</param>
        public void SortKeys(IComparer<TKey> comparer)
        {
            keys.Sort(comparer);
        }

        /// <summary>
        /// Sorts the keys.
        /// </summary>
        /// <typeparam name="TComparableType">The type of the t comparable type.</typeparam>
        /// <param name="comparableSelector">The comparable selector.</param>
        /// <param name="isDescending">if set to <c>true</c> [is descending].</param>
        public void SortKeys<TComparableType>(Func<TKey, TComparableType> comparableSelector, bool isDescending = false) where TComparableType : IComparable
        {
            keys.Sort(comparableSelector, isDescending);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns>true if <paramref name="item" /> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, false. This method also returns false if <paramref name="item" /> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1" />.</returns>
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return keys.Remove(item.Key) && dictionary.Remove(item.Key);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return InternalToKeyValueList().GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return InternalToKeyValueList().GetEnumerator();
        }

        /// <summary>
        /// Internals to key value list.
        /// </summary>
        /// <returns>List&lt;KeyValuePair&lt;TKey, TValue&gt;&gt;.</returns>
        protected List<KeyValuePair<TKey, TValue>> InternalToKeyValueList()
        {
            List<KeyValuePair<TKey, TValue>> tmp = new List<KeyValuePair<TKey, TValue>>();
            foreach (var key in this.dictionary.Keys)
            {
                tmp.Add(new KeyValuePair<TKey, TValue>(key, this.dictionary[key]));
            }
            return tmp;
        }
    }
}
