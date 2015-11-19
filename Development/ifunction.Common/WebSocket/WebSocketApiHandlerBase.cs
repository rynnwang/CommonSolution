using System;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;
using System.Web.WebSockets;
using ifunction.ApiTracking.Model;
using ifunction.ExceptionSystem;
using ifunction.RestApi;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ifunction.WebSocket
{
    /// <summary>
    /// Class WebSocketApiHandlerBase.
    /// </summary>
    public abstract class WebSocketApiHandlerBase : IRouteHandler, IHttpHandler
    {
        #region Protected fields

        /// <summary>
        /// The API tracking executor
        /// </summary>
        protected IApiTracking apiTrackingExecutor = null;

        #endregion


        #region Property

        /// <summary>
        /// Gets or sets the token header key.
        /// </summary>
        /// <value>The token header key.</value>
        public string TokenHeaderKey { get; protected set; }

        /// <summary>
        /// Gets or sets the client identifier header key.
        /// </summary>
        /// <value>The client identifier header key.</value>
        public string ClientIdentifierHeaderKey { get; protected set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="RestApiRouter" /> class.
        /// </summary>
        /// <param name="apiTracking">The API tracking.</param>
        /// <param name="tokenHeaderKey">The token header key.</param>
        /// <param name="clientIdentifierHeaderKey">The client identifier header key.</param>
        protected WebSocketApiHandlerBase(IApiTracking apiTracking, string tokenHeaderKey, string clientIdentifierHeaderKey = null)
        {
            this.apiTrackingExecutor = apiTracking;
            this.TokenHeaderKey = tokenHeaderKey;
            this.ClientIdentifierHeaderKey = clientIdentifierHeaderKey;
        }

        #region IHttpHandler

        /// <summary>
        /// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler" /> instance.
        /// </summary>
        /// <value><c>true</c> if this instance is reusable; otherwise, <c>false</c>.</value>
        /// <returns>true if the <see cref="T:System.Web.IHttpHandler" /> instance is reusable; otherwise, false.</returns>
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler" /> interface.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpContext" /> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
        public void ProcessRequest(HttpContext context)
        {
            if (context.IsWebSocketRequest)
            {
                context.AcceptWebSocketRequest(ProcessWebSocket);
            }
        }

        /// <summary>
        /// Processes the web socket.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        protected async Task ProcessWebSocket(AspNetWebSocketContext context)
        {
            System.Net.WebSockets.WebSocket socket = context.WebSocket;

            while (true)
            {
                ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[1024]);
                WebSocketReceiveResult result = await socket.ReceiveAsync(buffer, CancellationToken.None);
                if (socket.State == WebSocketState.Open)
                {
                    string userMessage = Encoding.UTF8.GetString(
                        buffer.Array, 0, result.Count);
                    userMessage = "You sent: " + userMessage + " at " +
                        DateTime.Now.ToLongTimeString();
                    buffer = new ArraySegment<byte>(
                        Encoding.UTF8.GetBytes(userMessage));
                    await socket.SendAsync(
                        buffer, WebSocketMessageType.Text, true, CancellationToken.None);
                }
                else
                {
                    break;
                }
            }
        }

        #endregion

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

        #region Protected Methods

        /// <summary>
        /// Prepares the specified request.
        /// <remarks>
        /// This method would be called before <c>ProcessRoute</c>. It can be used to help you to do some preparation, such as get something from headers or cookie for later actions.
        /// ou can save them in Thread data so that you can get them later in <c>ProcessRoute</c>, <c>Invoke</c>, <c>PackageOutput</c> ,etc.
        /// If any exception is throw from this method, the process flow would be interrupted.
        /// </remarks>
        /// </summary>
        /// <param name="request">The request.</param>
        protected virtual void Prepare(HttpRequest request)
        {
            //Do nothing here.
        }

        /// <summary>
        /// Initializes the context.
        /// <remarks>
        /// This method would be called after <c>ProcessRoute</c> and before <c>Invoke</c>. It can be used to help you to do some context initialization, such as get something from database for later actions.
        /// ou can save them in Thread data so that you can get them later in <c>Invoke</c>, <c>PackageOutput</c> ,etc.
        /// If any exception is throw from this method, the process flow would be interrupted.
        /// </remarks>
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="methodToInvoke">The method to invoke.</param>
        /// <param name="instanceToInvoke">The instance to invoke.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="version">The version.</param>
        /// <param name="action">The action.</param>
        /// <param name="entityKey">The entity key.</param>
        /// <param name="userIdentifier">The user identifier.</param>
        protected virtual void InitializeContext(HttpRequest request, MethodInfo methodToInvoke, object instanceToInvoke,
            string resourceName, string version, string action, string entityKey, string userIdentifier)
        {
            //Do nothing here.
        }

        /// <summary>
        /// Gets the token.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>System.String.</returns>
        protected string GetToken(HttpRequest request)
        {
            return (request != null && !string.IsNullOrWhiteSpace(TokenHeaderKey))
                ? request.TryGetHeader(TokenHeaderKey)
                : string.Empty;
        }

        /// <summary>
        /// Handles the exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="serviceIdentifier">The service identifier.</param>
        /// <param name="serverIdentifier">The server identifier.</param>
        /// <returns>Exception log key.</returns>
        protected virtual BaseException HandleException(Exception exception, string serviceIdentifier, string serverIdentifier)
        {
            var baseException = exception.Handle(null);
            if (apiTrackingExecutor != null)
            {
                apiTrackingExecutor.LogExceptionAsync(baseException, serviceIdentifier, serverIdentifier);
            }

            return baseException;
        }

        #endregion

        /// <summary>
        /// Gets the route.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>System.Web.Routing.Route.</returns>
        public static Route GetRoute<T>() where T : WebSocketApiHandlerBase, new()
        {
            var routeValueDictionary = new RouteValueDictionary { { "webSocket", RestApiExtension.apiUrlRegex } };

            return new Route("{*webSocket}", defaults: null, routeHandler: new T(), constraints: routeValueDictionary);
        }

        /// <summary>
        /// Gets the route.
        /// </summary>
        /// <param name="routeHandler">The route handler.</param>
        /// <returns>Route.</returns>
        public static Route GetRoute(WebSocketApiHandlerBase routeHandler)
        {
            var routeValueDictionary = new RouteValueDictionary { { "webSocket", RestApiExtension.apiUrlRegex } };

            return new Route("{*webSocket}", defaults: null, routeHandler: routeHandler, constraints: routeValueDictionary);
        }
    }
}
