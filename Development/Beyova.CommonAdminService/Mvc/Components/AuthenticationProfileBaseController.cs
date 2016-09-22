using System;
using System.Web.Mvc;
using Beyova;
using Beyova.Api;
using Beyova.CommonServiceInterface;
using Beyova.ExceptionSystem;
using Beyova.RestApi;
using System.Linq;
using Beyova.WebExtension;

namespace Beyova.CommonAdminService
{
    /// <summary>
    /// Class AuthenticationProfileBaseController.
    /// </summary>
    /// <typeparam name="TUserInfo">The type of the t user information.</typeparam>
    /// <typeparam name="TUserCriteria">The type of the t user criteria.</typeparam>
    /// <typeparam name="TAuthenticationResult">The type of the t authentication result.</typeparam>
    /// <typeparam name="TFunctionalRole">The type of the t functional role.</typeparam>
    [TokenRequired]
    [ApiPermission(Constants.Permission.Administration, ApiPermission.Required)]
    [RestApiContextConsistence()]
    public abstract class AuthenticationProfileBaseController<TUserInfo, TUserCriteria, TAuthenticationResult, TFunctionalRole> : EnvironmentBaseController
        where TUserInfo : IUserInfo<TFunctionalRole>
        where TUserCriteria : IUserCriteria<TFunctionalRole>, new()
        where TAuthenticationResult : IAuthenticationResult<TUserInfo, TFunctionalRole>
        where TFunctionalRole : struct, IConvertible
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnvironmentBaseController" /> class.
        /// </summary>
        /// <param name="moduleCode">The module code.</param>
        public AuthenticationProfileBaseController(string moduleCode) : base(moduleCode)
        {
        }

        /// <summary>
        /// Gets the client.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <returns>IAuthenticationProfileService&lt;TUserInfo, TUserCriteria, TAuthenticationResult, TFunctionalRole&gt;.</returns>
        protected abstract IAuthenticationProfileService<TUserInfo, TUserCriteria, TAuthenticationResult, TFunctionalRole> GetClient(EnvironmentEndpoint endpoint);

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult Index()
        {
            return View(GetViewFullPath(Constants.ViewNames.UserPanel));
        }

        /// <summary>
        /// Users the information.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult UserInfo()
        {
            return View(GetViewFullPath(Constants.ViewNames.UserPanel));
        }

        /// <summary>
        /// Users the information.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        public ActionResult UserInfo(TUserCriteria criteria)
        {
            try
            {
                criteria.CheckNullObject("criteria");
                var client = GetClient(GetEnvironmentEndpoint(CurrentEnvironment));
                var users = client.QueryUserInfo(criteria);
                return PartialView(GetViewFullPath(Constants.ViewNames.UserList), users);
            }
            catch (Exception ex)
            {
                return this.HandleExceptionToPartialView(ex, criteria);
            }
        }

        /// <summary>
        /// Users the detail.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        public ActionResult UserDetail(Guid? key)
        {
            try
            {
                key.CheckNullObject("key");
                var client = GetClient(GetEnvironmentEndpoint(CurrentEnvironment));
                var user = client.QueryUserInfo(new TUserCriteria { Key = key }).FirstOrDefault();

                return user == null ? this.RenderAsNotFoundPage() : View(GetViewFullPath(Constants.ViewNames.UserDetail), user);
            }
            catch (Exception ex)
            {
                return this.HandleExceptionToRedirection(ex, key);
            }
        }

        /// <summary>
        /// Sessions this instance.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        public ActionResult SessionInfo(string key = null)
        {
            return View(GetViewFullPath(Constants.ViewNames.SessionPanel), model: key);
        }

        /// <summary>
        /// Sessions the specified criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        public ActionResult SessionInfo(SessionCriteria criteria)
        {
            try
            {
                criteria.CheckNullObject("criteria");

                criteria.OrderDescending = true;

                var client = GetClient(GetEnvironmentEndpoint(CurrentEnvironment));
                var users = client.QuerySessionInfo(criteria);
                return PartialView(GetViewFullPath(Constants.ViewNames.SessionList), users);
            }
            catch (Exception ex)
            {
                return this.HandleExceptionToPartialView(ex, criteria);
            }
        }

        /// <summary>
        /// Sessions the detail.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        public ActionResult SessionDetail(Guid? key)
        {
            try
            {
                key.CheckNullObject("key");
                var client = GetClient(GetEnvironmentEndpoint(CurrentEnvironment));
                var user = client.QueryUserInfo(new TUserCriteria { Key = key }).FirstOrDefault();

                return user == null ? this.RenderAsNotFoundPage() : View(GetViewFullPath(Constants.ViewNames.SessionDetail), user);
            }
            catch (Exception ex)
            {
                return this.HandleExceptionToRedirection(ex, key);
            }
        }

        /// <summary>
        /// Disables the session.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        public ActionResult DisableSession(string token)
        {
            object result = null;
            BaseException exception = null;

            try
            {
                token.CheckEmptyString("token");

                var client = GetClient(GetEnvironmentEndpoint(CurrentEnvironment));
                client.Logout(token);

                result = DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                exception = ex.Handle(token);
            }

            ApiHandlerBase.PackageResponse(Response, result, exception);
            return null;
        }
    }
}