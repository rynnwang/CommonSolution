using System;
using System.Net;
using System.Web;
using Beyova.ApiTracking;
using Beyova;
using Beyova.RestApi;
using System.Collections.Generic;
using System.Globalization;

namespace Beyova
{
    /// <summary>
    /// Class ContextHelper.
    /// </summary>
    public static class ContextHelper
    {
        /// <summary>
        /// The thread key_ API context
        /// </summary>
        internal const string threadKey_ApiContext = "ApiContext";

        /// <summary>
        /// The thread key_ trace context
        /// </summary>
        internal const string threadKey_TraceContext = "TraceContext";

        /// <summary>
        /// Gets the current operator key.
        /// </summary>
        /// <returns>Guid.</returns>
        public static Guid GetCurrentOperatorKey()
        {
            var credential = ApiContext.CurrentCredential;
            credential.CheckNullObject("credential");
            credential.Key.CheckNullObject("credential.Key");

            return credential.Key.Value;
        }

        /// <summary>
        /// Gets the current user information.
        /// </summary>
        /// <value>The current user information.</value>
        public static IUserInfo CurrentUserInfo
        {
            get { return ApiContext.CurrentUserInfo; }
        }

        /// <summary>
        /// Gets the current culture code.
        /// Order: Query String[language] -> IUserInfo.Language -> CultureInfo.Current.
        /// </summary>
        /// <value>The current culture code.</value>
        public static CultureInfo CurrentCultureInfo
        {
            get
            {
                var code = (ApiContext.CurrentUserInfo?.Language).SafeToString(ApiContext.CultureCode);
                return code.AsCultureInfo() ?? CultureInfo.CurrentUICulture ?? CultureInfo.CurrentCulture;
            }
        }

        /// <summary>
        /// Gets the current credential.
        /// </summary>
        /// <value>The current credential.</value>
        public static BaseCredential CurrentCredential
        {
            get
            {
                return (ApiContext.CurrentCredential as BaseCredential) ?? ToBaseCredential(ApiContext.CurrentCredential);
            }
        }

        /// <summary>
        /// Gets the current permissions.
        /// </summary>
        /// <value>The current permissions.</value>
        public static List<string> CurrentPermissions
        {
            get { return ApiContext.CurrentUserInfo?.Permissions ?? new System.Collections.Generic.List<string>(); }
        }

        /// <summary>
        /// Gets the current full identifier.
        /// </summary>
        /// <value>The current full identifier.</value>
        public static string CurrentFullIdentifier
        {
            get
            {
                var credential = CurrentCredential;
                return string.Format("{0}[{1},TOKEN: {2}]", credential.Name, credential.Key, Token);
            }
        }

        /// <summary>
        /// Gets or sets the ip address.
        /// </summary>
        /// <value>The ip address.</value>
        public static string IpAddress
        {
            get { return ApiContext.IpAddress; }
        }

