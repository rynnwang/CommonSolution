using System;

namespace Beyova
{
    /// <summary>
    /// Class AdminPermissionCriteria.
    /// </summary>
    public class AdminPermissionCriteria : AdminPermissionBase
    {
        /// <summary>
        /// Gets or sets the by role key.
        /// </summary>
        /// <value>The by role key.</value>
        public Guid? ByRoleKey { get; set; }

        /// <summary>
        /// Gets or sets the by user key.
        /// </summary>
        /// <value>The by user key.</value>
        public Guid? ByUserKey { get; set; }
    }
}
