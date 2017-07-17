using System;

namespace Beyova
{
    /// <summary>
    /// Class DeepEquality
    /// </summary>
    public class DeepEquality
    {
        /// <summary>
        /// The locker
        /// </summary>
        private static object locker = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="DeepEquality"/> class.
        /// </summary>
        protected DeepEquality()
        {
        }

        /// <summary>
        /// Deeps the equals.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="object1">The object1.</param>
        /// <param name="object2">The object2.</param>
        /// <param name="stringComparison">The string comparison.</param>
        /// <returns></returns>
        public static bool DeepEquals<T>(T object1, T object2, StringComparison stringComparison = StringComparison.Ordinal)
        {
            var equality = CreateDeepEquality<T>(stringComparison);
            return equality.DeepEquals(object1, object2, stringComparison);
        }

        /// <summary>
        /// Creates the deep equality.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IDeepEquality<T> CreateDeepEquality<T>(StringComparison stringComparison)
        {
            return DeepEqualityGenerator.CreateDeepEqualityInstance<T>(stringComparison);
        }
    }
}