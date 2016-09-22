using System;
using System.Collections.Generic;

namespace Beyova
{
    /// <summary>
    /// Class LambdaComparableComparer. This class cannot be inherited.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TComparableType">The type of the t comparable type.</typeparam>
    public sealed class LambdaComparableComparer<T, TComparableType> : LambdaComparer<T, TComparableType> where TComparableType : IComparable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LambdaComparer{T, TComparableType}" /> class.
        /// </summary>
        /// <param name="comparer">The comparer.</param>
        /// <param name="isDescending">if set to <c>true</c> [is descending].</param>
        public LambdaComparableComparer(Func<T, TComparableType> comparer, bool isDescending = false)
            : base(comparer, GetDefaultComparison(isDescending))
        {
        }

        /// <summary>
        /// Gets the default comparison.
        /// </summary>
        /// <param name="isDescending">if set to <c>true</c> [is descending].</param>
        /// <returns>Func&lt;TComparableType, TComparableType, System.Int32&gt;.</returns>
        private static Func<TComparableType, TComparableType, int> GetDefaultComparison(bool isDescending = false)
        {
            return (x, y) =>
            {
                return x.CompareTo(y) * (isDescending ? -1 : 1);
            };
        }
    }
}
