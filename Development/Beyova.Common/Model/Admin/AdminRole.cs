
using System;

namespace Beyova.Model
{
    /// <summary>
    /// Class AdminRole.
    /// </summary>
    public class AdminRole : BaseObject
    {
        /// <summary>
        /// Gets or sets the domain key.
        /// </summary>
        /// <value>
        /// The domain key.
        /// </value>
        public Guid? DomainKey { get; set; }

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
    }
}
