using System;
using System.Collections.Generic;
using System.Drawing;

namespace Beyova
{
    /// <summary>
    /// Class LambdaComparer.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TComparableType">The type of the t comparable type.</typeparam>
    public class LambdaComparer<T, TComparableType> : Comparer<T> where TComparableType : IComparable
    {
        /// <summary>
        /// Gets or sets the comparer.
        /// </summary>
        /// <value>The comparer.</value>
        public Func<T, TComparableType> Comparer
        {
            get; protected set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is descending.
        /// </summary>
        /// <value><c>true</c> if this instance is descending; otherwise, <c>false</c>.</value>
        public bool IsDescending { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LambdaComparer{T, TComparableType}" /> class.
        /// </summary>
        /// <param name="comparer">The comparer.</param>
        /// <param name="isDescending">if set to <c>true</c> [is descending].</param>
        public LambdaComparer(Func<T, TComparableType> comparer, bool isDescending = false)
        {
            this.Comparer = comparer;
            this.IsDescending = isDescending;
        }

        /// <summary>
        /// Compares the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>System.Int32.</returns>
        public override int Compare(T x, T y)
        {
            return Comparer(x).CompareTo(Comparer(y)) * (IsDescending ? -1 : 1);
        }
    }
}
