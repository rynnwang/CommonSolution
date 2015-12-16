using System;

namespace Beyova.Model
{
    /// <summary>
    /// Class AdminRoleCriteria.
    /// </summary>
    public class AdminRoleCriteria
    {
        /// <summary>
        /// Gets or sets the domain key.
        /// </summary>
        /// <value>
        /// The domain key.
        /// </value>
        public Guid? DomainKey { get; set; }

        /// <summary>
        /// Gets or sets the by key.
        /// </summary>
        /// <value>The by key.</value>
        public Guid? ByKey { get; set; }

        /// <summary>
        /// Gets or sets the by parent key.
        /// </summary>
        /// <value>The by parent key.</value>
        public Guid? ByParentKey { get; set; }

        /// <summary>
        /// Gets or sets the by user key.
        /// </summary>
        /// <value>The by user key.</value>
        public Guid? ByUserKey { get; set; }

        /// <summary>
        /// Gets or sets the by permission identifier.
        /// </summary>
        /// <value>The by permission identifier.</value>
        public string ByPermissionIdentifier { get; set; }
    }
}
