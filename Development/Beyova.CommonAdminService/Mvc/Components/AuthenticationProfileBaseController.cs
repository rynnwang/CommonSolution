using System;
using System.Web.Mvc;
using Beyova;
using Beyova.ExceptionSystem;
using Beyova.RestApi;
using Beyova.WebExtension;

namespace Beyova.CommonAdminService
{
    /// <summary>
    /// Class AuthenticationProfileBaseController.
    /// </summary>
    [TokenRequired]
    [ApiPermission(Constants.Permission.Administration, ApiPermission.Required)]
    [RestApiSessionConsistence(null)]
    public abstract class AuthenticationProfileBaseController<TUserInfo, TUserCriteria, TAuthenticationResult, TFunctionalRole> : EnvironmentBaseController
        where TUserInfo : IUserInfo<TFunctionalRole>
        where TUserCriteria : IUserCriteria<TFunctionalRole>
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

        public ActionResult QueryUser(TUserCriteria criteria, string environment)
        {
            return null;
        }
    }
}