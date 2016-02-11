using System;
using Beyova.RestApi;

namespace Beyova
{
    /// <summary>
    /// Class AdminPermissionConnection.
    /// </summary>
    public class AdminPermissionConnection : AdminPermissionBase
    {
        /// <summary>
        /// Gets or sets the owner key.
        /// </summary>
        /// <value>The owner key.</value>
        public Guid? OwnerKey { get; set; }

        /// <summary>
        /// Gets or sets the permission.
        /// </summary>
        /// <value>The permission.</value>
        public ApiPermission Permission { get; set; }
    }
}
