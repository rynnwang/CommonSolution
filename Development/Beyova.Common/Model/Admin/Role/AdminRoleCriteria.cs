using System;

namespace Beyova
{
    /// <summary>
    /// Class AdminRoleCriteria.
    /// </summary>
    public class AdminRoleCriteria
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public Guid? Key { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the parent key.
        /// </summary>
        /// <value>The parent key.</value>
        public Guid? ParentKey { get; set; }

        /// <summary>
        /// Gets or sets the user key.
        /// </summary>
        /// <value>The user key.</value>
        public Guid? UserKey { get; set; }

        /// <summary>
        /// Gets or sets the permission key.
        /// </summary>
        /// <value>The permission key.</value>
        public Guid? PermissionKey { get; set; }
    }
}
