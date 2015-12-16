using System;

namespace Beyova
{
    /// <summary>
    /// Class for method filtering
    /// </summary>
    public class MethodFiltering
    {
        /// <summary>
        /// Gets or sets the allowed access.
        /// </summary>
        /// <value>The allowed access.</value>
        public int AllowedAccess { get; set; }

        /// <summary>
        /// Gets or sets the included attributes.
        /// If is null, it means no filtering based on attribute.
        /// </summary>
        /// <value>
        /// The included attributes.
        /// </value>
        public Attribute IncludedAttribute { get; set; }
    }
}
