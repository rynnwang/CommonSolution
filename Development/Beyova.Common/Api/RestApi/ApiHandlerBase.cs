﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;
using Beyova.Api;
using Beyova.ApiTracking;
using Beyova.Cache;
using Beyova.ExceptionSystem;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Beyova.RestApi
{
    /// <summary>
    /// Class ApiHandlerBase.
    /// </summary>
    public abstract class ApiHandlerBase : IHttpHandler
    {
        #region Protected static fields

        /// <summary>
        /// The built-in feature version keyword
        /// </summary>
        protected const string BuiltInFeatureVersionKeyword = "builtin";

        /// <summary>
        /// The default setting name
        /// </summary>
        protected const string defaultSettingName = "default";

        /// <summary>
        /// The json converters
        /// </summary>
        protected static readonly HashSet<JsonConverter> jsonConverters = new HashSet<JsonConverter>();

        /// <summary>
        /// The json converters
        /// </summary>
        internal static JsonConverter[] JsonConverters = null;

        /// <summary>
        /// The settings container
        /// </summary>
        internal static Dictionary<string, RestApiSettings> settingsContainer = new Dictionary<string, RestApiSettings>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// The default rest API settings
        /// </summary>
        protected static RestApiSettings defaultRestApiSettings = null;

        /// <summary>
        /// Gets the default rest API settings.
        /// </summary>
        /// <value>The default rest API settings.</value>
        public static RestApiSettings DefaultRestApiSettings { get { return defaultRestApiSettings; } }

        #endregion Protected static fields

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

        #endregion Property

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
                EnableContentCompression = true
            };

            DefaultSettings.Name = DefaultSettings.Name.SafeToString(defaultSettingName);
            DefaultSettings.ApiTracking = DefaultSettings.ApiTracking ?? Framework.ApiTracking;

            settingsContainer.Merge(DefaultSettings.Name, DefaultSettings, false);

            this.AllowOptions = allowOptions;

            if (defaultRestApiSettings == null)
            {
                defaultRestApiSettings = defaultApiSettings ?? new RestApiSettings();
            }
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
                return true;
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
            string traceId = null;
            int? traceSequence = default(int?);
            BaseException baseException = null;

            context.Response.Headers.Add(HttpConstants.HttpHeader.SERVERENTRYTIME, entryStamp.ToFullDateTimeTzString());

            var acceptEncoding = context.Request.Headers[HttpConstants.HttpHeader.AcceptEncoding].SafeToLower();

            try
            {
                //First of all, clean thread info for context.
                ContextHelper.Reinitialize();

                traceId = context.Request.TryGetHeader(HttpConstants.HttpHeader.TRACEID);
                traceSequence = context.Request.TryGetHeader(HttpConstants.HttpHeader.TRACESEQUENCE).ToNullableInt32();

                Prepare(context.Request);

                if (context.Request.HttpMethod.Equals(HttpConstants.HttpMethod.Options, StringComparison.OrdinalIgnoreCase))
                {
                    if (this.AllowOptions)
                    {
                        //Return directly. IIS would append following headers by default, according to what exactly web.config have.
                        //Access-Control-Allow-Origin: *
                        //Access-Control-Allow-Headers: Content-Type
                        //Access-Control-Allow-Methods: GET, POST, PUT, DELETE, OPTIONS

                        context.Response.Headers.Add(HttpConstants.HttpHeader.SERVEREXITTIME, DateTime.UtcNow.ToFullDateTimeTzString());

                        return;
                    }
                }

                // Authentication has already done inside ProcessRoute.
                runtimeContext = ProcessRoute(context.Request);
                runtimeContext.CheckNullObject(nameof(runtimeContext));

                settings = runtimeContext.Settings ?? DefaultSettings;

                // Fill basic context info.

                var userAgentHeaderKey = settings?.OriginalUserAgentHeaderKey;
                var apiContext = ContextHelper.ApiContext;
                apiContext.UserAgent = string.IsNullOrWhiteSpace(userAgentHeaderKey) ? context.Request.UserAgent : context.Request.TryGetHeader(userAgentHeaderKey).SafeToString(context.Request.UserAgent);
                apiContext.IpAddress = context.Request.TryGetHeader(settings?.OriginalIpAddressHeaderKey.SafeToString(HttpConstants.HttpHeader.ORIGINAL)).SafeToString(context.Request.UserHostAddress);
                apiContext.CurrentUri = context.Request.Url;

                apiContext.CultureCode = context.Request.QueryString.Get(HttpConstants.QueryString.Language).SafeToString(context.Request.UserLanguages.SafeFirstOrDefault()).EnsureCultureCode();
                if (runtimeContext.OperationParameters?.EntitySynchronizationMode != null)
                {
                    apiContext.LastSynchronizedStamp = context.Request.TryGetHeader(runtimeContext.OperationParameters.EntitySynchronizationMode.IfModifiedSinceKey).ObjectToDateTime();
                }

                // Fill finished.
                if (string.IsNullOrWhiteSpace(runtimeContext.Realm) && runtimeContext.Version.Equals(BuiltInFeatureVersionKeyword, StringComparison.OrdinalIgnoreCase))
                {
                    string contentType;
                    var buildInResult = ProcessBuiltInFeature(context.Request, runtimeContext, context.Request.IsLocal, out contentType);
                    PackageOutput(context.Response, buildInResult, new RuntimeApiOperationParameters { ContentType = contentType }, null, acceptEncoding, runtimeContext.IsVoid ?? false, settings);
                }
                else
                {
                    //Initialize additional header keys
                    if (runtimeContext.CustomizedHeaderKeys.HasItem())
                    {
                        var currentApiContext = ContextHelper.ApiContext;

                        foreach (var one in runtimeContext.CustomizedHeaderKeys)
                        {
                            currentApiContext.CustomizedHeaders.Merge(one, context.Request.TryGetHeader(one));
                        }
                    }

                    if (settings != null && settings.ApiTracking != null && settings.TrackingEvent)
                    {
                        eventLog = new ApiEventLog
                        {
                            RawUrl = context.Request.RawUrl,
                            EntryStamp = entryStamp,
                            UserAgent = context.Request.UserAgent,
                            TraceId = traceId,
                            // If request came from ApiTransport or other proxy ways, ORIGINAL stands for the IP ADDRESS from original requester.
                            IpAddress = context.Request.TryGetHeader(settings?.OriginalIpAddressHeaderKey.SafeToString(HttpConstants.HttpHeader.ORIGINAL)).SafeToString(context.Request.UserHostAddress),
                            CultureCode = context.Request.UserLanguages == null ? null : context.Request.UserLanguages.FirstOrDefault(),
                            ContentLength = context.Request.ContentLength,
                            OperatorCredential = ContextHelper.CurrentCredential as BaseCredential,
                            Protocol = context.Request.Url.Scheme
                        };
                    }

                    InitializeContext(context.Request, runtimeContext);

                    if (!string.IsNullOrWhiteSpace(traceId))
                    {
                        ApiTraceContext.Initialize(traceId, traceSequence, entryStamp);
                    }

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

                        var clientIdentifierHeaderKey = settings.ClientIdentifierHeaderKey;
                        if (!string.IsNullOrWhiteSpace(clientIdentifierHeaderKey))
                        {
                            eventLog.ClientIdentifier = context.Request.TryGetHeader(clientIdentifierHeaderKey);
                        }

                        eventLog.ModuleName = runtimeContext.OperationParameters.ModuleName;
                        eventLog.ResourceEntityKey = runtimeContext.IsActionUsed ? runtimeContext.Parameter2 : runtimeContext.Parameter1;
                    }

                    if (runtimeContext.Exception != null)
                    {
                        throw runtimeContext.Exception;
                    }

                    ApiTraceContext.Enter(runtimeContext, setNameAsMajor: true);
                    string jsonBody = null;

                    try
                    {
                        if (runtimeContext.ApiCacheStatus == ApiCacheStatus.UseCache)
                        {
                            jsonBody = runtimeContext.CachedResponseBody;
                        }
                        else
                        {
                            var invokeResult = Invoke(runtimeContext.ApiInstance, runtimeContext.ApiMethod, context.Request, runtimeContext.EntityKey, out jsonBody);

                            if (runtimeContext.ApiCacheStatus == ApiCacheStatus.UpdateCache)
                            {
                                runtimeContext.ApiCacheContainer.Update(runtimeContext.ApiCacheIdentity, jsonBody);
                            }

                            PackageOutput(context.Response, invokeResult, runtimeContext.OperationParameters, null, acceptEncoding, runtimeContext.IsVoid ?? false, settings: settings);
                        }
                    }
                    catch (Exception invokeEx)
                    {
                        baseException = invokeEx.Handle(new { Url = context.Request.RawUrl, Method = context.Request.HttpMethod });
                        throw baseException;
                    }
                    finally
                    {
                        if (eventLog != null && !string.IsNullOrWhiteSpace(jsonBody) && !(runtimeContext.OperationParameters?.IsDataSensitive ?? false))
                        {
                            eventLog.Content = jsonBody.Length > 50 ? ((jsonBody.Substring(0, 40) + "..." + jsonBody.Substring(jsonBody.Length - 6, 6))) : jsonBody;
                        }

                        ApiTraceContext.Exit(baseException?.Key);
                    }
                }
            }
            catch (Exception ex)
            {
                var apiTracking = settings?.ApiTracking;
                baseException = (ex as BaseException) ?? ex.Handle(new { Uri = context.Request.Url.ToString() });

                (apiTracking ?? Framework.ApiTracking)?.LogException(baseException.ToExceptionInfo());

                if (eventLog != null)
                {
                    eventLog.ExceptionKey = baseException.Key;
                }

                PackageOutput(context.Response, null, null, baseException, acceptEncoding, settings: settings);
            }
            finally
            {
                if (settings?.ApiTracking != null)
                {
                    var exitStamp = DateTime.UtcNow;
                    if (eventLog != null)
                    {
                        try
                        {
                            eventLog.ExitStamp = exitStamp;
                            settings.ApiTracking.LogApiEvent(eventLog);
                        }
                        catch { }
                    }

                    if (ApiTraceContext.Root != null)
                    {
                        try
                        {
                            ApiTraceContext.Exit(baseException?.Key, exitStamp);
                            settings.ApiTracking.LogApiTraceLog(ApiTraceContext.GetCurrentTraceLog(true));
                        }
                        catch { }
                    }
                }

                Dispose();
            }
        }

        /// <summary>
        /// Processes the built in feature.
        /// </summary>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <param name="runtimeContext">The runtime context.</param>
        /// <param name="isLocalhost">The is localhost.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <returns>
        /// System.Object.
        /// </returns>
        protected virtual object ProcessBuiltInFeature(HttpRequest httpRequest, RuntimeContext runtimeContext, bool isLocalhost, out string contentType)
        {
            object result = null;
            contentType = HttpConstants.ContentType.Json;

            switch (runtimeContext?.ResourceName.SafeToLower())
            {
                case "server":
                    result = Framework.AboutService();
                    break;

                case "machine":
                    result = SystemManagementExtension.GetMachineHealth();
                    break;

                case "cache":
                    result = CacheRealm.GetSummary();
                    break;

                case "clearcache":
                    result = isLocalhost ? CacheRealm.ClearAll() : "This API is available at localhost machine." as object;
                    break;

                case "gravity":
                    result = Gravity.GravityShell.Host?.Info;
                    break;

                case "i18n":
                    result = GlobalCultureResourceCollection.Instance?.AvailableCultureInfo ?? new Collection<CultureInfo>();
                    break;

                case "mirror":
                    var apiContext = ContextHelper.ApiContext;
                    var headers = new Dictionary<string, string>();

                    foreach (var key in httpRequest.Headers.AllKeys)
                    {
                        headers.Add(key, httpRequest.Headers[key]);
                    }

                    result = new
                    {
                        RawUrl = httpRequest.RawUrl,
                        HttpMethod = httpRequest.HttpMethod,
                        Headers = headers,
                        UserAgent = apiContext.UserAgent,
                        IpAddress = apiContext.IpAddress,
                        CultureCode = apiContext.CultureCode
                    };
                    break;

                case "assemblyhash":
                    result = EnvironmentCore.GetAssemblyHash();
                    break;

                case "dll":
                    var dllName = httpRequest.QueryString.Get("name");
                    if (!string.IsNullOrWhiteSpace(dllName) && httpRequest.HttpMethod.MeaningfulEquals(HttpConstants.HttpMethod.Post, StringComparison.OrdinalIgnoreCase))
                    {
                        try
                        {
                            var dllPath = Path.Combine(EnvironmentCore.ApplicationBaseDirectory, dllName + ".dll");
                            if (File.Exists(dllPath))
                            {
                                result = File.ReadAllBytes(dllPath);
                                contentType = HttpConstants.ContentType.BinaryDefault;
                            }
                        }
                        catch { }
                    }
                    break;

                default:
                    break;
            }

            return result;
        }

        /// <summary>
        /// Disposes this instance.
        /// </summary>
        protected virtual void Dispose()
        {
            ThreadExtension.Clear();
        }

        #endregion IHttpHandler

        #region Protected virtual methods

        /// <summary>
        /// Prepares the specified request.
        /// <remarks>
        /// This method would be called before <c>ProcessRoute</c>. It can be used to help you to do some preparation, such as get something from headers or cookie for later actions.
        /// ou can save them in Thread data so that you can get them later in <c>ProcessRoute</c>, <c>Invoke</c>, <c>PackageOutput</c> ,etc.
        /// If any exception is throw from this method, the process flow would be interrupted.
        /// </remarks></summary>
        /// <param name="request">The request.</param>
        protected virtual void Prepare(HttpRequest request)
        {
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
        /// </item></list></remarks></summary>
        /// <param name="instance">The instance.</param>
        /// <param name="methodInfo">The method information.</param>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <param name="key">The key.</param>
        /// <param name="jsonBody">The json body.</param>
        /// <returns>System.Object.</returns>
        protected virtual object Invoke(object instance, MethodInfo methodInfo, HttpRequest httpRequest, string key, out string jsonBody)
        {
            return InternalInvoke(instance, methodInfo, httpRequest.GetPostData(), httpRequest.Url, key, out jsonBody);
        }

        /// <summary>
        /// Internals the invoke.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="methodInfo">The method information.</param>
        /// <param name="bodyData">The body data.</param>
        /// <param name="uri">The URI.</param>
        /// <param name="key">The key.</param>
        /// <param name="jsonBody">The json body.</param>
        /// <returns>System.Object.</returns>
        internal static object InternalInvoke(object instance, MethodInfo methodInfo, byte[] bodyData, Uri uri, string key, out string jsonBody)
        {
            var inputParameters = methodInfo.GetParameters();
            jsonBody = null;

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
                    var json = jsonBody = (bodyData == null ? null : Encoding.UTF8.GetString(bodyData));
                    return methodInfo.Invoke(instance, new object[] { DeserializeJsonObject(json, inputParameters[0].ParameterType) });
                }
            }
            else
            {
                object[] parameters = new object[inputParameters.Length];

                var queryString = HttpUtility.ParseQueryString(uri.Query);
                if (queryString.Count > 0)
                {
                    var start = 1;
                    if (!string.IsNullOrWhiteSpace(key) && (inputParameters[0].ParameterType.TryGetNullableType().IsValueType))
                    {
                        parameters[0] = ReflectionExtension.ConvertToObjectByType(inputParameters[0].ParameterType, key);
                    }
                    else
                    {
                        var json = jsonBody = (bodyData == null ? null : Encoding.UTF8.GetString(bodyData));
                        parameters[0] = json.TryConvertJsonToObject();
                        if (parameters[0] == null)
                        {
                            start = 0;
                        }
                    }

                    for (var i = start; i < inputParameters.Length; i++)
                    {
                        parameters[i] = ReflectionExtension.ConvertToObjectByType(inputParameters[i].ParameterType,
                            queryString.Get(inputParameters[i].Name));
                    }
                }
                else
                {
                    var json = jsonBody = (bodyData == null ? null : Encoding.UTF8.GetString(bodyData));
                    var jsonObject = string.IsNullOrWhiteSpace(json) ? null : JObject.Parse(json);

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
        /// Processes the route. More than 1 parameters, from either query string or body json.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Exception.</returns>
        protected abstract RuntimeContext ProcessRoute(HttpRequest request);

        #endregion Protected virtual methods

        #region PackageResponse

        /// <summary>
        /// Packages the output.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="data">The data.</param>
        /// <param name="operationParameters">The operation parameters.</param>
        /// <param name="baseException">The base exception.</param>
        /// <param name="acceptEncoding">Name of the compress.</param>
        /// <param name="noBody">if set to <c>true</c> [no body].</param>
        /// <param name="settings">The settings.</param>
        /// <returns>System.Object.</returns>
        protected internal virtual void PackageOutput(HttpResponse response, object data, RuntimeApiOperationParameters operationParameters, BaseException baseException = null, string acceptEncoding = null, bool noBody = false, RestApiSettings settings = null)
        {
            PackageResponse(new HttpResponseWrapper(response), data, operationParameters, baseException, acceptEncoding, noBody, settings);
        }

        /// <summary>
        /// Packages the response.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="data">The data.</param>
        /// <param name="ex">The ex.</param>
        /// <param name="acceptEncoding">Name of the compression.</param>
        /// <param name="noBody">if set to <c>true</c> [no body].</param>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>System.Object.</returns>
        public static void PackageResponse(HttpResponse response, object data, BaseException ex = null, string acceptEncoding = null, bool noBody = false, string contentType = null, RestApiSettings settings = null)
        {
            if (response != null)
            {
                PackageResponse(new HttpResponseWrapper(response), data, new RuntimeApiOperationParameters { ContentType = contentType }, ex, acceptEncoding, noBody, settings);
            }
        }

        /// <summary>
        /// Packages the response.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="data">The data.</param>
        /// <param name="ex">The ex.</param>
        /// <param name="acceptEncoding">The accept encoding.</param>
        /// <param name="noBody">The no body.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="settings">The settings.</param>
        public static void PackageResponse(HttpResponseBase response, object data, BaseException ex = null, string acceptEncoding = null, bool noBody = false, string contentType = null, RestApiSettings settings = null)
        {
            if (response != null)
            {
                PackageResponse(response, data, new RuntimeApiOperationParameters { ContentType = contentType }, ex, acceptEncoding, noBody, settings);
            }
        }

        /// <summary>
        /// Packages the response.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="data">The data.</param>
        /// <param name="operationParameters">The operation parameters.</param>
        /// <param name="ex">The ex.</param>
        /// <param name="acceptEncoding">The accept encoding.</param>
        /// <param name="noBody">if set to <c>true</c> [no body].</param>
        /// <param name="settings">The settings.</param>
        internal static void PackageResponse(HttpResponseBase response, object data, RuntimeApiOperationParameters operationParameters, BaseException ex = null, string acceptEncoding = null, bool noBody = false, RestApiSettings settings = null)
        {
            if (response != null)
            {
                if (settings == null)
                {
                    settings = defaultRestApiSettings;
                }

                var objectToReturn = ex != null ? (settings.OmitExceptionDetail ? ex.ToSimpleExceptionInfo() : ex.ToExceptionInfo()) : data;

                response.Headers.Add(HttpConstants.HttpHeader.SERVERNAME, EnvironmentCore.ServerName);
                response.Headers.AddIfNotNullOrEmpty(HttpConstants.HttpHeader.TRACEID, ApiTraceContext.TraceId);
                int httpStatusCode = (int)(ex == null ? (noBody ? HttpStatusCode.NoContent : HttpStatusCode.OK) : ex.Code.ToHttpStatusCode());

                if (ex == null && operationParameters?.EntitySynchronizationMode != null)
                {
                    DateTime? lastModifiedStamp = null;
                    data = operationParameters.EntitySynchronizationMode.RebuildOutputObject(ContextHelper.ApiContext.LastSynchronizedStamp, data, ref httpStatusCode, ref noBody, out lastModifiedStamp);

                    if (lastModifiedStamp.HasValue)
                    {
                        response.Headers.Set(operationParameters.EntitySynchronizationMode.LastModifiedKey, lastModifiedStamp.Value.ToFullDateTimeTzString());
                    }
                }

                response.StatusCode = httpStatusCode;

                if (!noBody)
                {
                    response.ContentType = (ex != null ? HttpConstants.ContentType.Json : operationParameters?.ContentType).SafeToString(HttpConstants.ContentType.Json);
                    bool isStreamBased = (objectToReturn != null && response.ContentType.StartsWith("application/", StringComparison.OrdinalIgnoreCase) && objectToReturn.GetType() == typeof(byte[]));

                    if (settings.EnableContentCompression && !isStreamBased)
                    {
                        acceptEncoding = acceptEncoding.SafeToString().ToLower();
                        if (acceptEncoding.Contains(HttpConstants.HttpValues.GZip))
                        {
                            response.Filter = new System.IO.Compression.GZipStream(response.Filter,
                                                  System.IO.Compression.CompressionMode.Compress);
                            response.Headers.Remove(HttpConstants.HttpHeader.ContentEncoding);
                            response.AppendHeader(HttpConstants.HttpHeader.ContentEncoding, HttpConstants.HttpValues.GZip);
                        }
                        else if (acceptEncoding.Contains(HttpConstants.HttpValues.Deflate))
                        {
                            response.Filter = new System.IO.Compression.DeflateStream(response.Filter,
                                                System.IO.Compression.CompressionMode.Compress);
                            response.Headers.Remove(HttpConstants.HttpHeader.ContentEncoding);
                            response.AppendHeader(HttpConstants.HttpHeader.ContentEncoding, HttpConstants.HttpValues.Deflate);
                        }
                    }

                    if (isStreamBased)
                    {
                        ((byte[])objectToReturn).ToStream().CopyStream(response.OutputStream);
                        // return as bytes;
                    }
                    else
                    {
                        //return  as string;
                        response.Write(response.ContentType.Equals(HttpConstants.ContentType.Json, StringComparison.OrdinalIgnoreCase) ? objectToReturn.ToJson(true, JsonConverters) : objectToReturn);
                    }
                }

                response.Headers.Add(HttpConstants.HttpHeader.SERVEREXITTIME, DateTime.UtcNow.ToFullDateTimeTzString());
            }
        }

        #endregion PackageResponse

        /// <summary>
        /// Adds the setting.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="setting">The setting.</param>
        /// <param name="overrideIfExists">The override if exists.</param>
        /// <returns>System.Boolean.</returns>
        public static bool AddSetting(string name, RestApiSettings setting, bool overrideIfExists = false)
        {
            if (setting != null)
            {
                return settingsContainer.Merge(name.SafeToString(), setting, overrideIfExists);
            }

            return false;
        }

        /// <summary>
        /// Deserializes the json object.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="type">The type.</param>
        /// <returns>System.Object.</returns>
        protected static object DeserializeJsonObject(string value, Type type)
        {
            try
            {
                if (type == typeof(object))
                {
                    type = typeof(JToken);
                }
                return JsonConvert.DeserializeObject(value, type, JsonConverters);
            }
            catch (Exception ex)
            {
                throw new InvalidObjectException(ex, new { type = type.Name, value });
            }
        }

        /// <summary>
        /// Gets the name of the rest API setting by.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="useDefaultIfNotFound">The use default if not found.</param>
        /// <returns>Beyova.RestApi.RestApiSettings.</returns>
        public static RestApiSettings GetRestApiSettingByName(string name, bool useDefaultIfNotFound = true)
        {
            RestApiSettings setting;
            return settingsContainer.TryGetValue(name.SafeToString(), out setting) ? setting : (useDefaultIfNotFound ? settingsContainer[defaultSettingName] : null);
        }
    }
}