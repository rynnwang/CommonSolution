using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace ifunction
{
    /// <summary>
    /// 
    /// </summary>
    public static class ThreadExtension
    {
        /// <summary>
        /// The thread data
        /// </summary>
        [ThreadStatic]
        private static Dictionary<string, object> _threadData;

        /// <summary>
        /// Gets the thead data.
        /// </summary>
        /// <value>
        /// The thead data.
        /// </value>
        public static Dictionary<string, object> TheadData
        {
            get
            {
                if (_threadData == null)
                {
                    _threadData = new Dictionary<string, object>();
                }

                return _threadData;
            }
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public static void Clear()
        {
            _threadData = null;
        }

        #region Thread

        /// <summary>
        /// Sets the thread data.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public static void SetThreadData(this  string key, object value)
        {
            if (!string.IsNullOrWhiteSpace(key))
            {
                TheadData.Merge(key, value, true);
            }
        }

        /// <summary>
        /// Gets the thread data.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static object GetThreadData(this string key)
        {
            return (!string.IsNullOrWhiteSpace(key)) ? TheadData.SafeGetValue(key) : null;
        }

        #endregion
    }
}
