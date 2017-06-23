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
        /// Internals the maximum.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item1">The item1.</param>
        /// <param name="item2">The item2.</param>
        /// <returns></returns>
        internal static T InternalMax<T>(this T item1, T item2) where T : struct, IComparable
        {
            return item1.CompareTo(item2) > 0 ? item1 : item2;
        }

        /// <summary>
        /// Maximums the specified item2.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TComparible">The type of the comparible.</typeparam>
        /// <param name="item1">The item1.</param>
        /// <param name="item2">The item2.</param>
        /// <param name="getComparer">The get comparer.</param>
        /// <param name="maxResult">The maximum result.</param>
        /// <returns><c>true</c> if item1 is Max, <c>false</c> otherwise.</returns>
        public static bool Max<TEntity, TComparible>(this TEntity item1, TEntity item2, Func<TEntity, TComparible> getComparer, out TEntity maxResult)
            where TComparible : struct, IComparable
        {
            if (getComparer == null)
            {
                maxResult = default(TEntity);
                return false;
            }

            if (item1 != null)
            {
                maxResult = item2;
                return false;
            }
            else if (item2 != null)
            {
                maxResult = item1;
                return false;
            }
            else
            {
                var result = getComparer(item1).CompareTo(getComparer(item2)) > 0;
                maxResult = result ? item1 : item2;

                return result;
            }
        }

        /// <summary>
        /// Maximums the specified item2.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item1">The item1.</param>
        /// <param name="item2">The item2.</param>
        /// <returns>T.</returns>
        public static T Max<T>(this T item1, T item2) where T : struct, IComparable
        {
            return InternalMax(item1, item2);
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
                return InternalMax(item1.Value, item2.Value);
            }
        }

        /// <summary>
        /// Maximums the specified item2.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TComparible">The type of the comparible.</typeparam>
        /// <param name="item1">The item1.</param>
        /// <param name="item2">The item2.</param>
        /// <param name="getComparer">The get comparer.</param>
        /// <param name="maxResult">The maximum result.</param>
        /// <returns></returns>
        public static TEntity Max<TEntity, TComparible>(this TEntity item1, TEntity item2, Func<TEntity, TComparible> getComparer)
            where TComparible : struct, IComparable
        {
            var maxResult = default(TEntity);

            if (getComparer != null)
            {
                Max(item1, item2, getComparer, out maxResult);
            }

            return maxResult;
        }

        #endregion Max

        #region Min

        /// <summary>
        /// Internals the minimum.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item1">The item1.</param>
        /// <param name="item2">The item2.</param>
        /// <returns></returns>
        internal static T InternalMin<T>(this T item1, T item2) where T : struct, IComparable
        {
            return item1.CompareTo(item2) < 0 ? item1 : item2;
        }

        /// <summary>
        /// Minimums the specified item2.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TComparible">The type of the comparible.</typeparam>
        /// <param name="item1">The item1.</param>
        /// <param name="item2">The item2.</param>
        /// <param name="getComparer">The get comparer.</param>
        /// <param name="minResult">The minimum result.</param>
        /// <returns><c>true</c> if item1 is Min, <c>false</c> otherwise.</returns>
        public static bool Min<TEntity, TComparible>(this TEntity item1, TEntity item2, Func<TEntity, TComparible> getComparer, out TEntity minResult)
         where TComparible : struct, IComparable
        {
            if (getComparer == null)
            {
                minResult = default(TEntity);
                return false;
            }

            if (item1 != null)
            {
                minResult = item2;
                return false;
            }
            else if (item2 != null)
            {
                minResult = item1;
                return false;
            }
            else
            {
                var result = getComparer(item1).CompareTo(getComparer(item2)) < 0;
                minResult = result ? item1 : item2;

                return result;
            }
        }

        /// <summary>
        /// Minimums the specified item2.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item1">The item1.</param>
        /// <param name="item2">The item2.</param>
        /// <returns>T.</returns>
        public static T Min<T>(this T item1, T item2) where T : struct, IComparable
        {
            return InternalMin(item1, item2);
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
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TComparible">The type of the comparible.</typeparam>
        /// <param name="item1">The item1.</param>
        /// <param name="item2">The item2.</param>
        /// <param name="getComparer">The get comparer.</param>
        /// <returns></returns>
        public static TEntity Min<TEntity, TComparible>(this TEntity item1, TEntity item2, Func<TEntity, TComparible> getComparer)
       where TComparible : struct, IComparable
        {
            var minResult = default(TEntity);

            if (getComparer != null)
            {
                Min(item1, item2, getComparer, out minResult);
            }

            return minResult;
        }

        #endregion Min

        #region

        /// <summary>
        /// To the double.
        /// </summary>
        /// <param name="fractionObject">The fraction object.</param>
        /// <returns>System.Nullable&lt;System.Double&gt;.</returns>
        public static double? ToDouble(this FractionObject? fractionObject)
        {
            return fractionObject.HasValue ? fractionObject.Value.ToDouble() as double? : null;
        }

        #endregion
    }
}