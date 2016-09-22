using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Newtonsoft.Json.Linq;

namespace Beyova
{
    /// <summary>
    /// </summary>
    public abstract class GenericComparer<T> : IEqualityComparer<T>
    {
        /// <summary>
        /// The convert function
        /// </summary>
        protected Func<T, T> _convertFunction;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericComparer{T}" /> class.
        /// </summary>
        /// <param name="converter">The converter.</param>
        protected GenericComparer(Func<T, T> converter = null)
        {
            this._convertFunction = converter ?? (x => { return x; });
        }

        /// <summary>
        /// Equalses the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public bool Equals(T x, T y)
        {
            return x != null && y != null && _convertFunction(x).Equals(_convertFunction(y));
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public int GetHashCode(T obj)
        {
            return (_convertFunction(obj)?.GetHashCode()).GetValueOrDefault(0);
        }
    }
}
