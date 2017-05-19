using System;
using System.Collections.Generic;
using System.Reflection;
using Beyova.Api;
using Beyova.Cache;

namespace Beyova.RestApi
{
    /// <summary>
    /// Class RuntimeRoute.
    /// </summary>
    public class RuntimeRoute
    {
        /// <summary>
        /// Gets or sets the method information.
        /// </summary>
        /// <value>The method information.</value>
        public MethodInfo MethodInfo { get; protected set; }

        /// <summary>
        /// Gets or sets the type of the instance.
        /// </summary>
        /// <value>The type of the instance.</value>
        public Type InstanceType { get; protected set; }

        /// <summary>
        /// Gets or sets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public object Instance { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is action used.
        /// </summary>
        /// <value><c>true</c> if this instance is action used; otherwise, <c>false</c>.</value>
        public bool IsActionUsed { get; protected set; }

        /// <summary>
        /// Gets or sets the setting.
        /// </summary>
        /// <value>The setting.</value>
        public RestApiSettings Setting { get; protected set; }

        /// <summary>
        /// Gets or sets the operation parameters.
        /// </summary>
        /// <value>The operation parameters.</value>
        internal RuntimeApiOperationParameters OperationParameters { get; private set; }

        /// <summary>
        /// Gets the route key.
        /// </summary>
        /// <value>
        /// The route key.
        /// </value>
        internal string RouteKey { get; private set; }

        #region Cache

        /// <summary>
        /// Gets the API cache attribute.
        /// </summary>
        /// <value>
        /// The API cache attribute.
        /// </value>
        internal ApiCacheAttribute ApiCacheAttribute { get; private set; }

        /// <summary>
        /// Gets or sets the API cache container.
        /// </summary>
        /// <value>
        /// The API cache container.
        /// </value>
        internal ApiCacheContainer ApiCacheContainer { get; private set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="RuntimeRoute" /> class.
        /// </summary>
        /// <param name="routeKey">The route key.</param>
        /// <param name="methodInfo">The method information.</param>
        /// <param name="instanceType">Type of the instance.</param>
        /// <param name="instance">The instance.</param>
        /// <param name="isActionUsed">if set to <c>true</c> [is action used].</param>
        /// <param name="isTokenRequired">if set to <c>true</c> [is token required].</param>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="isDataSensitive">if set to <c>true</c> [is data sensitive].</param>
        /// <param name="setting">The setting.</param>
        /// <param name="apiCacheAttribute">The API cache attribute.</param>
        /// <param name="permissions">The permissions.</param>
        /// <param name="headerKeys">The header keys.</param>
        public RuntimeRoute(string routeKey, MethodInfo methodInfo, Type instanceType, object instance, bool isActionUsed, bool isTokenRequired, string moduleName, string contentType, bool isDataSensitive, RestApiSettings setting, ApiCacheAttribute apiCacheAttribute, IDictionary<string, ApiPermission> permissions = null, List<string> headerKeys = null)
        {
            this.MethodInfo = methodInfo;
            this.Instance = instance;
            this.IsActionUsed = isActionUsed;
            this.InstanceType = instanceType;
            this.Setting = setting;

            this.OperationParameters = new RuntimeApiOperationParameters
            {
                ContentType = contentType,
                IsTokenRequired = isTokenRequired,
                IsDataSensitive = isDataSensitive,
                CustomizedHeaderKeys = headerKeys,
                Permissions = permissions,
                ModuleName = moduleName
            };

            this.RouteKey = routeKey;

            if (apiCacheAttribute != null)
            {
                this.ApiCacheAttribute = apiCacheAttribute;
                this.ApiCacheContainer = new ApiCacheContainer(routeKey, apiCacheAttribute.CacheParameter);
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return this.MethodInfo == null ? (InstanceType == null ? string.Empty : InstanceType.FullName) : MethodInfo.GetFullName();
        }
    }
}
