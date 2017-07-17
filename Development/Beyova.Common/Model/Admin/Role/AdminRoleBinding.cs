using System;

namespace Beyova
{
    /// <summary>
    /// Class AdminRoleBinding.
    /// </summary>
    public class AdminRoleBinding : IOwnerIdentifiable, IProductIdentifier
    {
        /// <summary>
        /// Gets or sets the owner key.
        /// </summary>
        /// <value>The owner key.</value>
        public Guid? OwnerKey { get; set; }

        /// <summary>
        /// Gets or sets the role key.
        /// </summary>
        /// <value>The role key.</value>
        public Guid? RoleKey { get; set; }

        /// <summary>
        /// Gets or sets the product key.
        /// </summary>
        /// <value>The product key.</value>
        public Guid? ProductKey { get; set; }
    }
}