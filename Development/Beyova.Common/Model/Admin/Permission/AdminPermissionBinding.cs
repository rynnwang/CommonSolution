using System;

namespace Beyova
{
    /// <summary>
    /// Class AdminPermissionBinding.
    /// </summary>
    public class AdminPermissionBinding : IOwnerIdentifiable, IProductIdentifier
    {
        /// <summary>
        /// Gets or sets the owner key.
        /// </summary>
        /// <value>The owner key.</value>
        public Guid? OwnerKey { get; set; }

        /// <summary>
        /// Gets or sets the product key.
        /// </summary>
        /// <value>The product key.</value>
        public Guid? ProductKey { get; set; }

        /// <summary>
        /// Gets or sets the permission key.
        /// </summary>
        /// <value>The permission key.</value>
        public Guid? PermissionKey { get; set; }
    }
}