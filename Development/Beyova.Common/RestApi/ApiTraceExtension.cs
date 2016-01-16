using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Beyova.ApiTracking;

namespace Beyova.RestApi
{
    /// <summary>
    /// Class ApiTraceExtension.
    /// </summary>
    public static class ApiTraceExtension
    {
        /// <summary>
        /// To the trace log.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>Beyova.ApiTracking.ApiTraceLog.</returns>
        internal static ApiTraceLog ToTraceLog(this RuntimeContext context)
        {
            if (context != null)
            {
                return new ApiTraceLog
                {
                    MethodFullName = context.ApiMethod?.GetFullName(),
                    MethodParameters = new Dictionary<string, object> {
                        { "UrlParameter",context.IsActionUsed ? context.Parameter2 : context.Parameter1} }
                };
            }

            return null;
        }

        internal static ApiTraceLog ToTraceLog(this MethodInfo methodInfo)
        {


            return null;
        }
    }
}
