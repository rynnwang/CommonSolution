using System;

namespace Beyova
{
    /// <summary>
    /// Class AppProvisioningCriteria.
    /// </summary>
    public class AppProvisioningCriteria
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public Guid? Key { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the platform key.
        /// </summary>
        /// <value>
        /// The platform key.
        /// </value>
        public Guid? PlatformKey { get; set; }

        /// <summary>
        /// Gets or sets the operator key.
        /// </summary>
        /// <value>
        /// The operator key.
        /// </value>
        public Guid? OperatorKey { get; set; }
    }
}