using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Beyova.Web
{
    /// <summary>
    /// Class Extension.
    /// </summary>
    public static class Extension
    {
        /// <summary>
        /// Determines whether [is user agent from mobile] [the specified HTTP request].
        /// </summary>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <returns><c>true</c> if [is user agent from mobile] [the specified HTTP request]; otherwise, <c>false</c>.</returns>
        public static bool IsUserAgentFromMobile(this HttpRequest httpRequest)
        {
            return httpRequest != null && httpRequest.UserAgent.IsMobileUserAgent();
        }

        /// <summary>
        /// Gets the current HTTP context.
        /// </summary>
        /// <returns>System.Web.HttpContextBase.</returns>
        public static HttpContextBase GetCurrentHttpContext()
        {
            return new HttpContextWrapper(System.Web.HttpContext.Current);
        }

        /// <summary>
        /// Gets the default name of the module.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller">The controller.</param>
        /// <returns></returns>
        public static string GetDefaultModuleName<T>(this T controller)
            where T : System.Web.Mvc.Controller
        {
            return (controller != null ? controller.GetType() : typeof(T)).Name.TrimEnd("controller", true);
        }
    }
}
