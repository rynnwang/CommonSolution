using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Routing;
using Beyova.Api;
using Beyova.ExceptionSystem;
using Beyova.License;

namespace Beyova.RestApi
{
    /// <summary>
    /// Class RestApiRouter, which deeply integrated with <see cref="ContextHelper"/> for common usage.
    /// </summary>
    public class RestApiRouter : ApiHandlerBase, IRouteHandler
    {
        #region Protected fields

        /// <summary>
        /// The route operation locker
        /// </summary>
        protected static object routeOperationLocker = new object();

        /// <summary>
        /// The routes
        /// </summary>
        protected static volatile Dictionary<string, RuntimeRoute> routes =
            new Dictionary<string, RuntimeRoute>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// The initialized types
        /// </summary>
        protected static volatile HashSet<string> initializedTypes = new HashSet<string>();

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="RestApiRouter"/> class.
        /// </summary>
        public RestApiRouter()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RestApiRouter" /> class.
        /// </summary>
        /// <param name="defaultApiSettings">The default API settings.</param>
        /// <param name="allowOptions">if set to <c>true</c> [allow options].</param>
        public RestApiRouter(RestApiSettings defaultApiSettings, bool allowOptions = false)
            : base(defaultApiSettings, allowOptions)
        {
        }

        #endregion

        /// <summary>
        /// Adds the handler (instance and settings) into route.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="settings">The settings.</param>
        public void Add(object instance, RestApiSettings settings = null)
        {
            InitializeRoute(instance, settings);
        }

        /// <summary>
        /// Initializes the routes.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="settings">The settings.</param>
        /// <exception cref="DataConflictException">Route</exception>
        protected void InitializeRoute(object instance, RestApiSettings settings = null)
        {
            lock (routeOperationLocker)
            {
                if (instance != null)
                {
                    var typeName = instance.GetType().FullName;
                    if (!initializedTypes.Contains(typeName))
                    {
                        #region Initialize routes

                        var doneInterfaceTypes = new List<string>();

                        foreach (var interfaceType in instance.GetType().GetInterfaces())
                        {
                            InitializeApiType(doneInterfaceTypes, routes, interfaceType, instance, settings);
                        }

                        #endregion

                        initializedTypes.Add(typeName);
                    }
                }
            }
        }

