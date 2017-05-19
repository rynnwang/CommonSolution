using System;
using Beyova.RestApi;

namespace Beyova
{
    /// <summary>
    /// Class AdminPermissionBinding.
    /// </summary>
    public class AdminPermissionBinding : IOwnerIdentifiable, IProjectBased
    {
        /// <summary>
        /// Gets or sets the owner key.
        /// </summary>
        /// <value>The owner key.</value>
        public Guid? OwnerKey { get; set; }

        /// <summary>
        /// Gets or sets the project key.
        /// </summary>
        /// <value>The project key.</value>
        public Guid? ProjectKey { get; set; }

        /// <summary>
        /// Gets or sets the permission key.
        /// </summary>
        /// <value>The permission key.</value>
        public Guid? PermissionKey { get; set; }
    }
}
