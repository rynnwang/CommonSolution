using System;
using System.Runtime.CompilerServices;
using System.Web.Mvc;
using System.Web.Routing;
using Beyova.ExceptionSystem;

namespace Beyova.CommonAdminService
{
    /// <summary>
    /// Class AdminBaseController.
    /// </summary>
    [EnvironmentBased]
    public abstract class EnvironmentBaseController : AdminBaseController
    {
        /// <summary>
        /// The _module code
        /// </summary>
        protected string _moduleCode;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnvironmentBaseController" /> class.
        /// </summary>
        /// <param name="moduleCode">The module code.</param>
        protected EnvironmentBaseController(string moduleCode) : base()
        {
            this._moduleCode = moduleCode;
        }

        /// <summary>
        /// Gets the environment endpoint.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <returns>EnvironmentEndpoint.</returns>
        protected abstract EnvironmentEndpoint GetEnvironmentEndpoint(string environment);

        /// <summary>
        /// Setups the route using URL: {controller}/{action}/{environment}/{key}
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="route">The route.</param>
        /// <param name="defaultAction">The default action.</param>
        /// <param name="defaultEnvironment">The default environment.</param>
        public static void SetupRoute<T>(RouteCollection route, string defaultAction = "Index", string defaultEnvironment = "DEV")
                  where T : EnvironmentBaseController
        {
            if (route != null)
            {
                var type = typeof(T);
                var name = type.Name.SubStringBeforeLastMatch("Controller");

                route.MapRoute(
                    name: name,
                    url: (name + "/{action}/{environment}/{key}"),
                    defaults: new { controller = name, action = defaultAction, environment = defaultEnvironment, key = UrlParameter.Optional }
                );
            }
        }

        /// <summary>
        /// Gets the environment based URL.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="controller">The controller.</param>
        /// <param name="action">The action.</param>
        /// <param name="environment">The environment.</param>
        /// <returns>System.String.</returns>
        protected string GetEnvironmentBasedUrl(string key = null, string controller = null, string action = null, string environment = null)
        {
            return Url.Action(action.SafeToString(CurrentAction), controller.SafeToString(CurrentController), new { key, environment = environment.SafeToString(CurrentEnvironment) });
        }
    }
}