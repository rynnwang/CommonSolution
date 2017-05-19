using System;
using System.Collections.Generic;
using Beyova.Api;
using Beyova.RestApi;

namespace Beyova.CommonServiceInterface
{
    /// <summary>
    /// Interface ISSOAuthorizationService
    /// </summary>
    /// <typeparam name="TUserInfo">The type of the t user information.</typeparam>
    /// <typeparam name="TFunctionalRole">The type of the t functional role.</typeparam>
    /// <typeparam name="TAuthenticationResult">The type of the t authentication result.</typeparam>
    public interface ISSOAuthorizationService<TUserInfo, TFunctionalRole, TAuthenticationResult> : ISSOAuthorizationService<TUserInfo, TFunctionalRole, SSOAuthorizationPartner, SSOAuthorizationPartnerCriteria, SSOAuthorization, SSOAuthorizationCriteria, TAuthenticationResult>
        where TUserInfo : IUserInfo<TFunctionalRole>
        where TFunctionalRole : struct, IConvertible
        where TAuthenticationResult : IAuthenticationResult<TUserInfo, TFunctionalRole>
    {
    }

    /// <summary>
    /// Interface IAuthenticationProfileService
    /// </summary>
    /// <typeparam name="TUserInfo">The type of the t user information.</typeparam>
    /// <typeparam name="TFunctionalRole">The type of the t functional role.</typeparam>
    /// <typeparam name="TSSOAuthorizationPartner">The type of the SSO authorization partner.</typeparam>
    /// <typeparam name="TSSOAuthorizationPartnerCriteria">The type of the SSO authorization partner criteria.</typeparam>
    /// <typeparam name="TSSOAuthorization">The type of the SSO authorization.</typeparam>
    /// <typeparam name="TSSOAuthorizationCriteria">The type of the SSO authorization criteria.</typeparam>
    /// <typeparam name="TAuthenticationResult">The type of the t authentication result.</typeparam>
    public interface ISSOAuthorizationService<TUserInfo, TFunctionalRole, TSSOAuthorizationPartner, TSSOAuthorizationPartnerCriteria, TSSOAuthorization, TSSOAuthorizationCriteria, TAuthenticationResult>
        where TUserInfo : IUserInfo<TFunctionalRole>
        where TFunctionalRole : struct, IConvertible
        where TSSOAuthorizationPartner : SSOAuthorizationPartner
        where TSSOAuthorizationPartnerCriteria : SSOAuthorizationPartnerCriteria
        where TSSOAuthorization : SSOAuthorization
        where TSSOAuthorizationCriteria : SSOAuthorizationCriteria
        where TAuthenticationResult : IAuthenticationResult<TUserInfo, TFunctionalRole>
    {
        #region Authorization Partner

        /// <summary>
        /// Creates the or update sso authorization partner.
        /// </summary>
        /// <param name="partner">The partner.</param>
        /// <returns>System.Nullable&lt;System.Guid&gt;.</returns>
        [ApiOperation(CommonServiceConstants.ResourceName.SSOAuthorizationPartner, HttpConstants.HttpMethod.Put)]
        [ApiPermission(CommonServiceConstants.Permission.AuthenticationAdministrator)]
        Guid? CreateOrUpdateSSOAuthorizationPartner(TSSOAuthorizationPartner partner);

        /// <summary>
        /// Queries the sso authorization partner.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>System.Collections.Generic.List&lt;TSSOAuthorizationPartner&gt;.</returns>
        [ApiOperation(CommonServiceConstants.ResourceName.SSOAuthorizationPartner, HttpConstants.HttpMethod.Post)]
        List<TSSOAuthorizationPartner> QuerySSOAuthorizationPartner(TSSOAuthorizationPartnerCriteria criteria);

        #endregion

        #region SSO - Exchange Token

        /// <summary>
        /// Requests the token exchange.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>TSSOAuthorization.</returns>
        [ApiOperation(CommonServiceConstants.ResourceName.SSOTokenExchange, HttpConstants.HttpMethod.Put)]
        TSSOAuthorization RequestTokenExchange(SSOAuthorizationRequest request);

        /// <summary>
        /// Exchanges the token.
        /// </summary>
        /// <param name="authorization">The authorization.</param>
        /// <returns>SessionInfo.</returns>
        [ApiOperation(CommonServiceConstants.ResourceName.SSOTokenExchange, HttpConstants.HttpMethod.Post)]
        SessionInfo ExchangeToken(SSOAuthorizationBase authorization);

        #endregion

        #region SSO - OAuth

        /// <summary>
        /// Requests the o authentication.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>TSSOAuthorization.</returns>
        [ApiOperation(CommonServiceConstants.ResourceName.OAuth, HttpConstants.HttpMethod.Put)]
        TSSOAuthorization RequestOAuth(SSOAuthorizationRequest request);

        /// <summary>
        /// Grants the o authentication.
        /// </summary>
        /// <param name="authorization">The authorization.</param>
        /// <returns>TSSOAuthorization.</returns>
        [ApiOperation(CommonServiceConstants.ResourceName.OAuth, HttpConstants.HttpMethod.Post, "Grant")]
        TSSOAuthorization GrantOAuth(TSSOAuthorization authorization);

        /// <summary>
        /// Verifies the o authentication.
        /// </summary>
        /// <param name="authorization">The authorization.</param>
        /// <returns>TSSOAuthorization.</returns>
        [ApiOperation(CommonServiceConstants.ResourceName.OAuth, HttpConstants.HttpMethod.Post, "Verify")]
        TUserInfo VerifyOAuth(TSSOAuthorization authorization);

        #endregion

        /// <summary>
        /// Queries the sso authorization.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>TSSOAuthorization.</returns>
        [ApiOperation(CommonServiceConstants.ResourceName.SSOAuthorization, HttpConstants.HttpMethod.Post)]
        [ApiPermission(CommonServiceConstants.Permission.AuthenticationAdministrator)]
        List<TSSOAuthorization> QuerySSOAuthorization(TSSOAuthorizationCriteria criteria);
    }
}
