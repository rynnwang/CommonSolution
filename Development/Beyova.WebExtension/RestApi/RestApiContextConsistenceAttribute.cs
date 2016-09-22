using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using Beyova;
using Beyova.Api;
using Beyova.ApiTracking;
using Beyova.ExceptionSystem;
using Beyova.RestApi;

namespace Beyova.WebExtension
{
    /// <summary>
    /// Class RestApiContextConsistenceAttribute.
    /// This attribute is used in MVC to ensure following things are kept as same as REST API framework in Beyova.Common.
    /// <list type="number">
    /// <item><c>Token</c>, <c>User-Agent</c> and <c>IP address</c> would be initialized in <see cref="ContextHelper"/></item>
    /// <item><c>Exception</c>, <c>API Event</c> and <c>API Trace</c> would be handled.</item>
    /// </list> 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class RestApiContextConsistenceAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Gets or sets the API tracking.
        /// </summary>
        /// <value>The API tracking.</value>
        public IApiTracking ApiTracking { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether [log API event].
        /// </summary>
        /// <value><c>true</c> if [log API event]; otherwise, <c>false</c>.</value>
        public bool LogApiEvent { get; set; }

        /// <summary>
        /// The settings
        /// </summary>
        private RestApiSettings settings;

        /// <summary>
        /// The API event
        /// </summary>
        [ThreadStatic]
        internal static ApiEventLog ApiEvent;

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="RestApiContextConsistenceAttribute"/> class.
        /// </summary>
        /// <param name="restApiSetting">The rest API settings.</param>
        public RestApiContextConsistenceAttribute(RestApiSettings restApiSetting) : base()
        {
            this.settings = restApiSetting;
            ApiTracking = restApiSetting?.ApiTracking;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RestApiContextConsistenceAttribute"/> class.
        /// </summary>
        /// <param name="settingName">Name of the setting.</param>
        public RestApiContextConsistenceAttribute(string settingName = null)
            : this(ApiHandlerBase.GetRestApiSettingByName(settingName, false))
        {
        }

        #endregion

        /// <summary>
        /// Called when [result executed].
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);

            // If using BeyovaBaseController, exception should be logger there, and in this method, no exception should appear.
            var baseException = filterContext.Exception?.Handle(new ExceptionScene
            {
                MethodName = string.Format("{0}: {1}", filterContext.HttpContext.Request.HttpMethod, filterContext.HttpContext.Request.RawUrl)
            }, data: (filterContext.Exception as BaseException)?.ReferenceData);

            if (baseException != null)
            {
                filterContext.Exception = baseException;
            }

            if (ApiTracking != null)
            {
                DateTime exitStamp = DateTime.UtcNow;

                // API EXCEPTION
                if (baseException != null)
                {
                    try
                    {
                        ApiTracking.LogException(baseException.ToExceptionInfo());
                    }
                    catch { }
                }

                // API EVENT
                if (ApiEvent != null)
                {
                    try
                    {
                        ApiEvent.ExitStamp = exitStamp;
                        ApiEvent.ExceptionKey = baseException?.Key;

                        ApiTracking.LogApiEvent(ApiEvent);
                    }
                    catch { }
                }

                // API TRACE
                try
                {
                    ApiTraceContext.Exit((ApiEvent?.ExceptionKey) ?? (baseException?.Key), exitStamp);
                    var traceLog = ApiTraceContext.GetCurrentTraceLog(true);

                    if (traceLog != null)
                    {
                        ApiTracking.LogApiTraceLog(traceLog);
                    }
                }
                catch { }
            }

            ThreadExtension.Clear();
        }

        /// <summary>
        /// Called by the ASP.NET MVC framework before the action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            DateTime entryStamp = DateTime.UtcNow;
            var request = filterContext.HttpContext.Request;

            var traceId = request.TryGetHeader(HttpConstants.HttpHeader.TRACEID);
            var traceSequence = request.TryGetHeader(HttpConstants.HttpHeader.TRACESEQUENCE).ObjectToNullableInt32();
            var methodInfo = (filterContext.ActionDescriptor as ReflectedActionDescriptor)?.MethodInfo;

            ContextHelper.ConsistContext(request, this.settings?.Name);

            if (!string.IsNullOrWhiteSpace(traceId))
            {
                ApiTraceContext.Initialize(traceId, traceSequence, entryStamp);
            }

            if (settings != null && settings.TrackingEvent)
            {
                var context = filterContext.HttpContext;
                ApiEvent = new ApiEventLog
                {
                    ApiFullName = methodInfo?.GetFullName(),
                    RawUrl = context.Request.RawUrl,
                    EntryStamp = entryStamp,
                    UserAgent = context.Request.UserAgent,
                    TraceId = traceId,
                    // If request came from ApiTransport or other proxy ways, ORIGINAL stands for the IP ADDRESS from original requester.
                    IpAddress = context.Request.TryGetHeader(settings?.OriginalIpAddressHeaderKey.SafeToString(HttpConstants.HttpHeader.ORIGINAL)).SafeToString(context.Request.UserHostAddress),
                    CultureCode = ContextHelper.ApiContext.CultureCode,
                    ContentLength = context.Request.ContentLength,
                    OperatorCredential = ContextHelper.CurrentCredential as BaseCredential,
                    Protocol = context.Request.Url.Scheme,
                    ReferrerUrl = context.Request.UrlReferrer?.ToString(),
                    ServerIdentifier = EnvironmentCore.ServerName,
                    ServiceIdentifier = EnvironmentCore.ProjectName
                };
            }

            var controllerType = methodInfo?.DeclaringType;

            var tokenRequiredAttribute = methodInfo?.GetCustomAttribute<TokenRequiredAttribute>(true) ?? controllerType?.GetCustomAttribute<TokenRequiredAttribute>(true);
            var permissionAttributes = controllerType?.GetCustomAttributes<ApiPermissionAttribute>(true).ToDictionary();
            permissionAttributes.Merge(methodInfo?.GetCustomAttributes<ApiPermissionAttribute>(true).ToDictionary(), true);

            var tokenRequired = tokenRequiredAttribute != null && tokenRequiredAttribute.TokenRequired;

            if (tokenRequired)
            {
                if (!ContextHelper.IsUser)
                {
                    var baseException = (new UnauthorizedTokenException(ContextHelper.Token)).Handle(
                   filterContext.HttpContext.Request.ToExceptionScene(filterContext.RouteData?.GetControllerName()), data: new { filterContext.HttpContext.Request.RawUrl });

                    HandleUnauthorizedAction(filterContext, baseException);
                }
                else if (permissionAttributes.HasItem())
                {
                    var baseException = ContextHelper.CurrentUserInfo?.Permissions.ValidateApiPermission(permissionAttributes, ContextHelper.Token, methodInfo?.GetFullName());

                    if (baseException != null)
                    {
                        HandleUnauthorizedAction(filterContext, baseException);
                    }
                }
            }

            ApiTraceContext.Enter(methodInfo.GetFullName(), setNameAsMajor: true);
        }

        /// <summary>
        /// Handles the unauthorized action.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        /// <param name="baseException">The base exception.</param>
        protected virtual void HandleUnauthorizedAction(ActionExecutingContext filterContext, BaseException baseException)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                ApiHandlerBase.PackageResponse(filterContext.HttpContext.Response, null, baseException, settings: settings);
                filterContext.Result = null;
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(routeValues: new RouteValueDictionary
                {
                    {"controller", "Error"},
                    {"action", "Index"},
                    {"code", 401},
                    {"minor", baseException.Code.Minor},
                    {"message", baseException.Message}
                });
            }
        }
    }
}