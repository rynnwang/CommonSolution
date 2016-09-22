using System;
using Newtonsoft.Json.Linq;

namespace Beyova
{
    /// <summary>
    /// Class UserPreferenceCriteria.
    /// </summary>
    public class UserPreferenceCriteria
    {
        /// <summary>
        /// Gets or sets the realm. Realm can be used based on App, Application, Product, etc.
        /// </summary>
        /// <value>The realm.</value>
        public string Realm { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the owner key.
        /// </summary>
        /// <value>The owner key.</value>
        public virtual Guid? OwnerKey { get; set; }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>The category.</value>
        public string Category { get; set; }
    }
}
