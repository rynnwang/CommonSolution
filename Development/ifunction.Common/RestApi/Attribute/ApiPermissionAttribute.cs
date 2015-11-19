using System;

namespace ifunction.RestApi
{
    /// <summary>
    /// Class ApiPermissionAttribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class ApiPermissionAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the permission identifier.
        /// </summary>
        /// <value>The permission identifier.</value>
        public string PermissionIdentifier { get; protected set; }

        /// <summary>
        /// Gets or sets the permission.
        /// </summary>
        /// <value>The permission.</value>
        public ApiPermission Permission { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiPermissionAttribute" /> class.
        /// </summary>
        /// <param name="permissionIdentifier">The permission identifier.</param>
        /// <param name="permission">The permission.</param>
        public ApiPermissionAttribute(string permissionIdentifier, ApiPermission permission = ApiPermission.Required)
        {
            this.PermissionIdentifier = permissionIdentifier;
            this.Permission = permission;
        }
    }
}
