using System;
using System.Collections.Generic;
using Beyova.RestApi;

namespace Beyova.Gravity
{
    /// <summary>
    /// Interface ICentralAuthenticationProtocol
    /// </summary>
    public interface ICentralAuthenticationProtocol
    {
        /// <summary>
        /// Authenticates the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>AdminAuthenticationResult.</returns>
        [ApiOperation(CommonServiceConstants.ResourceName.CentralAuthentication, HttpConstants.HttpMethod.Put, "Authenticate")]
        AdminAuthenticationResult Authenticate(AuthenticationRequest request);

        /// <summary>
        /// Gets the user by token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>AdminUserInfoBase.</returns>
        AdminUserInfo GetUserByToken(string token);

        /// <summary>
        /// Disposes the session.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>System.Nullable&lt;DateTime&gt;.</returns>
        [ApiOperation(CommonServiceConstants.ResourceName.CentralAuthentication, HttpConstants.HttpMethod.Delete, "DisposeSession")]
        DateTime? DisposeSession(string token);

        /// <summary>
        /// Gets the session list.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>List&lt;AdminSession&gt;.</returns>
        [ApiOperation(CommonServiceConstants.ResourceName.CentralAuthentication, HttpConstants.HttpMethod.Get, "GetSessionList")]
        List<AdminSession> GetSessionList(string token);
    }
}