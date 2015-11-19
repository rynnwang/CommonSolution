using System;

namespace ifunction.Model
{
    /// <summary>
    /// Class AdminRoleConnection.
    /// </summary>
    public class AdminRoleConnection 
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
