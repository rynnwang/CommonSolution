using System;
using System.Collections.Generic;

namespace Beyova
{
    /// <summary>
    /// Class AdminUserInfo.
    /// </summary>
    public class AdminAuthenticationResult : IAuthenticationResult<AdminUserInfo>
    {
        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        /// <value>The token.</value>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the token expired stamp.
        /// </summary>
        /// <value>The token expired stamp.</value>
        public DateTime? TokenExpiredStamp { get; set; }

        /// <summary>
        /// Gets or sets the user information.
        /// </summary>
        /// <value>The user information.</value>
        public AdminUserInfo UserInfo { get; set; }
    }
}
