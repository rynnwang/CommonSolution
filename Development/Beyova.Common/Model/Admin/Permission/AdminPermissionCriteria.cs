using System;

namespace Beyova
{
    /// <summary>
    /// Class AdminPermissionCriteria.
    /// </summary>
    public class AdminPermissionCriteria : AdminPermissionBase
    {
        /// <summary>
        /// Gets or sets the role key.
        /// </summary>
        /// <value>The role key.</value>
        public Guid? RoleKey { get; set; }

        /// <summary>
        /// Gets or sets the user key.
        /// </summary>
        /// <value>The user key.</value>
        public Guid? UserKey { get; set; }
    }
}
