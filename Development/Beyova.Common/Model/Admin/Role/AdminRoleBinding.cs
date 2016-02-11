using System;

namespace Beyova
{
    /// <summary>
    /// Class AdminRoleBinding.
    /// </summary>
    public class AdminRoleBinding
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
    }
}
