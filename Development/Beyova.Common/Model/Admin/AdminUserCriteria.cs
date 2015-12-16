using System;

namespace Beyova.Model
{
    /// <summary>
    /// Class AdminUserCriteria.
    /// </summary>
    public class AdminUserCriteria 
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public Guid? Key { get; set; }

        /// <summary>
        /// Gets or sets the domain key.
        /// </summary>
        /// <value>
        /// The domain key.
        /// </value>
        public Guid? DomainKey { get; set; }

        /// <summary>
        /// Gets or sets the name of the login.
        /// </summary>
        /// <value>The name of the login.</value>
        public string LoginName { get; set; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>The display name.</value>
        public string DisplayName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminUserCriteria"/> class.
        /// </summary>
        public AdminUserCriteria()
            : base()
        {

        }
    }
}
