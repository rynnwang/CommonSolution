﻿//////////////////////////////
// This code is generated by Beyova.common.RestApiClientGenerator.
// UTC: 2016-05-25 12:57:58.015
//////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using Beyova.ProgrammingIntelligence;
using Beyova.ExceptionSystem;
using Beyova;
using Beyova.RestApi;
using Beyova.Api;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Beyova.CommonAdminService
{
    /// <summary>
    /// Class AuthenticationProfileServiceClient.
    /// </summary>
    /// <typeparam name="TUserInfo">The type of the t user information.</typeparam>
    /// <typeparam name="TUserCriteria">The type of the t user criteria.</typeparam>
    /// <typeparam name="TAuthenticationResult">The type of the t authentication result.</typeparam>
    /// <typeparam name="TFunctionalRole">The type of the t functional role.</typeparam>
    public class AuthenticationProfileServiceClient<TUserInfo, TUserCriteria, TAuthenticationResult, TFunctionalRole> : Beyova.RestApi.RestApiClient
        , Beyova.CommonServiceInterface.IAuthenticationProfileService<TUserInfo, TUserCriteria, TAuthenticationResult, TFunctionalRole>

            where TUserInfo : Beyova.IUserInfo<TFunctionalRole>
            where TUserCriteria : Beyova.IUserCriteria<TFunctionalRole>
            where TAuthenticationResult : Beyova.IAuthenticationResult<TUserInfo, TFunctionalRole>
            where TFunctionalRole : struct, System.IConvertible
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Beyova.RestApi.RestApiClient" /> class.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="acceptGZip">if set to <c>true</c> [accept g zip].</param>
        public AuthenticationProfileServiceClient(ApiEndpoint endpoint, bool acceptGZip = false) : base(endpoint, acceptGZip)
        {
        }

        /// <summary>
        /// Authenticates the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>TAuthenticationResult.</returns>
        public virtual TAuthenticationResult Authenticate(Beyova.AuthenticationRequest request)
        {
            try
            {
                return this.InvokeUsingBody("PUT", "Token", null, request).ToObject<TAuthenticationResult>();
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { request });
            }
        }

        /// <summary>
        /// Log Out.
        /// </summary>
        /// <param name="token">The token.</param>
        public virtual void Logout(System.String token)
        {
            try
            {
                this.InvokeUsingQueryString("DELETE", "Token", null, token.ToString());
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { token });
            }
        }

        /// <summary>
        /// Gets the user information by token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>TUserInfo.</returns>
        public virtual TUserInfo GetUserInfoByToken(System.String token)
        {
            try
            {
                return this.InvokeUsingQueryString("GET", "Token", null, token.ToString()).ToObject<TUserInfo>();
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { token });
            }
        }

        /// <summary>
        /// Renews the token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>LoginResult.</returns>
        public virtual TAuthenticationResult RenewToken(System.String token)
        {
            try
            {
                return this.InvokeUsingQueryString("POST", "Token", null, token.ToString()).ToObject<TAuthenticationResult>();
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { token });
            }
        }

        /// <summary>
        /// Gets the current session information.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>SessionInfo.</returns>
        public virtual Beyova.SessionInfo GetSessionInfoByToken(System.String token)
        {
            try
            {
                return this.InvokeUsingQueryString("GET", "SessionInfo", null, token.ToString()).ToObject<Beyova.SessionInfo>();
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { token });
            }
        }

        /// <summary>
        /// Queries the session information.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>SessionInfo.</returns>
        public virtual System.Collections.Generic.List<Beyova.SessionInfo> QuerySessionInfo(Beyova.SessionCriteria criteria)
        {
            try
            {
                return this.InvokeUsingBody("POST", "SessionInfo", null, criteria).ToObject<System.Collections.Generic.List<Beyova.SessionInfo>>();
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { criteria });
            }
        }

        /// <summary>
        /// Queries the user information.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;UserInfo&gt;.</returns>
        public virtual System.Collections.Generic.List<TUserInfo> QueryUserInfo(TUserCriteria criteria)
        {
            try
            {
                return this.InvokeUsingBody("POST", "UserInfo", null, criteria).ToObject<System.Collections.Generic.List<TUserInfo>>();
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { criteria });
            }
        }

        /// <summary>
        /// Gets the current user profile.
        /// </summary>
        /// <param name="userKey">The user key.</param>
        /// <returns>UserLogin.</returns>
        public virtual TUserInfo GetUserInfoByUserKey(System.Nullable<System.Guid> userKey)
        {
            try
            {
                return this.InvokeUsingQueryString("GET", "UserInfo", null, userKey == null ? null : userKey.ToString()).ToObject<TUserInfo>();
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { userKey });
            }
        }

    }
}

