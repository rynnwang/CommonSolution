using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Beyova.ExceptionSystem;
using Newtonsoft.Json.Linq;

namespace Beyova.RestApi
{
    /// <summary>
    /// Class RestApiClient.
    /// </summary>
    public class RestApiClient
    {
        /// <summary>
        /// Gets or sets the base URL.
        /// </summary>
        /// <value>The base URL.</value>
        public string BaseUrl { get; protected set; }

        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        /// <value>The token.</value>
        public string Token { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether [enable exception restore].
        /// </summary>
        /// <value><c>true</c> if [enable exception restore]; otherwise, <c>false</c>.</value>
        public bool EnableExceptionRestore { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether [accept g zip].
        /// </summary>
        /// <value><c>true</c> if [accept g zip]; otherwise, <c>false</c>.</value>
        public bool AcceptGZip { get; protected set; }

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="RestApiClient" /> class.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="version">The version.</param>
        /// <param name="isHttps">if set to <c>true</c> [is HTTPS].</param>
        /// <param name="token">The token.</param>
        /// <param name="enableExceptionRestore">if set to <c>true</c> [enable exception restore].</param>
        /// <param name="acceptGZip">if set to <c>true</c> [accept g zip].</param>
        public RestApiClient(string host, string version, bool isHttps = false, string token = null, bool enableExceptionRestore = false, bool acceptGZip = false)
            : this(new ApiEndpoint { Host = host, Version = version, Protocol = isHttps ? "https" : "http", Token = token }, acceptGZip, enableExceptionRestore)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RestApiClient" /> class.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="enableExceptionRestore">if set to <c>true</c> [enable exception restore].</param>
        /// <param name="acceptGZip">if set to <c>true</c> [accept g zip].</param>
        public RestApiClient(ApiEndpoint endpoint, bool enableExceptionRestore = false, bool acceptGZip = false)
               : this(endpoint.ToString(), endpoint.Token, acceptGZip, enableExceptionRestore)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RestApiClient" /> class.
        /// </summary>
        /// <param name="baseUrl">The base URL.
        /// <example>http://xxx.xx/api/</example></param>
        /// <param name="version">The version.</param>
        /// <param name="token">The token.</param>
        /// <param name="enableExceptionRestore">if set to <c>true</c> [enable exception restore].</param>
        /// <param name="acceptGZip">if set to <c>true</c> [accept g zip].</param>
        public RestApiClient(string baseUrl, string version, string token, bool enableExceptionRestore = false, bool acceptGZip = false)
            : this(string.Format("{0}/{1}", baseUrl.TrimEnd('/'), version), token, acceptGZip, enableExceptionRestore)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RestApiClient" /> class.
        /// </summary>
        /// <param name="baseUrl">The base URL.
        /// <example>http://xxx.xxx.com/api/v1/</example></param>
        /// <param name="token">The token.</param>
        /// <param name="enableExceptionRestore">if set to <c>true</c> [include exception detail].</param>
        /// <param name="acceptGZip">if set to <c>true</c> [accept g zip].</param>
        public RestApiClient(string baseUrl, string token, bool enableExceptionRestore = false, bool acceptGZip = false)
        {
            this.BaseUrl = baseUrl.SafeToString().TrimEnd('/') + "/";
            this.Token = token;
            this.EnableExceptionRestore = enableExceptionRestore;
            this.AcceptGZip = acceptGZip;
        }

        #endregion

        #region Common usage

        /// <summary>
        /// Invokes as void.
        /// </summary>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="resourceAction">The resource action.</param>
        /// <param name="key">The key.</param>
        /// <param name="queryString">The query string.</param>
        /// <param name="bodyJson">The body json.</param>
        public void InvokeAsVoid(string httpMethod, string resourceName, string resourceAction, string key = null, Dictionary<string, string> queryString = null, string bodyJson = null)
        {
            InvokeAsJToken(httpMethod, resourceName, resourceAction, key, queryString, bodyJson);
        }

        /// <summary>
        /// Invokes the specified HTTP method.
        /// </summary>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="resourceAction">The resource action.</param>
        /// <param name="key">The key.</param>
        /// <param name="queryString">The query string.</param>
        /// <param name="bodyJson">The body json.</param>
        /// <returns>JToken.</returns>
        public JToken Invoke(string httpMethod, string resourceName, string resourceAction, string key = null, Dictionary<string, string> queryString = null, string bodyJson = null)
        {
            return InvokeAsJToken(httpMethod, resourceName, resourceAction, key, queryString, bodyJson);
        }

        #endregion

        #region Programming Intelligence usage

        /// <summary>
        /// Invokes the using query string.
        /// </summary>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="resourceAction">The resource action.</param>
        /// <param name="parameter">The parameter.</param>
        /// <returns>JToken.</returns>
        protected JToken InvokeUsingQueryString(string httpMethod, string resourceName, string resourceAction, string parameter = null)
        {
            return InvokeAsJToken(httpMethod, resourceName, resourceAction, key: parameter);
        }

        /// <summary>
        /// Invokes the using combined query string.
        /// </summary>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="resourceAction">The resource action.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>JToken.</returns>
        protected JToken InvokeUsingCombinedQueryString(string httpMethod, string resourceName, string resourceAction, Dictionary<string, string> parameters)
        {
            return InvokeAsJToken(httpMethod, resourceName, resourceAction, queryString: parameters);
        }

        /// <summary>
        /// Invokes the using body.
        /// </summary>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="resourceAction">The resource action.</param>
        /// <param name="parameter">The parameter.</param>
        /// <returns>JToken.</returns>
        protected JToken InvokeUsingBody(string httpMethod, string resourceName, string resourceAction, object parameter)
        {
            return InvokeAsJToken(httpMethod, resourceName, resourceAction, bodyJson: parameter.ToJson());
        }

        /// <summary>
        /// Invokes as j token.
        /// </summary>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="resourceAction">The resource action.</param>
        /// <param name="key">The key.</param>
        /// <param name="queryString">The query string.</param>
        /// <param name="bodyJson">The body json.</param>
        /// <returns>JToken.</returns>
        protected JToken InvokeAsJToken(string httpMethod, string resourceName, string resourceAction, string key = null, Dictionary<string, string> queryString = null, string bodyJson = null)
        {
            try
            {
                var httpRequest = CreateHttpRequest(httpMethod, resourceName, resourceAction, key, queryString);

                if (httpMethod.IsInString(new string[] { HttpConstants.HttpMethod.Post, HttpConstants.HttpMethod.Put }, true))
                {
                    httpRequest.FillData(httpMethod, bodyJson.SafeToString());
                }

                WebHeaderCollection headers;
                CookieCollection cookie;
                HttpStatusCode httpStatusCode;
                var response = httpRequest.ReadResponseAsText(Encoding.UTF8, out httpStatusCode, out headers, out cookie);

                if (httpStatusCode == HttpStatusCode.NoContent)
                {
                    return null;
                }

                return JToken.Parse(response);
            }
            catch (HttpOperationException httpEx)
            {
                var reference = httpEx.ExceptionReference;
                var exceptionInfo = reference.ResponseText.TryDeserializeAsObject<ExceptionInfo>();

                if (this.EnableExceptionRestore)
                {
                    throw exceptionInfo.ToException().Handle("InvokeAsJToken", new { httpMethod, resourceName, resourceAction, key, queryString });
                }
                else
                {
                    throw httpEx;
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle("InvokeAsJToken", data: new { httpMethod, resourceName, resourceAction, key, queryString });
            }
        }

        #endregion

        #region Util

        /// <summary>
        /// Creates the HTTP request.
        /// </summary>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="resourceAction">The resource action.</param>
        /// <param name="key">The key.</param>
        /// <param name="queryString">The query string.</param>
        /// <returns>System.Net.HttpWebRequest.</returns>
        protected HttpWebRequest CreateHttpRequest(string httpMethod, string resourceName, string resourceAction, string key, Dictionary<string, string> queryString = null)
        {
            var url = string.Format("{0}{1}/{2}", this.BaseUrl, resourceName, resourceAction).TrimEnd('/') + "/";
            if (!string.IsNullOrWhiteSpace(key))
            {
                url += (key + "/");
            }
            if (queryString.HasItem())
            {
                url += ("?" + queryString.ToKeyValueStringWithUrlEncode());
            }

            var httpRequest = url.CreateHttpWebRequest(httpMethod, acceptGZip: this.AcceptGZip);
            FillAdditionalData(httpRequest);

            return httpRequest;
        }

        /// <summary>
        /// Fills the additional data.
        /// </summary>
        /// <param name="httpRequest">The HTTP request.</param>
        protected void FillAdditionalData(HttpWebRequest httpRequest)
        {
            if (httpRequest != null)
            {
                httpRequest.SafeSetHttpHeader(HttpConstants.HttpHeader.TOKEN, Token, ignoreIfNullOrEmpty: true);

                var currentApiContext = ContextHelper.ApiContext;

                httpRequest.SafeSetHttpHeader(HttpConstants.HttpHeader.ORIGINAL, currentApiContext.IpAddress, ignoreIfNullOrEmpty: true);
                httpRequest.SafeSetHttpHeader(HttpConstants.HttpHeader.TRACEID, ApiTraceContext.TraceId, ignoreIfNullOrEmpty: true);

                var userAgent = currentApiContext.UserAgent.SafeToString();
                if (!string.IsNullOrWhiteSpace(userAgent))
                {
                    httpRequest.UserAgent = userAgent;
                }
            }
        }

        #endregion
    }
}