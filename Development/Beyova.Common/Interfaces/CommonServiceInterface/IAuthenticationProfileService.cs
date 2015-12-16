﻿using System;
using System.Collections.Generic;
using Beyova.Model;
using Beyova.RestApi;

namespace Beyova.CommonServiceInterface
{
    /// <summary>
    /// Interface IAuthenticationProfileService
    /// </summary>
    /// <typeparam name="TUserInfo">The type of the t user information.</typeparam>
    /// <typeparam name="TUserCriteria">The type of the t user criteria.</typeparam>
    /// <typeparam name="TAuthenticationResult">The type of the t authentication result.</typeparam>
    /// <typeparam name="TFunctionalRole">The type of the t functional role.</typeparam>
    public interface IAuthenticationProfileService<TUserInfo, TUserCriteria, TAuthenticationResult, TFunctionalRole>
        where TUserInfo : IUserInfo<TFunctionalRole>
        where TUserCriteria : IUserCriteria<TFunctionalRole>
        where TAuthenticationResult : IAuthenticationResult<TUserInfo, TFunctionalRole>
        where TFunctionalRole : struct, IConvertible
    {
        #region Authentication

        /// <summary>
        /// Staffs the login.
        /// </summary>
        /// <param name="credential">The credential.</param>
        /// <returns>LoginResult.</returns>
        [ApiOperation("Token", HttpConstants.HttpMethod.Put)]
        TAuthenticationResult Authenticate(AccessCredential credential);

        /// <summary>
        /// Log Out this instance.
        /// </summary>
        /// <param name="token">The token.</param>
        [ApiOperation("Token", HttpConstants.HttpMethod.Delete)]
        void Logout(string token);

        /// <summary>
        /// Gets the user information by token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>TUserInfo.</returns>
        [ApiOperation("Token", HttpConstants.HttpMethod.Get)]
        TUserInfo GetUserInfoByToken(string token);

        /// <summary>
        /// Renews the token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>LoginResult.</returns>
        [ApiOperation("Token", HttpConstants.HttpMethod.Post)]
        TAuthenticationResult RenewToken(string token);

        /// <summary>
        /// Gets the current session information.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>SessionInfo.</returns>
        [ApiOperation("SessionInfo", HttpConstants.HttpMethod.Get)]
        SessionInfo GetSessionInfoByToken(string token);

        /// <summary>
        /// Queries the session information.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>SessionInfo.</returns>
        [ApiOperation("SessionInfo", HttpConstants.HttpMethod.Post)]
        [TokenRequired(true)]
        List<SessionInfo> QuerySessionInfo(SessionCriteria criteria);

        #endregion

        /// <summary>
        /// Queries the user information.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;UserInfo&gt;.</returns>
        [ApiOperation("UserInfo", HttpConstants.HttpMethod.Post)]
        [TokenRequired(true)]
        List<TUserInfo> QueryUserInfo(TUserCriteria criteria);

        /// <summary>
        /// Gets the current user profile.
        /// Only get from Mango But not Odin
        /// </summary>
        /// <param name="userKey">The user key.</param>
        /// <returns>UserLogin.</returns>
        [ApiOperation("UserInfo", HttpConstants.HttpMethod.Get)]
        TUserInfo GetUserInfoByUserKey(Guid? userKey);
    }
}