        /// <summary>
        /// Initializes the type of the API.
        /// </summary>
        /// <param name="doneInterfaceTypes">The done interface types.</param>
        /// <param name="routes">The routes.</param>
        /// <param name="interfaceType">Type of the interface.</param>
        /// <param name="instance">The instance.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="parentApiContractAttribute">The parent API class attribute.</param>
        /// <param name="parentApiModuleAttribute">The parent API module attribute.</param>
        protected void InitializeApiType(List<string> doneInterfaceTypes, Dictionary<string, RuntimeRoute> routes, Type interfaceType, object instance, RestApiSettings settings = null, ApiContractAttribute parentApiContractAttribute = null, ApiModuleAttribute parentApiModuleAttribute = null)
        {
            if (routes != null && interfaceType != null && doneInterfaceTypes != null)
            {
                if (doneInterfaceTypes.Contains(interfaceType.FullName))
                {
                    return;
                }

                var apiContract = parentApiContractAttribute ?? interfaceType.GetCustomAttribute<ApiContractAttribute>(true);
                var apiModule = parentApiModuleAttribute ?? interfaceType.GetCustomAttribute<ApiModuleAttribute>(true);
                var moduleName = apiModule?.ToString();

                if (apiContract != null && !string.IsNullOrWhiteSpace(apiContract.Version))
                {
                    if (apiContract.Version.SafeToLower().Equals(BuiltInFeatureVersionKeyword))
                    {
                        throw ExceptionFactory.CreateInvalidObjectException("apiContract.Version", reason: "<builtin> cannot be used as version due to it is used internally.");
                    }

                    foreach (var method in interfaceType.GetMethods())
                    {
                        var apiOperationAttribute = method.GetCustomAttribute<ApiOperationAttribute>(true);

                        #region Initialize based on ApiOperation

                        if (apiOperationAttribute != null)
                        {
                            var permissions = new Dictionary<string, ApiPermission>();
                            var additionalHeaderKeys = new HashSet<string>();

                            var apiPermissionAttributes =
                                method.GetCustomAttributes<ApiPermissionAttribute>(true);

                            if (apiPermissionAttributes != null)
                            {
                                foreach (var one in apiPermissionAttributes)
                                {
                                    permissions.Merge(one.PermissionIdentifier, one.Permission);
                                }
                            }

                            var headerKeyAttributes = method.GetCustomAttributes<ApiHeaderAttribute>(true);
                            if (headerKeyAttributes != null)
                            {
                                foreach (var one in headerKeyAttributes)
                                {
                                    additionalHeaderKeys.Add(one.HeaderKey);
                                }
                            }

                            var routeKey = GetRouteKey(apiContract.Version, apiOperationAttribute.ResourceName,
                                apiOperationAttribute.HttpMethod, apiOperationAttribute.Action);

                            var tokenRequired =
                                method.GetCustomAttribute<TokenRequiredAttribute>(true) ??
                                interfaceType.GetCustomAttribute<TokenRequiredAttribute>(true);

                            var runtimeRoute = new RuntimeRoute(method, interfaceType, instance,
                                   !string.IsNullOrWhiteSpace(apiOperationAttribute.Action),
                                   tokenRequired != null && tokenRequired.TokenRequired, moduleName, settings, permissions, additionalHeaderKeys.ToList());

                            if (routes.ContainsKey(routeKey))
                            {
                                throw new DataConflictException("Route", objectIdentity: routeKey, data: new
                                {
                                    existed = routes[routeKey].SafeToString(),
                                    newMethod = method.GetFullName(),
                                    newInterface = interfaceType.FullName
                                });
                            }

                            routes.Add(routeKey, runtimeRoute);
                        }

                        #endregion
                    }

                    foreach (var one in interfaceType.GetInterfaces())
                    {
                        InitializeApiType(doneInterfaceTypes, routes, one, instance, settings, apiContract, apiModule);
                    }

                    //Special NOTE:
                    // Move this add action in scope of if apiContract is valid.
                    // Reason: in complicated cases, when [A:Interface1] without ApiContract, but [Interface2: Interface] with defining ApiContract, and [B: A, Interface2], then correct contract definition might be missed.
                    doneInterfaceTypes.Add(interfaceType.FullName);
                }

            }
        }

        #region Protected Methods

        /// <summary>
        /// Processes the route.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Exception.</returns>
        protected override RuntimeContext ProcessRoute(HttpRequest request)
        {
            var result = new RuntimeContext();
            var rawUrl = request.RawUrl;
            var rawFullUrl = request.ToFullRawUrl();

            if (!request.FillRouteInfo(result))
            {
                throw ExceptionFactory.CreateInvalidObjectException("URL");
            }

            if (result.Version.Equals(BuiltInFeatureVersionKeyword, StringComparison.OrdinalIgnoreCase))
            {
                return result;
            }

            if (string.IsNullOrWhiteSpace(result.ResourceName))
            {
                throw new ResourceNotFoundException(rawFullUrl, "resourceName");
            }

            RuntimeRoute runtimeRoute;

            if (!routes.TryGetValue(GetRouteKey(result.Version, result.ResourceName, request.HttpMethod, result.Parameter1), out runtimeRoute))
            {
                routes.TryGetValue(GetRouteKey(result.Version, result.ResourceName, request.HttpMethod, null), out runtimeRoute);
            }
            else
            {
                if (runtimeRoute != null && (!string.IsNullOrWhiteSpace(result.Parameter1) && !runtimeRoute.IsActionUsed))
                {
                    throw new ResourceNotFoundException(rawFullUrl);
                }
            }

            if (runtimeRoute == null)
            {
                throw new ResourceNotFoundException(rawFullUrl);
            }

            // Override out parameters
            result.ModuleName = runtimeRoute.ModuleName;
            result.CustomizedHeaderKeys = runtimeRoute.HeaderKeys;

            result.ApiMethod = runtimeRoute.MethodInfo;
            result.ApiInstance = runtimeRoute.Instance;
            result.IsActionUsed = runtimeRoute.IsActionUsed;
            result.IsVoid = runtimeRoute.MethodInfo?.ReturnType?.IsVoid();
            result.Settings = runtimeRoute.Setting;

            var tokenHeaderKey = (result.Settings ?? DefaultSettings)?.TokenHeaderKey;
            var token = (request != null && !string.IsNullOrWhiteSpace(tokenHeaderKey)) ? request.TryGetHeader(tokenHeaderKey) : string.Empty;

            string userIdentifier = ContextHelper.ApiContext.Token = token;

            var authenticationException = Authenticate(runtimeRoute, token, out userIdentifier);

            if (authenticationException != null)
            {
                throw authenticationException.Handle(new { result.ApiMethod.Name, token });
            }

            return result;
        }

