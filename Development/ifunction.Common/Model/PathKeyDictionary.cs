using System;
using System.Collections.Generic;

namespace ifunction.Model
{
    /// <summary>
    /// Class DualKeyDictionary.
    /// </summary>
    public class PathKeyDictionary<TValue> : Dictionary<string, TValue>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PathKeyDictionary{TValue}"/> class.
        /// </summary>
        public PathKeyDictionary() : base(StringComparer.OrdinalIgnoreCase)
        {
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="allowPartialMatch">if set to <c>true</c> [allow partial match].</param>
        /// <returns>TValue.</returns>
        public TValue GetValue(string key, bool allowPartialMatch)
        {
            key.CheckEmptyString("key");

            if (allowPartialMatch)
            {
                foreach (var one in this)
                {
                    if (one.Key.StartsWith(key, StringComparison.OrdinalIgnoreCase))
                    {
                        return one.Value;
                    }
                }

                return default(TValue);
            }
            else
            {
                return this.ContainsKey(key) ? this[key] : default(TValue);
            }
        }

        /// <summary>
        /// Adds the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="keyPieces">The key pieces.</param>
        public void Add(TValue value, params string[] keyPieces)
        {
            value.CheckNullObject("value");
            var key = GetKey(keyPieces);
            key.CheckEmptyString("key");

            this.Add(key, value);
        }

        /// <summary>
        /// Sets the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="keyPieces">The key pieces.</param>
        public void Set(TValue value, params string[] keyPieces)
        {
            value.CheckNullObject("value");
            var key = GetKey(keyPieces);
            key.CheckEmptyString("key");

            this.Merge(key, value);
        }

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <param name="keyPieces">The key pieces.</param>
        /// <returns>System.String.</returns>
        protected static string GetKey(string[] keyPieces)
        {
            return (keyPieces == null || keyPieces.Length == 0) ? null : keyPieces.Join("/");
        }
    }
}
