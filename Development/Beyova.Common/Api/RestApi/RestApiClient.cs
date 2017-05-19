using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using Beyova.Api;
using Beyova.ExceptionSystem;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Beyova.RestApi
{
    /// <summary>
    /// Class RestApiClient.
    /// </summary>
    public abstract class RestApiClient
    {
        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        /// <value>The token.</value>
        public string Token
        {
            get
            {
                return this.Endpoint?.Token;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [accept GZIP].
        /// </summary>
        /// <value><c>true</c> if [accept g zip]; otherwise, <c>false</c>.</value>
        public bool AcceptGZip { get; protected set; }

        /// <summary>
        /// Gets or sets the host.
        /// </summary>
        /// <value>The host.</value>
        public ApiEndpoint Endpoint { get; protected set; }

        /// <summary>
        /// Gets or sets the timeout.
        /// </summary>
        /// <value>
        /// The timeout.
        /// </value>
        public int? Timeout { get; protected set; }

        /// <summary>
        /// The base client version
        /// </summary>
        internal const int BaseClientVersion = 4;

        /// <summary>
        /// The client generated version
        /// </summary>
        protected abstract int ClientGeneratedVersion { get; }

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="RestApiClient" /> class.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="acceptGZip">if set to <c>true</c> [accept g zip].</param>
        /// <param name="timeout">The timeout.</param>
        /// <exception cref="System.NotSupportedException"></exception>
        public RestApiClient(ApiEndpoint endpoint, bool acceptGZip = false, int? timeout = null)
        {
            //Try detecting base client version and generated version.
            if (ClientGeneratedVersion != BaseClientVersion)
            {
                throw new NotSupportedException(string.Format("ClientGeneratedVersion [{0}] doesnot match BaseClientVersion [{1}].", ClientGeneratedVersion, BaseClientVersion));
            }

            this.Endpoint = endpoint ?? new ApiEndpoint();
            this.AcceptGZip = acceptGZip;
            this.Timeout = timeout;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RestApiClient" /> class.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="token">The token.</param>
        /// <param name="acceptGZip">The accept g zip.</param>
        [Obsolete("Not supported anymore.", true)]
        protected RestApiClient(ApiEndpoint endpoint, string token, bool acceptGZip = false)
        {
        }

        #endregion

        #region Common usage

        /// <summary>
        /// Invokes as void.
        /// </summary>
        /// <param name="realm">The realm.</param>
        /// <param name="version">The version.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="resourceAction">The resource action.</param>
        /// <param name="key">The key.</param>
        /// <param name="queryString">The query string.</param>
        /// <param name="bodyJson">The body json.</param>
        /// <param name="methodNameForTrace">The method name for trace.</param>
        public void InvokeAsVoid(string realm, string version, string httpMethod, string resourceName, string resourceAction, string key = null, Dictionary<string, string> queryString = null, string bodyJson = null, [CallerMemberName] string methodNameForTrace = null)
        {
            InvokeAsJToken(realm, version, httpMethod, resourceName, resourceAction, key, queryString, bodyJson, this.Timeout, methodNameForTrace);
        }

        /// <summary>
        /// Invokes the specified HTTP method.
        /// </summary>
        /// <param name="realm">The realm.</param>
        /// <param name="version">The version.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="resourceAction">The resource action.</param>
        /// <param name="key">The key.</param>
        /// <param name="queryString">The query string.</param>
        /// <param name="bodyJson">The body json.</param>
        /// <param name="methodNameForTrace">The method name for trace.</param>
        /// <returns>JToken.</returns>
        public JToken Invoke(string realm, string version, string httpMethod, string resourceName, string resourceAction, string key = null, Dictionary<string, string> queryString = null, string bodyJson = null, [CallerMemberName] string methodNameForTrace = null)
        {
            return InvokeAsJToken(realm, version, httpMethod, resourceName, resourceAction, key, queryString, bodyJson, this.Timeout, methodNameForTrace);
        }

        #endregion

        #region Programming Intelligence usage

        /// <summary>
        /// Invokes the using query string.
        /// </summary>
        /// <param name="realm">The realm.</param>
        /// <param name="version">The version.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="resourceAction">The resource action.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="methodNameForTrace">The method name for trace.</param>
        /// <returns>JToken.</returns>
        protected JToken InvokeUsingQueryString(string realm, string version, string httpMethod, string resourceName, string resourceAction, string parameter = null, [CallerMemberName] string methodNameForTrace = null)
        {
            return InvokeAsJToken(realm, version, httpMethod, resourceName, resourceAction, key: parameter, timeout: this.Timeout, methodNameForTrace: methodNameForTrace);
        }

        /// <summary>
        /// Invokes the using combined query string.
        /// </summary>
        /// <param name="realm">The realm.</param>
        /// <param name="version">The version.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="resourceAction">The resource action.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="methodNameForTrace">The method name for trace.</param>
        /// <returns>JToken.</returns>
        protected JToken InvokeUsingCombinedQueryString(string realm, string version, string httpMethod, string resourceName, string resourceAction, Dictionary<string, string> parameters, [CallerMemberName] string methodNameForTrace = null)
        {
            return InvokeAsJToken(realm, version, httpMethod, resourceName, resourceAction, queryString: parameters, timeout: this.Timeout, methodNameForTrace: methodNameForTrace);
        }

        /// <summary>
        /// Invokes the using body.
        /// </summary>
        /// <param name="realm">The realm.</param>
        /// <param name="version">The version.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="resourceAction">The resource action.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="methodNameForTrace">The method name for trace.</param>
        /// <returns>JToken.</returns>
        protected JToken InvokeUsingBody(string realm, string version, string httpMethod, string resourceName, string resourceAction, object parameter, [CallerMemberName] string methodNameForTrace = null)
        {
            return InvokeAsJToken(realm, version, httpMethod, resourceName, resourceAction, bodyJson: parameter.ToJson(), timeout: this.Timeout, methodNameForTrace: methodNameForTrace);
        }

        /// <summary>
        /// Invokes as j token.
        /// </summary>
        /// <param name="realm">The realm.</param>
        /// <param name="version">The version.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="resourceAction">The resource action.</param>
        /// <param name="key">The key.</param>
        /// <param name="queryString">The query string.</param>
        /// <param name="bodyJson">The body json.</param>
        /// <param name="timeout">The timeout.</param>
        /// <param name="methodNameForTrace">The method name for trace.</param>
        /// <returns>
        /// JToken.
        /// </returns>
        protected JToken InvokeAsJToken(string realm, string version, string httpMethod, string resourceName, string resourceAction, string key = null, Dictionary<string, string> queryString = null, string bodyJson = null, int? timeout = null, [CallerMemberName] string methodNameForTrace = null)
        {
            BaseException exception = null;

            try
            {
                ApiTraceContext.Enter("RestApiClient", methodNameForTrace);

                var httpRequest = CreateHttpRequest(realm, version, httpMethod, resourceName, resourceAction, key, queryString);
                if (timeout.HasValue)
                {
                    httpRequest.Timeout = timeout.Value;
                }

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

                return response != null ? JToken.Parse(response) : null;
            }
            catch (HttpOperationException httpEx)
            {
                ExceptionInfo externalExceptionInfo = JsonExtension.TryConvertJsonToObject<ExceptionInfo>(httpEx.ExceptionReference.ResponseText);

                exception = httpEx.Handle(new { httpMethod, resourceName, resourceAction, key, queryString, externalExceptionInfo });
                //Reset key to new one so that in log system, this exception can be identified correctly.
                exception.Key = Guid.NewGuid();
                throw exception;
            }
            catch (Exception ex)
            {
                exception = ex.Handle(new { httpMethod, resourceName, resourceAction, key, queryString });
                throw exception;
            }
            finally
            {
                ApiTraceContext.Exit(exception?.Key);
            }
        }

        #endregion

        #region Util

        /// <summary>
        /// Gets the request endpoint.
        /// </summary>
        /// <param name="apiEndpoint">The API endpoint.</param>
        /// <param name="realm">The realm.</param>
        /// <param name="version">The version.</param>
        /// <returns>System.String.</returns>
        protected internal virtual string GetRequestEndpoint(ApiEndpoint apiEndpoint, string realm, string version)
        {
            return string.Format("{0}{1}/", this.Endpoint == null ? "http://localhost/api/" : ((UriEndpoint)Endpoint).ToString(), version);
        }

        /// <summary>
        /// Creates the HTTP request.
        /// </summary>
        /// <param name="realm">The realm.</param>
        /// <param name="version">The version.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="resourceAction">The resource action.</param>
        /// <param name="key">The key.</param>
        /// <param name="queryString">The query string.</param>
        /// <returns>System.Net.HttpWebRequest.</returns>
        protected HttpWebRequest CreateHttpRequest(string realm, string version, string httpMethod, string resourceName, string resourceAction, string key, Dictionary<string, string> queryString = null)
        {
            var url = string.Format("{0}{1}/{2}", GetRequestEndpoint(this.Endpoint, realm, version), resourceName, resourceAction).TrimEnd('/') + "/";
            if (!string.IsNullOrWhiteSpace(key))
            {
                url += (key.ToUrlEncodedText() + "/");
            }
            if (queryString.HasItem())
            {
                url += ("?" + queryString.ToKeyValueStringWithUrlEncode());
            }

            var httpRequest = url.CreateHttpWebRequest(httpMethod, acceptGZip: this.AcceptGZip, omitServerCertificateValidation: true);
            FillAdditionalData(httpRequest);

            httpRequest.Referer = ContextHelper.ApiContext?.CurrentUri?.ToString();
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
                httpRequest.SafeSetHttpHeader(HttpConstants.HttpHeader.TRACESEQUENCE, ApiTraceContext.TraceSequence, ignoreIfNullOrEmpty: true);

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