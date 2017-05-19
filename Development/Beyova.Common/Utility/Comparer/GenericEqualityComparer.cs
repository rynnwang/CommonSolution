using System;
using System.Collections.Generic;

namespace Beyova
{
    /// <summary>
    /// Class GenericEqualityComparer.
    /// </summary>
    /// <typeparam name="TOriginal">The type of the t original.</typeparam>
    /// <typeparam name="TComparer">The type of the t comparer.</typeparam>
    public class GenericEqualityComparer<TOriginal, TComparer> : IEqualityComparer<TOriginal>
    {
        /// <summary>
        /// The _convert function
        /// </summary>
        protected Func<TOriginal, TComparer> _convertFunction;

        /// <summary>
        /// The _comparer
        /// </summary>
        protected IEqualityComparer<TComparer> _comparer;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericEqualityComparer{TOriginal, TComparer}" /> class.
        /// </summary>
        /// <param name="converter">The converter.</param>
        /// <param name="comparer">The comparer.</param>
        public GenericEqualityComparer(Func<TOriginal, TComparer> converter, IEqualityComparer<TComparer> comparer = null)
        {
            this._convertFunction = converter ?? (x => { return default(TComparer); });
            this._comparer = comparer ?? EqualityComparer<TComparer>.Default;
        }

        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x">The first object of type <paramref name="x" /> to compare.</param>
        /// <param name="y">The second object of type <paramref name="y" /> to compare.</param>
        /// <returns>true if the specified objects are equal; otherwise, false.</returns>
        public bool Equals(TOriginal x, TOriginal y)
        {
            return x != null && y != null && _convertFunction(x).Equals(_convertFunction(y));
        }

        /// <summary>
        /// Gets the hash code.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>System.Int32.</returns>
        public int GetHashCode(TOriginal obj)
        {
            return (_convertFunction(obj)?.GetHashCode()).GetValueOrDefault(0);
        }
    }
}
