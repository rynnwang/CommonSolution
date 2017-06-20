using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Beyova.Api;
using Beyova.ApiTracking;
using Beyova.ExceptionSystem;

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
        /// <param name="builder">The builder.</param>
        /// <param name="log">The log.</param>
        /// <param name="level">The level.</param>
        /// <returns>System.String.</returns>
        private static void ApiTraceLogToString(StringBuilder builder, ApiTraceLogPiece log, int level)
        {
            if (builder != null && log != null)
            {
                builder.AppendIndent(' ', 2 * (level + 1));
                builder.AppendLineWithFormat("Entry: {0}", log.EntryStamp.ToFullDateTimeString());
                builder.AppendIndent(' ', 2 * (level + 1));
                builder.AppendLineWithFormat("Exit: {0}", log.ExitStamp.ToFullDateTimeString());
                builder.AppendIndent(' ', 2 * (level + 1));
                builder.AppendLineWithFormat("Exception Key: {0}", log.ExceptionKey);
                builder.AppendIndent(' ', 2 * (level + 1));

                foreach (var one in log.InnerTraces)
                {
                    builder.AppendLineWithFormat("Inner trace: ");
                    ApiTraceLogToString(builder, one, level + 1);
                }
            }
        }

        /// <summary>
        /// APIs the trace log to string.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <returns>System.String.</returns>
        public static string ApiTraceLogToString(this ApiTraceLog log)
        {
            StringBuilder builder = new StringBuilder();

            if (log != null)
            {
                builder.AppendLineWithFormat("Trace ID: {0}", log.TraceId);
                ApiTraceLogToString(builder, log, 0);
            }

            return builder.ToString();
        }

        #region Route Url

        /// <summary>
        /// The API URL regex
        /// </summary>
        internal const string apiUrlRegex = @"(([^\/\?]+)/)?[aA][pP][iI]/([0-9a-zA-Z\-_\.]+)/([^\/\?]+)(/([^\/\?]+))?(/(([^\/\?]+)))?(/)?";

        /// <summary>
        /// The method match
        /// </summary>
        private static Regex methodMatch = new Regex(@"(/(?<realm>([^\/\?]+)))?/api/(?<version>([0-9a-zA-Z\-_\.]+))/(?<resource>([^\/\?]+))?(/(?<parameter1>([^\/\?]+)))?(/(?<parameter2>([^\/\?]+)))?(/)?", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Fills the route information.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="context">The context.</param>
        /// <returns><c>true</c> if succeed to match url format and fill route information, <c>false</c> otherwise.</returns>
        internal static bool FillRouteInfo(this Uri uri, RuntimeContext context)
        {
            try
            {
                uri.CheckNullObject(nameof(uri));
                context.CheckNullObject(nameof(context));

                var match = methodMatch.Match(uri.PathAndQuery);

                context.ResourceName = match.Success ? match.Result("${resource}") : string.Empty;
                context.Version = match.Success ? match.Result("${version}") : string.Empty;
                context.Realm = match.Success ? match.Result("${realm}") : string.Empty;
                context.Parameter1 = match.Success ? match.Result("${parameter1}") : string.Empty;
                context.Parameter2 = match.Success ? match.Result("${parameter2}") : string.Empty;

                return match.Success;
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { Uri = uri?.ToString() });
            }
        }

        #endregion Route Url

        /// <summary>
        /// To the dictionary.
        /// </summary>
        /// <param name="methodPermissionAttributes">The method permission attributes.</param>
        /// <returns>Dictionary&lt;System.String, ApiPermission&gt;.</returns>
        public static Dictionary<string, ApiPermission> ToDictionary(this IEnumerable<ApiPermissionAttribute> methodPermissionAttributes)
        {
            Dictionary<string, ApiPermission> result = new Dictionary<string, ApiPermission>();

            if (methodPermissionAttributes != null)
            {
                foreach (var one in methodPermissionAttributes)
                {
                    result.Merge(one.PermissionIdentifier, one.Permission);
                }
            }

            return result;
        }

        /// <summary>
        /// Validates the API permission. Return the permission which cause validation failure.
        /// </summary>
        /// <param name="userPermissions">The user permissions.</param>
        /// <param name="methodPermissions">The method permissions.</param>
        /// <returns>System.Nullable&lt;KeyValuePair&lt;System.String, ApiPermission&gt;&gt;.</returns>
        public static KeyValuePair<string, ApiPermission>? ValidateApiPermission(this IList<string> userPermissions, IDictionary<string, ApiPermission> methodPermissions)
        {
            if (methodPermissions == null)
            {
                return null;
            }

            userPermissions = userPermissions ?? new List<string>();

            // Check deny first
            foreach (var one in (from item in methodPermissions where item.Value == ApiPermission.Denied select item.Key))
            {
                if (userPermissions.Contains(one))
                {
                    return new KeyValuePair<string, ApiPermission>(one, ApiPermission.Denied);
                }
            }

            // Check required permissions
            foreach (var one in (from item in methodPermissions where item.Value == ApiPermission.Required select item.Key))
            {
                if (!userPermissions.Contains(one))
                {
                    return new KeyValuePair<string, ApiPermission>(one, ApiPermission.Required);
                }
            }

            return null;
        }

        /// <summary>
        /// Validates the API permission.
        /// </summary>
        /// <param name="userPermissions">The user permissions.</param>
        /// <param name="methodPermissions">The method permissions.</param>
        /// <param name="token">The token.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <returns>BaseException.</returns>
        public static BaseException ValidateApiPermission(this IList<string> userPermissions, IDictionary<string, ApiPermission> methodPermissions, string token, string methodName)
        {
            var permissionValidationResult = userPermissions.ValidateApiPermission(methodPermissions);

            if (permissionValidationResult != null)
            {
                return new UnauthorizedOperationException(methodName, permissionValidationResult.Value.Key, permissionValidationResult.Value.Value, new ExceptionScene
                {
                    MethodName = "ValidateApiPermission"
                });
            }

            return null;
        }
    }
}