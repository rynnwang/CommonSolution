using System;

namespace Beyova
{
    /// <summary>
    /// Interface IUserInfo
    /// </summary>
    public interface IAuthenticationResult<TUserInfo, TFunctionalRole>
        where TUserInfo : IUserInfo<TFunctionalRole>
        where TFunctionalRole : struct, IConvertible
    {
        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        /// <value>The token.</value>
        string Token { get; set; }

        /// <summary>
        /// Gets or sets the token expired stamp.
        /// </summary>
        /// <value>The token expired stamp.</value>
        DateTime? TokenExpiredStamp { get; set; }

        /// <summary>
        /// Gets or sets the user information.
        /// </summary>
        /// <value>The user information.</value>
        TUserInfo UserInfo { get; set; }
    }
}
