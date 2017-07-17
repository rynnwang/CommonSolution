using System;

namespace Beyova.SaasPlatform
{
    /// <summary>
    /// Class BusinessPartition
    /// </summary>
    /// <seealso cref="Beyova.IIdentifier" />
    /// <seealso cref="Beyova.IOwnerIdentifiable" />
    public class BusinessPartition : IIdentifier, IOwnerIdentifiable
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public Guid? Key { get; set; }

        /// <summary>
        /// Gets or sets the owner key.
        /// </summary>
        /// <value>
        /// The owner key.
        /// </value>
        public Guid? OwnerKey { get; set; }
    }
}