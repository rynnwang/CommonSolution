using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beyova.CommonService.DataAccessController;
using Beyova.CommonServiceInterface;

namespace Beyova.CommonService
{
    /// <summary>
    /// Class SSOAuthorizationService. This class cannot be inherited.
    /// </summary>
    public class SSOAuthorizationService<TUserInfo, TFunctionalRole, TAuthenticationResult> : SSOAuthorizationService<TUserInfo, TFunctionalRole, SSOAuthorizationPartner, SSOAuthorizationPartnerCriteria, SSOAuthorization, SSOAuthorizationCriteria, TAuthenticationResult, SSOAuthorizationPartnerAccessController, SSOAuthorizationAccessController>
        where TUserInfo : IUserInfo<TFunctionalRole>
        where TFunctionalRole : struct, IConvertible
        where TAuthenticationResult : IAuthenticationResult<TUserInfo, TFunctionalRole>
    {
    }

    /// <summary>
    /// Class SSOAuthorizationService.
    /// </summary>
    /// <typeparam name="TUserInfo">The type of the t user information.</typeparam>
    /// <typeparam name="TFunctionalRole">The type of the t functional role.</typeparam>
    /// <typeparam name="TSSOAuthorizationPartner">The type of the sso authorization partner.</typeparam>
    /// <typeparam name="TSSOAuthorizationPartnerCriteria">The type of the sso authorization partner criteria.</typeparam>
    /// <typeparam name="TSSOAuthorization">The type of the sso authorization.</typeparam>
    /// <typeparam name="TSSOAuthorizationCriteria">The type of the sso authorization criteria.</typeparam>
    /// <typeparam name="TAuthenticationResult">The type of the t authentication result.</typeparam>
    /// <typeparam name="TSSOAuthorizationPartnerAccessController">The type of the sso authorization partner access controller.</typeparam>
    /// <typeparam name="TSSOAuthorizationAccessController">The type of the sso authorization access controller.</typeparam>
    public abstract class SSOAuthorizationService<TUserInfo, TFunctionalRole, TSSOAuthorizationPartner, TSSOAuthorizationPartnerCriteria, TSSOAuthorization, TSSOAuthorizationCriteria, TAuthenticationResult, TSSOAuthorizationPartnerAccessController, TSSOAuthorizationAccessController>
        : ISSOAuthorizationService<TUserInfo, TFunctionalRole, TSSOAuthorizationPartner, TSSOAuthorizationPartnerCriteria, TSSOAuthorization, TSSOAuthorizationCriteria, TAuthenticationResult>
        where TUserInfo : IUserInfo<TFunctionalRole>
        where TFunctionalRole : struct, IConvertible
        where TAuthenticationResult : IAuthenticationResult<TUserInfo, TFunctionalRole>
        where TSSOAuthorizationPartner : SSOAuthorizationPartner, new()
        where TSSOAuthorizationPartnerCriteria : SSOAuthorizationPartnerCriteria
        where TSSOAuthorization : SSOAuthorization, new()
        where TSSOAuthorizationCriteria : SSOAuthorizationCriteria, new()
        where TSSOAuthorizationPartnerAccessController : SSOAuthorizationPartnerAccessController<TSSOAuthorizationPartner, TSSOAuthorizationPartnerCriteria>, new()
        where TSSOAuthorizationAccessController : SSOAuthorizationAccessController<TSSOAuthorization, TSSOAuthorizationCriteria>, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SSOAuthorizationService{TUserInfo, TFunctionalRole, TSSOAuthorizationPartner, TSSOAuthorizationPartnerCriteria, TSSOAuthorization, TSSOAuthorizationCriteria, TAuthenticationResult, TSSOAuthorizationPartnerAccessController, TSSOAuthorizationAccessController}"/> class.
        /// </summary>
        protected SSOAuthorizationService() : base()
        {
        }

        /// <summary>
        /// Generates the token.
        /// </summary>
        /// <returns></returns>
        protected string GenerateToken()
        {
            return this.CreateRandomString(32);
        }

