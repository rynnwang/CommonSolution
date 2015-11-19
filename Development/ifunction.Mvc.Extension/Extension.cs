using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ifunction.Mvc.Extension
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
            return httpRequest != null && !string.IsNullOrWhiteSpace(httpRequest.UserAgent) && (
                    httpRequest.UserAgent.IndexOf("pad", StringComparison.InvariantCultureIgnoreCase) > -1
                        || httpRequest.UserAgent.IndexOf("android", StringComparison.InvariantCultureIgnoreCase) > -1
                        || httpRequest.UserAgent.IndexOf("phone", StringComparison.InvariantCultureIgnoreCase) > -1
                    );
        }

        /// <summary>
        /// Gets the current HTTP context.
        /// </summary>
        /// <returns>System.Web.HttpContextBase.</returns>
        public static HttpContextBase GetCurrentHttpContext()
        {
            return new HttpContextWrapper(System.Web.HttpContext.Current);
        }
    }
}
