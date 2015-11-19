using System;
using System.Net;
using System.Web;
using ifunction.Model;
using ifunction.RestApi;

namespace ifunction
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
        /// Gets the current credential.
        /// </summary>
        /// <value>The current credential.</value>
        public static ICredential CurrentCredential
        {
            get { return ApiContext.CurrentCredential; }
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
            set { ApiContext.IpAddress = value; }
        }

        /// <summary>
        /// Gets or sets the user agent.
        /// </summary>
        /// <value>The user agent.</value>
        public static string UserAgent
        {
            get { return ApiContext.UserAgent; }
            set { ApiContext.UserAgent = value; }
        }

        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        /// <value>The token.</value>
        public static string Token
        {
            get { return ApiContext.Token; }
            set { ApiContext.Token = value; }
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
        public static void ConsistContext(HttpRequestBase httpRequest, string settingName)
        {
            if (httpRequest != null)
            {
                ConsistContext(httpRequest.Headers.Get(HttpConstants.HttpHeader.TOKEN) ?? httpRequest.Cookies.TryGetValue(HttpConstants.HttpHeader.TOKEN), settingName);
            }
        }

        /// <summary>
        /// Consists the context.
        /// </summary>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <param name="settingName">Name of the setting.</param>
        public static void ConsistContext(HttpRequest httpRequest, string settingName)
        {
            if (httpRequest != null)
            {
                ConsistContext(httpRequest.Headers.Get(HttpConstants.HttpHeader.TOKEN) ?? httpRequest.Cookies.TryGetValue(HttpConstants.HttpHeader.TOKEN), settingName);
            }
        }

        /// <summary>
        /// Consists the context.
        /// </summary>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <param name="settingName">Name of the setting.</param>
        public static void ConsistContext(HttpListenerRequest httpRequest, string settingName)
        {
            if (httpRequest != null)
            {
                ConsistContext(httpRequest.Headers.Get(HttpConstants.HttpHeader.TOKEN), settingName);
            }
        }

        /// <summary>
        /// Consists the context.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="settingName">Name of the setting.</param>
        private static void ConsistContext(string token, string settingName)
        {
            RestApiSettings setting;
            RestApiEventHandlers restApiEventHandlers = null;

            if (!string.IsNullOrWhiteSpace(settingName) && ApiHandlerBase.settingsContainer.TryGetValue(settingName, out setting))
            {
                restApiEventHandlers = setting.EventHandlers;
            }

            if (restApiEventHandlers != null && !string.IsNullOrWhiteSpace(token))
            {
                ContextHelper.ApiContext.CurrentCredential = restApiEventHandlers.GetCredentialByToken(token);
                ContextHelper.ApiContext.Token = token;
            }
            else
            {
                ContextHelper.ApiContext.CurrentUserInfo = null;
                ContextHelper.ApiContext.Token = null;
            }
        }

        #endregion

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public static void Clear()
        {
            threadKey_ApiContext.SetThreadData(null);
        }

        /// <summary>
        /// Reinitializes this instance.
        /// </summary>
        public static void Reinitialize()
        {
            threadKey_ApiContext.SetThreadData(new ApiContext());
        }
    }
}
