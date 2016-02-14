using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using Beyova;
using Beyova.ExceptionSystem;
using Beyova.RestApi;

namespace Beyova.WebExtension
{
    /// <summary>
    /// Class RestApiSessionConsistenceAttribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class RestApiSessionConsistenceAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Gets or sets the name of the setting.
        /// </summary>
        /// <value>The name of the setting.</value>
        public string SettingName { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RestApiSessionConsistenceAttribute" /> class.
        /// </summary>
        /// <param name="settingName">Name of the setting.</param>
        public RestApiSessionConsistenceAttribute(string settingName)
            : base()
        {
            this.SettingName = settingName;
        }

        /// <summary>
        /// Called by the ASP.NET MVC framework before the action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            var request = filterContext.HttpContext.Request;
            ContextHelper.ConsistContext(request, this.SettingName);

            var methodInfo = (filterContext.ActionDescriptor as ReflectedActionDescriptor)?.MethodInfo;
            var controllerType = methodInfo?.DeclaringType;

            var tokenRequiredAttribute = methodInfo?.GetCustomAttribute<TokenRequiredAttribute>(true) ?? controllerType?.GetCustomAttribute<TokenRequiredAttribute>(true);
            var permissionAttributes = controllerType?.GetCustomAttributes<ApiPermissionAttribute>(true).ToDictionary();
            permissionAttributes.Merge(methodInfo?.GetCustomAttributes<ApiPermissionAttribute>(true).ToDictionary(), true);

            var tokenRequired = tokenRequiredAttribute != null && tokenRequiredAttribute.TokenRequired;

            if (tokenRequired)
            {
                if (!ContextHelper.IsUser)
                {
                    var baseException = (new UnauthorizedTokenException(ContextHelper.Token)).Handle(filterContext.HttpContext.Request.RawUrl);
                    HandleUnauthorizedAction(filterContext, baseException);
                }
                else if (permissionAttributes.HasItem())
                {
                    var baseException = ContextHelper.CurrentUserInfo?.Permissions.ValidateApiPermission(permissionAttributes, ContextHelper.Token, methodInfo?.GetFullName());

                    HandleUnauthorizedAction(filterContext, baseException);
                }
            }
        }

        /// <summary>
        /// Handles the unauthorized action.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        /// <param name="baseException">The base exception.</param>
        protected virtual void HandleUnauthorizedAction(ActionExecutingContext filterContext, BaseException baseException)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                ApiHandlerBase.PackageResponse(filterContext.HttpContext.Response, null, baseException);
                filterContext.Result = null;
            }
            else
            {
                "Exception".SetThreadData(baseException);
                filterContext.Result = new RedirectToRouteResult(routeValues: new RouteValueDictionary
                {
                    {"controller", "Error"},
                    {"action", "Index"},
                    {"errorCode", 401}
                });
            }
        }
    }
}