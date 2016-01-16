using System;

namespace Beyova
{
    /// <summary>
    /// Class AdminPermissionBase.
    /// </summary>
    public class AdminPermissionBase : IIdentifier
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public Guid? Key { get; set; }

        /// <summary>
        /// Gets or sets the type of the interface.
        /// </summary>
        /// <value>The type of the interface.</value>
        public AdminInterfaceType InterfaceType { get; set; }

        /// <summary>
        /// Gets or sets the domain key.
        /// </summary>
        /// <value>
        /// The domain key.
        /// </value>
        public Guid? DomainKey { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// <remarks>Identifier can be full name of method, or some other customization identifier.</remarks>
        /// </summary>
        /// <value>The identifier.</value>
        public string Identifier { get; set; }

        /// <summary>
        /// Gets or sets the type of the permission.
        /// </summary>
        /// <value>The type of the permission.</value>
        public AccessPermissionType? PermissionType { get; set; }
    }
}
