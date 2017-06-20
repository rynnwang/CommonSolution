using System;
using System.Collections.Generic;

namespace Beyova
{
    /// <summary>
    /// Class LinqExtension.
    /// </summary>
    public static class LinqExtension
    {
        /// <summary>
        /// Wheres the specified dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TInputValue">The type of the input value.</typeparam>
        /// <typeparam name="TOutputValue">The type of the output value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="resultContainer">The result container.</param>
        /// <param name="whereClause">The where clause.</param>
        /// <param name="selector">The selector.</param>
        public static void Where<TKey, TInputValue, TOutputValue>(this IDictionary<TKey, TInputValue> dictionary, Dictionary<TKey, TOutputValue> resultContainer, Func<TKey, TInputValue, bool> whereClause, Func<TInputValue, TOutputValue> selector)
        {
            if (dictionary != null && whereClause != null && resultContainer != null && selector != null)
            {
                foreach (var one in dictionary)
                {
                    if (whereClause(one.Key, one.Value))
                    {
                        resultContainer.Add(one.Key, selector(one.Value));
                    }
                }
            }
        }

        /// <summary>
        /// Wheres the specified result container.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="resultContainer">The result container.</param>
        /// <param name="whereClause">The where clause.</param>
        public static void Where<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, Dictionary<TKey, TValue> resultContainer, Func<TKey, TValue, bool> whereClause)
        {
            if (dictionary != null && whereClause != null && resultContainer != null)
            {
                foreach (var one in dictionary)
                {
                    if (whereClause(one.Key, one.Value))
                    {
                        resultContainer.Add(one.Key, one.Value);
                    }
                }
            }
        }
    }
}