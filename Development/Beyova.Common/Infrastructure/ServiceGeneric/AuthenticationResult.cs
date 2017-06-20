using System;

namespace Beyova
{
    /// <summary>
    /// Interface IUserInfo
    /// </summary>
    public class AuthenticationResult<TUserInfo, TFunctionalRole> : IAuthenticationResult<TUserInfo, TFunctionalRole>
        where TUserInfo : class, IUserInfo<TFunctionalRole>
        where TFunctionalRole : struct, IConvertible
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
        public TUserInfo UserInfo { get; set; }
    }

    /// <summary>
    /// Interface IAuthenticationResult
    /// </summary>
    /// <typeparam name="TUserInfo">The type of the t user information.</typeparam>
    public class AuthenticationResult<TUserInfo> : IAuthenticationResult<TUserInfo>
        where TUserInfo : class, ICredential
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
        public TUserInfo UserInfo { get; set; }
    }
}