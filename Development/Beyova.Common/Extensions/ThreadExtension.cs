using System;
using System.Collections.Generic;
using System.Threading;

namespace Beyova
{
    /// <summary>
    /// Class ThreadExtension.
    /// </summary>
    public static class ThreadExtension
    {
        /// <summary>
        /// The thread data
        /// </summary>
        [ThreadStatic]
        private static Dictionary<string, object> _threadData;

        /// <summary>
        /// Gets the thread data.
        /// </summary>
        /// <value>The thread data.</value>
        public static Dictionary<string, object> ThreadData
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
        public static void SetThreadData(this string key, object value)
        {
            if (!string.IsNullOrWhiteSpace(key))
            {
                ThreadData.Merge(key, value, true);
            }
        }

        /// <summary>
        /// Gets the thread data.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static object GetThreadData(this string key)
        {
            return (!string.IsNullOrWhiteSpace(key)) ? ThreadData.SafeGetValue(key) : null;
        }

        #endregion

        /// <summary>
        /// Runs as new background thread.
        /// </summary>
        /// <param name="threadStart">The thread start.</param>
        /// <returns>System.Nullable&lt;System.Int32&gt;.</returns>
        public static Thread RunAsNewBackgroundThread(this ThreadStart threadStart)
        {
            if (threadStart != null)
            {
                var thread = new Thread(threadStart) { IsBackground = true };
                thread.Start();
                return thread;
            }

            return null;
        }

        /// <summary>
        /// Runs as new background thread.
        /// </summary>
        /// <param name="threadStart">The thread start.</param>
        /// <param name="inputParameter">The input parameter.</param>
        /// <returns>System.Nullable&lt;System.Int32&gt;.</returns>
        public static Thread RunAsNewBackgroundThread(this ParameterizedThreadStart threadStart, object inputParameter)
        {
            if (threadStart != null)
            {
                var thread = new Thread(threadStart) { IsBackground = true };
                thread.Start(inputParameter);
                return thread;
            }

            return null;
        }

        /// <summary>
        /// Runs the thread task.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <returns><c>true</c> if succeed to run, <c>false</c> otherwise.</returns>
        public static bool RunThreadTask(this WaitCallback callback)
        {
            return callback != null ? ThreadPool.QueueUserWorkItem(callback) : false;
        }

        /// <summary>
        /// Runs the thread task.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <param name="inputParameter">The input parameter.</param>
        /// <returns><c>true</c> if succeed to run, <c>false</c> otherwise.</returns>
        public static bool RunThreadTask(this WaitCallback callback, object inputParameter)
        {
            return callback != null ? ThreadPool.QueueUserWorkItem(callback, inputParameter) : false;
        }
    }
}
