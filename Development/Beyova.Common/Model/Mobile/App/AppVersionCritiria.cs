using System;

namespace Beyova
{
    /// <summary>
    /// Class AppVersionCriteria.
    /// </summary>
    public class AppVersionCriteria
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public Guid? Key { get; set; }

        /// <summary>
        /// Gets or sets the platform key.
        /// </summary>
        /// <value>
        /// The platform key.
        /// </value>
        public Guid? PlatformKey { get; set; }

        /// <summary>
        /// Gets or sets the platform.
        /// </summary>
        /// <value>The platform.</value>
        public PlatformType? Platform { get; set; }

        /// <summary>
        /// Gets or sets the operator key.
        /// </summary>
        /// <value>
        /// The operator key.
        /// </value>
        public Guid? OperatorKey { get; set; }
    }
}