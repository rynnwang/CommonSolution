using System;
using System.Collections.Generic;

namespace Beyova
{
    /// <summary>
    /// Class LambdaComparer.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LambdaComparer<T> : LambdaComparer<T, T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LambdaComparer{T}"/> class.
        /// </summary>
        /// <param name="comparison">The comparison.</param>
        public LambdaComparer(Func<T, T, int> comparison)
            : base(x => x, comparison)
        {
        }
    }

    /// <summary>
    /// Class LambdaComparer.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TCompareType">The type of the t compare type.</typeparam>
    public class LambdaComparer<T, TCompareType> : Comparer<T>
    {
        /// <summary>
        /// Gets or sets the comparer.
        /// </summary>
        /// <value>The comparer.</value>
        public Func<T, TCompareType> Comparer
        {
            get; protected set;
        }

        /// <summary>
        /// Gets the comparison.
        /// </summary>
        /// <value>The comparison.</value>
        public Func<TCompareType, TCompareType, int> Comparison
        {
            get; protected set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LambdaComparer{T, TCompareType}" /> class.
        /// </summary>
        /// <param name="comparer">The comparer.</param>
        /// <param name="comparison">The comparison.</param>
        public LambdaComparer(Func<T, TCompareType> comparer, Func<TCompareType, TCompareType, int> comparison)
        {
            this.Comparer = comparer;
            this.Comparison = comparison;
        }

        /// <summary>
        /// When overridden in a derived class, performs a comparison of two objects of the same type and returns a value indicating whether one object is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />, as shown in the following table.Value Meaning Less than zero <paramref name="x" /> is less than <paramref name="y" />.Zero <paramref name="x" /> equals <paramref name="y" />.Greater than zero <paramref name="x" /> is greater than <paramref name="y" />.</returns>
        public override int Compare(T x, T y)
        {
            return (this.Comparer != null && this.Comparison != null) ? this.Comparison(Comparer(x), Comparer(y)) : 0;
        }
    }
}
