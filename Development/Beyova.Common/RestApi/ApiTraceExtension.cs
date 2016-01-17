using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Beyova.ApiTracking;
using Beyova.ExceptionSystem;

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
        internal static RuntimeApiTraceLog ToTraceLog(this RuntimeContext context, RuntimeApiTraceLog parent)
        {
            if (context != null)
            {
                return new RuntimeApiTraceLog
                {
                    Parent = parent,
                    RawTraceLog = new ApiTraceLog()
                    {
                        MethodFullName = context.ApiMethod?.GetFullName(),
                        MethodParameters = new Dictionary<string, object> {
                        { "UrlParameter",context.IsActionUsed ? context.Parameter2 : context.Parameter1} }
                    }
                };
            }

            return null;
        }

        internal static RuntimeApiTraceLog ToTraceLog(this MethodInfo methodInfo, RuntimeApiTraceLog parent)
        {


            return null;
        }

        /// <summary>
        /// To the trace log.
        /// </summary>
        /// <param name="methodCallMessage">The method call message.</param>
        /// <param name="parent">The parent.</param>
        /// <returns>Beyova.ApiTracking.RuntimeApiTraceLog.</returns>
        internal static RuntimeApiTraceLog ToTraceLog(this IMethodCallMessage methodCallMessage, RuntimeApiTraceLog parent)
        {
            if (methodCallMessage != null)
            {
                return new RuntimeApiTraceLog
                {
                    Parent = parent,
                    RawTraceLog = new ApiTraceLog()
                    {
                        CreatedStamp = DateTime.UtcNow,
                        EntryStamp = DateTime.UtcNow,
                        MethodFullName = methodCallMessage.MethodBase.GetFullName(),
                        MethodParameters = methodCallMessage.ToMethodParameters()
                    }
                };
            }

            return null;
        }

        #region ToMethodParameters 

        /// <summary>
        /// To the method parameters.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <returns>System.Collections.Generic.Dictionary&lt;System.String, System.Object&gt;.</returns>
        internal static Dictionary<string, object> ToMethodParameters(this IMethodCallMessage method)
        {
            return ToMethodParameters(method?.MethodBase, method?.Args);
        }

        /// <summary>
        /// To the method parameters.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>System.Collections.Generic.Dictionary&lt;System.String, System.Object&gt;.</returns>
        internal static Dictionary<string, object> ToMethodParameters(this MethodBase method, object[] parameters)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            if (method != null)
            {
                var parameterDeclaration = method.GetParameters();
                if (parameterDeclaration?.Length != parameters?.Length)
                {
                    throw new InvalidObjectException("parameters", data: new { parameterSize = parameterDeclaration.Length, valueSize = parameters?.Length });
                }

                for (var i = 0; i < (parameterDeclaration?.Length ?? 0); i++)
                {
                    result.Add(parameterDeclaration[i].Name, parameters[i]);
                }
            }

            return result;
        }

        #endregion
    }
}
