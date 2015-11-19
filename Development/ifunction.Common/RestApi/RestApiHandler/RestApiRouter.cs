using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Routing;
using ifunction.ExceptionSystem;

namespace ifunction.RestApi
{
    /// <summary>
    /// Class RestApiRouter, which deeply integrated with <see cref="ContextHelper"/> for common usage.
    /// </summary>
    public class RestApiRouter : ApiHandlerBase
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

                var ApiContract = parentApiContractAttribute ?? interfaceType.GetCustomAttribute<ApiContractAttribute>(true);
                var apiModule = parentApiModuleAttribute ?? interfaceType.GetCustomAttribute<ApiModuleAttribute>(true);
                var moduleName = apiModule?.ToString();

                if (ApiContract != null && !string.IsNullOrWhiteSpace(ApiContract.Version))
                {
                    if (ApiContract.Version.SafeToLower().Equals(BuildInFeatureVersionKeyword))
                    {
                        throw new InvalidObjectException("ApiContract.Version", reason: "<buildin> cannot be used as version due to it is used internally.");
                    }

                    foreach (var method in interfaceType.GetMethods())
                    {
                        var apiOperationAttribute = method.GetCustomAttribute<ApiOperationAttribute>(true);
                        var apiTransportAttribute = method.GetCustomAttribute<ApiTransportAttribute>();

                        #region Initialize based on ApiOperation

                        if (apiOperationAttribute != null)
                        {
                            var permissions = new Dictionary<string, ApiPermission>();
                            var apiPermissionAttributes =
                                method.GetCustomAttributes<ApiPermissionAttribute>(true);

                            if (apiPermissionAttributes != null)
                            {
                                foreach (var one in apiPermissionAttributes)
                                {
                                    permissions.Add(one.PermissionIdentifier, one.Permission);
                                }
                            }

                            var routeKey = GetRouteKey(ApiContract.Version, apiOperationAttribute.ResourceName,
                                apiOperationAttribute.HttpMethod, apiOperationAttribute.Action);

                            RuntimeRoute runtimeRoute = null;

                            if (apiTransportAttribute != null)
                            {
                                runtimeRoute = new RuntimeRoute(apiTransportAttribute);
                            }
                            else
                            {
                                var tokenRequired =
                                    method.GetCustomAttribute<TokenRequiredAttribute>(true) ??
                                    interfaceType.GetCustomAttribute<TokenRequiredAttribute>(true);

                                runtimeRoute = new RuntimeRoute(method, interfaceType, instance,
                                    !string.IsNullOrWhiteSpace(apiOperationAttribute.Action),
                                    tokenRequired != null && tokenRequired.TokenRequired, moduleName, settings, permissions);
                            }

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
                        InitializeApiType(doneInterfaceTypes, routes, one, instance, settings, ApiContract, apiModule);
                    }
                }

                doneInterfaceTypes.Add(interfaceType.FullName);
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
                throw new InvalidObjectException("URL");
            }

            if (result.Version.Equals(BuildInFeatureVersionKeyword, StringComparison.OrdinalIgnoreCase))
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
            result.ApiMethod = runtimeRoute.MethodInfo;
            result.ApiInstance = runtimeRoute.Instance;
            result.IsActionUsed = runtimeRoute.IsActionUsed;
            result.ModuleName = runtimeRoute.ModuleName;
            result.ApiTransportAttribute = runtimeRoute.Transport;
            result.IsVoid = runtimeRoute.MethodInfo.ReturnType.IsVoid();
            result.Settings = runtimeRoute.Setting;

            var tokenHeaderKey = (result.Settings ?? DefaultSettings)?.TokenHeaderKey;
            var token = (request != null && !string.IsNullOrWhiteSpace(tokenHeaderKey))
              ? request.TryGetHeader(tokenHeaderKey)
              : string.Empty;

            ContextHelper.ApiContext.Token = token;

            string userIdentifier = token;
            if (runtimeRoute.Transport == null && !Authenticate(runtimeRoute, token, out userIdentifier))
            {
                throw new UnauthorizedOperationException(result.ApiMethod.Name, token);
            }

            result.UserIdentifier = userIdentifier;
            return result;
        }

        #endregion

        /// <summary>
        /// Processes the build in feature.
        /// </summary>
        /// <param name="runtimeContext">The runtime context.</param>
        /// <returns>System.Object.</returns>
        protected override object ProcessBuildInFeature(RuntimeContext runtimeContext)
        {
            object result = null;

            switch (runtimeContext?.ResourceName.SafeToLower())
            {
                case "apilist":
                    result = routes.Keys.Select(x => x.TrimEnd('/') + "/").ToList();
                    break;
                default: break;
            }

            return result ?? base.ProcessBuildInFeature(runtimeContext);
        }

        /// <summary>
        /// Authenticates the specified service type.
        /// </summary>
        /// <param name="runtimeRoute">The runtime route.</param>
        /// <param name="token">The token.</param>
        /// <param name="userIdentifier">The user identifier.</param>
        /// <returns>System.Nullable&lt;Guid&gt;.</returns>
        protected bool Authenticate(RuntimeRoute runtimeRoute, string token, out string userIdentifier)
        {
            userIdentifier = token;
            ICredential credential = null;

            if (!string.IsNullOrWhiteSpace(token))
            {
                var eventHandlers = (runtimeRoute.Setting ?? DefaultSettings)?.EventHandlers;

                if (eventHandlers != null)
                {
                    credential = eventHandlers.GetCredentialByToken(token);

                    if (credential != null)
                    {
                        userIdentifier = credential.Name;
                    }
                }
            }

            ContextHelper.ApiContext.CurrentCredential = credential;

            if (!runtimeRoute.IsTokenRequired)
            {
                return true;
            }
            else if (credential == null)
            {
                throw new UnauthorizedTokenException(string.Empty, new { token });
            }

            ContextHelper.ApiContext.CurrentCredential = credential;

            return credential != null;
        }

        /// <summary>
        /// Prepares the specified request.
        ///             
        /// <remarks>
        ///             This method would be called before <c>ProcessRoute</c>. It can be used to help you to do some preparation, such as get something from headers or cookie for later actions.
        ///             ou can save them in Thread data so that you can get them later in <c>ProcessRoute</c>, <c>Invoke</c>, <c>PackageOutput</c> ,etc.
        ///             If any exception is throw from this method, the process flow would be interrupted.
        ///             </remarks>
        /// </summary>
        /// <param name="request">The request.</param>
        protected override void Prepare(HttpRequest request)
        {
            if (request != null)
            {
                ContextHelper.Reinitialize();
                ContextHelper.ApiContext.UserAgent = request.UserAgent;
                ContextHelper.ApiContext.IpAddress = request.UserHostAddress;
                ContextHelper.ApiContext.CultureCode = request.QueryString.Get("language").SafeToString(request.UserLanguages.SafeFirstOrDefault());
            }
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
    }
}
