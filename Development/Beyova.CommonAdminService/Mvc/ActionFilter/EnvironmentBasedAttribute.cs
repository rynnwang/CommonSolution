using System.Web.Mvc;

namespace Beyova.CommonAdminService
{
    /// <summary>
    /// Class EnvironmentBasedAttribute.
    /// </summary>
    public class EnvironmentBasedAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnvironmentBasedAttribute"/> class.
        /// </summary>
        public EnvironmentBasedAttribute()
            : base()
        {
        }

        /// <summary>
        /// Called by the ASP.NET MVC framework before the action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            AdminBaseController.CurrentEnvironment = filterContext.RouteData.Values["environment"].SafeToString();
            AdminBaseController.CurrentController = filterContext.RouteData.Values["controller"].ObjectToString();
        }

        /// <summary>
        /// Called by the ASP.NET MVC framework after the action result executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);

            AdminBaseController.CurrentEnvironment = null;
            AdminBaseController.CurrentController = null;
        }
    }
}