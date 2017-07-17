using System;

namespace Beyova
{
    /// <summary>
    /// Class AdminRole.
    /// </summary>
    public class AdminRole : IIdentifier, IProductIdentifier
    {
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
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the product key.
        /// </summary>
        /// <value>The product key.</value>
        public Guid? ProductKey { get; set; }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public Guid? Key { get; set; }
    }
}