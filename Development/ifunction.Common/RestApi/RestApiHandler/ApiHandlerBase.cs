using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Routing;
using ifunction.ApiTracking.Model;
using ifunction.ExceptionSystem;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ifunction.RestApi
{
    /// <summary>
    /// Class ApiHandlerBase.
    /// </summary>
    public abstract class ApiHandlerBase : IRouteHandler, IHttpHandler
    {
        #region Protected static fields

        /// <summary>
        /// The build in feature version keyword
        /// </summary>
        protected const string BuildInFeatureVersionKeyword = "buildin";

        /// <summary>
        /// The json converters
        /// </summary>
        protected static readonly HashSet<JsonConverter> jsonConverters = new HashSet<JsonConverter>();

        /// <summary>
        /// The json converters
        /// </summary>
        protected static JsonConverter[] JsonConverters = null;

        /// <summary>
        /// The settings container
        /// </summary>
        internal static Dictionary<string, RestApiSettings> settingsContainer = new Dictionary<string, RestApiSettings>();

        #endregion

        #region Property

        /// <summary>
        /// Gets or sets the default settings.
        /// </summary>
        /// <value>The default rest settings.</value>
        public RestApiSettings DefaultSettings { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether [allow options].
        /// </summary>
        /// <value><c>true</c> if [allow options]; otherwise, <c>false</c>.</value>
        public bool AllowOptions { get; protected set; }

        #endregion

        /// <summary>
        /// Specifies the global json serialization converters.
        /// </summary>
        /// <param name="converters">The converters.</param>
        public static void SpecifyGlobalJsonSerializationConverters(params JsonConverter[] converters)
        {
            if (converters != null && converters.Any())
            {
                jsonConverters.AddRange(converters);
                JsonConverters = jsonConverters.ToArray();
            }
        }

        /// <summary>
        /// Initializes static members of the <see cref="ApiHandlerBase"/> class.
        /// </summary>
        static ApiHandlerBase()
        {
            jsonConverters.Add(JsonExtension.IsoDateTimeConverter);
            JsonConverters = jsonConverters.ToArray();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiHandlerBase" /> class.
        /// </summary>
        /// <param name="defaultApiSettings">The default API settings.</param>
        /// <param name="allowOptions">if set to <c>true</c> [allow options].</param>
        protected ApiHandlerBase(RestApiSettings defaultApiSettings, bool allowOptions = false)
        {
            // Ensure it is never null. Default values should be safe.
            DefaultSettings = defaultApiSettings ?? new RestApiSettings
            {
                TokenHeaderKey = HttpConstants.HttpHeader.TOKEN,
                ClientIdentifierHeaderKey = HttpConstants.HttpHeader.CLIENTIDENTIFIER,
                EnableContentCompression = true,
                TrackingEvent = false,
                EnableOutputFullExceptionInfo = false
            };

            DefaultSettings.Name = DefaultSettings.Name.SafeToString("Default");
            DefaultSettings.ApiTracking = DefaultSettings.ApiTracking ?? DiagnosticFileLogger.CreateOrUpdateDiagnosticFileLogger(DefaultSettings.Name);

            settingsContainer.Merge(DefaultSettings.Name, DefaultSettings, false);

            this.AllowOptions = allowOptions;
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
            ApiEventLog eventLog = null;
            var entryStamp = DateTime.UtcNow;
            RestApiSettings settings = DefaultSettings;
            RuntimeContext runtimeContext = null;

            context.Response.Headers.Add(HttpConstants.HttpHeader.SERVERHANDLETIME, entryStamp.ToFullDateTimeTzString());
            var acceptEncoding = context.Request.Headers["Accept-Encoding"].SafeToLower();

            try
            {
                Prepare(context.Request);

                if (context.Request.HttpMethod.Equals(HttpConstants.HttpMethod.Options, StringComparison.OrdinalIgnoreCase))
                {
                    if (this.AllowOptions)
                    {
                        //Return directly. IIS would append following headers by default, according to what exactly web.config have.
                        //Access-Control-Allow-Origin: *
                        //Access-Control-Allow-Headers: Content-Type
                        //Access-Control-Allow-Methods: GET, POST, PUT, DELETE, OPTIONS
                        return;
                    }
                }

                runtimeContext = ProcessRoute(context.Request);
                runtimeContext.CheckNullObject("runtimeContext");

                settings = runtimeContext.Settings ?? DefaultSettings;

                if (runtimeContext.Version.Equals(BuildInFeatureVersionKeyword, StringComparison.OrdinalIgnoreCase))
                {
                    var buildInResult = ProcessBuildInFeature(runtimeContext, context.Request.IsLocal);
                    PackageOutput(context.Response, buildInResult, null, acceptEncoding, runtimeContext.IsVoid ?? false, settings.EnableOutputFullExceptionInfo);
                }
                else
                {
                    if (settings != null && settings.ApiTracking != null && settings.TrackingEvent)
                    {
                        eventLog = new ApiEventLog
                        {
                            RawUrl = context.Request.RawUrl,
                            EntryStamp = entryStamp,
                            UserAgent = context.Request.UserAgent,
                            // If request came from ApiTransport or other proxy ways, ORIGINAL stands for the IP ADDRESS from original requester.
                            IpAddress = context.Request.TryGetHeader(HttpConstants.HttpHeader.ORIGINAL).SafeToString(context.Request.UserHostAddress),
                            CultureCode = context.Request.UserLanguages == null ? null : context.Request.UserLanguages.FirstOrDefault()
                        };
                    }

                    InitializeContext(context.Request, runtimeContext);

                    if (eventLog != null)
                    {
                        if (runtimeContext.ApiInstance != null)
                        {
                            eventLog.ServiceIdentifier = runtimeContext.ApiInstance.GetType().Name;
                        }

                        if (runtimeContext.ApiMethod != null)
                        {
                            eventLog.ApiFullName = runtimeContext.ApiMethod.Name;
                        }

                        eventLog.ResourceName = runtimeContext.ResourceName;
                        eventLog.ServerIdentifier = EnvironmentCore.ServerName;
                        eventLog.UserIdentifier = runtimeContext.UserIdentifier;

                        var clientIdentifierHeaderKey = settings.ClientIdentifierHeaderKey;
                        if (!string.IsNullOrWhiteSpace(clientIdentifierHeaderKey))
                        {
                            eventLog.ClientIdentifier = context.Request.TryGetHeader(clientIdentifierHeaderKey);
                        }

                        eventLog.ModuleName = runtimeContext.ModuleName;
                        eventLog.ResourceEntityKey = runtimeContext.IsActionUsed ? runtimeContext.Parameter2 : runtimeContext.Parameter1;
                    }

                    if (runtimeContext.Exception != null)
                    {
                        throw runtimeContext.Exception;
                    }

                    try
                    {
                        if (runtimeContext.ApiTransportAttribute != null)
                        {
                            if (runtimeContext.ApiTransportAttribute.ApiTransportAdapter != null)
                            {
                                runtimeContext.ApiTransportAttribute.ApiTransportAdapter.PrepareInteraction(runtimeContext.ApiMethod, runtimeContext.ApiInstance, runtimeContext.EntityKey);
                            }

                            InvokeByTransport(context.Request, context.Response, runtimeContext.ApiTransportAttribute);
                        }
                        else
                        {
                            var invokeResult = Invoke(runtimeContext.ApiInstance, runtimeContext.ApiMethod, context.Request, runtimeContext.EntityKey);

                            PackageOutput(context.Response, invokeResult, null, acceptEncoding, runtimeContext.IsVoid ?? false, settings.EnableOutputFullExceptionInfo);
                        }
                    }
                    catch (Exception invokeEx)
                    {
                        throw invokeEx.Handle("Invoke", new { Url = context.Request.RawUrl, Method = context.Request.HttpMethod });
                    }
                }
            }
            catch (Exception ex)
            {
                var apiTracking = settings?.ApiTracking;
                var baseException = HandleException(apiTracking ?? Framework.ApiTracking, ex, runtimeContext?.ApiInstance?.GetType().FullName, EnvironmentCore.ServerName);

                if (apiTracking != null && eventLog != null)
                {
                    eventLog.ExceptionKey = baseException.Key;
                }

                PackageOutput(context.Response, null, baseException, acceptEncoding, exceptionRestoreEnabled: settings.EnableOutputFullExceptionInfo);
            }
            finally
            {
                if (eventLog != null && settings?.ApiTracking != null)
                {
                    try
                    {
                        eventLog.ExitStamp = DateTime.UtcNow;
                        settings.ApiTracking.LogApiEventAsync(eventLog);
                    }
                    catch { }
                }

                ThreadExtension.Clear();
            }
        }

        /// <summary>
        /// Processes the build in feature.
        /// </summary>
        /// <param name="runtimeContext">The runtime context.</param>
        /// <param name="isLocalhost">The is localhost.</param>
        /// <returns>System.Object.</returns>
        protected virtual object ProcessBuildInFeature(RuntimeContext runtimeContext, bool isLocalhost)
        {
            object result = null;

            switch (runtimeContext?.ResourceName.SafeToLower())
            {
                case "server":
                    result = Framework.AboutService();
                    break;
                default: break;
            }

            return result;
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

        #region Protected virtual methods

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
        /// <param name="runtimeContext">The runtime context.</param>
        protected virtual void InitializeContext(HttpRequest request, RuntimeContext runtimeContext)
        {
            //Do nothing here.
        }

        /// <summary>
        /// Invokes the specified method information.
        /// <remarks>
        /// Invoke action would regard to method parameter to use difference logic. Following steps show the IF-ELSE case. When it is hit, other would not go through.
        /// <list type="number"><item>
        /// If input parameter count is 0, invoke without parameter object.
        /// </item><item>
        /// If input parameter count is 1 and key is not empty or null, invoke using key.
        /// </item><item>
        /// If input parameter count is 1 and key is empty or null, invoke using key, try to get JSON object from request body and convert to object for invoke.
        /// </item><item>
        /// If input parameter count more than 1, try read JSON data to match parameters by name (ignore case) in root level, then invoke.
        /// </item></list></remarks>
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="methodInfo">The method information.</param>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <param name="key">The key.</param>
        /// <returns>System.Object.</returns>
        protected virtual object Invoke(object instance, MethodInfo methodInfo, HttpRequest httpRequest, string key)
        {
            var inputParameters = methodInfo.GetParameters();

            if (!string.IsNullOrWhiteSpace(key) && key.Contains('%'))
            {
                key = key.ToUrlDecodedText();
            }

            if (inputParameters.Length == 0)
            {
                return methodInfo.Invoke(instance, null);
            }
            else if (inputParameters.Length == 1)
            {
                if (!string.IsNullOrWhiteSpace(key) && (inputParameters[0].ParameterType == typeof(string) || inputParameters[0].ParameterType.IsValueType))
                {
                    return methodInfo.Invoke(instance, new object[] { ReflectionExtension.ConvertToObjectByType(inputParameters[0].ParameterType, key) });
                }
                else
                {
                    var json = httpRequest.GetPostJson(Encoding.UTF8);
                    return methodInfo.Invoke(instance, new object[] { DeserializeJsonObject(json, inputParameters[0].ParameterType) });
                }
            }
            else
            {
                object[] parameters = new object[inputParameters.Length];

                if (httpRequest.QueryString.Count > 0)
                {
                    var start = 1;
                    if (!string.IsNullOrWhiteSpace(key) && (inputParameters[0].ParameterType.TryGetNullableType().IsValueType))
                    {
                        parameters[0] = ReflectionExtension.ConvertToObjectByType(inputParameters[0].ParameterType, key);
                    }
                    else
                    {
                        var json = httpRequest.GetPostJson(Encoding.UTF8);
                        parameters[0] = json.TryConvertJsonToObject();
                        if (parameters[0] == null)
                        {
                            start = 0;
                        }
                    }

                    for (var i = start; i < inputParameters.Length; i++)
                    {
                        parameters[i] = ReflectionExtension.ConvertToObjectByType(inputParameters[i].ParameterType,
                            httpRequest.QueryString.Get(inputParameters[i].Name));
                    }
                }
                else
                {
                    var json = httpRequest.GetPostJson(Encoding.UTF8);
                    var jsonObject = json.IsNullOrWhiteSpace() ? null : JObject.Parse(json);

                    if (jsonObject != null)
                    {
                        for (int i = 0; i < inputParameters.Length; i++)
                        {
                            var jTokenObject = jsonObject.GetProperty(inputParameters[i].Name);
                            parameters[i] = jTokenObject == null ? null : jTokenObject.ToObject(inputParameters[i].ParameterType);
                        }
                    }
                }

                return methodInfo.Invoke(instance, parameters);
            }
        }

        /// <summary>
        /// Invokes the by transport.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="response">The response.</param>
        /// <param name="apiTransportAttribute">The API transport attribute.</param>
        protected virtual void InvokeByTransport(HttpRequest request, HttpResponse response,
            ApiTransportAttribute apiTransportAttribute)
        {
            if (request != null && response != null && apiTransportAttribute != null && !string.IsNullOrWhiteSpace(apiTransportAttribute.DestinationHost))
            {
                if (apiTransportAttribute.ApiTransportAdapter != null)
                {
                    var httpRequest = request.CopyHttpRequestToHttpWebRequest(apiTransportAttribute.DestinationHost, apiTransportAttribute.ApiTransportAdapter.RewriteHeader);
                    httpRequest.GetResponse().TransportHttpResponse(response);
                }
            }
        }

        /// <summary>
        /// Handles the exception.
        /// </summary>
        /// <param name="apiTrackingExecutor">The API tracking executor.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="serviceIdentifier">The service identifier.</param>
        /// <param name="serverIdentifier">The server identifier.</param>
        /// <returns>Exception log key.</returns>
        protected virtual BaseException HandleException(IApiTracking apiTrackingExecutor, Exception exception, string serviceIdentifier, string serverIdentifier)
        {
            var baseException = exception.Handle(null);
            if (apiTrackingExecutor != null)
            {
                apiTrackingExecutor.LogExceptionAsync(baseException, serviceIdentifier, serverIdentifier);
            }

            return baseException;
        }

        /// <summary>
        /// Processes the route.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Exception.</returns>
        protected abstract RuntimeContext ProcessRoute(HttpRequest request);

        #endregion

        #region PackageResponse

        /// <summary>
        /// Packages the output.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="data">The data.</param>
        /// <param name="baseException">The base exception.</param>
        /// <param name="acceptEncoding">Name of the compress.</param>
        /// <param name="noBody">if set to <c>true</c> [no body].</param>
        /// <param name="exceptionRestoreEnabled">The exception restore enabled.</param>
        /// <returns>System.Object.</returns>
        protected virtual void PackageOutput(HttpResponse response, object data, BaseException baseException = null, string acceptEncoding = null, bool noBody = false, bool exceptionRestoreEnabled = false)
        {
            PackageResponse(response, data, baseException, acceptEncoding, noBody, exceptionRestoreEnabled);
        }

        /// <summary>
        /// Packages the response.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="data">The data.</param>
        /// <param name="ex">The ex.</param>
        /// <param name="acceptEncoding">Name of the compression.</param>
        /// <param name="noBody">if set to <c>true</c> [no body].</param>
        /// <param name="exceptionRestoreEnabled">The exception restore enabled.</param>
        /// <returns>System.Object.</returns>
        public static void PackageResponse(HttpResponse response, object data, BaseException ex = null, string acceptEncoding = null, bool noBody = false, bool exceptionRestoreEnabled = false)
        {
            if (response != null)
            {
                PackageResponse(new HttpResponseWrapper(response), data, ex, acceptEncoding, noBody, exceptionRestoreEnabled);
            }
        }

        /// <summary>
        /// Packages the response.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="data">The data.</param>
        /// <param name="ex">The ex.</param>
        /// <param name="acceptEncoding">The accept encoding.</param>
        /// <param name="noBody">if set to <c>true</c> [no body].</param>
        /// <param name="exceptionRestoreEnabled">The exception restore enabled.</param>
        public static void PackageResponse(HttpResponseBase response, object data, BaseException ex = null, string acceptEncoding = null, bool noBody = false, bool exceptionRestoreEnabled = false)
        {
            if (response != null)
            {
                var objectToReturn = ex != null ? (exceptionRestoreEnabled ? ex.ToExceptionInfo() : new SimpleExceptionInfo
                {
                    Message = ex.Hint != null ? (ex.Hint.Message ?? ex.Hint.Cause) : ex.RootCause.Message,
                    Data = data == null ? null : JObject.FromObject(data),
                    Code = ex.Hint?.Code ?? ex.Code
                } as IExceptionInfo) : data;

                response.Headers.Add(HttpConstants.HttpHeader.SERVERTIME, DateTime.UtcNow.ToFullDateTimeString());
                response.StatusCode = (int)(ex == null ? (noBody ? HttpStatusCode.NoContent : HttpStatusCode.OK) : ex.Code.ToHttpStatusCode());

                if (!noBody)
                {
                    response.ContentType = "application/json";
                    acceptEncoding = acceptEncoding.SafeToString().ToLower();
                    if (acceptEncoding.Contains("gzip"))
                    {
                        response.Filter = new System.IO.Compression.GZipStream(response.Filter,
                                              System.IO.Compression.CompressionMode.Compress);
                        response.Headers.Remove(HttpConstants.HttpHeader.ContentEncoding);
                        response.AppendHeader(HttpConstants.HttpHeader.ContentEncoding, "gzip");
                    }
                    else if (acceptEncoding.Contains("deflate"))
                    {
                        response.Filter = new System.IO.Compression.DeflateStream(response.Filter,
                                            System.IO.Compression.CompressionMode.Compress);
                        response.Headers.Remove(HttpConstants.HttpHeader.ContentEncoding);
                        response.AppendHeader(HttpConstants.HttpHeader.ContentEncoding, "deflate");
                    }

                    response.Write(objectToReturn.ToJson(true, JsonConverters));
                }
            }
        }


        #endregion

        /// <summary>
        /// Deserializes the json object.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="type">The type.</param>
        /// <returns>System.Object.</returns>
        protected object DeserializeJsonObject(string value, Type type)
        {
            try
            {
                if (type == typeof(object))
                {
                    type = typeof(JObject);
                }
                return JsonConvert.DeserializeObject(value, type);
            }
            catch (Exception ex)
            {
                throw new InvalidObjectException(ex, new { type = type.Name });
            }
        }

        /// <summary>
        /// Gets the route key.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="resource">The resource.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="action">The action.</param>
        /// <returns>System.String.</returns>
        internal static string GetRouteKey(string version, string resource, string httpMethod, string action)
        {
            return string.Format("{0}:/{1}/{2}/{3}/", httpMethod, version, resource, action);
        }
    }
}
