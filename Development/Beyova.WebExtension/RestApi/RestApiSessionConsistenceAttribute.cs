using System;
using System.Linq;
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

            bool tokenRequired = true;
            var tokenRequiredAttribute =
                filterContext.ActionDescriptor.GetCustomAttributes(typeof(TokenRequiredAttribute), true)
                    .FirstOrDefault() as TokenRequiredAttribute;

            if (tokenRequiredAttribute != null && !tokenRequiredAttribute.TokenRequired)
            {
                tokenRequired = false;
            }

            if (tokenRequired && !ContextHelper.IsUser)
            {
                HandleUnauthorizedAction(filterContext);
            }
        }

        /// <summary>
        /// Handles the unauthorized action.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        protected virtual void HandleUnauthorizedAction(ActionExecutingContext filterContext)
        {
            var baseException = (new UnauthorizedAccountException("Invalid token")).Handle(filterContext.HttpContext.Request.RawUrl);

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