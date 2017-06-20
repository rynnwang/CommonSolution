using System;
using System.Collections.Generic;

namespace Beyova
{
    /// <summary>
    /// Class AdminUserInfo
    /// </summary>
    public class AdminUserInfo : BaseCredential, IUserInfo, IIdentifier, ICredential, IProjectBased, IThirdPartyIdentifier
    {
        /// <summary>
        /// Gets or sets the name of the login.
        /// </summary>
        /// <value>The name of the login.</value>
        public string LoginName { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the third party identifier.
        /// </summary>
        /// <value>The third party identifier.</value>
        public string ThirdPartyId { get; set; }

        /// <summary>
        /// Gets or sets the permissions.
        /// </summary>
        /// <value>The permissions.</value>
        public List<string> Permissions { get; set; }

        /// <summary>
        /// Gets or sets the avatar key.
        /// </summary>
        /// <value>The avatar key.</value>
        public Guid? AvatarKey { get; set; }

        /// <summary>
        /// Gets or sets the avatar URL.
        /// </summary>
        /// <value>The avatar URL.</value>
        public string AvatarUrl { get; set; }

        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        /// <value>The language.</value>
        public string Language { get; set; }

        /// <summary>
        /// Gets or sets the time zone. Unit: minute
        /// </summary>
        /// <value>The time zone.</value>
        public int? TimeZone { get; set; }

        /// <summary>
        /// Gets or sets the project key.
        /// </summary>
        /// <value>The project key.</value>
        public Guid? ProjectKey { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminUserInfo"/> class.
        /// </summary>
        public AdminUserInfo() { this.Permissions = new List<string>(); }
    }
}