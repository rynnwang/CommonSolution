using System;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Routing;

namespace ifunction.RestApi
{
    /// <summary>
    /// Class RestApiTransparentProxy.
    /// </summary>
    public abstract class RestApiTransparentProxy : IRouteHandler, IHttpHandler
    {
        /// <summary>
        /// Gets or sets the destination base URL.
        /// </summary>
        /// <value>The destination base URL.</value>
        public string DestinationBaseUrl { get; protected set; }

        /// <summary>
        /// Gets or sets the match regex.
        /// </summary>
        /// <value>The match regex.</value>
        public string MatchRegex { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RestApiTransparentProxy" /> class.
        /// </summary>
        /// <param name="matchRegex">The match regex.</param>
        /// <param name="destinationBaseUrl">The destination base URL.</param>
        protected RestApiTransparentProxy(string matchRegex, string destinationBaseUrl)
        {
            this.MatchRegex = matchRegex.SafeToString("/[Aa][Pp][Ii]/.+");
            this.DestinationBaseUrl = destinationBaseUrl;
        }

        #region IRouteHandler

        /// <summary>
        /// Provides the object that processes the request.
        /// </summary>
        /// <param name="requestContext">An object that encapsulates information about the request.</param>
        /// <returns>An object that processes the request.</returns>
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return this;
        }

        #endregion

        /// <summary>
        /// Gets the route.
        /// </summary>
        /// <param name="routeHandler">The route handler.</param>
        /// <returns>Route.</returns>
        public Route GetRoute(ApiHandlerBase routeHandler)
        {
            var routeValueDictionary = new RouteValueDictionary { { "apiUrl", RestApiExtension.apiUrlRegex } };

            return new Route("{*apiUrl}", defaults: null, routeHandler: routeHandler, constraints: routeValueDictionary);
        }

        #region IHttpHandler

        /// <summary>
        /// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler" /> instance.
        /// </summary>
        /// <value><c>true</c> if this instance is reusable; otherwise, <c>false</c>.</value>
        /// <returns>true if the <see cref="T:System.Web.IHttpHandler" /> instance is reusable; otherwise, false.</returns>
        public bool IsReusable
        {
            get { return false; }
        }

        /// <summary>
        /// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler" /> interface.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpContext" /> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";

            try
            {
                var newRequest = context.Request.CopyHttpRequestToHttpWebRequest(DestinationBaseUrl);
                HttpStatusCode statusCode;
                WebHeaderCollection headers;
                var response = newRequest.ReadResponseAsText(Encoding.UTF8, out statusCode, out headers);
                context.Response.Headers.Clear();
                context.Response.Headers.AddRange(headers);
                context.Response.StatusCode = (int)statusCode;
                context.Response.WriteContent(response);
            }
            catch (Exception ex)
            {
                var exception = ex.Handle("ProcessRequest");

                context.Response.StatusCode = (int)exception.Code.ToHttpStatusCode();
                context.Response.WriteContent(exception.ToJson());
            }
        }

        #endregion
    }
}