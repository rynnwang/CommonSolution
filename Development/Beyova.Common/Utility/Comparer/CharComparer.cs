using System;

namespace Beyova
{
    /// <summary>
    /// Class CharComparer.
    /// </summary>
    public sealed class CharComparer : GenericEqualityComparer<char, char>
    {
        /// <summary>
        /// The ordinal
        /// </summary>
        private static CharComparer ordinal = new CharComparer(x => { return x; });

        /// <summary>
        /// The ordinal ignore case
        /// </summary>
        private static CharComparer ordinalIgnoreCase = new CharComparer(Char.ToLowerInvariant);

        /// <summary>
        /// Initializes a new instance of the <see cref="CharComparer"/> class.
        /// </summary>
        /// <param name="converter">The converter.</param>
        private CharComparer(Func<char, char> converter = null) : base(converter)
        {
        }

        /// <summary>
        /// Gets the ordinal.
        /// </summary>
        /// <value>
        /// The ordinal.
        /// </value>
        public static CharComparer Ordinal { get { return ordinal; } }

        /// <summary>
        /// Gets the ordinal ignore case.
        /// </summary>
        /// <value>
        /// The ordinal ignore case.
        /// </value>
        public static CharComparer OrdinalIgnoreCase { get { return ordinalIgnoreCase; } }
    }
}