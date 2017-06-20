using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using Beyova.ApiTracking;
using Beyova.RestApi;

namespace Beyova.Api
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
        /// <param name="parent">The parent.</param>
        /// <param name="entryStamp">The entry stamp.</param>
        /// <returns>Beyova.ApiTracking.ApiTraceLogPiece.</returns>
        internal static ApiTraceLogPiece ToTraceLog(this RuntimeContext context, ApiTraceLogPiece parent, DateTime? entryStamp = null)
        {
            return context != null ? new ApiTraceLogPiece(parent, context.ApiMethod?.GetFullName(), entryStamp) : null;
        }

        /// <summary>
        /// To the trace log.
        /// </summary>
        /// <param name="methodInfo">The method information.</param>
        /// <param name="parent">The parent.</param>
        /// <param name="entryStamp">The entry stamp.</param>
        /// <returns>Beyova.ApiTracking.ApiTraceLogPiece.</returns>
        internal static ApiTraceLogPiece ToTraceLog(this MethodInfo methodInfo, ApiTraceLogPiece parent, DateTime? entryStamp = null)
        {
            return methodInfo != null ? new ApiTraceLogPiece(parent, methodInfo.GetFullName(), entryStamp) : null;
        }

        /// <summary>
        /// To the trace log.
        /// </summary>
        /// <param name="methodCallMessage">The method call message.</param>
        /// <param name="parent">The parent.</param>
        /// <param name="entryStamp">The entry stamp.</param>
        /// <returns>Beyova.ApiTracking.ApiTraceLogPiece.</returns>
        internal static ApiTraceLogPiece ToTraceLog(this IMethodCallMessage methodCallMessage, ApiTraceLogPiece parent, DateTime? entryStamp = null)
        {
            return methodCallMessage != null ? new ApiTraceLogPiece(parent, methodCallMessage.MethodBase.GetFullName()) : null;
        }

        /// <summary>
        /// To the trace log.
        /// </summary>
        /// <param name="methodCallMessage">The method call message.</param>
        /// <param name="parent">The parent.</param>
        /// <param name="entryStamp">The entry stamp.</param>
        /// <returns></returns>
        internal static ApiTraceLogPiece ToTraceLog(this AOP.MethodCallInfo methodCallMessage, ApiTraceLogPiece parent, DateTime? entryStamp = null)
        {
            return methodCallMessage != null ? new ApiTraceLogPiece(parent, methodCallMessage.MethodFullName) : null;
        }

        #region ToMethodParameters

        ///// <summary>
        ///// To the method parameters.
        ///// </summary>
        ///// <param name="method">The method.</param>
        ///// <returns>System.Collections.Generic.Dictionary&lt;System.String, Newtonsoft.Json.Linq.JToken&gt;.</returns>
        //internal static Dictionary<string, JToken> ToMethodParameters(this IMethodCallMessage method)
        //{
        //    return ToMethodParameters(method?.MethodBase, method?.Args);
        //}

        ///// <summary>
        ///// To the method parameters.
        ///// </summary>
        ///// <param name="method">The method.</param>
        ///// <param name="parameters">The parameters.</param>
        ///// <returns>System.Collections.Generic.Dictionary&lt;System.String, Newtonsoft.Json.Linq.JToken&gt;.</returns>
        //internal static Dictionary<string, JToken> ToMethodParameters(this MethodBase method, object[] parameters)
        //{
        //    Dictionary<string, JToken> result = new Dictionary<string, JToken>();

        //    if (method != null)
        //    {
        //        var parameterDeclaration = method.GetParameters();
        //        if (parameterDeclaration?.Length != parameters?.Length)
        //        {
        //            throw new InvalidObjectException("parameters", data: new { parameterSize = parameterDeclaration.Length, valueSize = parameters?.Length });
        //        }

        //        for (var i = 0; i < (parameterDeclaration?.Length ?? 0); i++)
        //        {
        //            result.Add(parameterDeclaration[i].Name, JToken.FromObject(parameters[i]));
        //        }
        //    }

        //    return result;
        //}

        #endregion ToMethodParameters

        /// <summary>
        /// To the flat.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <returns>List&lt;ApiTraceLogPiece&gt;.</returns>
        public static List<ApiTraceLogPiece> ToFlat(this ApiTraceLog log)
        {
            List<ApiTraceLogPiece> result = new List<ApiTraceLogPiece>();

            if (log != null)
            {
                result.Add(log as ApiTraceLogPiece);

                FillInnerTraceLog(result, log);
            }

            return result;
        }

        /// <summary>
        /// Fills the inner trace log.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="log">The log.</param>
        private static void FillInnerTraceLog(List<ApiTraceLogPiece> container, ApiTraceLogPiece log)
        {
            if (log.InnerTraces != null)
            {
                foreach (var one in log.InnerTraces)
                {
                    container.Add(one);
                    FillInnerTraceLog(container, one);
                }
            }
        }
    }
}