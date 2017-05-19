using System;

namespace Beyova
{
    /// <summary>
    /// Class MathExtension.
    /// </summary>
    public static class MathExtension
    {
        #region Max

        /// <summary>
        /// Maximums the specified item2.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item1">The item1.</param>
        /// <param name="item2">The item2.</param>
        /// <returns>T.</returns>
        public static T Max<T>(this T item1, T item2) where T : struct, IComparable
        {
            return item1.CompareTo(item2) > 0 ? item1 : item2;
        }

        /// <summary>
        /// Maximums the specified item2.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item1">The item1.</param>
        /// <param name="item2">The item2.</param>
        /// <param name="maxResult">The maximum result.</param>
        /// <returns><c>true</c> if item1 is Max, <c>false</c> otherwise.</returns>
        public static bool Max<T>(this T item1, T item2, out T maxResult) where T : struct, IComparable
        {
            bool result = item1.CompareTo(item2) > 0;
            maxResult = result ? item1 : item2;

            return result;
        }

        /// <summary>
        /// Maximums the specified item1.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item1">The item1.</param>
        /// <param name="item2">The item2.</param>
        /// <returns>System.Nullable&lt;T&gt;.</returns>
        public static T? Max<T>(this T? item1, T? item2) where T : struct, IComparable
        {
            if (!item1.HasValue)
            {
                return item2;
            }
            else if (!item2.HasValue)
            {
                return item1;
            }
            else
            {
                return Max(item1.Value, item2.Value);
            }
        }

        /// <summary>
        /// Maximums the specified item2.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item1">The item1.</param>
        /// <param name="item2">The item2.</param>
        /// <param name="maxResult">The maximum result.</param>
        /// <returns><c>true</c> if item1 is max, <c>false</c> otherwise.</returns>
        public static bool Max<T>(this T? item1, T? item2, out T? maxResult) where T : struct, IComparable
        {
            if (!item1.HasValue)
            {
                maxResult = item2;
                return false;
            }
            else if (!item2.HasValue)
            {
                maxResult = item1;
                return false;
            }
            else
            {
                return Max(item1.Value, item2.Value, out maxResult);
            }
        }

        #endregion

        #region Min

        /// <summary>
        /// Minimums the specified item2.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item1">The item1.</param>
        /// <param name="item2">The item2.</param>
        /// <returns>T.</returns>
        public static T Min<T>(this T item1, T item2) where T : struct, IComparable
        {
            return item1.CompareTo(item2) < 0 ? item1 : item2;
        }

        /// <summary>
        /// Minimums the specified item2.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item1">The item1.</param>
        /// <param name="item2">The item2.</param>
        /// <param name="minResult">The minimum result.</param>
        /// <returns><c>true</c> if item1 is min, <c>false</c> otherwise.</returns>
        public static bool Min<T>(this T item1, T item2, out T minResult) where T : struct, IComparable
        {
            bool result = item1.CompareTo(item2) < 0;
            minResult = result ? item1 : item2;

            return result;
        }

        /// <summary>
        /// Minimums the specified item2.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item1">The item1.</param>
        /// <param name="item2">The item2.</param>
        /// <returns>System.Nullable&lt;T&gt;.</returns>
        public static T? Min<T>(this T? item1, T? item2) where T : struct, IComparable
        {
            if (!item1.HasValue)
            {
                return item2;
            }
            else if (!item2.HasValue)
            {
                return item1;
            }
            else
            {
                return Min(item1.Value, item2.Value);
            }
        }

        /// <summary>
        /// Minimums the specified item2.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item1">The item1.</param>
        /// <param name="item2">The item2.</param>
        /// <param name="minResult">The minimum result.</param>
        /// <returns><c>true</c> if item1 is min, <c>false</c> otherwise.</returns>
        public static bool Min<T>(this T? item1, T? item2, out T? minResult) where T : struct, IComparable
        {
            if (!item1.HasValue)
            {
                minResult = item2;
                return false;
            }
            else if (!item2.HasValue)
            {
                minResult = item1;
                return false;
            }
            else
            {
                return Min(item1.Value, item2.Value, out minResult);
            }
        }

        #endregion

        #region 

        /// <summary>
        /// To the double.
        /// </summary>
        /// <param name="fractionObject">The fraction object.</param>
        /// <returns>System.Nullable&lt;System.Double&gt;.</returns>
        public static double? ToDouble(this FractionObject? fractionObject)
        {
            return fractionObject.HasValue ? fractionObject.Value.ToDouble() as double? : null ;
        }

        #endregion
    }
}
