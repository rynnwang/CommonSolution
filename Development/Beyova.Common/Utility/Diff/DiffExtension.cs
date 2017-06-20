using System;
using System.Collections.Generic;

namespace Beyova
{
    /// <summary>
    /// Class DiffExtension
    /// </summary>
    public static class DiffExtension
    {
        /// <summary>
        /// Simples the difference.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
        /// <returns></returns>
        public static ValueDiffResult SimpleStringDiff(this string source, string destination, bool ignoreCase = false)
        {
            var result = new ValueDiffResult
            {
                SourceValue = source,
                DestinationValue = destination,
                DiffResult = DiffResult.NoChange
            };

            if (string.IsNullOrEmpty(source))
            {
                if (!string.IsNullOrEmpty(destination))
                {
                    result.DiffResult = DiffResult.DestinationAdded;
                }
            }
            else if (string.IsNullOrEmpty(destination))
            {
                if (!string.IsNullOrEmpty(source))
                {
                    result.DiffResult = DiffResult.SourceDeleted;
                }
            }
            else
            {
                result.DiffResult = source.Equals(destination,
                    ignoreCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture)
                    ? DiffResult.NoChange
                    : DiffResult.Replace;
            }

            return result;
        }

        /// <summary>
        /// Simples the difference.
        /// </summary>
        /// <typeparam name="TKey">The type of the t key.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
        /// <returns>Dictionary&lt;TKey, ValueDiffResult&gt;.</returns>
        public static Dictionary<TKey, ValueDiffResult> SimpleStringDiff<TKey>(this Dictionary<TKey, string> source, Dictionary<TKey, string> destination, bool ignoreCase = false)
        {
            var result = new Dictionary<TKey, ValueDiffResult>();

            if (source == null)
            {
                source = new Dictionary<TKey, string>();
            }

            if (destination == null)
            {
                destination = new Dictionary<TKey, string>();
            }

            foreach (var one in source.Keys.Union(destination.Keys))
            {
                result.Add(one, source.SafeGetValue(one).SimpleStringDiff(destination.SafeGetValue(one), ignoreCase));
            }

            return result;
        }

        /// <summary>
        /// Simples the difference.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        /// <returns></returns>
        public static ValueDiffResult<T> SimpleDiff<T>(this T source, T destination)
        {
            var result = new ValueDiffResult<T>
            {
                SourceValue = source,
                DestinationValue = destination,
                DiffResult = DiffResult.NoChange
            };

            if (source == null)
            {
                if (destination != null)
                {
                    result.DiffResult = DiffResult.DestinationAdded;
                }
            }
            else if (destination == null)
            {
                result.DiffResult = DiffResult.SourceDeleted;
            }
            else
            {
                result.DiffResult = source.Equals(destination) ? DiffResult.NoChange : DiffResult.Replace;
            }

            return result;
        }
    }
}