using System;
using System.Net;
using ifunction.ApiTracking.Model;
using ifunction.ExceptionSystem;

namespace ifunction.ApiTracking
{
    /// <summary>
    /// Class ApiTrackingRestClient.
    /// </summary>
    public class ApiTrackingRestClient : IApiTracking
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
        public string Token { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiTrackingRestClient" /> class.
        /// </summary>
        /// <param name="baseUrl">The base URL.<remarks>
        /// <example>http://host.com/</example></remarks>
        /// </param>
        /// <param name="token">The token.</param>
        public ApiTrackingRestClient(string baseUrl, string token)
        {
            this.BaseUrl = baseUrl;
            this.Token = token;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiTrackingRestClient"/> class.
        /// </summary>
        /// <param name="accountUrl">The account URL. e.g.: http://token@host.com/ </param>
        public ApiTrackingRestClient(string accountUrl)
        {
            string userInfo;
            this.BaseUrl = (new Uri(accountUrl)).GetPureUri(out userInfo);
            this.Token = userInfo;
        }

        /// <summary>
        /// Logs the API event asynchronous.
        /// </summary>
        /// <param name="eventLog">The event log.</param>
        public void LogApiEventAsync(ApiEventLog eventLog)
        {
            try
            {
                eventLog.CheckNullObject("eventLog");

                var httpRequest = CreateHttpWebRequest("log");
                ProcessHttp(httpRequest, HttpConstants.HttpMethod.Post, eventLog);
            }
            catch (Exception ex)
            {
                throw ex.Handle("LogApiEventAsync", eventLog);
            }
        }

        /// <summary>
        /// Logs the exception asynchronous.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="serviceIdentifier">The service identifier.</param>
        /// <param name="serverIdentifier">The server identifier.</param>
        public void LogExceptionAsync(BaseException exception, string serviceIdentifier = null, string serverIdentifier = null)
        {
            try
            {
                exception.CheckNullObject("exception");
                var exceptionInfo = exception.ToExceptionInfo(serviceIdentifier, serverIdentifier);

                var httpRequest = CreateHttpWebRequest("exception");
                ProcessHttp(httpRequest, HttpConstants.HttpMethod.Post, exceptionInfo);
            }
            catch (Exception ex)
            {
                throw ex.Handle("LogExceptionAsync");
            }
        }

        /// <summary>
        /// Logs the API trace log asynchronous.
        /// </summary>
        /// <param name="traceLog">The trace log.</param>
        public void LogApiTraceLogAsync(ApiTraceLog traceLog)
        {
            try
            {
                traceLog.CheckNullObject("traceLog");

                var httpRequest = CreateHttpWebRequest("log");
                ProcessHttp(httpRequest, HttpConstants.HttpMethod.Post, traceLog);
            }
            catch (Exception ex)
            {
                throw ex.Handle("LogApiTraceLogAsync", traceLog);
            }
        }

        /// <summary>
        /// Creates the HTTP web request.
        /// </summary>
        /// <param name="resourceName">Name of the resource.</param>
        /// <returns>HttpWebRequest.</returns>
        protected HttpWebRequest CreateHttpWebRequest(string resourceName)
        {
            return
                string.Format("{0}tracking/{1}/", BaseUrl, resourceName).CreateHttpWebRequest();
        }

        /// <summary>
        /// Processes the HTTP.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="obj">The object.</param>
        protected void ProcessHttp(HttpWebRequest request, string httpMethod, object obj)
        {
            if (request != null)
            {
                request.Headers.Add(HttpConstants.HttpHeader.TOKEN, Token);
                request.FillData(httpMethod, obj.ToJson(false));
                request.ReadAsyncResponseAsText(null);
            }
        }
    }
}
