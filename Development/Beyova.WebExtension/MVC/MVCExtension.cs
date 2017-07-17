using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;

namespace Beyova.Web
{
    /// <summary>
    /// Class MVCExtension.
    /// </summary>
    public static class MVCExtension
    {
        /// <summary>
        /// Gets the name of the controller.
        /// </summary>
        /// <param name="routeData">The route data.</param>
        /// <returns>System.String.</returns>
        public static string GetControllerName(this RouteData routeData)
        {
            return routeData?.Values["controller"]?.ToString();
        }

        /// <summary>
        /// Gets the name of the area.
        /// </summary>
        /// <param name="routeData">The route data.</param>
        /// <returns>System.String.</returns>
        public static string GetAreaName(this RouteData routeData)
        {
            return routeData?.Values["area"]?.ToString();
        }

        /// <summary>
        /// Gets the name of the action.
        /// </summary>
        /// <param name="routeData">The route data.</param>
        /// <returns>System.String.</returns>
        public static string GetActionName(this RouteData routeData)
        {
            return routeData?.Values["action"]?.ToString();
        }

        /// <summary>
        /// Gets the name of the current controller.
        /// </summary>
        /// <param name="anyObject">Any object.</param>
        /// <returns>System.String.</returns>
        public static string GetCurrentControllerName(this object anyObject)
        {
            try
            {
                return HttpContext.Current.Request.RequestContext.RouteData.GetControllerName();
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the name of the current area.
        /// </summary>
        /// <param name="anyObject">Any object.</param>
        /// <returns>System.String.</returns>
        public static string GetCurrentAreaName(this object anyObject)
        {
            try
            {
                return HttpContext.Current.Request.RequestContext.RouteData.GetAreaName();
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the name of the current acton.
        /// </summary>
        /// <param name="anyObject">Any object.</param>
        /// <returns>System.String.</returns>
        public static string GetCurrentActonName(this object anyObject)
        {
            try
            {
                return HttpContext.Current.Request.RequestContext.RouteData.GetActionName();
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
