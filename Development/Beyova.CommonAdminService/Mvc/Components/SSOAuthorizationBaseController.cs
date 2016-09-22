using System;
using System.Web.Mvc;
using Beyova.Api;
using Beyova.CommonServiceInterface;
using Beyova.ExceptionSystem;

namespace Beyova.CommonAdminService
{
    /// <summary>
    /// Class SSOAuthorizationBaseController.
    /// </summary>
    /// <typeparam name="TUserInfo">The type of the t user information.</typeparam>
    /// <typeparam name="TFunctionalRole">The type of the t functional role.</typeparam>
    /// <typeparam name="TAuthenticationResult">The type of the t authentication result.</typeparam>
    public abstract class SSOAuthorizationBaseController<TUserInfo, TFunctionalRole, TAuthenticationResult>
        : SSOAuthorizationBaseController<TUserInfo, TFunctionalRole, SSOAuthorizationPartner, SSOAuthorizationPartnerCriteria, SSOAuthorization, SSOAuthorizationCriteria, TAuthenticationResult>
        where TUserInfo : IUserInfo<TFunctionalRole>
        where TFunctionalRole : struct, IConvertible
        where TAuthenticationResult : IAuthenticationResult<TUserInfo, TFunctionalRole>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Beyova.CommonAdminService.SSOAuthorizationBaseController`7" /> class.
        /// </summary>
        /// <param name="moduleName">Name of the module.</param>
        public SSOAuthorizationBaseController(string moduleName = null) : base(moduleName)
        {
        }
    }

    /// <summary>
    /// Class SSOAuthorizationBaseController.
    /// </summary>
    [TokenRequired]
    public abstract class SSOAuthorizationBaseController<TUserInfo, TFunctionalRole, TSSOAuthorizationPartner, TSSOAuthorizationPartnerCriteria, TSSOAuthorization, TSSOAuthorizationCriteria, TAuthenticationResult>
        : EnvironmentBaseController
        where TUserInfo : IUserInfo<TFunctionalRole>
        where TFunctionalRole : struct, IConvertible
        where TSSOAuthorizationPartner : SSOAuthorizationPartner, new()
        where TSSOAuthorizationPartnerCriteria : SSOAuthorizationPartnerCriteria, new()
        where TSSOAuthorization : SSOAuthorization
        where TSSOAuthorizationCriteria : SSOAuthorizationCriteria, new()
        where TAuthenticationResult : IAuthenticationResult<TUserInfo, TFunctionalRole>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SSOAuthorizationBaseController{TUserInfo, TFunctionalRole, TSSOAuthorizationPartner, TSSOAuthorizationPartnerCriteria, TSSOAuthorization, TSSOAuthorizationCriteria, TAuthenticationResult}"/> class.
        /// </summary>
        /// <param name="moduleName">Name of the module.</param>
        public SSOAuthorizationBaseController(string moduleName = null) : base(moduleName.SafeToString("SSO"))
        {
        }

        /// <summary>
        /// Gets the client.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <returns></returns>
        protected abstract ISSOAuthorizationService<TUserInfo, TFunctionalRole, TSSOAuthorizationPartner, TSSOAuthorizationPartnerCriteria, TSSOAuthorization, TSSOAuthorizationCriteria, TAuthenticationResult> GetClient(string environment);

        /// <summary>
        /// Gets the view full path.
        /// </summary>
        /// <param name="viewName">Name of the view.</param>
        /// <returns>System.String.</returns>
        protected override string GetViewFullPath(string viewName)
        {
            return string.Format(Constants.ViewNames.BeyovaComponentDefaultViewPath, "SSOAuthorization", viewName);
        }

        /// <summary>
        /// Ssoes the authorization partner.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult SSOAuthorizationPartner(string environment)
        {
            return View(GetViewFullPath(Constants.ViewNames.SSOAuthorizationPartnerPanelView));
        }

        /// <summary>
        /// Ssoes the authorization partner.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="environment">The environment.</param>
        /// <returns>
        /// ActionResult.
        /// </returns>
        public PartialViewResult QuerySSOAuthorizationPartner(TSSOAuthorizationPartnerCriteria criteria, string environment)
        {
            try
            {
                criteria.CheckNullObject(nameof(criteria));

                var client = GetClient(environment);
                var partners = client.QuerySSOAuthorizationPartner(criteria);
                return PartialView(GetViewFullPath(Constants.ViewNames.SSOAuthorizationPartnerListView), partners);
            }
            catch (Exception ex)
            {
                return this.HandleExceptionToPartialView(ex, new { criteria, environment });
            }
        }

        /// <summary>
        /// Gets the sso authorization partner.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="environment">The environment.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetSSOAuthorizationPartner(Guid? key, string environment)
        {
            object result = null;
            BaseException exception = null;

            try
            {
                key.CheckNullObject(nameof(key));

                var client = GetClient(environment);
                result = client.QuerySSOAuthorizationPartner(new TSSOAuthorizationPartnerCriteria { Key = key }).SafeFirstOrDefault();
            }
            catch (Exception ex)
            {
                exception = ex.Handle(new { key, environment });
            }

            Beyova.RestApi.ApiHandlerBase.PackageResponse(this.Response, result, exception);
            return null;
        }

        /// <summary>
        /// Creates the or update sso authorization partner.
        /// </summary>
        /// <param name="partner">The partner.</param>
        /// <param name="environment">The environment.</param>
        /// <returns></returns>
        public JsonResult CreateOrUpdateSSOAuthorizationPartner(TSSOAuthorizationPartner partner, string environment)
        {
            object result = null;
            BaseException exception = null;

            try
            {
                partner.CheckNullObject(nameof(partner));

                var client = GetClient(environment);
                result = client.CreateOrUpdateSSOAuthorizationPartner(partner);
            }
            catch (Exception ex)
            {
                exception = ex.Handle(new { partner, environment });
            }

            Beyova.RestApi.ApiHandlerBase.PackageResponse(this.Response, result, exception);
            return null;
        }

        /// <summary>
        /// Renews the sso authorization partner token.
        /// </summary>
        /// <param name="partnerKey">The partner key.</param>
        /// <param name="environment">The environment.</param>
        /// <returns></returns>
        public JsonResult RenewSSOAuthorizationPartnerToken(Guid? partnerKey, string environment)
        {
            object result = null;
            BaseException exception = null;

            try
            {
                partnerKey.CheckNullObject(nameof(partnerKey));

                var client = GetClient(environment);
                var newToken = this.CreateRandomString(64);
                client.CreateOrUpdateSSOAuthorizationPartner(new TSSOAuthorizationPartner { Token = newToken, Key = partnerKey });
                result = newToken;
            }
            catch (Exception ex)
            {
                exception = ex.Handle(new { partnerKey, environment });
            }

            Beyova.RestApi.ApiHandlerBase.PackageResponse(this.Response, result, exception);
            return null;
        }
    }
}