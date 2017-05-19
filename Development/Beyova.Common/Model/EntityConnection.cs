using System;

namespace Beyova
{
    /// <summary>
    /// Class EntityConnection
    /// </summary>
    public class EntityConnection<TPrimaryIdentifier, TSecondaryIdentifier>
    {
        /// <summary>
        /// Gets or sets the primary identifier.
        /// </summary>
        /// <value>The primary identifier.</value>
        public TPrimaryIdentifier PrimaryIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the secondary identifier.
        /// </summary>
        /// <value>The secondary identifier.</value>
        public TSecondaryIdentifier SecondaryIdentifier { get; set; }
    }
}
