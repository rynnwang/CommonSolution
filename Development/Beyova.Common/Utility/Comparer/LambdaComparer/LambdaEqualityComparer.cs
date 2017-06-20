using System;
using System.Collections.Generic;

namespace Beyova
{
    /// <summary>
    /// Class LambdaEqualityComparer. This class cannot be inherited.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class LambdaEqualityComparer<T> : IEqualityComparer<T>
    {
        /// <summary>
        /// Gets or sets the comparer.
        /// </summary>
        /// <value>The comparer.</value>
        public Func<T, T, bool> Comparer { get; set; }

        /// <summary>
        /// Gets or sets the hash code getter.
        /// </summary>
        /// <value>The hash code getter.</value>
        public Func<T, int> HashCodeGetter { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LambdaEqualityComparer{T}"/> class.
        /// </summary>
        /// <param name="comparer">The comparer.</param>
        /// <param name="hashCodeGetter">The hash code getter.</param>
        public LambdaEqualityComparer(Func<T, T, bool> comparer, Func<T, int> hashCodeGetter)
        {
            this.Comparer = comparer == null ? GetDefaultComparison() : comparer;
            this.HashCodeGetter = hashCodeGetter == null ? GetDefaultHashCodeGetter() : hashCodeGetter;
        }

        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x">The first object of type T to compare.</param>
        /// <param name="y">The second object of type T to compare.</param>
        /// <returns>true if the specified objects are equal; otherwise, false.</returns>
        public bool Equals(T x, T y)
        {
            return Comparer(x, y);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object" /> for which a hash code is to be returned.</param>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public int GetHashCode(T obj)
        {
            return HashCodeGetter(obj);
        }

        /// <summary>
        /// Gets the default comparison.
        /// </summary>
        /// <returns>Func&lt;T, T, System.Boolean&gt;.</returns>
        private static Func<T, T, bool> GetDefaultComparison()
        {
            return (x, y) =>
            {
                return x.Equals(y);
            };
        }

        /// <summary>
        /// Gets the default hash code getter.
        /// </summary>
        /// <returns>Func&lt;T, System.Int32&gt;.</returns>
        private static Func<T, int> GetDefaultHashCodeGetter()
        {
            return (x) =>
            {
                return x.GetHashCode();
            };
        }
    }
}