        #region SSOAuthorizationPartner

        /// <summary>
        /// Creates the or update sso authorization partner.
        /// </summary>
        /// <param name="partner">The partner.</param>
        /// <returns>
        /// System.Nullable&lt;System.Guid&gt;.
        /// </returns>
        public Guid? CreateOrUpdateSSOAuthorizationPartner(TSSOAuthorizationPartner partner)
        {
            try
            {
                partner.CheckNullObject(nameof(partner));

                if (!partner.Key.HasValue && string.IsNullOrWhiteSpace(partner.Token))
                {
                    partner.Token = GenerateToken();
                }

                using (var controller = new SSOAuthorizationPartnerAccessController<TSSOAuthorizationPartner, TSSOAuthorizationPartnerCriteria>())
                {
                    return controller.CreateOrUpdateSSOAuthorizationPartner(partner, ContextHelper.GetCurrentOperatorKey());
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(partner);
            }
        }

        /// <summary>
        /// Queries the sso authorization partner.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>System.Collections.Generic.List&lt;TSSOAuthorizationPartner&gt;.</returns>
        public List<TSSOAuthorizationPartner> QuerySSOAuthorizationPartner(TSSOAuthorizationPartnerCriteria criteria)
        {
            try
            {
                criteria.CheckNullObject(nameof(criteria));

                using (var controller = new SSOAuthorizationPartnerAccessController<TSSOAuthorizationPartner, TSSOAuthorizationPartnerCriteria>())
                {
                    return controller.QuerySSOAuthorizationPartner(criteria);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(criteria);
            }
        }

        #endregion

        #region Token Rechange

        /// <summary>
        /// Exchanges the token.
        /// </summary>
        /// <param name="authorization">The authorization.</param>
        /// <returns>SessionInfo.</returns>
        public SessionInfo ExchangeToken(SSOAuthorizationBase authorization)
        {
            try
            {
                authorization.CheckNullObject(nameof(authorization));
                authorization.AuthorizationToken.CheckEmptyString("authorization.AuthorizationToken");
                authorization.ClientRequestId.CheckEmptyString("authorization.ClientRequestId");

                using (var controller = new SessionInfoAccessController())
                {
                    return controller.ExchangeSSOAuthorization(authorization);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(authorization);
            }
        }

        /// <summary>
        /// Requests the token exchange.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>TSSOAuthorization.</returns>
        public TSSOAuthorization RequestTokenExchange(SSOAuthorizationRequest request)
        {
            try
            {
                request.CheckNullObject(nameof(request));

                using (var controller = new SSOAuthorizationAccessController<TSSOAuthorization, TSSOAuthorizationCriteria>())
                {
                    return controller.RequestSSOTokenExchange(request);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(request);
            }
        }

        #endregion

        #region OAuth

        public TSSOAuthorization GrantOAuth(TSSOAuthorization authorization)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Requests the o authentication.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>TSSOAuthorization.</returns>
        public TSSOAuthorization RequestOAuth(SSOAuthorizationRequest request)
        {
            try
            {
                request.CheckNullObject(nameof(request));

                using (var controller = new SSOAuthorizationAccessController<TSSOAuthorization, TSSOAuthorizationCriteria>())
                {
                    return controller.RequestOAuth(request);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(request);
            }
        }

        public TUserInfo VerifyOAuth(TSSOAuthorization authorization)
        {
            throw new NotImplementedException();
        }

        #endregion

        /// <summary>
        /// Queries the sso authorization.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>TSSOAuthorization.</returns>
        public List<TSSOAuthorization> QuerySSOAuthorization(TSSOAuthorizationCriteria criteria)
        {
            try
            {
                criteria.CheckNullObject(nameof(criteria));

                using (var controller = new SSOAuthorizationAccessController<TSSOAuthorization, TSSOAuthorizationCriteria>())
                {
                    return controller.QuerySSOAuthorization(criteria);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(criteria);
            }
        }
    }
}