        /// <summary>
        /// Gets or sets the user agent.
        /// </summary>
        /// <value>The user agent.</value>
        public static string UserAgent
        {
            get { return ApiContext.UserAgent; }
        }

        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        /// <value>The token.</value>
        public static string Token
        {
            get { return ApiContext.Token; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is user.
        /// </summary>
        /// <value><c>true</c> if this instance is user; otherwise, <c>false</c>.</value>
        public static bool IsUser
        {
            get { return ApiContext.CurrentUserInfo != null; }
        }

        /// <summary>
        /// Gets or sets the API context.
        /// </summary>
        /// <value>The API context.</value>
        public static ApiContext ApiContext
        {
            get
            {
                var result = threadKey_ApiContext.GetThreadData() as ApiContext;
                if (result == null)
                {
                    result = new ApiContext();
                    threadKey_ApiContext.SetThreadData(result);
                }

                return result;
            }
        }

        #region ConsistContext

        /// <summary>
        /// Consists the context.
        /// </summary>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <param name="settingName">Name of the setting.</param>
        internal static void ConsistContext(HttpRequestBase httpRequest, string settingName = null)
        {
            if (httpRequest != null)
            {
                ConsistContext(
                    httpRequest.Headers.Get(HttpConstants.HttpHeader.TOKEN) ?? httpRequest.Cookies.TryGetValue(HttpConstants.HttpHeader.TOKEN),
                    settingName,
                    httpRequest.UserHostAddress,
                    httpRequest.UserAgent,
                    httpRequest.QueryString.Get(HttpConstants.QueryString.Language).SafeToString(httpRequest.Cookies.Get(HttpConstants.QueryString.Language)?.Value).SafeToString(httpRequest.UserLanguages.SafeFirstOrDefault()).EnsureCultureCode()
                    );
            }
        }

        /// <summary>
        /// Consists the context.
        /// </summary>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <param name="settingName">Name of the setting.</param>
        internal static void ConsistContext(HttpRequest httpRequest, string settingName = null)
        {
            if (httpRequest != null)
            {
                ConsistContext(httpRequest.Headers.Get(HttpConstants.HttpHeader.TOKEN) ?? httpRequest.Cookies.TryGetValue(HttpConstants.HttpHeader.TOKEN),
                    settingName,
                    httpRequest.UserHostAddress,
                    httpRequest.UserAgent,
                    httpRequest.QueryString.Get(HttpConstants.QueryString.Language).SafeToString(httpRequest.Cookies.Get(HttpConstants.QueryString.Language)?.Value).SafeToString(httpRequest.UserLanguages.SafeFirstOrDefault()).EnsureCultureCode()
                    );
            }
        }

        /// <summary>
        /// Consists the context.
        /// </summary>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <param name="settingName">Name of the setting.</param>
        internal static void ConsistContext(HttpListenerRequest httpRequest, string settingName)
        {
            if (httpRequest != null)
            {
                ConsistContext(httpRequest.Headers.Get(HttpConstants.HttpHeader.TOKEN),
                    settingName,
                    httpRequest.UserHostAddress,
                    httpRequest.UserAgent,
                    httpRequest.QueryString.Get(HttpConstants.QueryString.Language).SafeToString(httpRequest.UserLanguages.SafeFirstOrDefault()).EnsureCultureCode()
                    );
            }
        }

        /// <summary>
        /// Consists the context.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="settingName">Name of the setting.</param>
        /// <param name="ipAddress">The ip address.</param>
        /// <param name="userAgent">The user agent.</param>
        /// <param name="cultureCode">The culture code.</param>
        private static void ConsistContext(string token, string settingName, string ipAddress, string userAgent, string cultureCode)
        {
            RestApiSettings setting = ApiHandlerBase.GetRestApiSettingByName(settingName, true);
            RestApiEventHandlers restApiEventHandlers = setting?.EventHandlers;

            var apiContext = ContextHelper.ApiContext;

            apiContext.UserAgent = userAgent;
            apiContext.IpAddress = ipAddress;
            ApiContext.CultureCode = cultureCode;

            if (restApiEventHandlers != null && !string.IsNullOrWhiteSpace(token))
            {
                apiContext.CurrentCredential = restApiEventHandlers.GetCredentialByToken(token);
                apiContext.Token = token;
            }
            else
            {
                apiContext.CurrentCredential = null;
                apiContext.Token = null;
            }
        }

        #endregion

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public static void Clear()
        {
            threadKey_ApiContext.SetThreadData(null);
            threadKey_TraceContext.SetThreadData(null);
        }

        /// <summary>
        /// Reinitializes this instance.
        /// </summary>
        public static void Reinitialize()
        {
            threadKey_ApiContext.SetThreadData(new ApiContext());
        }

        /// <summary>
        /// To the base credential.
        /// </summary>
        /// <param name="credential">The credential.</param>
        /// <returns>Beyova.BaseCredential.</returns>
        private static BaseCredential ToBaseCredential(ICredential credential)
        {
            return credential == null ? null : new BaseCredential { Key = credential.Key, Name = credential.Name };
        }
    }
}