        #endregion

        /// <summary>
        /// Processes the build in feature.
        /// </summary>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <param name="runtimeContext">The runtime context.</param>
        /// <param name="isLocalhost">The is localhost.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <returns>
        /// System.Object.
        /// </returns>
        protected override object ProcessBuiltInFeature(HttpRequest httpRequest, RuntimeContext runtimeContext, bool isLocalhost, out string contentType)
        {
            object result = null;
            contentType = HttpConstants.ContentType.Json;

            switch (runtimeContext?.ResourceName.SafeToLower())
            {
                case "apilist":
                    result = routes.Select(x => new { Url = x.Key.TrimEnd('/') + "/", Method = x.Value.MethodInfo?.Name }).ToList();
                    break;
                case "configuration":
                    result = isLocalhost ? Framework.ConfigurationReader.GetValues() : "This API is available at localhost machine." as object;
                    break;
                case "license":
                    if (isLocalhost)
                    {
                        var licenseDictionary = new Dictionary<string, object>();
                        foreach (var one in BeyovaLicenseContainer.Licenses)
                        {
                            licenseDictionary.Add(one.Key, new
                            {
                                one.Value.LicensePath,
                                one.Value.Entry
                            });
                        }
                    }
                    else
                    {
                        result = "This API is available at localhost machine.";
                    }
                    break;
                case "doc":
                    DocumentGenerator generator = new DocumentGenerator(DefaultSettings.TokenHeaderKey.SafeToString(HttpConstants.HttpHeader.TOKEN));
                    result = generator.WriteHtmlDocumentToZip((from item in routes select item.Value.InstanceType).Distinct().ToArray());
                    contentType = HttpConstants.ContentType.ZipFile;
                    break;
                default: break;
            }

            return result ?? base.ProcessBuiltInFeature(httpRequest, runtimeContext, isLocalhost, out contentType);
        }

        /// <summary>
        /// Authenticates the specified service type.
        /// </summary>
        /// <param name="runtimeRoute">The runtime route.</param>
        /// <param name="token">The token.</param>
        /// <param name="userIdentifier">The user identifier.</param>
        /// <returns>System.Nullable&lt;Guid&gt;.</returns>
        protected BaseException Authenticate(RuntimeRoute runtimeRoute, string token, out string userIdentifier)
        {
            userIdentifier = token;
            ICredential credential = null;

            if (!string.IsNullOrWhiteSpace(token))
            {
                var eventHandlers = (runtimeRoute.Setting ?? DefaultSettings)?.EventHandlers;

                if (eventHandlers != null)
                {
                    try
                    {
                        credential = eventHandlers.GetCredentialByToken(token);
                    }
                    catch { }

                    if (credential != null)
                    {
                        userIdentifier = credential.Name;
                    }
                }
            }

            ContextHelper.ApiContext.CurrentCredential = credential;

            if (!runtimeRoute.IsTokenRequired)
            {
                return null;
            }

            //Check permissions
            if (credential != null)
            {
                var userPermissions = ContextHelper.ApiContext.CurrentPermissionIdentifiers?.Permissions ?? new List<string>();
                return userPermissions.ValidateApiPermission(runtimeRoute.Permissions, token, runtimeRoute.MethodInfo.GetFullName());
            }

            return new UnauthorizedTokenException(new { token });
        }

        /// <summary>
        /// To the web route.
        /// </summary>
        /// <returns>Route.</returns>
        public Route ToWebRoute()
        {
            var routeValueDictionary = new RouteValueDictionary { { "apiUrl", RestApiExtension.apiUrlRegex } };

            return new Route("{*apiUrl}", defaults: null, routeHandler: this, constraints: routeValueDictionary);
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
    }
}
