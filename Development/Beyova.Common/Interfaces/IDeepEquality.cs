using System;

namespace Beyova
{
    /// <summary>
    /// Interface IDeepEquality
    /// </summary>
    public interface IDeepEquality<T>
    {
        /// <summary>
        /// Deeps the equals.
        /// </summary>
        /// <param name="obj1">The obj1.</param>
        /// <param name="obj2">The obj2.</param>
        /// <param name="stringComparison">The string comparison.</param>
        /// <returns></returns>
        bool DeepEquals(T obj1, T obj2, StringComparison stringComparison);
    }
}