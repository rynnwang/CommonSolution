using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Beyova.WebExtension
{
    /// <summary>
    /// Class MVCExtension.
    /// </summary>
    public static class MVCExtension
    {
        /// <summary>
        /// Gets the name of the current controller.
        /// </summary>
        /// <param name="anyObject">Any object.</param>
        /// <returns>System.String.</returns>
        public static string GetCurrentControllerName(this object anyObject)
        {
            try
            {
                return HttpContext.Current.Request.RequestContext.RouteData.Values["controller"].ToString();
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
                return HttpContext.Current.Request.RequestContext.RouteData.Values["area"].ToString();
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
                return HttpContext.Current.Request.RequestContext.RouteData.Values["action"].ToString();
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
