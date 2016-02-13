using System;
using System.Web.Mvc;
using System.Web.Routing;
using Beyova.ExceptionSystem;
using Beyova.WebExtension;

namespace Beyova.CommonAdminService
{
    /// <summary>
    /// Class AdminBaseController.
    /// </summary>
    [RestApiSessionConsistenceAttribute(null)]
    public abstract class AdminBaseController : Controller
    {
        #region ThreadStatic fields

        /// <summary>
        /// The current controller
        /// </summary>
        [ThreadStatic]
        public static string CurrentController;

        /// <summary>
        /// The current action
        /// </summary>
        [ThreadStatic]
        public static string CurrentAction;

        /// <summary>
        /// Gets or sets the current environment.
        /// </summary>
        /// <value>The current environment.</value>
        [ThreadStatic]
        public static string CurrentEnvironment;

        #endregion      

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminBaseController" /> class.
        /// </summary>
        protected AdminBaseController()
        {
        }

        /// <summary>
        /// Handles the exception to partial view.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="exceptionObject">The exception object.</param>
        /// <returns>PartialViewResult.</returns>
        public virtual PartialViewResult HandleExceptionToPartialView(Exception ex,
            string httpMethod, string methodName, object exceptionObject)
        {
            var baseException = ex.Handle(
                string.Format("{0}: /{1}/", httpMethod, methodName), exceptionObject);
            "Exception".SetThreadData(baseException);
            return PartialView(Constants.ViewNames.ErrorPartialView, baseException.Code.Major);
        }

        /// <summary>
        /// Handles the exception to redirection.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="exceptionObject">The exception object.</param>
        /// <returns>RedirectToRouteResult.</returns>
        public virtual RedirectToRouteResult HandleExceptionToRedirection(Exception ex,
            string httpMethod, string methodName, object exceptionObject = null)
        {
            var baseException = ex.Handle(
                string.Format("{0}: /{1}/", httpMethod, methodName), exceptionObject);
            "Exception".SetThreadData(baseException);
            return RedirectToAction("Index", "Error", new { errorCode = (int)(baseException.Code.Major) });
        }

        /// <summary>
        /// Redirects to not found page.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult RedirectToNotFoundPage()
        {
            return View(Constants.ViewNames.ErrorView, ExceptionCode.MajorCode.ResourceNotFound);
        }

        /// <summary>
        /// Redirects to action forbidden page.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult RedirectToActionForbiddenPage()
        {
            return View(Constants.ViewNames.ErrorView, ExceptionCode.MajorCode.OperationForbidden);
        }

        /// <summary>
        /// Setups the route using URL: {controller}/{action}/{environment}/{key}
        /// </summary>
        /// <param name="route">The route.</param>
        /// <param name="defaultController">The default controller.</param>
        /// <param name="defaultAction">The default action.</param>
        /// <param name="defaultEnvironment">The default environment.</param>
        /// <param name="routeName">Name of the route.</param>
        public static void SetupRoute(RouteCollection route, string defaultController = "Admin", string defaultAction = "Index", string routeName = "AdminBased")
        {
            if (route != null)
            {
                route.MapRoute(
                    name: "EnvironmentBased",
                    url: "{controller}/{action}/{environment}/{key}",
                    defaults: new { controller = defaultController, action = defaultAction, key = UrlParameter.Optional }
                );
            }
        }
    }
}