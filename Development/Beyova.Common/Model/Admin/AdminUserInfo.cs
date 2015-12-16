using System;

namespace Beyova.Model
{
    /// <summary>
    /// Class AdminUserInfo.
    /// </summary>
    public class AdminUserInfo : BaseObject
    {
        /// <summary>
        /// Gets or sets the name of the login.
        /// </summary>
        /// <value>The name of the login.</value>
        public string LoginName { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>The display name.</value>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the domain key.
        /// </summary>
        /// <value>
        /// The domain key.
        /// </value>
        public Guid? DomainKey { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminUserInfo"/> class.
        /// </summary>
        public AdminUserInfo()
            : base()
        {

        }
    }
}
