using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Beyova.ApiTracking.Model;

namespace Beyova.RestApi
{
    /// <summary>
    /// Class RestApiExtension.
    /// </summary>
    public static class RestApiExtension
    {
        /// <summary>
        /// APIs the event log to string.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <returns>System.String.</returns>
        public static string ApiEventLogToString(this ApiEventLog log)
        {
            var builder = new StringBuilder();

            if (log != null)
            {
                builder.AppendLineWithFormat("{0}: {1}", log.CreatedStamp.ToFullDateTimeString(), log.ApiFullName);
                builder.AppendLine(log.ToJson());
            }

            return builder.ToString();
        }

        /// <summary>
        /// APIs the trace log to string.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="level">The level.</param>
        /// <returns>System.String.</returns>
        private static string ApiTraceLogToString(ApiTraceLog log, int level)
        {
            StringBuilder builder = new StringBuilder();

            if (log != null)
            {
                builder.AppendIndent(' ', 2 * (level));
                builder.AppendLineWithFormat("{0}: {1}", log.CreatedStamp.ToFullDateTimeString(), log.MethodFullName);
                builder.AppendIndent(' ', 2 * (level + 1));
                builder.AppendLineWithFormat("Entry: {0}", log.EntryStamp.ToFullDateTimeString());
                builder.AppendIndent(' ', 2 * (level + 1));
                builder.AppendLineWithFormat("Exit: {0}", log.ExitStamp.ToFullDateTimeString());
                builder.AppendIndent(' ', 2 * (level + 1));
                builder.AppendLineWithFormat("Parameters: {0}", log.MethodParameters.ToJson());
                builder.AppendIndent(' ', 2 * (level + 1));
                builder.AppendLineWithFormat("Exception: {0}", log.Exception == null ? "NA" : log.Exception.ToJson());
                builder.AppendIndent(' ', 2 * (level + 1));
                foreach (var one in log.InnerTraces)
                {
                    builder.AppendLineWithFormat("Inner trace: {0}", ApiTraceLogToString(one, level + 1));
                }
            }

            return builder.ToString();
        }

        /// <summary>
        /// APIs the trace log to string.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <returns>System.String.</returns>
        public static string ApiTraceLogToString(this ApiTraceLog log)
        {
            return ApiTraceLogToString(log, 0);
        }

        #region Route Url

        /// <summary>
        /// The API URL regex
        /// </summary>
        internal const string apiUrlRegex = @"[aA][pP][iI]/([0-9a-zA-Z\-_\.]+)/([^\/]+)(/([^\/]+))?(/(([^\/]+)))?(/)?";

        /// <summary>
        /// The proxy URL regex
        /// </summary>
        internal const string proxyUrlRegex = @"[pP][rR][oO][cC][yY]/([0-9a-zA-Z\-_\.]+)/(.+)";

        /// <summary>
        /// The method match
        /// </summary>
        private static Regex methodMatch = new Regex(@"/api/(?<version>([0-9a-zA-Z\-_\.]+))/(?<resource>([^\/]+))?(/(?<parameter1>([^\/]+)))?(/(?<parameter2>([^\/]+)))?(/)?", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// The proxy match
        /// </summary>
        private static Regex proxyMatch = new Regex(@"/proxy/(?<client>([0-9a-zA-Z\-_\.]+))/(?<destination>(.+))", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Gets the route information.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="version">The version.</param>
        /// <param name="parameter1">The parameter1.</param>
        /// <param name="parameter2">The parameter2.</param>
        /// <returns><c>true</c> if succeed to match, <c>false</c> otherwise.</returns>
        internal static bool GetRouteInfo(this HttpRequest request, out string resourceName, out string version, out string parameter1, out string parameter2)
        {
            request.CheckNullObject("request");

            var match = methodMatch.Match(request.Url.PathAndQuery);

            resourceName = match.Success ? match.Result("${resource}") : string.Empty;
            version = match.Success ? match.Result("${version}") : string.Empty;
            parameter1 = match.Success ? match.Result("${parameter1}") : string.Empty;
            parameter2 = match.Success ? match.Result("${parameter2}") : string.Empty;

            return match.Success;
        }

        /// <summary>
        /// Fills the route information.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="context">The context.</param>
        /// <returns><c>true</c> if succeed to match url format and fill route information, <c>false</c> otherwise.</returns>
        internal static bool FillRouteInfo(this HttpRequest request, RuntimeContext context)
        {
            try
            {
                request.CheckNullObject("request");
                context.CheckNullObject("context");

                var match = methodMatch.Match(request.Url.PathAndQuery);

                context.ResourceName = match.Success ? match.Result("${resource}") : string.Empty;
                context.Version = match.Success ? match.Result("${version}") : string.Empty;
                context.Parameter1 = match.Success ? match.Result("${parameter1}") : string.Empty;
                context.Parameter2 = match.Success ? match.Result("${parameter2}") : string.Empty;

                return match.Success;
            }
            catch (Exception ex)
            {
                throw ex.Handle("FillRouteInfo");
            }
        }

        /// <summary>
        /// Gets the proxy information.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="clientIdentifier">The client identifier.</param>
        /// <param name="destination">The destination.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="body">The body.</param>
        /// <returns><c>true</c> if succeed to match, <c>false</c> otherwise.</returns>
        internal static bool GetProxyInfo(this HttpRequest request, out string clientIdentifier, out string destination, out string httpMethod, out string body)
        {
            request.CheckNullObject("request");

            var match = proxyMatch.Match(request.Url.PathAndQuery);

            clientIdentifier = match.Success ? match.Result("${client}") : string.Empty;
            destination = match.Success ? match.Result("${destination}") : string.Empty;
            httpMethod = request.HttpMethod;
            body = request.GetPostJson();

            return match.Success;
        }

        #endregion

    }
}
