﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;
using Beyova;
using Beyova.ExceptionSystem;
using Newtonsoft.Json.Linq;

namespace Beyova
{
    /// <summary>
    /// Extension class for collection type object.
    /// </summary>
    public static class CollectionExtension
    {
        /// <summary>
        /// The format
        /// </summary>
        private const string keyValueFormat = "&{0}={1}";

        #region Add If Not xxx

        /// <summary>
        /// Adds if not exists.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="item">The item.</param>
        /// <param name="comparer">The comparer.</param>
        public static void AddIfNotExists<T>(this ICollection<T> collection, T item, Func<T, T, bool> comparer = null)
        {
            if (collection != null && item != null)
            {
                var found = (from one in collection where (comparer != null ? (comparer(one, item)) : (item.Equals(one))) select one).Any();

                if (!found)
                {
                    collection.Add(item);
                }
            }
        }

        /// <summary>
        /// Adds if not null.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="item">The item.</param>
        /// <returns><c>true</c> if succeed to add, <c>false</c> otherwise.</returns>
        public static bool AddIfNotNullOrEmpty(this ICollection<string> collection, string item)
        {
            if (collection != null && !string.IsNullOrWhiteSpace(item))
            {
                collection.Add(item);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Adds if not null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="item">The item.</param>
        /// <returns><c>true</c> if succeed to add, <c>false</c> otherwise.</returns>
        public static bool AddIfNotNull<T>(this ICollection<T> collection, Nullable<T> item) where T : struct
        {
            if (collection != null && item.HasValue)
            {
                collection.Add(item.Value);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Adds if not null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="item">The item.</param>
        /// <returns><c>true</c> if succeed to add, <c>false</c> otherwise.</returns>
        public static bool AddIfNotNull<T>(this ICollection<T> collection, T item)
        {
            if (collection != null && item != null)
            {
                collection.Add(item);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Adds if not null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="item">The item.</param>
        public static void AddIfNotNull<T>(this HashSet<T> collection, Nullable<T> item) where T : struct
        {
            if (collection != null && item.HasValue)
            {
                collection.Add(item.Value);
            }
        }

        /// <summary>
        /// Adds if not null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="item">The item.</param>
        public static void AddIfNotNull<T>(this HashSet<T> collection, T item)
        {
            if (collection != null && item != null)
            {
                collection.Add(item);
            }
        }

        /// <summary>
        /// Adds if not null.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public static void AddIfNotNullOrEmpty(this NameValueCollection collection, string key, string value)
        {
            if (collection != null && !string.IsNullOrWhiteSpace(key) && !string.IsNullOrWhiteSpace(value))
            {
                collection.Add(key, value);
            }
        }

        /// <summary>
        /// Adds if not exists.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="item">The item.</param>
        /// <param name="comparer">The comparer.</param>
        public static void AddIfNotExists<T>(this HashSet<T> collection, T item, Func<T, T, bool> comparer = null)
        {
            if (collection != null && item != null)
            {
                var found = (from one in collection where (comparer != null ? (comparer(one, item)) : (item.Equals(one))) select one).Any();

                if (!found)
                {
                    collection.Add(item);
                }
            }
        }

        /// <summary>
        /// Adds if not null.
        /// </summary>
        /// <typeparam name="TKey">The type of the t key.</typeparam>
        /// <typeparam name="TValue">The type of the t value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public static void AddIfNotNull<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary != null && key != null && value != null)
            {
                dictionary.Add(key, value);
            }
        }

        /// <summary>
        /// Adds if not null or empty. Key and value would be added only when neither of them is null or empty.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public static void AddIfNotNullOrEmpty(this IDictionary<string, string> dictionary, string key, string value)
        {
            if (dictionary != null && !string.IsNullOrWhiteSpace(key) && !string.IsNullOrWhiteSpace(value))
            {
                dictionary.Add(key, value);
            }
        }

        #endregion Add If Not xxx

        #region IEnumerable, ICollection, IList, IDictionary, HashSet

        /// <summary>
        /// To the j array.
        /// </summary>
        /// <typeparam name="TKey">The type of the t key.</typeparam>
        /// <typeparam name="TValue">The type of the t value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="eachObjectGenerateFunc">The each object generate function.</param>
        /// <returns>JArray.</returns>
        public static JArray ToJArray<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, Func<TKey, TValue, JToken> eachObjectGenerateFunc)
        {
            JArray result = new JArray();
            if (dictionary.HasItem() && eachObjectGenerateFunc != null)
            {
                foreach (var one in dictionary)
                {
                    result.AddIfNotNull(eachObjectGenerateFunc(one.Key, one.Value));
                }
            }

            return result;
        }

        /// <summary>
        /// To the j array.
        /// </summary>
        /// <typeparam name="TKey">The type of the t key.</typeparam>
        /// <typeparam name="TValue">The type of the t value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="keyPropertyName">Name of the key property.</param>
        /// <param name="valuePropertyName">Name of the value property.</param>
        /// <returns>JArray.</returns>
        public static JArray ToJArray<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, string keyPropertyName, string valuePropertyName)
        {
            if (dictionary.HasItem() && !string.IsNullOrWhiteSpace(keyPropertyName) && !string.IsNullOrWhiteSpace(valuePropertyName))
            {
                if (keyPropertyName == valuePropertyName)
                {
                    throw ExceptionFactory.CreateInvalidObjectException("keyPropertyName/valuePropertyName", new { keyPropertyName, valuePropertyName });
                }

                return ToJArray(dictionary, (k, v) =>
                {
                    var o = new JObject();
                    o.Add(keyPropertyName, JToken.FromObject(k));
                    o.Add(valuePropertyName, JToken.FromObject(v));

                    return o;
                });
            }

            return new JArray();
        }

        /// <summary>
        /// Merges to.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        /// <param name="overrideIfExists">if set to <c>true</c> [override if exists].</param>
        /// <param name="equalityComparer">The equality comparer.</param>
        public static void MergeTo<T>(this IEnumerable<T> source, IList<T> destination, bool overrideIfExists = false, IEqualityComparer<T> equalityComparer = null)
        {
            if (source.HasItem() && destination.HasItem())
            {
                if (equalityComparer == null)
                {
                    equalityComparer = EqualityComparer<T>.Default;
                }

                foreach (var one in source)
                {
                    for (var i = 0; i < destination.Count; i++)
                    {
                        if (equalityComparer.Equals(destination[i], one))
                        {
                            if (overrideIfExists)
                            {
                                destination[i] = one;
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            destination.Add(one);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Matches any.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="hitSubjects">The hit subjects.</param>
        /// <param name="equalityComparer">The equality comparer.</param>
        /// <returns><c>true</c> if exists any match between <c>source</c> and <c>hitSubjects</c>, <c>false</c> otherwise.</returns>
        public static bool MatchAny<T>(this IEnumerable<T> source, IEnumerable<T> hitSubjects, IEqualityComparer<T> equalityComparer = null)
        {
            if (source.HasItem() && hitSubjects.HasItem())
            {
                if (equalityComparer == null)
                {
                    equalityComparer = EqualityComparer<T>.Default;
                }

                foreach (var one in source)
                {
                    foreach (var item in hitSubjects)
                    {
                        if (equalityComparer.Equals(one, item))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Matches all.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="hitSubjects">The hit subjects.</param>
        /// <param name="equalityComparer">The equality comparer.</param>
        /// <returns><c>true</c> if match all from <c>source</c> and <c>hitSubjects</c> (including <c>hitSubjects</c> has no item), <c>false</c> otherwise.</returns>
        public static bool MatchAll<T>(this IEnumerable<T> source, IEnumerable<T> hitSubjects, IEqualityComparer<T> equalityComparer = null)
        {
            if (source.HasItem())
            {
                if (!hitSubjects.HasItem())
                {
                    return true;
                }

                if (equalityComparer == null)
                {
                    equalityComparer = EqualityComparer<T>.Default;
                }

                foreach (var one in source)
                {
                    foreach (var item in hitSubjects)
                    {
                        if (!equalityComparer.Equals(one, item))
                        {
                            return false;
                        }
                    }
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Subs the collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="count">The count.</param>
        /// <returns>List&lt;T&gt;.</returns>
        /// <exception cref="InvalidObjectException">
        /// startIndex
        /// or
        /// count
        /// </exception>
        public static List<T> SubCollection<T>(this IList<T> collection, int startIndex, int? count = null)
        {
            if (collection != null)
            {
                if (startIndex < 0 || startIndex >= collection.Count)
                {
                    throw ExceptionFactory.CreateInvalidObjectException("startIndex", data: new { startIndex, collectionCount = collection.Count });
                }

                if (count != null && (count.Value < 0 || (count.Value + startIndex - 1) > collection.Count))
                {
                    throw ExceptionFactory.CreateInvalidObjectException("count", data: new { count, collectionCount = collection.Count });
                }

                var result = new List<T>();
                for (var i = startIndex; i < (count ?? collection.Count); i++)
                {
                    result.Add(collection[i]);
                }

                return result;
            }

            return null;
        }

        /// <summary>
        /// Removes from.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <param name="startIndex">The start index.</param>
        /// <exception cref="InvalidObjectException">startIndex</exception>
        public static void RemoveFrom<T>(this List<T> list, int startIndex)
        {
            if (list != null)
            {
                if (startIndex < 0 || startIndex >= list.Count)
                {
                    throw ExceptionFactory.CreateInvalidObjectException("startIndex", data: new { startIndex, collectionCount = list.Count });
                }

                list.RemoveRange(startIndex, list.Count - startIndex);
            }
        }

        /// <summary>
        /// Gets the by last index of. (index should >= 1)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="lastIndex">The last index. (should >= 1)</param>
        /// <returns>T.</returns>
        public static T GetByLastIndexOf<T>(this IList<T> collection, int lastIndex)
        {
            return (collection != null && lastIndex > 0 && collection.Count >= lastIndex) ? collection[collection.Count - lastIndex] : default(T);
        }

        /// <summary>
        /// Gets the by index of. (index should >= 1)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="index">The index.</param>
        /// <returns>T.</returns>
        public static T GetByIndexOf<T>(this IList<T> collection, int index)
        {
            return (collection != null && index > 0 && collection.Count >= index) ? collection[index - 1] : default(T);
        }

        /// <summary>
        /// Indexes the of.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public static int IndexOf<T>(this IEnumerable<T> collection, Func<T, bool> predicate)
        {
            if (collection.HasItem() && predicate != null)
            {
                int index = 0;
                foreach (var one in collection)
                {
                    if (predicate(one))
                    {
                        return index;
                    }
                    index++;
                }
            }

            return -1;
        }

        /// <summary>
        /// Indexes the of.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TFactor">The type of the factor.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="factorValue">The factor value.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public static int IndexOf<TEntity, TFactor>(this IEnumerable<TEntity> collection, TFactor factorValue, Func<TEntity, TFactor, bool> predicate)
        {
            if (collection.HasItem() && predicate != null)
            {
                int index = 0;
                foreach (var one in collection)
                {
                    if (predicate(one, factorValue))
                    {
                        return index;
                    }
                    index++;
                }
            }

            return -1;
        }

        /// <summary>
        /// Sorts the specified comparable selector.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TComparableType">The type of the t comparable type.</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="comparableSelector">The comparable selector.</param>
        /// <param name="isDescending">if set to <c>true</c> [is descending].</param>
        public static void Sort<T, TComparableType>(this List<T> list, Func<T, TComparableType> comparableSelector, bool isDescending = false) where TComparableType : IComparable
        {
            var comparer = new LambdaComparableComparer<T, TComparableType>(comparableSelector, isDescending);
            list?.Sort(comparer);
        }

        /// <summary>
        /// Sorts the specified list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TComparableType">The type of the t comparable type.</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="comparableSelector">The comparable selector.</param>
        /// <param name="comparison">The comparison.</param>
        public static void Sort<T, TComparableType>(this List<T> list, Func<T, TComparableType> comparableSelector, Func<TComparableType, TComparableType, int> comparison)
        {
            if (list != null)
            {
                var comparer = new LambdaComparer<T, TComparableType>(comparableSelector, comparison);
                list.Sort(comparer);
            }
        }

        /// <summary>
        /// Sorts as.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TSortFactor">The type of the sort factor.</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="sortFactors">The sort factors.</param>
        /// <param name="getSortFactor">The get sort factor.</param>
        public static void SortAs<T, TSortFactor>(this List<T> list, List<TSortFactor> sortFactors, Func<T, TSortFactor> getSortFactor)
        {
            if (sortFactors.HasItem() && getSortFactor != null)
            {
                Sort(list, x => { return sortFactors.IndexOf(getSortFactor(x)); }, (a, b) => { return a.CompareTo(b); });
            }
        }

        /// <summary>
        /// Try to the replace with specific item. Replaced index would be returned.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TComparerIdentifier">The type of the t comparer identifier.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="itemToReplace">The item to replace.</param>
        /// <param name="comparerIdentifier">The comparer identifier.</param>
        /// <param name="comparer">The comparer.</param>
        /// <returns>System.Nullable&lt;System.Int32&gt; for replaced index.</returns>
        public static int? TryReplace<T, TComparerIdentifier>(this IList<T> collection, T itemToReplace, TComparerIdentifier comparerIdentifier, Func<T, TComparerIdentifier, bool> comparer)
        {
            if (collection != null && comparer != null && itemToReplace != null)
            {
                for (var i = 0; i < collection.Count;)
                {
                    if (comparer(collection[i], comparerIdentifier))
                    {
                        var tmp = collection[i];
                        collection.RemoveAt(i);
                        collection.Insert(i, itemToReplace);
                        return i;
                    }

                    i++;
                }
            }

            return null;
        }

        /// <summary>
        /// Try to the replace all with specific item. Replaced count would be returned.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TComparerIdentifier">The type of the t comparer identifier.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="itemToReplace">The item to replace.</param>
        /// <param name="comparerIdentifier">The comparer identifier.</param>
        /// <param name="comparer">The comparer.</param>
        /// <returns>System.Int32.</returns>
        public static int TryReplaceAll<T, TComparerIdentifier>(this IList<T> collection, T itemToReplace, TComparerIdentifier comparerIdentifier, Func<T, TComparerIdentifier, bool> comparer)
        {
            int count = 0;
            if (collection != null && comparer != null && itemToReplace != null)
            {
                for (var i = 0; i < collection.Count; i++)
                {
                    if (comparer(collection[i], comparerIdentifier))
                    {
                        collection.RemoveAt(i);
                        collection.Insert(i, itemToReplace);
                        count++;
                    }
                }
            }

            return count;
        }

        /// <summary>
        /// Tries the get value.
        /// </summary>
        /// <typeparam name="TKey">The type of the t key.</typeparam>
        /// <typeparam name="TValue">The type of the t value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>TValue.</returns>
        public static TValue TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue = default(TValue))
        {
            if (dictionary != null)
            {
                return dictionary.ContainsKey(key) ? dictionary[key] : defaultValue;
            }

            return defaultValue;
        }

        /// <summary>
        /// To the hash set.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">The items.</param>
        /// <param name="comparer">The comparer.</param>
        /// <returns>HashSet&lt;T&gt;.</returns>
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> items, IEqualityComparer<T> comparer = null)
        {
            items = items ?? new Collection<T>();

            return comparer == null ?
                new HashSet<T>(items)
                : new HashSet<T>(items, comparer);
        }

        /// <summary>
        /// To the dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="items">The items.</param>
        /// <param name="comparer">The comparer.</param>
        /// <returns></returns>
        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> items, IEqualityComparer<TKey> comparer = null)
        {
            if (items != null)
            {
                var result = comparer == null ? new Dictionary<TKey, TValue>() : new Dictionary<TKey, TValue>(comparer);

                foreach (var one in items)
                {
                    result.Merge(one.Key, one.Value);
                }

                return result;
            }

            return null;
        }

        /// <summary>
        /// Converts list to XML.
        /// <remarks>Serialize object to &lt;List&gt;&lt;Item&gt;Value&lt;/Item&gt;&lt;/List&gt;</remarks>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="convertFunc">The convert function.</param>
        /// <returns>XElement.</returns>
        public static XElement ListToXml<T>(this ICollection<T> collection, Func<T, object> convertFunc = null)
        {
            var result = XmlConstants.node_List.CreateXml();

            if (collection != null)
            {
                foreach (var one in collection)
                {
                    var item = XmlConstants.node_Item.CreateXml();
                    var value = convertFunc == null ? one.ToString() : convertFunc(one);
                    var xmlObject = value as XNode;

                    if (xmlObject != null)
                    {
                        item.Add(xmlObject);
                    }
                    else if (value != null)
                    {
                        item.SetValue(value);
                    }
                    else
                    {
                        continue;
                    }

                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Converts list to XML.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="fillXmlItemFunc">The fill XML item function. XElement item is for fill, bool is to indicating needs to add it into list as result or not.</param>
        /// <returns></returns>
        public static XElement ListToXml<T>(this ICollection<T> collection, Func<T, XElement, bool> fillXmlItemFunc)
        {
            var result = XmlConstants.node_List.CreateXml();

            if (collection != null && fillXmlItemFunc != null)
            {
                foreach (var one in collection)
                {
                    var item = XmlConstants.node_Item.CreateXml();
                    if (fillXmlItemFunc(one, item))
                    {
                        result.Add(item);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Dictionaries to XML.
        /// </summary>
        /// <typeparam name="TValue">The type of the t value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="convertFunc">The convert function.</param>
        /// <returns>XElement.</returns>
        public static XElement DictionaryToXml<TValue>(this IDictionary<string, TValue> dictionary, Func<TValue, string> convertFunc = null)
        {
            var result = XmlConstants.node_Dictionary.CreateXml();

            if (dictionary != null)
            {
                foreach (var one in dictionary)
                {
                    var item = XmlConstants.node_Item.CreateXml();
                    item.SetAttributeValue(XmlConstants.attribute_Key, one.Key);
                    var value = convertFunc == null ? one.ToString() : convertFunc(one.Value);
                    item.SetValue(value);

                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Dictionaries to XML.
        /// </summary>
        /// <typeparam name="TKey">The type of the t key.</typeparam>
        /// <typeparam name="TValue">The type of the t value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="convertFunc">The convert function.</param>
        /// <returns>XElement.</returns>
        public static XElement DictionaryToXml<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, Func<KeyValuePair<TKey, TValue>, XElement> convertFunc = null)
            where TKey : IConvertible
        {
            var result = XmlConstants.node_Dictionary.CreateXml();

            if (dictionary != null)
            {
                foreach (var one in dictionary)
                {
                    if (convertFunc == null)
                    {
                        var item = XmlConstants.node_Item.CreateXml();
                        item.SetAttributeValue(XmlConstants.attribute_Key, one.Key);
                        item.SetValue(one.Value.ToString());
                        result.Add(item);
                    }
                    else
                    {
                        result.Add(convertFunc(one));
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Converts XML to list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml">The XML.</param>
        /// <param name="xmlNodeToEntity">The XML node to entity.</param>
        /// <returns>List{System.String}.</returns>
        public static List<T> XmlToList<T>(this XElement xml, Func<XElement, T> xmlNodeToEntity)
        {
            var result = new List<T>();

            if (xml != null && xml.Name.LocalName.Equals(XmlConstants.node_List))
            {
                result.AddRange(xml.Elements(XmlConstants.node_Item).Select(one => xmlNodeToEntity.Invoke(one)));
            }

            return result;
        }

        /// <summary>
        /// XMLs to dictionary.
        /// </summary>
        /// <typeparam name="TValue">The type of the t value.</typeparam>
        /// <param name="xml">The XML.</param>
        /// <param name="xmlNodeToEntity">The XML node to entity.</param>
        /// <returns>Dictionary&lt;System.String, TValue&gt;.</returns>
        public static Dictionary<string, TValue> XmlToDictionary<TValue>(this XElement xml, Func<XElement, TValue> xmlNodeToEntity)
        {
            var result = new Dictionary<string, TValue>();

            if (xml != null && xml.Name.LocalName.Equals(XmlConstants.node_Dictionary))
            {
                result.AddRange(xml.Elements(XmlConstants.node_Item).Select(one =>
                {
                    return new KeyValuePair<string, TValue>(one.GetAttributeValue(XmlConstants.attribute_Key), xmlNodeToEntity.Invoke(one));
                }));
            }

            return result;
        }

        /// <summary>
        /// XMLs to dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of the t key.</typeparam>
        /// <typeparam name="TValue">The type of the t value.</typeparam>
        /// <param name="xml">The XML.</param>
        /// <param name="xmlNodeToEntity">The XML node to entity.</param>
        /// <returns>Dictionary&lt;TKey, TValue&gt;.</returns>
        public static Dictionary<TKey, TValue> XmlToDictionary<TKey, TValue>(this XElement xml, Func<XElement, KeyValuePair<TKey, TValue>> xmlNodeToEntity)
            where TKey : IConvertible
        {
            var result = new Dictionary<TKey, TValue>();

            if (xml != null && xml.Name.LocalName.Equals(XmlConstants.node_Dictionary))
            {
                result.AddRange(xml.Elements(XmlConstants.node_Item).Select(one => xmlNodeToEntity(one)));
            }

            return result;
        }

        /// <summary>
        /// Converts XML to list.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns>List&lt;System.String&gt;.</returns>
        public static List<string> XmlToList(this XElement xml)
        {
            return XmlToList<string>(xml, x => x.Value);
        }

        /// <summary>
        /// Converts XML to list.
        /// </summary>
        /// <param name="xmlString">The XML string.</param>
        /// <returns>List{System.String}.</returns>
        public static List<string> XmlToList(this string xmlString)
        {
            return !string.IsNullOrWhiteSpace(xmlString) ? XElement.Parse(xmlString).XmlToList() : new List<string>();
        }

        /// <summary>
        /// SQLs the json to list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlJson">The SQL json.</param>
        /// <returns>System.Collections.Generic.List&lt;T&gt;.</returns>
        public static List<T> SqlJsonToList<T>(this object sqlJson)
        {
            return sqlJson?.ObjectToJToken()?.ToObject<List<T>>();
        }

        /// <summary>
        /// Converts SQL the json to simple list.
        /// Example: Input [{item:x},{item:y}] + "item", then get [x,y]
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlJson">The SQL json.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>System.Collections.Generic.List&lt;T&gt;.</returns>
        public static List<T> SqlJsonToSimpleList<T>(this object sqlJson, string propertyName)
        {
            List<T> result = new List<T>();

            if (!string.IsNullOrWhiteSpace(propertyName) && sqlJson != null)
            {
                var items = sqlJson?.ObjectToJToken();
                if (items.HasItem())
                {
                    foreach (JObject one in items)
                    {
                        if (one != null)
                        {
                            result.AddIfNotNull(one.SafeGetValue<T>(propertyName));
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// SQLs the json to dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of the t key.</typeparam>
        /// <typeparam name="TValue">The type of the t value.</typeparam>
        /// <param name="sqlJson">The SQL json.</param>
        /// <param name="keyPropertyName">Name of the key property.</param>
        /// <param name="valuePropertyName">Name of the value property.</param>
        /// <param name="equalityComparer">The equality comparer.</param>
        /// <param name="useMerge">if set to <c>true</c> [use merge].</param>
        /// <returns>Dictionary&lt;TKey, TValue&gt;.</returns>
        public static Dictionary<TKey, TValue> SqlJsonToDictionary<TKey, TValue>(this object sqlJson, string keyPropertyName, string valuePropertyName, IEqualityComparer<TKey> equalityComparer = null, bool useMerge = false)
        {
            Dictionary<TKey, TValue> result = new Dictionary<TKey, TValue>(equalityComparer ?? EqualityComparer<TKey>.Default);

            if (!string.IsNullOrWhiteSpace(keyPropertyName) && !string.IsNullOrWhiteSpace(valuePropertyName) && sqlJson != null)
            {
                var items = sqlJson.ObjectToJToken();
                if (items.HasItem())
                {
                    foreach (JObject one in items)
                    {
                        if (one != null)
                        {
                            if (useMerge)
                            {
                                result.Merge(one.SafeGetValue<TKey>(keyPropertyName), one.SafeGetValue<TValue>(valuePropertyName));
                            }
                            else
                            {
                                result.Add(one.SafeGetValue<TKey>(keyPropertyName), one.SafeGetValue<TValue>(valuePropertyName));
                            }
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Joins the within format.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="format">The format. {0} would be used for index, {1} would be used as value.</param>
        /// <returns>
        /// System.String.
        /// </returns>
        public static string JoinWithinFormat<T>(this IEnumerable<T> instance, string format)
        {
            var builder = new StringBuilder();

            if (instance != null && !string.IsNullOrWhiteSpace(format))
            {
                var index = 1;
                foreach (var one in instance)
                {
                    builder.AppendFormat(format, index, one);
                    index++;
                }
            }

            return builder.ToString();
        }

        /// <summary>
        /// Determines whether the specified instance has item.
        /// <remarks><c>Instance</c> can be null. Return true only when instance is not null and has item.</remarks>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns><c>true</c> if the specified instance has item; otherwise, <c>false</c>.</returns>
        public static bool HasItem<T>(this IEnumerable<T> instance)
        {
            return instance != null && instance.Any();
        }

        /// <summary>
        /// Determines whether the specified instance has item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="objToMatch">The object to match.</param>
        /// <returns>System.Boolean.</returns>
        public static bool HasItem<T>(this IList<T> instance, T objToMatch)
        {
            return instance != null && instance.IndexOf(objToMatch) > -1;
        }

        /// <summary>
        /// Joins the specified instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="separator">The separator.</param>
        /// <param name="valueSelector">The value selector.</param>
        /// <returns>System.String.</returns>
        public static string Join<T>(this IEnumerable<T> instance, string separator, Func<T, string> valueSelector = null)
        {
            if (valueSelector == null)
            {
                return instance == null ? string.Empty : string.Join<T>(separator, instance);
            }
            else
            {
                var builder = new StringBuilder();
                foreach (var one in instance)
                {
                    builder.Append(valueSelector(one));
                    builder.Append(separator);
                }

                builder.RemoveLast(separator.Length);

                return builder.ToString();
            }
        }

        #endregion IEnumerable, ICollection, IList, IDictionary, HashSet

        /// <summary>
        /// To the key value string.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="encodeHtml">if set to <c>true</c> [encode HTML].</param>
        /// <returns>System.String.</returns>
        public static string ToKeyValueString(this NameValueCollection dictionary, bool encodeHtml = false)
        {
            var builder = new StringBuilder();

            if (dictionary != null)
            {
                foreach (string key in dictionary.Keys)
                {
                    builder.AppendFormat(keyValueFormat, key, encodeHtml ? dictionary[key].ToHtmlEncodedText() : dictionary[key]);
                }

                if (builder.Length > 0)
                {
                    builder.Remove(0, 1);
                }
            }

            return builder.ToString();
        }

        /// <summary>
        /// Fills the values by key value string.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="keyValueString">The key value string.</param>
        public static void FillValuesByKeyValueString(this NameValueCollection dictionary, string keyValueString)
        {
            if (dictionary == null || string.IsNullOrWhiteSpace(keyValueString))
            {
                return;
            }

            var webDictionary = keyValueString.ParseToNameValueCollection();
            foreach (string key in dictionary.Keys)
            {
                dictionary.Add(key, webDictionary[key]);
            }
        }

        /// <summary>
        /// Determines whether [contains] [the specified any string].
        /// </summary>
        /// <param name="anyString">Any string.</param>
        /// <param name="value">The value.</param>
        /// <param name="ignoreCase">The ignore case.</param>
        /// <returns>System.Boolean.</returns>
        public static bool Contains(this string anyString, char value, bool ignoreCase = false)
        {
            return Contains<char>(anyString.ToCharArray(), value, ignoreCase ? CharComparer.OrdinalIgnoreCase : CharComparer.Ordinal);
        }

        /// <summary>
        /// Determines whether [contains] [the specified string array].
        /// </summary>
        /// <param name="stringArray">The string array.</param>
        /// <param name="value">The value.</param>
        /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
        /// <returns>
        ///   <c>true</c> if [contains] [the specified string array]; otherwise, <c>false</c>.</returns>
        public static bool Contains(this string[] stringArray, string value, bool ignoreCase)
        {
            return Contains(stringArray, value, ignoreCase ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal);
        }

        /// <summary>
        /// Determines whether [contains] [the specified array].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">The array.</param>
        /// <param name="value">The value.</param>
        /// <param name="equalityComparer">The equality comparer.</param>
        /// <returns>System.Boolean.</returns>
        public static bool Contains<T>(this T[] array, T value, IEqualityComparer<T> equalityComparer = null)
        {
            if (array != null && value != null)
            {
                if (equalityComparer == null)
                {
                    equalityComparer = EqualityComparer<T>.Default;
                }

                for (var i = 0; i < array.Length; i++)
                {
                    if (equalityComparer.Equals(array[i], value))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Determines whether the specified predicate has item.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>
        ///   <c>true</c> if the specified predicate has item; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasItem<TEntity>(this IEnumerable<TEntity> collection, Func<TEntity, bool> predicate)
        {
            return collection != null && predicate != null && collection.Any(predicate);
        }

        /// <summary>
        /// Determines whether [contains] [the specified collection]. Rules: exists any item where equalityComparer.Equals(selector(x), factor) == true
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <typeparam name="TFactor">The type of the t factor.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="factor">The factor.</param>
        /// <param name="selector">The selector.</param>
        /// <param name="equalityComparer">The equality comparer.</param>
        /// <returns>System.Boolean.</returns>
        public static bool HasItem<TEntity, TFactor>(this IEnumerable<TEntity> collection, TFactor factor, Func<TEntity, TFactor> selector, IEqualityComparer<TFactor> equalityComparer = null)
        {
            return (collection.HasItem() && selector != null)
                && collection.Any(x =>
             {
                 return (equalityComparer ?? EqualityComparer<TFactor>.Default).Equals(selector(x), factor);
             });
        }

        /// <summary>
        /// Determines whether [contains] [the specified collection].
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TFactor">The type of the factor.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="factor">The factor.</param>
        /// <param name="selector">The selector.</param>
        /// <param name="equalityComparer">The equality comparer.</param>
        /// <returns>
        ///   <c>true</c> if [contains] [the specified collection]; otherwise, <c>false</c>.
        /// </returns>
        public static bool Contains<TEntity, TFactor>(this IEnumerable<TEntity> collection, TFactor factor, Func<TEntity, TFactor> selector, Func<TFactor, TFactor, bool> equalityComparer)
        {
            return (collection.HasItem() && selector != null)
                && collection.Any(x =>
                {
                    return (equalityComparer ?? EqualityComparer<TFactor>.Default.Equals)(factor, selector(x));
                });
        }

        /// <summary>
        /// Determines whether [contains] [the specified dictionary].
        /// </summary>
        /// <typeparam name="TKey">The type of the t key.</typeparam>
        /// <typeparam name="TValue">The type of the t value.</typeparam>
        /// <typeparam name="TFactor">The type of the t factor.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="factor">The factor.</param>
        /// <param name="selector">The selector.</param>
        /// <param name="equalityComparer">The equality comparer.</param>
        /// <returns>System.Boolean.</returns>
        public static bool Contains<TKey, TValue, TFactor>(this IDictionary<TKey, TValue> dictionary, TFactor factor, Func<TValue, TFactor> selector, IEqualityComparer<TFactor> equalityComparer = null)
        {
            return (dictionary.HasItem() && selector != null)
                && dictionary.Any(x =>
                {
                    return (equalityComparer ?? EqualityComparer<TFactor>.Default).Equals(selector(x.Value), factor);
                });
        }

        /// <summary>
        /// Tries the get value.
        /// </summary>
        /// <param name="cookieCollection">The cookie collection.</param>
        /// <param name="key">The key.</param>
        /// <returns>System.String.</returns>
        public static string TryGetValue(this HttpCookieCollection cookieCollection, string key)
        {
            string result = null;

            if (cookieCollection != null && !string.IsNullOrWhiteSpace(key))
            {
                var cookie = cookieCollection.Get(key);

                if (cookie != null)
                {
                    result = cookie.Value;
                }
            }

            return result;
        }

        /// <summary>
        /// Safe to get first or default. If instance is null, return default(T);
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <returns>T.</returns>
        public static T SafeFirstOrDefault<T>(this IEnumerable<T> enumerable)
        {
            return enumerable == null ? default(T) : enumerable.FirstOrDefault();
        }

        /// <summary>
        /// Safes the first or default value.
        /// </summary>
        /// <typeparam name="TKey">The type of the t key.</typeparam>
        /// <typeparam name="TValue">The type of the t value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <returns>TValue.</returns>
        public static TValue SafeFirstOrDefaultValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            return (dictionary != null && dictionary.HasItem()) ? dictionary.First().Value : default(TValue);
        }

        /// <summary>
        /// Firsts the or default.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">The array.</param>
        /// <returns>T.</returns>
        public static T FirstOrDefault<T>(this T[] array)
        {
            return array.Cast<T>().FirstOrDefault();
        }

        /// <summary>
        /// Firsts the or default.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">The array.</param>
        /// <returns>T.</returns>
        public static T FirstOrDefault<T>(this Array array)
        {
            return array.Cast<T>().FirstOrDefault();
        }

        /// <summary>
        /// Safes the first or default.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">The array.</param>
        /// <returns>T.</returns>
        public static T SafeFirstOrDefault<T>(this Array array)
        {
            return array == null ? default(T) : array.FirstOrDefault<T>();
        }

        /// <summary>
        /// Safes the first or default.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">The array.</param>
        /// <returns>T.</returns>
        public static T SafeFirstOrDefault<T>(this T[] array)
        {
            return array == null ? default(T) : array.FirstOrDefault<T>();
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="anyObject">Any object.</param>
        /// <returns>List&lt;T&gt;.</returns>
        public static List<T> AsList<T>(this T anyObject)
        {
            return anyObject != null ? new List<T>() { anyObject } : new List<T>();
        }

        /// <summary>
        /// Ases the collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="anyObject">Any object.</param>
        /// <returns>Collection&lt;T&gt;.</returns>
        public static Collection<T> AsCollection<T>(this T anyObject)
        {
            return anyObject != null ? new Collection<T> { anyObject } : new Collection<T>();
        }

        /// <summary>
        /// Ases the hash set.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="anyObject">Any object.</param>
        /// <returns>System.Collections.Generic.HashSet&lt;T&gt;.</returns>
        public static HashSet<T> AsHashSet<T>(this T anyObject)
        {
            return anyObject != null ? new HashSet<T> { anyObject } : new HashSet<T>();
        }

        /// <summary>
        /// Ases the array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="anyObject">Any object.</param>
        /// <returns>T[].</returns>
        public static T[] AsArray<T>(this T anyObject)
        {
            return anyObject != null ? new T[] { anyObject } : new T[] { };
        }

        /// <summary>
        /// Ases the dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of the t key.</typeparam>
        /// <typeparam name="TValue">The type of the t value.</typeparam>
        /// <param name="anyObject">Any object.</param>
        /// <param name="key">The key.</param>
        /// <returns>Dictionary&lt;TKey, TValue&gt;.</returns>
        public static Dictionary<TKey, TValue> AsDictionary<TKey, TValue>(this TValue anyObject, TKey key)
        {
            return anyObject == null ? new Dictionary<TKey, TValue>() : new Dictionary<TKey, TValue>() { { key, anyObject } };
        }

        /// <summary>
        /// Ases the dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of the t key.</typeparam>
        /// <typeparam name="TValue">The type of the t value.</typeparam>
        /// <param name="anyObject">Any object.</param>
        /// <param name="keyGetter">The key getter.</param>
        /// <param name="keyEqualityComparer">The key equality comparer.</param>
        /// <param name="overrideIfExists">The override if exists.</param>
        /// <returns>System.Collections.Generic.Dictionary&lt;TKey, TValue&gt;.</returns>
        public static Dictionary<TKey, TValue> AsDictionary<TKey, TValue>(this IEnumerable<TValue> anyObject, Func<TValue, TKey> keyGetter, IEqualityComparer<TKey> keyEqualityComparer = null, bool overrideIfExists = false)
        {
            Dictionary<TKey, TValue> result = new Dictionary<TKey, TValue>(keyEqualityComparer ?? EqualityComparer<TKey>.Default);

            if (anyObject.HasItem() && keyGetter != null)
            {
                foreach (var one in anyObject)
                {
                    var key = keyGetter(one);

                    if (key != null)
                    {
                        result.Merge(keyGetter(one), one, overrideIfExists);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="hashSet">The hash set.</param>
        /// <param name="items">The items.</param>
        public static void AddRange<T>(this HashSet<T> hashSet, IEnumerable<T> items)
        {
            if (hashSet != null && items != null)
            {
                foreach (var one in items)
                {
                    hashSet.Add(one);
                }
            }
        }

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <typeparam name="TKey">The type of the t key.</typeparam>
        /// <typeparam name="TValue">The type of the t value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="items">The items.</param>
        /// <param name="overrideIfExists">if set to <c>true</c> [override if exists].</param>
        public static void AddRange<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, IEnumerable<KeyValuePair<TKey, TValue>> items, bool overrideIfExists = false)
        {
            if (dictionary != null && items != null)
            {
                foreach (var one in items)
                {
                    dictionary.Merge(one.Key, one.Value, overrideIfExists);
                }
            }
        }

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="nameValueCollection">The name value collection.</param>
        /// <param name="items">The items.</param>
        public static void AddRange(this NameValueCollection nameValueCollection, NameValueCollection items)
        {
            if (nameValueCollection != null && items != null)
            {
                foreach (var key in items.AllKeys)
                {
                    nameValueCollection.Add(key, items.Get(key));
                }
            }
        }

        /// <summary>
        /// Merges the specified name value collection.
        /// </summary>
        /// <param name="nameValueCollection">The name value collection.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="overrideIfExists">if set to <c>true</c> [override if exists].</param>
        public static void Merge(this NameValueCollection nameValueCollection, string key, string value, bool overrideIfExists = false)
        {
            if (nameValueCollection != null && !string.IsNullOrWhiteSpace(key))
            {
                if (!nameValueCollection.AllKeys.Contains(key))
                {
                    nameValueCollection.Add(key, value);
                }
                else if (overrideIfExists)
                {
                    nameValueCollection.Set(key, value);
                }
            }
        }

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="nameValueCollection">The name value collection.</param>
        /// <param name="items">The items.</param>
        public static void AddRange(this NameValueCollection nameValueCollection, IDictionary<string, string> items)
        {
            if (nameValueCollection != null && items != null)
            {
                foreach (var one in items)
                {
                    nameValueCollection.Add(one.Key, one.Value);
                }
            }
        }

        /// <summary>
        /// Unions the specified item2.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item1">The item1.</param>
        /// <param name="item2">The item2.</param>
        /// <param name="allowDuplicated">if set to <c>true</c> [allow duplicated].</param>
        /// <param name="equalityComparer">The equality comparer.</param>
        /// <returns></returns>
        public static ICollection<T> Union<T>(this IEnumerable<T> item1, IEnumerable<T> item2, bool allowDuplicated = false, IEqualityComparer<T> equalityComparer = null)
        {
            var container = allowDuplicated ? new List<T>(item1?.Count() ?? 0 + item2?.Count() ?? 0) : new HashSet<T>(equalityComparer) as ICollection<T>;

            if (item1 != null)
            {
                foreach (var one in item1)
                {
                    container.Add(one);
                }
            }

            if (item2 != null)
            {
                foreach (var one in item2)
                {
                    container.Add(one);
                }
            }

            return container;
        }

        /// <summary>
        /// Finds the first match and remove it from list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TComparerIdentifier">The type of the t comparer identifier.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="comparerIdentifier">The comparer identifier.</param>
        /// <param name="comparer">The comparer.</param>
        /// <returns>T.</returns>
        public static T FindAndRemove<T, TComparerIdentifier>(this List<T> collection, TComparerIdentifier comparerIdentifier, Func<T, TComparerIdentifier, bool> comparer)
        {
            // Can NOT use IList<T>, because Array is IList<T> too, but it does not support remove at.
            if (collection != null && comparer != null)
            {
                for (var i = 0; i < collection.Count; i++)
                {
                    if (comparer(collection[i], comparerIdentifier))
                    {
                        var tmp = collection[i];
                        collection.RemoveAt(i);
                        return tmp;
                    }
                }
            }

            return default(T);
        }

        #region Dictionary Extensions

        /// <summary>
        /// Safe to get value. Only when instance is not null and key is contained, return value. Otherwise return default(T).
        /// </summary>
        /// <typeparam name="TKey">T of the key.</typeparam>
        /// <typeparam name="TValue">T of the value.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static TValue SafeGetValue<TKey, TValue>(this IDictionary<TKey, TValue> instance, TKey key, TValue defaultValue = default(TValue))
        {
            return (instance != null && key != null && instance.ContainsKey(key)) ? instance[key] : defaultValue;
        }

        /// <summary>
        /// Gets the or create.
        /// </summary>
        /// <typeparam name="TKey">The type of the t key.</typeparam>
        /// <typeparam name="TValue">The type of the t value.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="key">The key.</param>
        /// <param name="valueForCreate">The value for create.</param>
        /// <returns>TValue.</returns>
        public static TValue GetOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> instance, TKey key, TValue valueForCreate = default(TValue))
        {
            if (instance != null && key != null)
            {
                TValue result;
                if (!instance.TryGetValue(key, out result))
                {
                    instance.Add(key, valueForCreate);
                    result = valueForCreate;
                }

                return result;
            }
            else
            {
                return default(TValue);
            }
        }

        /// <summary>
        /// Merges the specified instance.
        /// </summary>
        /// <typeparam name="TKey">The type of the t key.</typeparam>
        /// <typeparam name="TValue">The type of the t value.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="keyValueCollection">The key value collection.</param>
        /// <param name="overrideIfExists">if set to <c>true</c> [override if exists].</param>
        public static void Merge<TKey, TValue>(this IDictionary<TKey, TValue> instance, IEnumerable<KeyValuePair<TKey, TValue>> keyValueCollection, bool overrideIfExists = false)
        {
            if (instance != null && keyValueCollection != null)
            {
                if (keyValueCollection.Any())
                {
                    foreach (var one in keyValueCollection)
                    {
                        var key = one.Key;
                        var value = one.Value;

                        if (key != null)
                        {
                            instance.Merge(key, value, overrideIfExists);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Loads the specified instance.
        /// </summary>
        /// <typeparam name="TKey">T of the key.</typeparam>
        /// <typeparam name="TValue">T of the value.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="keyValueCollection">The key value collection.</param>
        public static void Load<TKey, TValue>(this Dictionary<TKey, TValue> instance, ICollection<KeyValuePair<TKey, TValue>> keyValueCollection)
        {
            if (instance != null)
            {
                instance.Clear();
                instance.Merge(keyValueCollection);
            }
        }

        /// <summary>
        /// Merges the specified instance.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="overrideIfExists">if set to <c>true</c> [override if exists].</param>
        /// <returns><c>true</c> if value is inserted, <c>false</c> otherwise.</returns>
        public static bool Merge<TKey, TValue>(this IDictionary<TKey, TValue> instance, TKey key, TValue value, bool overrideIfExists = true)
        {
            if (instance != null && key != null)
            {
                if (instance.ContainsKey(key))
                {
                    if (overrideIfExists)
                    {
                        instance[key] = value;
                    }
                }
                else
                {
                    instance.Add(key, value);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Merges the specified key.
        /// </summary>
        /// <typeparam name="TKey">The type of the t key.</typeparam>
        /// <typeparam name="TValue">The type of the t value.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="determineOverride">The determine override.</param>
        /// <returns><c>true</c> if it is added, <c>false</c> otherwise.</returns>
        public static bool Merge<TKey, TValue>(this IDictionary<TKey, TValue> instance, TKey key, TValue value, Func<TKey, TValue, TValue, bool> determineOverride)
        {
            if (instance != null && key != null)
            {
                if (determineOverride == null)
                {
                    determineOverride = (k, v1, v2) => { return v1 == null; };
                }

                if (instance.ContainsKey(key))
                {
                    if (determineOverride(key, instance[key], value))
                    {
                        instance[key] = value;
                    }
                }
                else
                {
                    instance.Add(key, value);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Merges the specified instance.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="mergeObject">The merge object.</param>
        /// <param name="overrideIfExists">if set to <c>true</c> [override if exists].</param>
        /// <returns>System.Int32.</returns>
        public static int Merge<TKey, TValue>(this IDictionary<TKey, TValue> instance, IDictionary<TKey, TValue> mergeObject, bool overrideIfExists = true)
        {
            int total = 0;
            if (instance != null && mergeObject != null)
            {
                foreach (var key in mergeObject.Keys)
                {
                    if (instance.Merge(key, mergeObject[key], overrideIfExists))
                    {
                        total++;
                    }
                }
            }

            return total;
        }

        /// <summary>
        /// Merges the specified merge object.
        /// </summary>
        /// <typeparam name="TKey">The type of the t key.</typeparam>
        /// <typeparam name="TValue">The type of the t value.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="mergeObject">The merge object.</param>
        /// <param name="determineOverride">The determine override.</param>
        /// <returns>System.Int32.</returns>
        public static int Merge<TKey, TValue>(this IDictionary<TKey, TValue> instance, IDictionary<TKey, TValue> mergeObject, Func<TKey, TValue, TValue, bool> determineOverride)
        {
            int total = 0;
            if (instance != null && mergeObject != null)
            {
                if (determineOverride == null)
                {
                    determineOverride = (k, v1, v2) => { return v1 == null; };
                }

                foreach (var key in mergeObject.Keys)
                {
                    if (instance.Merge(key, mergeObject[key], determineOverride))
                    {
                        total++;
                    }
                }
            }

            return total;
        }

        /// <summary>
        /// Safes the try get value.
        /// </summary>
        /// <typeparam name="TKey">The type of the t key.</typeparam>
        /// <typeparam name="TValue">The type of the t value.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if succeed to get value, <c>false</c> otherwise.</returns>
        public static bool SafeTryGetValue<TKey, TValue>(this Dictionary<TKey, TValue> instance, TKey key, out TValue value)
        {
            value = default(TValue);
            return key != null && instance != null && instance.TryGetValue(key, out value);
        }

        /// <summary>
        /// To the key value pair string.
        /// </summary>
        /// <typeparam name="TKey">The type of the t key.</typeparam>
        /// <typeparam name="TValue">The type of the t value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="separatorChar">The separator character.</param>
        /// <param name="encodeKeyValue">if set to <c>true</c> [encode key value].</param>
        /// <returns>System.String.</returns>
        public static string ToKeyValuePairString<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, char separatorChar = '&', bool encodeKeyValue = false)
        {
            string format = "{0}={1}" + separatorChar;

            var builder = new StringBuilder(64);

            if (dictionary != null)
            {
                foreach (var one in dictionary)
                {
                    builder.AppendFormat(format, encodeKeyValue ?
                        one.Key.ToString().ToUrlPathEncodedText() : one.Key.ToString(),
                        encodeKeyValue ? one.Value.ToString().ToUrlPathEncodedText() : one.Value.ToString());
                }
            }

            return builder.ToString().TrimEnd(separatorChar);
        }

        #endregion Dictionary Extensions
    }
}